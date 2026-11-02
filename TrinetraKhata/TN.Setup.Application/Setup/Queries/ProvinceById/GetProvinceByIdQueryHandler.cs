using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.ProvinceById
{
    public sealed class GetProvinceByIdQueryHandler : IRequestHandler<GetProvinceByIdQuery, Result<GetProvinceByIdResponse>>
    {
        private readonly IProvinceServices _services;
        private readonly IMapper _mapper;

        public GetProvinceByIdQueryHandler(IProvinceServices provinceServices, IMapper mapper)
        {
            _services = provinceServices;
            _mapper = mapper;
            
        }
        public async Task<Result<GetProvinceByIdResponse>> Handle(GetProvinceByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var provinceById = await _services.GetProvinceById(request.Id);

                return Result<GetProvinceByIdResponse>.Success(provinceById.Data);

            }catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching province by Id", ex);
            }
        }
    }
}
