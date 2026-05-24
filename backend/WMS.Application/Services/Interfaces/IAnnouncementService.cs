using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDTO>> GetAllAsync();
        Task<AnnouncementDTO> CreateAsync(CreateAnnouncementDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
