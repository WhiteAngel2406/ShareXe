using AutoMapper;

using ShareXe.Base.Attributes;
using ShareXe.Base.Dtos;
using ShareXe.Base.Exceptions;
using ShareXe.Base.Extensions;
using ShareXe.Modules.Auth;
using ShareXe.Modules.Minio.Dtos;
using ShareXe.Modules.Minio.Services;
using ShareXe.Modules.Users.Dtos;
using ShareXe.Modules.Users.Entities;
using ShareXe.Modules.Users.Repositories;
using ShareXe.src.Base.Enums;

namespace ShareXe.Modules.Users.Services
{
    [Injectable]
    public class UsersService(UsersRepository usersRepository, MinioService minioService, UserContext userContext, IMapper mapper)
    {
        public async Task<User> GetCurrentUserAsync()
        {
            var firebaseUid = userContext.FirebaseUid;

            return await usersRepository.GetOneAsync(u => u.Account.FirebaseUid == firebaseUid, "Account")
              ?? throw new AppException(ErrorCode.UserNotFound);
        }

        public async Task<User> UpdateCurrentUserProfileAsync(PatchRequest<UpdateUserProfileDto> updateUserProfileDto)
        {
            var user = await GetCurrentUserAsync();

            user.ApplyPatch(updateUserProfileDto);
            await usersRepository.UpdateAsync(user);

            return user;
        }

        public async Task<User> UpdateCurrentUserAvatarAsync(UpdateAvatarDto updateAvatarDto)
        {
            var user = await GetCurrentUserAsync();
            var folder = $"avatars/{user.Id}";

            var response = await minioService.UploadFileAsync(updateAvatarDto.Avatar, folder);

            user.Avatar = response.FileName;
            await usersRepository.UpdateAsync(user);

            return user;
        }

        public async Task<UserProfileDto> MapToUserProfileDtoAsync(User user)
        {
            var userProfileDto = mapper.Map<UserProfileDto>(user);

            if (!string.IsNullOrEmpty(user.Avatar))
            {
                userProfileDto.Avatar = new MinioFileResponse
                {
                    FileName = user.Avatar,
                    Url = await minioService.GeneratePresignedUrlAsync(user.Avatar)
                };
            }

            return userProfileDto;
        }
    }
}
