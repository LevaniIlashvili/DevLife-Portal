using DevLifePortal.Application.Contracts.Application;

namespace DevLifePortal.Application.Services
{
    public class CodeRoastService : ICodeRoastService
    {
        private readonly IOpenAiService _openAiService;

        public CodeRoastService(IOpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        public async Task<string> GetProblem(string difficulty)
        {
            var problem = await _openAiService.AskAsync($"return {difficulty} level leetcode problem");

            return problem;
        }

        public async Task<string> RoastCode(string problemTitle, string solution)
        {
            var roast = await _openAiService.AskAsync(
                $@"im gonna give you leetcode problem and my solution, you review my code and 
                praise or roast me in georgian based on the correctness of code, you can get 
                insipartion from the example im gonna give but dont repeat it, come up with your 
                own words, here is the example when solution is bad როუსტინგი თუ ცუდია: ეს კოდი 
                ისე ცუდია, კომპილატორმა დეპრესია დაიწყო, here is example when solution is good 
                შექება თუ კარგია: ბრავო! ამ კოდს ჩემი ბებიაც დაწერდა, მაგრამ მაინც კარგია, 
                here is the problem title {problemTitle}, 
                here is the soluton: {solution}, return only roast or praise nothing else");

            return roast;
        }
    }
}
