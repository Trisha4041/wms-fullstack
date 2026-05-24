using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserLoginRepository userLoginRepository, IConfiguration configuration)
        {
            _userLoginRepository = userLoginRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userLoginRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
                throw new Exception("Invalid username or password");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            user.LastLogin = DateTime.Now;
            await _userLoginRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                Username = user.Username,
                Role = user.Role.RoleName,
                UserId = user.UserId,
                EmployeeId = user.EmployeeId,
                Expiry = DateTime.Now.AddHours(8)
            };
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var existing = await _userLoginRepository.GetByUsernameAsync(dto.Username);
            if (existing != null)
                throw new Exception("Username already exists");

            var user = new UserLogin
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                EmployeeId = dto.EmployeeId
            };

            await _userLoginRepository.AddAsync(user);
            return true;
        }

        private string GenerateJwtToken(UserLogin user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("EmployeeId", user.EmployeeId?.ToString() ?? "0"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
