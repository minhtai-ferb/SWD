using CursusJapaneseLearningPlatform.Service.BusinessModels.UserModels.Requests;
using CursusJapaneseLearningPlatform.Service.BusinessModels.UserModels.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Service.Interfaces;
public interface IUserService
{
    Task<UserResponseModel> GetUserByIdAsync(Guid id);

    Task<string> UploadAvatar(IFormFile file, Guid userId);

    Task<UserResponseModel> UpdateUserAsync(Guid id, UserUpdateRequestModel userUpdateRequestModel);

    Task<UserResponseModel> GetUserByIdWithAvaterAsync(Guid id);
}