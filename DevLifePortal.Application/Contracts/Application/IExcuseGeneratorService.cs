using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IExcuseGeneratorService
    {
        Excuse Generate(string category, string type);
        Task SaveFavoriteAsync(string userId, Excuse excuse);
        Task<List<Excuse>> GetFavoritesAsync(string userId);
    }
}
