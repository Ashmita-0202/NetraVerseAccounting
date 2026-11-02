using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Application.Account.Command.AddCustomer;
using TN.Account.Application.Account.Command.UpdateCustomer;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.Account.Queries.CustomerById;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;


namespace TN.Account.Infrastructure.ServiceImpl
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        
        }

        public async Task<Result<AddCustomerResponse>> Add(AddCustomerCommand addCustomerCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var customerData = new Customers
                   (
                        newId,
                        addCustomerCommand.fullName,
                        addCustomerCommand.address,
                        addCustomerCommand.contact,
                        addCustomerCommand.email,
                        addCustomerCommand.description,
                        addCustomerCommand.panNo,
                        addCustomerCommand.maxDueDates,
                        addCustomerCommand.maxCreditLimit,
                        addCustomerCommand.isEnabled,
                        addCustomerCommand.openingBalance,
                        addCustomerCommand.balanceType,
                        addCustomerCommand.isSmsEnabled,
                        addCustomerCommand.isEmailEnabled,
                        addCustomerCommand.ledgerId,
                        true


                    );

                    await _unitOfWork.BaseRepository<Customers>().AddAsync(customerData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddCustomerResponse>(customerData);
                    return Result<AddCustomerResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Customer ", ex);

                }
            }

        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {

            try
            {
                var customer = await _unitOfWork.BaseRepository<Customers>().GetByGuIdAsync(id);
                customer.IsActive = false;
                if (customer is null)
                {
                    return Result<bool>.Failure("NotFound", "Customer Cannot be Found");
                }

                _unitOfWork.BaseRepository<Customers>().Update(customer);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Customer having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllCustomerByQueryResponse>>> GetAllCustomer(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var customer = await _unitOfWork.BaseRepository<Customers>().GetAllAsyncWithPagination();
                var customerPagedResult = await customer.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allCustomerResponse = _mapper.Map<PagedResult<GetAllCustomerByQueryResponse>>(customerPagedResult.Data);
                
                return Result<PagedResult<GetAllCustomerByQueryResponse>>.Success(allCustomerResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all ledger group", ex);
            }
        }

        public async  Task<Result<GetCustomerByIdResponse>> GetCustomerById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
               
                var customer = await _unitOfWork.BaseRepository<Customers>().GetByGuIdAsync(id);

                var customerResponse = _mapper.Map<GetCustomerByIdResponse>(customer);
                              
                return Result<GetCustomerByIdResponse >.Success(customerResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Customer by using Id", ex);
            }
        }

        public async Task<Result<UpdateCustomerResponse>> Update(string id, UpdateCustomerCommand updateCustomerCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateCustomerResponse>.Failure("NotFound", "Please provide valid customer id");
                    }

                    var customerToBeUpdated = await _unitOfWork.BaseRepository<Customers>().GetByGuIdAsync(id);
                    if (customerToBeUpdated is null)
                    {
                        return Result<UpdateCustomerResponse>.Failure("NotFound", "customer are not Found");
                    }

                    _mapper.Map(updateCustomerCommand, customerToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateCustomerResponse
                        (
                            customerToBeUpdated.FullName,
                            customerToBeUpdated.Address,
                            customerToBeUpdated.Contact,
                            customerToBeUpdated.Email,
                            customerToBeUpdated.Description,
                            customerToBeUpdated.PanNo,
                            customerToBeUpdated.MaxDueDates,
                            customerToBeUpdated.MaxCreditLimit,
                            customerToBeUpdated.IsEnabled,
                            customerToBeUpdated.OpeningBalance,
                            customerToBeUpdated.BalanceType,
                            customerToBeUpdated.IsSmsEnabled,
                            customerToBeUpdated.IsEmailEnabled,
                            customerToBeUpdated.LedgerId
                       
                        );

                    return Result<UpdateCustomerResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating customer");
                }
            }
        }
    }
}
