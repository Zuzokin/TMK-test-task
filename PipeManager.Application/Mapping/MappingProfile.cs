using PipeManager.Core.Models;
using AutoMapper;
using PipeManager.Core.Contracts.Requests;
using PipeManager.Core.Contracts.Responses;

namespace PipeManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Pipe, PipeResponse>()
            .ForMember(dest => dest.SteelGradeName,
                opt 
                    => opt.MapFrom(src => src.SteelGrade.Name))
            .ForMember(dest => dest.PackageNumber, 
                opt => opt.MapFrom(src => src.Package.Number));
        
        // Игнорируем Id, т.к. он будет задан в контроллере
        CreateMap<PipeRequest, Pipe>()
            .ForMember(dest => dest.Id, 
                opt => opt.Ignore());
        
        CreateMap<SteelGrade, SteelGradeResponse>(); 

        CreateMap<SteelGradeRequest, SteelGrade>() 
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Игнорируем Id, так как он задается на сервере
        
        CreateMap<Package, PackageResponse>(); // Маппинг для ответа клиенту

        CreateMap<PackageRequest, Package>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}