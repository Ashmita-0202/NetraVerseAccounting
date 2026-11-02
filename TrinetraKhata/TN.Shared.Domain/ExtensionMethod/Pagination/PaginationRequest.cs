using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.ExtensionMethod.Pagination
{
    public record PaginationRequest
    (
        int pageSize,
        int pageIndex,
        bool IsPagination
        );
}
