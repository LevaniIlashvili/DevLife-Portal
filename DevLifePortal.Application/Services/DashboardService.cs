using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;

namespace DevLifePortal.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private static readonly List<string> Horoscopes = new()
        {
            "Today is a great day to debug that tricky issue!",
            "Your creativity will shine in solving complex problems.",
            "Expect a surprise code review with positive feedback.",
            "Patience will help you conquer today’s coding challenges.",
            "New opportunities are coming, keep your IDE ready."
        };

        private static readonly List<string> CodeAdvices = new()
        {
            "Remember to write unit tests for your new features.",
            "Keep your code DRY — don't repeat yourself.",
            "Use meaningful variable names for better readability.",
            "Refactor small pieces often to keep code clean.",
            "Optimize your algorithms for better performance."
        };

        private static readonly List<string> LuckyTechnologies = new()
        {
            "React",
            "Docker",
            ".NET 8",
            "Redis",
            "PostgreSQL",
            "TypeScript"
        };

        private readonly Random _random;

        public DashboardService()
        {
            _random = new Random();
        }

        public DashboardDTO GenerateDashboard(string username)
        {
            return new DashboardDTO
            {
                DailyHoroscope = GetRandomItem(Horoscopes),
                DailyCodeAdvice = GetRandomItem(CodeAdvices),
                LuckyTechnologyOfTheDay = GetRandomItem(LuckyTechnologies)
            };
        }

        private string GetRandomItem(List<string> list)
        {
            int index = _random.Next(list.Count);
            return list[index];
        }
    }
}
