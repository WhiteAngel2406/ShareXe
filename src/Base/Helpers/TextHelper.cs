using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace ShareXe.Base.Helpers
{
    public static class TextHelper
    {
        //  "Hà Nội" to "ha-noi"
        public static string ToUnsignedString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }

        // 2. Validate số điện thoại Việt Nam (Regex) 
        // Chấp nhận: 0912345678, +84912345678
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            // Regex đơn giản cho VN: Bắt đầu bằng 0 hoặc 84, theo sau là 9 số
            string pattern = @"^(84|0[3|5|7|8|9])+([0-9]{8})$";
            return Regex.IsMatch(phone, pattern);
        }

        // 3. Chuẩn hóa số điện thoại về dạng chuẩn (09xxx) để lưu DB
        public static string NormalizePhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return null;

            // Xóa khoảng trắng, dấu gạch ngang
            phone = phone.Replace(" ", "").Replace("-", "").Replace(".", "");

            if (phone.StartsWith("+84"))
            {
                phone = "0" + phone.Substring(3);
            }
            else if (phone.StartsWith("84"))
            {
                phone = "0" + phone.Substring(2);
            }

            return phone;
        }
    }
}
