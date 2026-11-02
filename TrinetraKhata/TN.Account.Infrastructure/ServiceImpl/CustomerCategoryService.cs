using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TN.Account.Application.Account.Command.AddCustomerCategory;
using TN.Account.Application.Account.Command.UpdateCustomerCategory;
using TN.Account.Application.Account.Queries.CustomerCategory;
using TN.Account.Application.Account.Queries.CustomerCategoryById;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace TN.Account.Infrastructure.ServiceImpl
{
    public class CustomerCategoryService : ICustomerCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerCategoryService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        
        }


        public async Task<Result<AddCustomerCategoryResponse>> Add(AddCustomerCategoryCommand addCustomerCategoryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var customerCategoryData = new CustomerCategory
                    (
                        newId,
                        addCustomerCategoryCommand.name,
                        addCustomerCategoryCommand.createdAt,
                        addCustomerCategoryCommand.isEnabled,
                        addCustomerCategoryCommand.customerId,
                        true


                    );

                    await _unitOfWork.BaseRepository<CustomerCategory>().AddAsync(customerCategoryData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddCustomerCategoryResponse>(customerCategoryData);
                    return Result<AddCustomerCategoryResponse>.Success(resultDTOs);

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
                var customerCategory = await _unitOfWork.BaseRepository<CustomerCategory>().GetByGuIdAsync(id);
                customerCategory.IsActive = false;
                if (customerCategory is null)
                {
                    return Result<bool>.Failure("NotFound", "CustomerCategory Cannot be Found");
                }

                _unitOfWork.BaseRepository<CustomerCategory>().Update(customerCategory);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting CustomerCategory having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllCustomerCategoryByResponse>>> GetAllCustomerCategory(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var customerCategory = await _unitOfWork.BaseRepository<CustomerCategory>().GetAllAsyncWithPagination();
                var customerCategoryPagedResult = await customerCategory.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allCustomerCategoryResponse = _mapper.Map<PagedResult<GetAllCustomerCategoryByResponse>>(customerCategoryPagedResult.Data);

                return Result<PagedResult<GetAllCustomerCategoryByResponse>>.Success(allCustomerCategoryResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all ledger group", ex);
            }
        }

        public async Task<Result<GetCustomerCategoryByIdResponse>> GetCustomerCategoryById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var customerCategory = await _unitOfWork.BaseRepository<CustomerCategory>().GetByGuIdAsync(id);

                var customerCategoryResponse = _mapper.Map<GetCustomerCategoryByIdResponse>(customerCategory);

                return Result<GetCustomerCategoryByIdResponse>.Success(customerCategoryResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Customer Category by using Id", ex);
            }
        }

        public async Task<Result<UpdateCustomerCategoryResponse>> Update(string id, UpdateCustomerCategoryCommand updateCustomerCategoryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateCustomerCategoryResponse>.Failure("NotFound", "Please provide valid customer category id");
                    }

                    var customerCategoryToBeUpdated = await _unitOfWork.BaseRepository<CustomerCategory>().GetByGuIdAsync(id);
                    if (customerCategoryToBeUpdated is null)
                    {
                        return Result<UpdateCustomerCategoryResponse>.Failure("NotFound", "customer category are not Found");
                    }

                    _mapper.Map(updateCustomerCategoryCommand, customerCategoryToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateCustomerCategoryResponse
                        (
                            
                            customerCategoryToBeUpdated.Name,
                            customerCategoryToBeUpdated.CreatedAt,
                            customerCategoryToBeUpdated.IsEnabled,
                            customerCategoryToBeUpdated.CustomerId
                           

                        );

                    return Result<UpdateCustomerCategoryResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating customer category");
                }
            }
        }
    }
}
