using ShareXe.Base.Dtos;

namespace ShareXe.Base.Extensions
{
    public static class PagingExtension
    {
        public static PagedResponse<T> ToPagedResponse<T>(this (IEnumerable<T> Items, int TotalCount) result, int pageIndex, int pageSize)
        {
            return PagedResponse<T>.WithPaging(
              [.. result.Items],
              result.TotalCount,
              pageIndex,
              pageSize
            );
        }
    }
}
