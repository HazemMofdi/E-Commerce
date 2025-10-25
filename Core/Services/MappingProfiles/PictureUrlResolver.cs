using AutoMapper;
using Domain.Entities.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductResultDTO, string>
    {
        // IConfiguration is injected based on a package installed from the nuget package (Microsoft.Extensions.Configuration)
        public string Resolve(Product source, ProductResultDTO destination, string destMember, ResolutionContext context)
        {
            //(src => $"https://localhost:7290/{src.PictureUrl}")
            if (string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;
            return $"{configuration.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
