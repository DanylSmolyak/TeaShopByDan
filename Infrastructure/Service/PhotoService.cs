using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Service;

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IRepository<Photo> _context;
    private readonly UserManager<User> _userManager;

    public PhotoService(IWebHostEnvironment environment, IRepository<Photo> context, UserManager<User> userManager)
    {
        _environment = environment;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Photo> UploadPhoto(IFormFile file, int ProductId)
    {
        var uploads = Path.Combine(_environment.WebRootPath, "uploads");
        var filePath = Path.Combine(uploads, file.FileName);
        
        var relativePath = Path.GetRelativePath(_environment.WebRootPath, filePath);
        
        var normalizedPath = relativePath.Replace("\\", "/");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        var photo = new Photo
        {
            Image = normalizedPath,
            ProductId = ProductId
        };
        await _context.AddAsync(photo);

        return photo;
    }

    
    public async Task<List<PhotoDto>> GetPhotoUrlById(int id)
    {
        var photos = await _context.ListAsync(x => x.ProductId == id);
        var returnedPhotosList = new List<PhotoDto>();
        foreach (var photo in photos)
        {
            var photoDto = new PhotoDto
            {
                Image = "http://localhost:5097/" + photo.Image ,
                ProductId = photo.ProductId
            };
            returnedPhotosList.Add(photoDto);
        }
     
        return returnedPhotosList;
    }
    
    public async Task<string> UploadUserPhoto(IFormFile file, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return "User not found";

        var uploads = Path.Combine(_environment.WebRootPath, "user_uploads");
        var filePath = Path.Combine(uploads, file.FileName);
        var relativePath = Path.GetRelativePath(_environment.WebRootPath, filePath).Replace("\\", "/");

        Directory.CreateDirectory(uploads);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        user.Image = relativePath;
        await _userManager.UpdateAsync(user);

        return "http://localhost:5097/" + relativePath;
    }


}