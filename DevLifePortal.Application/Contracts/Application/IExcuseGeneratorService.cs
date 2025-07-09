using DevLifePortal.Domain.Entities;
using DevLifePortal.Domain.Enums;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IExcuseGeneratorService
    {
        Excuse Generate(string category, ExcuseType type);
        Task SaveFavoriteAsync(string userId, Excuse excuse);
        Task<List<Excuse>> GetFavoritesAsync(string userId);
    }
}
