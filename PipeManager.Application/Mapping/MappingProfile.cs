using PipeManager.Core.Models;
using AutoMapper;
using PipeManager.Core.Contracts.Requests;
using PipeManager.Core.Contracts.Responses;
using PipeManager.DataAccess.Entites;

namespace PipeManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Pipe -> PipeResponse
        CreateMap<Pipe, PipeResponse>()
            // .ForMember(dest => dest.SteelGradeName,
            //     opt => opt
            //         .MapFrom(src => src.SteelGrade != null ? src.SteelGrade.Name : null))
            .ForMember(dest => dest.PackageNumber,
                opt => opt
                    .MapFrom(src => src.Package != null ? src.Package.Number : null));

        // PipeRequest -> Pipe
        CreateMap<PipeRequest, Pipe>()
            .ConstructUsing(src => Pipe
                .Create(
                    Guid.NewGuid(),
                    src.Label,
                    src.IsGood,
                    src.SteelGradeId,
                    src.Diameter,
                    src.Length,
                    src.Weight).Value);

        // SteelGrade -> SteelGradeResponse
        CreateMap<SteelGrade, SteelGradeResponse>();

        // SteelGradeRequest -> SteelGrade
        CreateMap<SteelGradeRequest, SteelGrade>()
            .ConstructUsing(src => SteelGrade
                .Create(Guid.NewGuid(), src.Name).Value);

        // Package -> PackageResponse
        CreateMap<Package, PackageResponse>();

        // PackageRequest -> Package
        CreateMap<PackageRequest, Package>()
            .ConstructUsing(src => Package
                .Create(Guid.NewGuid(), src.Number, src.Date).Value);

        // PipeEntity -> Pipe
        CreateMap<PipeEntity, Pipe>()
            // .ForMember(dest => dest.SteelGrade, opt => opt.MapFrom(src => src.SteelGrade != null 
            //     ? SteelGrade.Create(src.SteelGrade.Id, src.SteelGrade.Name).Value 
            //     : null))
            .ForMember(dest => dest.Package, opt => opt
                .MapFrom(src => src.Package != null 
                ? Package.Create(src.Package.Id, src.Package.Number, src.Package.Date).Value 
                : null));

        // Pipe -> PipeEntity
        CreateMap<Pipe, PipeEntity>()
            .ForMember(
                dest => dest.SteelGrade,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Package,
                opt => opt.Ignore());

        // SteelGradeEntity <-> SteelGrade
        CreateMap<SteelGradeEntity, SteelGrade>();
        CreateMap<SteelGrade, SteelGradeEntity>();

        // PackageEntity <-> Package
        CreateMap<PackageEntity, Package>();
        CreateMap<Package, PackageEntity>();
    }
}