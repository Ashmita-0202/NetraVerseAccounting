using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.DayBook.FilterSalesDayBook;
using TN.Reports.Application.DayBook.FilterSalesReturnDayBook;
using TN.Reports.Application.ItemwiseSalesReport;
using TN.Reports.Application.PurchaseReturnReport;
using TN.Reports.Application.PurchaseSummaryReport;
using TN.Reports.Application.SalesReport;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.SalesSummaryReport;
using TN.Reports.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class SalesReportService:ISalesReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;


        public SalesReportService(IUnitOfWork unitOfWork,IDateConvertHelper dateConvertHelper,IMapper mapper,ITokenService tokenService, IGetUserScopedData getUserScopedData) 
        {
            _unitOfWork=unitOfWork;
            _dateConvertHelper = dateConvertHelper;
            _mapper =mapper;
            _tokenService=tokenService;
            _getUserScopedData = getUserScopedData;
        }

        public async Task<PagedResult<GetSalesReportQueryResponse>> GetAllSalesReport(PaginationRequest paginationRequest, SalesReportDtos salesReportDtos)
        {
             try
                {
                var (salesQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesDetails>();

     
                    IQueryable<SalesDetails> filteredSales = salesQuery;

                    if (!string.IsNullOrEmpty(salesReportDtos.companyId))
                    {
                        filteredSales = filteredSales.Where(x => x.CompanyId == salesReportDtos.companyId);
                    }
                    else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                    {
                        filteredSales = filteredSales.Where(x => x.CompanyId == currentCompanyId);
                    }
                    else if (!string.IsNullOrEmpty(institutionId))
                    {
                        var companyIds = await _unitOfWork.BaseRepository<Company>()
                            .GetConditionalFilterType(
                                x => x.InstitutionId == institutionId,
                                query => query.Select(c => c.Id)
                            );

                        filteredSales = filteredSales.Where(x => companyIds.Contains(x.CompanyId));
                    }

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(salesReportDtos.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(salesReportDtos.startDate.Trim());

                    if (DateTime.TryParse(salesReportDtos.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }


                if (!string.IsNullOrWhiteSpace(salesReportDtos.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(salesReportDtos.endDate.Trim());

                    if (DateTime.TryParse(salesReportDtos.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                filteredSales = filteredSales.Where(s => s.CreatedAt >= startDateUtc && s.CreatedAt < endDateUtc);

                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                filteredSales = filteredSales.Where(s =>
                (string.IsNullOrEmpty(salesReportDtos.stockCenterId) || s.StockCenterId == salesReportDtos.stockCenterId) &&
                (string.IsNullOrEmpty(salesReportDtos.itemGroupId) || s.SalesItems.Any(i => i.Item.ItemGroupId == salesReportDtos.itemGroupId)) &&
                (string.IsNullOrEmpty(salesReportDtos.billNumber) || s.BillNumber == salesReportDtos.billNumber) &&
                (string.IsNullOrEmpty(salesReportDtos.ledgerId) || s.LedgerId == salesReportDtos.ledgerId) &&
                (string.IsNullOrEmpty(salesReportDtos.ItemId) || s.SalesItems.Any(i => i.ItemId == salesReportDtos.ItemId)) &&
                (salesReportDtos.SerialNumbers == null || !salesReportDtos.SerialNumbers.Any() ||
                         s.SalesItems.Any(i => i.ItemInstances.Any(ii => !string.IsNullOrEmpty(ii.SerialNumber) &&
                                                                             salesReportDtos.SerialNumbers.Contains(ii.SerialNumber))))
                );

               
                //int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                //    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                //    var totalItems = await filteredSales.CountAsync();

                    var pagedSales = await filteredSales
                        .Include(s => s.SalesItems)
                            .ThenInclude(si => si.Item)
                        .Include(s => s.SalesItems)
                            .ThenInclude(si => si.ItemInstances)
                        .OrderByDescending(s => s.CreatedAt) 
                        //.Skip((pageIndex - 1) * pageSize)
                        //.Take(pageSize)
                        .SelectMany(s => s.SalesItems.Select(si => new GetSalesReportQueryResponse(
                            si.ItemId,
                            si.Item.ItemGroupId ?? "",
                            si.ItemInstances
                                .Where(ii => !string.IsNullOrEmpty(ii.SerialNumber) && ii.SalesItemsId == si.Id)
                                .Select(ii => ii.SerialNumber)
                                .ToList(),
                            s.Date ?? string.Empty,
                            s.BillNumber ?? string.Empty,
                            s.LedgerId ?? string.Empty,
                            si.Quantity,
                            si.Price,
                            si.Quantity * si.Price, // NetAmount
                            s.DiscountAmount,
                            s.VatAmount,
                            s.GrandTotalAmount,
                            s.StockCenterId ?? string.Empty
                        )))
                        .ToListAsync();


                PagedResult<GetSalesReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = pagedSales.Count();

                    var pagedItems = pagedSales
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetSalesReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetSalesReportQueryResponse>
                    {
                        Items = pagedSales.ToList(),
                        TotalItems = pagedSales.Count(),
                        PageIndex = 1,
                        pageSize = pagedSales.Count() // all items in one page
                    };
                }
                return finalResponseList;
            }
             catch (Exception ex)
             {
                throw new Exception("An error occurred while generating the sales report.", ex);
             }
        }

        public async Task<Result<PagedResult<FilterSalesDayBookQueryResponse>>> GetFilterSalesDayBook(PaginationRequest paginationRequest, FilterSalesDayBookDto filterSalesDayBookDto)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesDetails>();

               
                var filterItems = isSuperAdmin
                    ? ledger
                    : ledger.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(filterSalesDayBookDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesDayBookDto.startDate.Trim());

                    if (DateTime.TryParse(filterSalesDayBookDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

               
                if (!string.IsNullOrWhiteSpace(filterSalesDayBookDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesDayBookDto.endDate.Trim());

                    if (DateTime.TryParse(filterSalesDayBookDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

               
                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                var userId = _tokenService.GetUserId;

               
                var salesDayBookResult = await _unitOfWork.BaseRepository<SalesDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            x.CreatedBy == userId() &&
                            x.IsActive == true &&
                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt < endDateUtc),
                        queryModifier: q => q.AsNoTracking()
                    );

               
                var responseList = salesDayBookResult.Select(sd => new FilterSalesDayBookQueryResponse(
                    sd.Date ?? sd.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                      sd.BillNumber ?? "",
                      sd.LedgerId ?? "",
                      sd.VatAmount,
                      sd.GrandTotalAmount,
                      netAmount: (sd.GrandTotalAmount-(sd.VatAmount ?? 0))
                    )).ToList();

                PagedResult<FilterSalesDayBookQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSalesDayBookQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSalesDayBookQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<FilterSalesDayBookQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching sales day book: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>> GetFilterSalesReturnDayBook(PaginationRequest paginationRequest, FilterSalesReturnDayBookDto filterSalesReturnDayBookDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (dayBook, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesReturnDetails>();


                var filterItems = isSuperAdmin
                    ? dayBook
                    : dayBook.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(filterSalesReturnDayBookDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesReturnDayBookDto.startDate.Trim());

                    if (DateTime.TryParse(filterSalesReturnDayBookDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }


                if (!string.IsNullOrWhiteSpace(filterSalesReturnDayBookDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesReturnDayBookDto.endDate.Trim());

                    if (DateTime.TryParse(filterSalesReturnDayBookDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }


                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                var userId = _tokenService.GetUserId;


                var salesReturnDayBookResult = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            x.CreatedBy == userId() &&
                        
                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt < endDateUtc),
                        queryModifier: q => q.AsNoTracking().Include(sr=>sr.SalesDetails)
                    );


                var responseList = salesReturnDayBookResult.Select(sr => new FilterSalesReturnDayBookQueryResponse(
                         sr.ReturnDate,
                      sr.SalesDetails?.BillNumber ?? "",
                      sr.LedgerId ?? "",
                      sr.NetReturnAmount,
                      sr.TaxAdjustment,
                      sr.TotalReturnAmount
                     
                    )).ToList();


                PagedResult<FilterSalesReturnDayBookQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSalesReturnDayBookQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSalesReturnDayBookQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching SalesReturn day book: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<ItemwiseSalesReportQueryResponse>>> GetItemwiseSalesReport(PaginationRequest paginationRequest, ItemwiseSalesReportDto itemwiseSalesReportDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (salesDetailsQueryable, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesDetails>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();

                var filterSales = isSuperAdmin
                    ? salesDetailsQueryable
                    : salesDetailsQueryable.Where(x => x.CompanyId == currentCompanyId);
                // Date filters
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(itemwiseSalesReportDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(itemwiseSalesReportDto.startDate.Trim());
                    if (DateTime.TryParse(itemwiseSalesReportDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(itemwiseSalesReportDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(itemwiseSalesReportDto.endDate.Trim());
                    if (DateTime.TryParse(itemwiseSalesReportDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // Handle invalid date cases
                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                var userId = _tokenService.GetUserId;

                // Query SalesDetails including SalesItems & Item
                var salesResult = await filterSales
                    .Where(x =>
                        x.CreatedBy == userId() &&
                        (!startDateUtc.HasValue || x.CreatedAt >= startDateUtc.Value) &&
                        (!endDateUtc.HasValue || x.CreatedAt <= endDateUtc.Value) &&
                        (string.IsNullOrWhiteSpace(itemwiseSalesReportDto.ledgerId) || x.LedgerId == itemwiseSalesReportDto.ledgerId) &&
                        (string.IsNullOrWhiteSpace(itemwiseSalesReportDto.stockCenterId) || x.StockCenterId == itemwiseSalesReportDto.stockCenterId)
                    )
                    .Select(x => new
                    {
                        x.Id,
                        x.Date,
                        x.LedgerId,
                        x.BillNumber,
                        x.StockCenterId,
                        x.DiscountAmount,
                        x.VatAmount,
                        x.GrandTotalAmount,
                        x.Status,
                        SalesItems = x.SalesItems.Select(si => new
                        {
                            si.Id,
                            si.Quantity,
                            si.Price,
                            si.Item.ItemGroup,
                            si.ItemId,
                            si.UnitId,
                            si.Amount,
                            Item = new
                            {
                                si.Item.Id,
                                si.Item.Name,
                                si.Item.ItemGroup,
                                si.Item.UnitId,
                                ItemInstances = si.Item.ItemInstances.Select(ii => new
                                {
                                    ii.Id,
                                    ii.SerialNumber,
                                
                                })
                            }
                        })
                    })
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);


                // Flatten SalesDetails -> SalesItems into response list
                var responseList = salesResult
                    .SelectMany(sd => sd.SalesItems.Select(si => new ItemwiseSalesReportQueryResponse(
                        sd.Date,
                        sd.BillNumber,
                        sd.LedgerId,
                        si.ItemId,
                        si.Item?.ItemGroup.Id,
                        si.UnitId,
                        si.Price,
                        si.Quantity,
                        si.Amount,
                        sd.DiscountAmount,
                        sd.VatAmount,
                        sd.GrandTotalAmount,
                        sd.Status,
                        sd.StockCenterId ?? ""
                    )))
                    .Where(r =>
                        (string.IsNullOrWhiteSpace(itemwiseSalesReportDto.itemId) || r.itemId == itemwiseSalesReportDto.itemId) &&
                        (string.IsNullOrWhiteSpace(itemwiseSalesReportDto.itemGroupId) || r.itemGroupId == itemwiseSalesReportDto.itemGroupId)
                    )
                    .ToList();

                PagedResult<ItemwiseSalesReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<ItemwiseSalesReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<ItemwiseSalesReportQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }


                return Result<PagedResult<ItemwiseSalesReportQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Itemwise Sales Report: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetSalesReturnReportQueryResponse>>> GetSalesReturnReport(PaginationRequest paginationRequest, SalesReturnReportDto salesReturnReportDto, CancellationToken cancellationToken = default)
        {

            try
            {
                var (salesReturn, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesReturnDetails>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();
                // Filter based on user scope
                var filteredLedger = isSuperAdmin
                    ? salesReturn
                    : salesReturn.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || string.IsNullOrEmpty(x.CompanyId));

                // Convert start and end dates
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(salesReturnReportDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(salesReturnReportDto.startDate.Trim());
                    if (DateTime.TryParse(salesReturnReportDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(salesReturnReportDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(salesReturnReportDto.endDate.Trim());
                    if (DateTime.TryParse(salesReturnReportDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // Default to today if dates are invalid
                if (!startDateUtc.HasValue && !endDateUtc.HasValue)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (startDateUtc.HasValue && !endDateUtc.HasValue)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (!startDateUtc.HasValue && endDateUtc.HasValue)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                var userId = _tokenService.GetUserId;

                // Fetch data with filters and includes
                var salesReturnReportResult = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                    .GetConditionalAsync(
                      predicate: x =>
                        x.CreatedBy == userId() &&
                        (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                        (endDateUtc == null || x.CreatedAt < endDateUtc) &&
                        (string.IsNullOrWhiteSpace(salesReturnReportDto.ledgerId) || x.LedgerId == salesReturnReportDto.ledgerId) &&
                        (string.IsNullOrWhiteSpace(salesReturnReportDto.stockCentreId) || x.SalesDetails.StockCenterId == salesReturnReportDto.stockCentreId) &&
                        (isSuperAdmin || x.SalesDetails.CompanyId == currentCompanyId),
                    queryModifier: q => q
                        .AsNoTracking()
                        .Include(x => x.SalesReturnItems)
                            .ThenInclude(sri => sri.SalesItems)
                                .ThenInclude(si => si.Item)
                                    .ThenInclude(i => i.ItemGroup)
                        .Include(x => x.SalesDetails)
                    );

                // Flatten and filter sales items
                var responseList = salesReturnReportResult
                    .SelectMany(sr => sr.SalesReturnItems
                        .Where(sri =>
                            (string.IsNullOrWhiteSpace(salesReturnReportDto.salesItemsId) || sri.SalesItemsId == salesReturnReportDto.salesItemsId) &&
                            (string.IsNullOrWhiteSpace(salesReturnReportDto.itemGroupId) || sri.SalesItems.Item.ItemGroupId == salesReturnReportDto.itemGroupId)
                        )
                        .Select(sri => new GetSalesReturnReportQueryResponse(
                            sr.ReturnDate,
                            sr.SalesReturnNumber ?? "", 
                            sr.LedgerId ?? "",
                            sri.SalesItemsId,
                            sri.SalesItems.Item.ItemGroupId ?? "",
                            sri.SalesItems.UnitId,
                            sri.ReturnQuantity,
                            sri.ReturnUnitPrice,
                            sr.NetReturnAmount,
                            sr.TaxAdjustment,
                            sr.TotalReturnAmount,
                            sr.StockCenterId ?? ""
                        ))
                    )
                    .ToList();

                PagedResult<GetSalesReturnReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetSalesReturnReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetSalesReturnReportQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }


                return Result<PagedResult<GetSalesReturnReportQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching SalesReturn Report: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetSalesSummaryQueryResponse>>> GetSalesSummaryReport(PaginationRequest paginationRequest, SalesSummaryDtos salesSummaryDtos, CancellationToken cancellationToken = default)
        {
            try
            {
      
                var (salesQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesDetails>();

                IQueryable<SalesDetails> filteredSales = salesQuery;


                if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filteredSales = filteredSales.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filteredSales = filteredSales.Where(x => companyIds.Contains(x.CompanyId));
                }
                var topItem = await filteredSales
                .SelectMany(s => s.SalesItems)
                .GroupBy(si => new { si.Id, si.Item.Name })
                .Select(g => new
                {
                    g.Key.Id,
                    g.Key.Name,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Select(g => g.Name)
                .FirstOrDefaultAsync(cancellationToken) ?? "";

                var query = filteredSales
                    .SelectMany(pd => pd.SalesItems.Select(pi => new
                    {
                        pd.Id,
                        pd.BillNumber,
                        pd.LedgerId,
                        pd.StockCenterId,
                        pd.DiscountAmount,
                        pd.VatAmount,
                        pd.GrandTotalAmount,
                        pi.ItemId,
                        ItemName = pi.Item.Name,
                        pi.Quantity,
                        pi.Price,
                        pi.Amount
                    }));

                // 3. Apply filters from DTO
                if (!string.IsNullOrEmpty(salesSummaryDtos.itemId))
                    query = query.Where(x => x.ItemId == salesSummaryDtos.itemId);

                if (!string.IsNullOrEmpty(salesSummaryDtos.ledgerId))
                    query = query.Where(x => x.LedgerId == salesSummaryDtos.ledgerId);

                if (!string.IsNullOrEmpty(salesSummaryDtos.stockCenterId))
                    query = query.Where(x => x.StockCenterId == salesSummaryDtos.stockCenterId);

                // 4. Group by Item/Ledger/StockCenter
                var grouped = await query
                    .GroupBy(x => new { x.ItemId, x.LedgerId, x.StockCenterId })
                    .Select(g => new GetSalesSummaryQueryResponse(

                        g.Key.ItemId,
                        g.Key.LedgerId,
                        g.Key.StockCenterId,
                        g.Select(x => x.BillNumber).Distinct().Count(),
                        g.Sum(x => x.Quantity),
                        g.Sum(x => x.Amount),
                        g.Sum(x => x.DiscountAmount ?? 0),
                        g.Sum(x => x.VatAmount ?? 0),
                        g.Sum(x => x.GrandTotalAmount),
                       topItem
                    ))
                    .ToListAsync(cancellationToken);
                PagedResult<GetSalesSummaryQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = grouped.Count();

                    var pagedItems = grouped
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetSalesSummaryQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetSalesSummaryQueryResponse>
                    {
                        Items = grouped.ToList(),
                        TotalItems = grouped.Count(),
                        PageIndex = 1,
                        pageSize = grouped.Count()
                    };
                }

                return Result<PagedResult<GetSalesSummaryQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetSalesSummaryQueryResponse>>.Failure(ex.Message);
            }
        }
    }
}
