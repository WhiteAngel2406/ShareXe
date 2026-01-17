using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

using ShareXe.Base.Dtos;

namespace ShareXe.Base.Configs
{
    public class PatchRequestModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var modelType = context.Metadata.ModelType;
            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(PatchRequest<>))
            {
                return new BinderTypeModelBinder(typeof(PatchModelBinder));
            }

            return null;
        }
    }
}
