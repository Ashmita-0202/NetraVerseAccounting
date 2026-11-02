

namespace TN.Inventory.Application.Inventory.Command.AddItemGroup.RequestCommandMapper
{
    public static class AddItemGroupRequestMapper
    {
        public static AddItemGroupCommand ToCommand(this AddItemGroupRequest request) 
        {
            return new AddItemGroupCommand
                (
                    request.name,
                    request.description,
                    request.isPrimary,
                    request.itemsGroupId
                );
        }
    }
}
