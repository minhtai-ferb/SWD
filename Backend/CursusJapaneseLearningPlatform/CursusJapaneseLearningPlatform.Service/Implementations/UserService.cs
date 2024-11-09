using CursusJapaneseLearningPlatform.Repository.Interfaces;
using CursusJapaneseLearningPlatform.Service.BusinessModels.UserModels.Responses;
using CursusJapaneseLearningPlatform.Service.Commons.Exceptions;
using CursusJapaneseLearningPlatform.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using CursusJapaneseLearningPlatform.Service.Commons.Interfaces;
using CursusJapaneseLearningPlatform.Service.BusinessModels.UserModels.Requests;
using CursusJapaneseLearningPlatform.Repository.Entities;

namespace CursusJapaneseLearningPlatform.Service.Implementations;
public class UserService : IUserService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAWSS3Service _awsS3Service;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAWSS3Service aWSS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = aWSS3Service;
    }
    public async Task<UserResponseModel> GetUserByIdAsync(Guid id)
    {
        try
        {
            var existingUser = await _unitOfWork.UserRepository.GetByIdAllAsync(id) ?? throw new CustomException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, ResponseMessages.NOT_FOUND);
            var user = _mapper.Map<UserResponseModel>(existingUser);
            return user;
        }
        catch (CustomException)
        {
            _unitOfWork.RollBack();
            throw;
        }
        catch (Exception)
        {
            _unitOfWork.RollBack();
            throw new CustomException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessages.INTERNAL_SERVER_ERROR);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    public async Task<string> UploadAvatar(IFormFile file, Guid userId)
    {
        try
        {
            if (!_awsS3Service.IsImageFile(file))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FILE_UPLOAD_INVALID, $"{ResponseMessages.UPDATE_FAIL.Replace("{0}", "FILE")}");
            }
            var existingUser = await _unitOfWork.UserRepository.GetByIdAllAsync(userId) ?? throw new CustomException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, ResponseMessages.NOT_FOUND);
            if (existingUser.ImagePath != null)
            {
                string key = existingUser.ImagePath;
                //kiểm tra trên cloud đã có chưa đã
                if (_awsS3Service.IsImagePathValid(key))
                {
                    //nếu có thì xóa file cũ
                    bool check = await _awsS3Service.DeleteFileAsync(key);
                    if (!check)
                    {
                        throw new CustomException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessages.INTERNAL_SERVER_ERROR);
                    }
                }
            }

            var keyUpload = await _awsS3Service.UploadFileAsync(file, userId.ToString());
            existingUser.ImagePath = keyUpload;
            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveAsync();
            return keyUpload;
        }
        catch (CustomException)
        {
            _unitOfWork.RollBack();
            throw;
        }
        catch (Exception)
        {
            _unitOfWork.RollBack();
            throw new CustomException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessages.INTERNAL_SERVER_ERROR);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    public async Task<UserResponseModel> UpdateUserAsync(Guid id, UserUpdateRequestModel userUpdateRequestModel)
    {
        try
        {
            var existingUser = await _unitOfWork.UserRepository.GetByIdAllAsync(id) ?? throw new CustomException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, ResponseMessages.NOT_FOUND);
            existingUser.FullName = userUpdateRequestModel.FullName;
            existingUser.LastUpdatedTime = DateTimeOffset.UtcNow;
            existingUser.LastUpdatedBy = existingUser.FullName.ToString();
            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveAsync();
            var user = _mapper.Map<UserResponseModel>(existingUser);
            return user;
        }
        catch (CustomException)
        {
            _unitOfWork.RollBack();
            throw;
        }
        catch (Exception)
        {
            _unitOfWork.RollBack();
            throw new CustomException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessages.INTERNAL_SERVER_ERROR);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    public async Task<UserResponseModel> GetUserByIdWithAvaterAsync(Guid id)
    {
        try
        {
            var existingUser = await _unitOfWork.UserRepository.GetByIdAllAsync(id) ?? throw new CustomException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, ResponseMessages.NOT_FOUND);
            var user = _mapper.Map<UserResponseModel>(existingUser);
            string url = null;
            if (user.ImagePath != null)
            {
                url = await _awsS3Service.GetFileUrl(user.ImagePath, 60);
            }
            if (url != null)
            {
                user.ImagePath = url;
            }
            return user;
        }
        catch (CustomException)
        {
            _unitOfWork.RollBack();
            throw;
        }
        catch (Exception)
        {
            _unitOfWork.RollBack();
            throw new CustomException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessages.INTERNAL_SERVER_ERROR);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}