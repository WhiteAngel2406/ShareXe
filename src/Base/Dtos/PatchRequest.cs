namespace ShareXe.Base.Dtos
{
    /// <summary>
    /// Represents a generic PATCH request containing data to be updated and a list of fields that should be modified.
    /// </summary>
    /// <typeparam name="T">The type of data being patched. Must be a reference type.</typeparam>
    public class PatchRequest<T> where T : class
    {
        /// <summary>
        /// Gets or sets the data object containing the values to be updated.
        /// </summary>
        public T Data { get; set; } = default!;

        /// <summary>
        /// Gets or sets the list of field names that should be updated during the PATCH operation.
        /// </summary>
        public List<string> FieldsToUpdate { get; set; } = [];

        /// <summary>
        /// Determines whether a specific field is marked for update in the PATCH request.
        /// </summary>
        /// <param name="fieldName">The name of the field to check. The comparison is case-insensitive.</param>
        /// <returns><c>true</c> if the field is present in the <see cref="FieldsToUpdate"/> list; otherwise, <c>false</c>.</returns>
        public bool IsFieldPresent(string fieldName)
        {
            return FieldsToUpdate.Contains(fieldName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
