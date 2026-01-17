using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Enums;
using ShareXe.Base.Exceptions;
using ShareXe.Modules.Auth.Entities;
using ShareXe.Modules.Auth.Repositories;
using ShareXe.Modules.Minio.Dtos;
using ShareXe.Modules.Minio.Services;
using ShareXe.Modules.Users.Dtos;
using ShareXe.Modules.Users.Entities;

namespace ShareXe.Modules.Auth.Services
{
    [Injectable]
    public class AuthService(
      AccountsRepository accountsRepository,
      UserContext userContext,
      HttpClient httpClient,
      ILogger<AuthService> logger,
      IMapper mapper,
      MinioService minioService,
      IConfiguration config)
    {
        public async Task<Account> SyncAccountFromFirebaseAsync()
        {
            var firebaseUid = userContext.FirebaseUid;

            if (string.IsNullOrEmpty(firebaseUid))
            {
                throw new AppException(ErrorCode.Unauthorized, "Missing required Firebase claims.");
            }

            var fullName = userContext.FullName;
            var email = userContext.Email;
            var avatar = userContext.Avatar;

            var account = await accountsRepository.GetByFirebaseUidAsync(firebaseUid);

            if (account == null)
            {
                logger.LogInformation("Creating new account for Firebase UID: {FirebaseUid}", firebaseUid);

                account = new Account
                {
                    FirebaseUid = firebaseUid,
                    Email = email,
                    Role = Role.Passenger,
                    IsActive = true,
                    User = new User
                    {

                        FullName = fullName,
                        Avatar = avatar
                    }
                };

                await accountsRepository.AddAsync(account);
            }
            else
            {
                if (!account.IsActive)
                {
                    throw new AppException(ErrorCode.UserBanned, "This account has been banned.");
                }

                account.Email = email;
                if (account.User != null)
                {
                    account.User.FullName = fullName;
                    account.User.Avatar = avatar;
                }
                await accountsRepository.UpdateAsync(account);
            }

            return account;
        }

        public async Task<Account> GetCurrentAccountAsync()
        {
            var firebaseUid = userContext.FirebaseUid;

            if (string.IsNullOrEmpty(firebaseUid))
            {
                throw new AppException(ErrorCode.Unauthorized, "No authenticated user.");
            }

            var account = await accountsRepository.GetByFirebaseUidAsync(firebaseUid, "User");

            if (account == null)
            {
                throw new AppException(ErrorCode.Unauthorized, "Account not found.");
            }

            return account;
        }

        public async Task<string> SignInWithEmailPasswordAsync(string email, string password)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={config["FIREBASE_API_KEY"]}";

            var payload = new { email, password, returnSecureToken = true };
            var response = await httpClient.PostAsJsonAsync(url, payload);

            if (!response.IsSuccessStatusCode)
            {
                throw new AppException(ErrorCode.Unauthorized, "Invalid email or password.");
            }

            var data = await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>();
            return data!.IdToken;
        }

        public async Task<UserProfileDto> MapToUserProfileDtoAsync(Account account)
        {
            var userProfileDto = mapper.Map<UserProfileDto>(account);

            if (!string.IsNullOrEmpty(account.User?.Avatar))
            {
                userProfileDto.Avatar = new MinioFileResponse
                {
                    FileName = account.User.Avatar,
                    Url = await minioService.GeneratePresignedUrlAsync(account.User.Avatar)
                };
            }

            return userProfileDto;
        }
    }

    public class FirebaseSignInResponse
    {
        public string IdToken { get; set; } = null!;
    }
}
