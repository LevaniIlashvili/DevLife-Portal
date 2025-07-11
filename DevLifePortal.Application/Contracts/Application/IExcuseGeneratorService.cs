using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IExcuseGeneratorService
    {
        Task<Excuse> Generate(ExcuseGeneratorGenerateExcuseDTO excuseDTO);
        Task SaveFavoriteAsync(string userId, Excuse excuse);
        Task<List<Excuse>> GetFavoritesAsync(string userId);
    }
}
