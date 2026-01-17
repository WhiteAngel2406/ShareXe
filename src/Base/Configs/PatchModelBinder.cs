using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ShareXe.Base.Configs
{
    public class PatchModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            var json = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            if (string.IsNullOrEmpty(json)) return;

            var jsonNode = JsonNode.Parse(json);
            var changedProps = jsonNode?.AsObject().Select(x => x.Key).ToList() ?? new List<string>();

            var dtoType = bindingContext.ModelType.GenericTypeArguments[0];

            var dtoData = JsonSerializer.Deserialize(json, dtoType,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var patchRequest = Activator.CreateInstance(bindingContext.ModelType);
            bindingContext.ModelType.GetProperty("Data")?.SetValue(patchRequest, dtoData);
            bindingContext.ModelType.GetProperty("FieldsToUpdate")?.SetValue(patchRequest, changedProps);

            bindingContext.Result = ModelBindingResult.Success(patchRequest);
        }
    }
}
