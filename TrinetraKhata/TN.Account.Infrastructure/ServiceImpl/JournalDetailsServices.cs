using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateJournalEntryDetails;
using TN.Account.Application.Account.Queries.JournalEntryDetails;
using TN.Account.Application.Account.Queries.JournalEntryDetailsById;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;




namespace TN.Account.Infrastructure.ServiceImpl
{
    public class JournalDetailsServices : IJournalDetailsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly FiscalContext _fiscalContext;
        private readonly IBillNumberGenerator _billNumberGenerator;

        public JournalDetailsServices(IUnitOfWork unitOfWork,IBillNumberGenerator billNumberGenerator  , IMapper mapper,ITokenService tokenService, FiscalContext fiscalContext, IFiscalYearService fiscalYearService)
        {
           
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService=tokenService;
            _fiscalYearService = fiscalYearService;
            _fiscalContext = fiscalContext;
            _billNumberGenerator = billNumberGenerator;
        }

        public async Task<Result<AddJournalEntryDetailsResponse>> Add(AddJournalEntryDetailsCommand addJournalEntryDetailsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    var journalDetailsData = new JournalEntryDetails
                    (
                            newId,
                            "ADASDASDASDASD",
                           addJournalEntryDetailsCommand.ledgerId,
                           addJournalEntryDetailsCommand.debitAmount,
                           addJournalEntryDetailsCommand.creditAmount,
                           default,
                           "",
                           "",
                           true

                    );

                    await _unitOfWork.BaseRepository<JournalEntryDetails>().AddAsync(journalDetailsData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddJournalEntryDetailsResponse>(journalDetailsData);
                    return Result<AddJournalEntryDetailsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding journal entry details ", ex);

                }
            }
        }



        public async Task<Result<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>> GetAllJournalDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var journal = await _unitOfWork.BaseRepository<JournalEntryDetails>().GetAllAsyncWithPagination();
                var journalPagedResult = await journal.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);
                var allJournalResponse = _mapper.Map<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>(journalPagedResult.Data);
                return Result<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>.Success(allJournalResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all journal details", ex);
            }
        }

        public async Task<Result<string>> GetCurrentJournalRefNo()
        {
            string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;

            var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x=>x.CompanyId == companyId);
            if (companySettings == null)
            {
                return Result<string>.Failure("Invalid CompanyId. Company Id do not exist.");
            }

            string referenceNumber = "";

            if (companySettings.JournalReference == CompanySettings.JournalReferencesType.Manual)
            {
                return Result<string>.Failure("Reference number for Journal is manual.");
            }
            else
            {
                
                referenceNumber = await _billNumberGenerator.GenerateJournalReference(companyId);
            }

            if (string.IsNullOrEmpty(referenceNumber))
            {
                return Result<string>.Failure("Failed to generate journal reference number. Please check company settings.");
            }

            return Result<string>.Success(referenceNumber);
        }

        public async Task<Result<GetJournalEntryDetailsByIdResponse>> GetJournalDetailsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
               
                var journalDetails= await _unitOfWork.BaseRepository<JournalEntryDetails>().GetByGuIdAsync(id);
                var journalDetailsResponse = _mapper.Map<GetJournalEntryDetailsByIdResponse>(journalDetails);
                                
                return Result<GetJournalEntryDetailsByIdResponse>.Success(journalDetailsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Journal Entry Details by using Id", ex);
            }
        }

        public async Task<Result<UpdateJournalDetailsResponse>> Update(string id, UpdateJournalDetailsCommand updateJournalDetailsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateJournalDetailsResponse>.Failure("NotFound", "Please provide valid journal entry details Id");
                    }

                    var journalDetailsToBeUpdated = await _unitOfWork.BaseRepository<JournalEntryDetails>().GetByGuIdAsync(id);
                    if (journalDetailsToBeUpdated is null)
                    {
                        return Result<UpdateJournalDetailsResponse>.Failure("NotFound", "journalEntry Details are not Found");
                    }

                    _mapper.Map(updateJournalDetailsCommand, journalDetailsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateJournalDetailsResponse
                        (

                            id,
                           journalDetailsToBeUpdated.JournalEntryId,
                           journalDetailsToBeUpdated.LedgerId,
                           journalDetailsToBeUpdated.DebitAmount,
                           journalDetailsToBeUpdated.CreditAmount


                        );

                    return Result<UpdateJournalDetailsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the journalEntry Details", ex);
                }

            }
        }
    }
}
           
