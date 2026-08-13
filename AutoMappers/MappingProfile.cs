using System;
using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.AutoMappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BeerInsertDto, Beer>();
        CreateMap<Beer, BeerDto>()
            .ForMember(
                origin => origin.Id, // Origin Field
                map => map.MapFrom(destination => destination.BeerId) // Destination field
            );
        CreateMap<BeerUpdateDto, Beer>();
    }
}

