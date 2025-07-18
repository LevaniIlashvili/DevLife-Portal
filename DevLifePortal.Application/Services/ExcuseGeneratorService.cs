﻿using DevLifePortal.Application.Contracts.Application;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Application.Validators;
using DevLifePortal.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DevLifePortal.Application.Services
{
    public class ExcuseGeneratorService : IExcuseGeneratorService
    {
        private readonly IDistributedCache _cache;

        private static readonly Dictionary<string, string[]> Templates = new()
        {
            ["Technical"] = new[]
            {
            "სერვერს ცეცხლი გაუჩნდა და უნდა ჩავაქრო.",
            "CI/CD pipeline-ში გაუთვალისწინებელი ლუპია.",
            "VPN ავარიულად ითიშება ყოველ 5 წუთში."
        },
            ["Personal"] = new[]
            {
            "კატამ production-ში შეაღწია და სერვერებს ეძებს.",
            "დედამ დარეკა და ამბობს რომ მეზობელს პრობლემები აქვს Wi-Fi-ით.",
            "სუნთქვას ვერ ვახერხებ... სარელიზო დღეზე ვარ."
        },
            ["Creative"] = new[]
            {
            "AI-მ consciousness შეიძინა და დახმარება სჭირდება.",
            "დროის მარყუჟში მოვხვდი — იგივე შეხვედრა მეორდება.",
            "Matrix-მა მითხრა, რომ ეს შეხვედრა ილუზიაა."
        }
        };

        public ExcuseGeneratorService(IDistributedCache cache)
        {
            _cache = cache;            
        }

        public async Task<Excuse> Generate(ExcuseGeneratorGenerateExcuseDTO excuseDTO)
        {
            var validator = new ExcuseGeneratorGeneratoExcuseDTOValidator();
            var result = await validator.ValidateAsync(excuseDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var rand = new Random();
            var text = Templates.TryGetValue(excuseDTO.Type.ToString(), out var list)
                ? list[rand.Next(list.Length)]
                : "მიუხედავად ყველაფრისა, უბრალოდ ვერ მოვალ.";

            var excuse = new Excuse()
            {
                Id = Guid.NewGuid(),
                Category = excuseDTO.Category,
                Type = excuseDTO.Type.ToString(),
                Text = text
            };

            return excuse;
        }

        public async Task SaveFavoriteAsync(string userId, Excuse excuse)
        {
            var key = $"favorites:{userId}";
            var data = await _cache.GetStringAsync(key) ?? "[]";
            var list = JsonSerializer.Deserialize<List<Excuse>>(data) ?? new();
            list.Add(excuse);
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
        }

        public async Task<List<Excuse>> GetFavoritesAsync(string userId)
        {
            var key = $"favorites:{userId}";
            var data = await _cache.GetStringAsync(key);
            return data != null ? JsonSerializer.Deserialize<List<Excuse>>(data) ?? [] : [];
        }
    }
}
