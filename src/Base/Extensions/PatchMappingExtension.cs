using System.Reflection;

using ShareXe.Base.Dtos;
using ShareXe.Base.Entities;

namespace ShareXe.Base.Extensions
{
    public static class PatchMappingExtensions
    {
        public static void ApplyPatch<TEntity, TDto>(this TEntity entity, PatchRequest<TDto> patchRequest)
            where TEntity : BaseEntity
            where TDto : class
        {
            var dtoType = typeof(TDto);
            var entityType = entity.GetType();

            foreach (var propertyName in patchRequest.FieldsToUpdate)
            {
                var forbiddenProperties = new[] { "Id", "CreatedAt" };
                if (forbiddenProperties.Contains(propertyName)) continue;

                var dtoProp = dtoType.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (dtoProp == null) continue;

                var entityProp = entityType.GetProperty(dtoProp.Name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (entityProp != null && entityProp.CanWrite)
                {
                    var value = dtoProp.GetValue(patchRequest.Data);
                    entityProp.SetValue(entity, value);
                }
            }
        }
    }
}
