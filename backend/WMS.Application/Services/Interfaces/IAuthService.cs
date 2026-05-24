using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
        Task<bool> RegisterAsync(RegisterDTO dto);
    }
}
