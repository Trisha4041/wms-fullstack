using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IGenericRepository<Announcement> _announcementRepository;

        public AnnouncementService(IGenericRepository<Announcement> announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<IEnumerable<AnnouncementDTO>> GetAllAsync()
        {
            var announcements = await _announcementRepository.GetAllAsync();
            return announcements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedOn)
                .Select(a => new AnnouncementDTO
                {
                    AnnouncementId = a.AnnouncementId,
                    Title = a.Title,
                    Message = a.Message,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    IsActive = a.IsActive
                });
        }

        public async Task<AnnouncementDTO> CreateAsync(CreateAnnouncementDTO dto)
        {
            var announcement = new Announcement
            {
                Title = dto.Title,
                Message = dto.Message,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            var created = await _announcementRepository.AddAsync(announcement);
            return new AnnouncementDTO
            {
                AnnouncementId = created.AnnouncementId,
                Title = created.Title,
                Message = created.Message,
                CreatedBy = created.CreatedBy,
                CreatedOn = created.CreatedOn,
                IsActive = created.IsActive
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            if (announcement == null) return false;
            announcement.IsActive = false;
            await _announcementRepository.UpdateAsync(announcement);
            return true;
        }
    }
}
