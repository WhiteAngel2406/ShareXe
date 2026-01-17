using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ShareXe.Base.Dtos;

using System.ComponentModel.DataAnnotations;

namespace ShareXe.Base.Filters
{
    public class PatchValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Tìm tham số có kiểu dữ liệu là PatchRequest<T>
            var patchArgument = context.ActionArguments.Values
                .FirstOrDefault(v => v != null && v.GetType().IsGenericType &&
                                     v.GetType().GetGenericTypeDefinition() == typeof(PatchRequest<>));

            if (patchArgument == null) return;

            // 2. Lấy dữ liệu Data và danh sách ChangedProperties từ PatchRequest
            var dataType = patchArgument.GetType().GetProperty("Data");
            var changedPropsType = patchArgument.GetType().GetProperty("ChangedProperties");

            var dataValue = dataType?.GetValue(patchArgument);
            var changedProps = changedPropsType?.GetValue(patchArgument) as List<string>;

            if (dataValue == null || changedProps == null) return;

            // 3. Thực hiện Validation thủ công cho Object Data
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dataValue);

            // TryValidateObject sẽ kiểm tra tất cả DataAnnotation ([Required], [StringLength],...)
            Validator.TryValidateObject(dataValue, validationContext, validationResults, true);

            // 4. Lọc lỗi: CHỈ lấy lỗi của những trường thực sự có trong JSON gửi lên
            var relevantErrors = validationResults
                .Where(error => error.MemberNames.Any(memberName =>
                    changedProps.Contains(memberName, StringComparer.OrdinalIgnoreCase)))
                .ToList();

            // 5. Nếu có lỗi, chặn Request và trả về ErrorResponse chuẩn
            if (relevantErrors.Count != 0)
            {
                var errorList = relevantErrors.Select(e => new ErrorResponse.ValidationError
                {
                    Field = e.MemberNames.FirstOrDefault()?.Replace("Data.", "") ?? "Unknown",
                    Message = e.ErrorMessage ?? "Invalid value"
                }).ToList();

                // Sử dụng static method WithValidationErrors mà Dũng đã định nghĩa trong DTO
                context.Result = new BadRequestObjectResult(
                    ErrorResponse.WithValidationErrors(errorList)
                );
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Không cần xử lý sau khi Action chạy
        }
    }
}
