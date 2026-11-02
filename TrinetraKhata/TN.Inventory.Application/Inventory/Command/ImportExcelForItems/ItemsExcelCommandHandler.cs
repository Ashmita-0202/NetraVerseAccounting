using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.ImportExcelForItems
{
    public class ItemsExcelCommandHandler : IRequestHandler<ItemsExcelCommand, Result<ItemsExcelResponse>>
    {
        private readonly IItemsServices _itemsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<ItemsExcelCommand> _validator;

        public ItemsExcelCommandHandler(IItemsServices itemsServices, IMapper mapper, IValidator<ItemsExcelCommand> validator)
        {
            _itemsServices = itemsServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<ItemsExcelResponse>> Handle(ItemsExcelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<ItemsExcelResponse>.Failure(errors);
                }

                var itemsExcel = await _itemsServices.AddItemsExcel(request.formFile);

                if (itemsExcel.Errors.Any())
                {
                    var errors = string.Join(", ", itemsExcel.Errors);
                    return Result<ItemsExcelResponse>.Failure(errors);
                }

                if (itemsExcel is null || !itemsExcel.IsSuccess)
                {
                    return Result<ItemsExcelResponse>.Failure(" ");
                }

                return Result<ItemsExcelResponse>.Success(itemsExcel.Message);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Item", ex);


            }
        }
    }
}
