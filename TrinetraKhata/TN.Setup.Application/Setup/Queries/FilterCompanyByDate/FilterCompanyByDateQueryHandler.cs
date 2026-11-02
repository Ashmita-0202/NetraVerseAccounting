using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.FilterCompanyByDate
{
    public class FilterCompanyByDateQueryHandler : IRequestHandler<FilterCompanyByDateQuery, Result<IEnumerable<FilterCompanyByDateQueryResponse>>>
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public FilterCompanyByDateQueryHandler(ICompanyServices companyServices, IMapper mapper)
        {

            _companyServices = companyServices;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<FilterCompanyByDateQueryResponse>>> Handle(FilterCompanyByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterCompany = await _companyServices.GetCompanyFilter(request.FilterCompanyDTOs, cancellationToken);

                if (!filterCompany.IsSuccess || filterCompany.Data == null)
                {
                    return Result<IEnumerable<FilterCompanyByDateQueryResponse>>.Failure(filterCompany.Message);
                }

                var filterCompanyResult = _mapper.Map<List<FilterCompanyByDateQueryResponse>>(filterCompany.Data);

                return Result<IEnumerable<FilterCompanyByDateQueryResponse>>.Success(filterCompanyResult);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FilterCompanyByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching Company  by date: {ex.Message}");
            }
        }
    }
}
