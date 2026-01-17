namespace ShareXe.Base.Attributes
{
    /// <summary>
    /// Specifies that a property value must be unique within the database, optionally grouped by a specified group name.
    /// </summary>
    /// <remarks>
    /// When applied to a property, this attribute indicates that the property's value should be unique across all records.
    /// If a <see cref="GroupName"/> is specified, uniqueness is enforced only within records sharing the same group name value.
    /// This is typically used for validation and database constraint generation.
    /// </remarks>
    ///
    /// <example>
    /// <code>
    /// [Unique]
    /// public string Email { get; set; }
    /// 
    /// [Unique("UserType")]
    /// public string Username { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class UniqueAttribute(string? groupName = null) : Attribute
    {
        public string? GroupName { get; } = groupName;
    }
}
