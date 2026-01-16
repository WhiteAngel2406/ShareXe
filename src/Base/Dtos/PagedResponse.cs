namespace ShareXe.Base.Dtos
{
    /// <summary>
    /// A generic response class for paginated data that extends the base Response class.
    /// Provides pagination metadata, sorting information, and filtering details along with the response data.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the paginated response.</typeparam>
    public class PagedResponse<T> : Response
    {
        public List<T> Data { get; set; } = [];
        public ResponseMetadata Metadata { get; set; } = new ResponseMetadata();

        protected PagedResponse() { Success = true; }

        public static PagedResponse<T> WithPaging(List<T> data, int totalRecords, int currentPage, int pageSize)
        {
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PagedResponse<T>
            {
                Success = true,
                Data = data,
                Metadata = new ResponseMetadata
                {
                    Pagination = new ResponseMetadata.ResponsePagination
                    {
                        CurrentPage = currentPage,
                        PageSize = pageSize,
                        TotalRecords = totalRecords,
                        TotalPages = totalPages,
                        HasNextPage = currentPage < totalPages,
                        HasPreviousPage = currentPage > 1
                    }
                }
            };
        }

        public static PagedResponse<T> WithFullMetadata(
            List<T> data,
            int totalRecords,
            int currentPage,
            int pageSize,
            List<ResponseMetadata.ResponseOrder>? orders = null,
            Dictionary<string, object>? filters = null
        )
        {
            var response = WithPaging(data, totalRecords, currentPage, pageSize);
            response.Metadata.Orders = orders ?? [];
            response.Metadata.Filters = filters ?? [];
            return response;
        }

        /// <summary>
        /// Contains pagination, filtering, and ordering metadata for the response.
        /// </summary>
        public class ResponseMetadata
        {
            public ResponsePagination Pagination { get; set; } = new ResponsePagination();
            public List<ResponseOrder> Orders { get; set; } = [];
            public Dictionary<string, object> Filters { get; set; } = [];

            /// <summary>
            /// Contains pagination-related information such as current page, page size, and total pages.
            /// </summary>
            public class ResponsePagination
            {
                public int CurrentPage { get; set; }
                public int PageSize { get; set; }
                public int TotalRecords { get; set; }
                public int TotalPages { get; set; }
                public bool HasNextPage { get; set; }
                public bool HasPreviousPage { get; set; }
            }

            public class ResponseOrder
            {
                public string Field { get; set; } = string.Empty;
                public string Direction { get; set; } = string.Empty;
            }
        }
    }
}
