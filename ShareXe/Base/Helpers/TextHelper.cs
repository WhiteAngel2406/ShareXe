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
    }
}
