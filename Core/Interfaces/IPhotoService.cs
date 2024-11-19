
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IPhotoService
{
     Task<Photo> UploadPhoto(IFormFile file, int ProductId);

     Task<List<PhotoDto>> GetPhotoUrlById(int id);

     Task<string> UploadUserPhoto(IFormFile file, string userId);
}