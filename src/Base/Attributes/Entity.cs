namespace ShareXe.Base.Attributes
{
  /// <summary>
  /// Specifies that a class represents a database entity and maps to a table.
  /// </summary>
  /// <remarks>
  /// This attribute is used to decorate entity classes and optionally specify the name of the database table
  /// they map to. If no table name is provided, the default naming convention should be applied.
  /// </remarks>
  /// <param name="tableName">The name of the database table that this entity maps to. If null or not specified, 
  /// the default table naming convention will be used.</param>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public class EntityAttribute(string? tableName) : Attribute
  {
    public string? TableName { get; } = tableName;
  }
}