using Microsoft.Extensions.DependencyInjection;
using NV.Payment.Application.ServiceInterface;
using NV.Payment.Infrastructure.ServiceImpl;

namespace NV.Payment.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddPaymentInfrastructure(this IServiceCollection services) 
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region InjectDependencies
 
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            #endregion

            return services;
        
        }

    }
}
