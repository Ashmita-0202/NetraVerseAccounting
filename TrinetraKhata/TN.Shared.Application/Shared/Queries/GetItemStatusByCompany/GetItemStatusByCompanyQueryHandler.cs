using AutoMapper;
using MediatR;
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusByCompany;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Authentication.Application.Authentication.Queries.GetItemStatusByCompany
{
    public class GetItemStatusByCompanyQueryHandler : IRequestHandler<GetItemStatusByCompanyQuery, Result<GetItemStatusByCompanyResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetItemStatusByCompanyQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _mapper = mapper;
            _settingServices = settingServices;

        }
        public async Task<Result<GetItemStatusByCompanyResponse>> Handle(GetItemStatusByCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var expiredDateStatus = await _settingServices.GetItemStatusByCompany(request.companyId, cancellationToken);
                var expiredDateStatusDisplay = _mapper.Map<GetItemStatusByCompanyResponse>(expiredDateStatus.Data);
                return Result<GetItemStatusByCompanyResponse>.Success(expiredDateStatusDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting ExpiredDate Status by company{request.companyId}");
            }
        }
    }
}
