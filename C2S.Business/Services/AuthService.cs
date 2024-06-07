using C2S.Business.Interfaces;
using C2S.Business.Models;
using C2S.Data.Enumrations;
using C2S.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace C2S.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private const int SaltSize = 16; // 16 bytes salt for SHA256

        public AuthService(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<bool> RegisterAsync(RegisterationRequest registerationModel, Role role)
        {
            if (await UserExistsAsync(registerationModel.Email))
                return default; // User already exists

            var newUser = new User
            {
                Email = registerationModel.Email,
                PasswordHash = GeneratePasswordHash(registerationModel.Password),
                Id = Guid.NewGuid(),
                Name = registerationModel.Name,
                Address = registerationModel.Address,
                Phone = registerationModel.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ProfilePicture = registerationModel.ProfilePicture,
                Role = role,
            };

            var identityUserCreated = await CreateIdentityUser(registerationModel);
            if(identityUserCreated)
            {
                _context.Users.Add(newUser);
                var saved = await _context.SaveChangesAsync();
                if (saved > 0)
                    return true;
            }
            return false;
        }

        public async Task<bool> LoginAsync([FromBody]LoginModel loginModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user == null)
                return false; // User not found

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
                return false; // Incorrect password

            return true;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }



        public static string GeneratePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Generate a random salt
                byte[] salt = new byte[SaltSize];
                new RNGCryptoServiceProvider().GetBytes(salt);

                // Compute hash with salt
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));
                byte[] hash = sha256.ComputeHash(saltedPassword);

                // Combine salt and hash
                byte[] saltedHash = new byte[SaltSize + hash.Length];
                Array.Copy(salt, 0, saltedHash, 0, SaltSize);
                Array.Copy(hash, 0, saltedHash, SaltSize, hash.Length);

                // Convert to base64 string for storage
                return Convert.ToBase64String(saltedHash);
            }
        }

        public static bool VerifyPasswordHash(string password, string storedHash)
        {
            byte[] saltedHash = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[SaltSize];
            Array.Copy(saltedHash, 0, salt, 0, SaltSize);

            using (var sha256 = SHA256.Create())
            {
                // Compute hash with stored salt
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));
                byte[] hash = sha256.ComputeHash(saltedPassword);

                // Compare computed hash with stored hash
                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != saltedHash[i + SaltSize])
                        return false;
                }
                return true;
            }
        }

        private async Task<bool> CreateIdentityUser(RegisterationRequest request)
        {
            var role = _roleManager.Roles.FirstOrDefault(a => a.Name.Contains(request.Role.ToString()));
            if (role == null)
            {
                return false;
            }

            var identityUserModel = GetIdentityUserModel(request);
            var createUserResponse = await _userManager.CreateAsync(identityUserModel, request.Password);
            if (createUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUserModel, role.Name);
                return true;
            }
            return false;
        }

        private IdentityUser GetIdentityUserModel(BaseModel request)
        {
            return new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email,
            };
        }

    }
}
