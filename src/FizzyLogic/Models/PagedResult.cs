// unset

namespace FizzyLogic.Models
{
    using System.Collections.Generic;

    public record PagedResult<T>(IEnumerable<T> Items, int PageIndex, int PageSize, int TotalItems);
}