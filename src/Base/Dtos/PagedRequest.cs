using System.ComponentModel.DataAnnotations;

using ShareXe.Base.Attributes;

namespace ShareXe.Base.Dtos
{
    /// <summary>
    /// Represents a paginated request with filtering and sorting capabilities.
    /// </summary>
    /// <remarks>
    /// This class provides a standard contract for paginated API requests, allowing clients to specify
    /// pagination parameters, sorting order, and custom filters through inherited or additional properties.
    /// </remarks>
    public class PagedRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int? Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int? PageSize { get; set; } = 10;

        [OrderParam("CreatedAt", "LastModifiedAt", "DeletedAt")]
        public List<string> Order { get; set; } = [];

        public Dictionary<string, object> GetFilters()
        {
            var filters = new Dictionary<string, object>();

            var properties = GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name == nameof(Page) || prop.Name == nameof(PageSize) || prop.Name == nameof(Order))
                {
                    continue;
                }

                var value = prop.GetValue(this);
                if (value != null)
                {
                    filters.Add(prop.Name, value);
                }
            }

            return filters;
        }
    }
}
