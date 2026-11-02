using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.TrialBalance;
using TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class VatDetailsServices : IVatDetails
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;


        public VatDetailsServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;
        }
        public async Task<Result<PagedResult<PurchaseAndSalesVATQueryResponse>>> GetVATReport(PurchaseAndSalesVATQueries request, CancellationToken cancellationToken = default)
        {
            try
    {
                var userScope = await _getUserScopedData.GetUserScopedData<JournalEntry>();
                var companyId = string.IsNullOrWhiteSpace(request.companyId)
                    ? userScope.CompanyId
                    : request.companyId;
                var relevantJournalEntries = await _unitOfWork.BaseRepository<JournalEntry>()
            .GetConditionalFilterType(
                je => je.CompanyId == companyId &&
                      (je.ReferenceNumber == "Sales Vouchers" || je.ReferenceNumber == "Purchase Vouchers"),
                query => query.Select(je => new
                {
                    je.Id,
                    je.ReferenceNumber,
                    je.BillNumbers
                }));

        var journalEntryIds = relevantJournalEntries.Select(x => x.Id).ToList();

       
        var journalEntryDetails = (await _unitOfWork.BaseRepository<JournalEntryDetails>()
            .FindBy(x => journalEntryIds.Contains(x.JournalEntryId)))
            .ToList(); // Materialize to avoid delegate inference issues

      
        var grouped = journalEntryDetails
            .GroupBy(x => new { x.LedgerId, x.JournalEntryId })
            .ToList();

       
        var vatReportDetails = grouped
            .Select(g =>
            {
                var journalEntry = relevantJournalEntries
                    .FirstOrDefault(je => je.Id == g.Key.JournalEntryId);

                return new PurchaseAndSalesVATQueryResponse(
                    journalEntry?.ReferenceNumber ?? "Unknown",
                    g.Key.LedgerId,
                    journalEntry?.ReferenceNumber == "Purchase Vouchers"
                        ? g.Sum(x => x.DebitAmount)
                        : null,
                    journalEntry?.ReferenceNumber == "Sales Vouchers"
                        ? g.Sum(x => x.CreditAmount)
                        : null,
                    journalEntry?.BillNumbers
                );
            })
            .ToList();

        var totalItems = vatReportDetails.Count;

        // Apply pagination if enabled
        var paginatedData = request.PaginationRequest != null && request.PaginationRequest.IsPagination
            ? vatReportDetails
                .Skip((request.PaginationRequest.pageIndex - 1) * request.PaginationRequest.pageSize)
                .Take(request.PaginationRequest.pageSize)
                .ToList()
            : vatReportDetails;

        // Return the paged result
        var result = new PagedResult<PurchaseAndSalesVATQueryResponse>
        {
            Items = paginatedData,
            TotalItems = totalItems,
            PageIndex = request.PaginationRequest?.pageIndex ?? 1,
            pageSize = request.PaginationRequest?.pageSize ?? 100
        };

        return Result<PagedResult<PurchaseAndSalesVATQueryResponse>>.Success(result);
    }
    catch (Exception ex)
    {
        throw new Exception("An error occurred while fetching VAT Report", ex);
    }
        }
    }
}
