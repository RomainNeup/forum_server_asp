using ForumAPI.Repositories;
using ForumAPI.Repositories.Models;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ForumAPI.Services
{
    public class AuthService
    {
        private readonly UserRepository _usersRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, UserRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
            _userManager = userManager;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (user.Email == null || await _usersRepository.GetByEmail(user.Email) != null)
            {
                throw new ArgumentException($"User with email {user.Email} already exists.");
            }

            user.Password = HashPassword(user.Password);
            return await _usersRepository.CreateAsync(user);
        }

        public async Task<JwtSecurityToken> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            var secretKey = _configuration["JWT:SecretKey"];
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password) || user.Email == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            if (secretKey == null)
            {
                throw new Exception("JWT:SecretKey is not set in appsettings.json");
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

    }
}