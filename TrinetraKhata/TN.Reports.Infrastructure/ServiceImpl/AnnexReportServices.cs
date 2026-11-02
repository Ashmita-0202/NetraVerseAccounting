using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Reports.Application.AccountBook.Queries.SalesRegister;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class AnnexReportServices : IAnnexReportServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;

        public AnnexReportServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;

        }
        public async Task<Result<PagedResult<AnnexReportQueryResponse>>> GetAnnexReport(PaginationRequest paginationRequest, AnnexReportDTOs annexReportDTOs, CancellationToken cancellationToken = default)
        {
            try
            {
                var journalEntries = await _unitOfWork.BaseRepository<JournalEntry>()
               .GetConditionalAsync(
                   predicate: x => x.CompanyId == annexReportDTOs.CompanyId && x.FyId == annexReportDTOs.fyId,
                    query => query
                       .Include(x => x.JournalEntryDetails)
                       .Include(x => x.PurchaseDetails)
               );

                var annexReportData = journalEntries
                     .SelectMany(x => x.PurchaseDetails)
                     .Where(p => p.LedgerId != null)
                     .GroupBy(p => p.LedgerId)
                     .Select(g =>
                     {
                         var ledgerId = g.Key!;
                         var taxableAmount = g.Sum(p =>
                         {
                             decimal gross = p.GrandTotalAmount;
                             decimal vat = p.VatAmount ?? 0;
                             return gross - vat;
                         });

                         return new AnnexReportQueryResponse(
                             PanNumber: "123",
                             LedgerId: ledgerId,
                             Type: "Purchase",
                             TaxableAmount: taxableAmount, 
                             NonTaxableAmount: 0
                         );
                     })
                     .ToList();
                PagedResult<AnnexReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = annexReportData.Count();

                    var pagedItems = annexReportData
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<AnnexReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<AnnexReportQueryResponse>
                    {
                        Items = annexReportData.ToList(),
                        TotalItems = annexReportData.Count(),
                        PageIndex = 1,
                        pageSize = annexReportData.Count()
                    };
                }


                return Result<PagedResult<AnnexReportQueryResponse>>.Success(finalResponseList);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the annex report.", ex);
            }
        }
    }
}
