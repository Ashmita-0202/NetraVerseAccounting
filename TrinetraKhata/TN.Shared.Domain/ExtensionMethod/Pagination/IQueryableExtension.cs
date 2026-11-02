using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Domain.ExtensionMethod.Pagination
{
    public static class IQueryableExtension
    {

        public static async Task<Result<PagedResult<T>>> ToPagedResultAsync<T>(this IQueryable<T> query, int? pageIndex, int? pageSize, bool IsPagination)
        {
            try
            {
                List<T> items;
                int totalItems = await query.CountAsync();

                if (IsPagination)
                {
                    int validPageIndex = pageIndex ?? 1;
                    int validPageSize = pageSize ?? 10;

                    if (validPageIndex < 1)
                    {
                        validPageIndex = 1;
                    }

                    if (validPageSize < 1)
                    {
                        validPageSize = 10;
                    }

                    items = await query.Skip((validPageIndex - 1) * validPageSize).Take(validPageSize).ToListAsync();

                    var pagedResult = new PagedResult<T>
                    {
                        Items = items,
                        TotalItems = totalItems,
                        PageIndex = validPageIndex,
                        pageSize = validPageSize
                    };

                    return Result<PagedResult<T>>.Success(pagedResult);

                }
                else
                {
                    //Fetch all data without permission
                    items = await query.ToListAsync();

                    return Result<PagedResult<T>>.Success(new PagedResult<T>
                    {
                        Items = items,
                        TotalItems = totalItems,
                        PageIndex = 1,
                        pageSize = totalItems
                    });
                }

            }
            catch (Exception)
            {
                return Result<PagedResult<T>>.Failure("NotFound", "Getting problem while fetchig data");
            }
        }
    }
}
