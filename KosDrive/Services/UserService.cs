using KosDrive.Data;
using KosDrive.DTOs;
using KosDrive.Interfaces;
using KosDrive.Models;
using Microsoft.AspNetCore.Identity;

namespace KosDrive.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context, IWebHostEnvironment enviroment, UserManager<User> userManager)
        {
            _context = context;
            _enviroment = enviroment;   
            _userManager = userManager; 
        }

        public async Task<OperationResult<UserProfileResponse>> DeleteAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return OperationResult<UserProfileResponse>.Failure("Couldn't find the User");

            var profile = new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber
            };

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return OperationResult<UserProfileResponse>.Failure("Error deleting the user");

            return OperationResult<UserProfileResponse>.Success(profile);
        }

        public async Task<OperationResult<UserProfileResponse>> UpdateUserProfileAsync(string userId, UpdateUserProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return OperationResult<UserProfileResponse>.Failure("User not found");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.Phone;
            user.Email = dto.Email;

            if(dto.ProfileImage != null && dto.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_enviroment.WebRootPath, "uploads/profiles");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{dto.ProfileImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProfileImage.CopyToAsync(fileStream);
                }
                user.ProfileImagePath = $"/uploads/profiles/{uniqueFileName}";
            }

            await _userManager.UpdateAsync(user);
            var response = new UserProfileResponse
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = user.PhoneNumber,
                ImageUrl = user.ProfileImagePath
            };

            return OperationResult<UserProfileResponse>.Success(response);
        }
    }
}
