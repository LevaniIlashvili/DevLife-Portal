using DevLifePortal.Application.DTOs;

namespace DevLifePortal.Application.Contracts.Application
{
    public interface IDashboardService
    {
        DashboardDTO GenerateDashboard(string username);
    }
}
