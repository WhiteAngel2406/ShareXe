using System.Security.Cryptography;
using System.Text;

namespace ShareXe.Base.Helpers
{
    public static  class SecurityHelper
    {
        // 1. Tạo OTP 
        public static string GenerateOtp(int length = 6)
        {
            var random = new Random();
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            return otp;
        }

        // 2. Hash 
        public static string Hash(string text )
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // 3. Mask số điện thoại 
        public static string MaskPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 7)
                return phoneNumber;

            var firstPart = phoneNumber.Substring(0, 3);
            var lastPart = phoneNumber.Substring(phoneNumber.Length - 3, 3);
            return $"{firstPart}****{lastPart}";
        }
    }
}
