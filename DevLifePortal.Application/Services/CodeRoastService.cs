﻿using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Application.Validators;
using FluentValidation;

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

        public async Task<string> RoastCode(CodeRoastSolutionDTO solutionDTO)
        {
            var validator = new CodeRoastSolutionDTOValidator();
            var result = await validator.ValidateAsync(solutionDTO);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var roast = await _openAiService.AskAsync(
                $@"im gonna give you leetcode problem and my solution, you review my code and 
                praise or roast me in georgian based on the correctness of code, you can get 
                insipartion from the example im gonna give but dont repeat it, come up with your 
                own words, here is the example when solution is bad: ეს კოდი 
                ისე ცუდია, კომპილატორმა დეპრესია დაიწყო, here is example when solution is good: 
                ბრავო! ამ კოდს ჩემი ბებიაც დაწერდა, მაგრამ მაინც კარგია, 
                here is the problem title {solutionDTO.ProblemName}, 
                here is the soluton: {solutionDTO.Solution}, return only roast or praise nothing else");

            return roast;
        }
    }
}
