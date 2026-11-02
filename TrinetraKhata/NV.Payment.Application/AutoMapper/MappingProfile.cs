using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NV.Payment.Application.Payment.Command.AddPayment;
using NV.Payment.Application.Payment.Command.UpdatePayment;
using NV.Payment.Application.Payment.Queries.GetPaymentMethod;
using NV.Payment.Application.Payment.Queries.GetPaymentMethodById;
using NV.Payment.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace NV.Payment.Application.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region PaymentMethod
            CreateMap<AddPaymentMethodResponse, PaymentMethod>().ReverseMap();
            CreateMap<AddPaymentMethodCommand, AddPaymentMethodResponse>();
            CreateMap<GetAllPaymentMethodQueryResponse, PaymentMethod>().ReverseMap();
            CreateMap<PagedResult<PaymentMethod>, PagedResult<GetAllPaymentMethodQueryResponse>>().ReverseMap();
            CreateMap<UpdatePaymentMethodCommand, PaymentMethod>();
            CreateMap<GetPaymentMethodByIdQueryResponse, PaymentMethod>().ReverseMap();
            #endregion
        }
    }
}
