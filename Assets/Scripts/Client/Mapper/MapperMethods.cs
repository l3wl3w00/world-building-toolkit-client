#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Client.Dto;
using Common;
using Common.Geometry.Coordinate._3D;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Model.Abstractions;
using UnityEngine;

namespace Client.Mapper
{
    public static class MapperMethods
    {
        public static ICollection<Continent> ToModels(this IEnumerable<ContinentDto> dtos)
        {
            return dtos.Select(ToModel).ToHashSet();
        }
        public static Continent ToModel(this ContinentDto dto)
        {
            var bounds = dto.Bounds
                .Select(c => new SphereSurfaceCoordinate(c.Radius, c.Polar.AsRadians(), c.Azimuthal.AsRadians()))
                .ToList();
            Option<IdOf<Continent>> parentIdOpt;
            if (dto.ParentContinentId == Guid.Empty)
            {
                parentIdOpt = Option<IdOf<Continent>>.None;
            }
            else
            {
                parentIdOpt = dto.ParentContinentId.ToTypesafe<Continent>();
            }
            return new Continent(
                Id: dto.Id.ToTypesafe<Continent>(),
                ParentIdOpt: parentIdOpt,
                Name: dto.Name,
                Description: dto.Description,
                Inverted: dto.Inverted,
                Regions: dto.Regions.ToModels(),
                GlobalBounds: bounds);
        }
        
        public static Region ToModel(this RegionDto dto)
        {
            var bounds = dto.Bounds
                .Select(c => new SphereSurfaceCoordinate(c.Radius, c.Polar.AsRadians(), c.Azimuthal.AsRadians()))
                .ToList();

            return new Region(
                Id: dto.Id.ToTypesafe<Region>(),
                ContinentId: dto.ContinentId.ToTypesafe<Continent>(),
                Type: dto.RegionType,
                Name: dto.Name,
                Color: dto.Color.ToUnityColor(), 
                Description: dto.Description,
                Inverted: dto.Inverted,
                GlobalBounds: bounds);
        }
        
        public static ICollection<Calendar> ToModels(this IEnumerable<CalendarDto> dtos) => dtos.Select(ToModel).ToHashSet();
        public static Calendar ToModel(this CalendarDto dto) =>
            new(
                Id: dto.Id.ToTypesafe<Calendar>(),
                PlanetId: dto.PlanetId.ToTypesafe<Planet>(),
                Name: dto.Name,
                Description: dto.Description,
                FirstYear: dto.FirstYear,
                YearPhases: dto.YearPhases);

        public static Planet ToModel(this WorldDetailedDto dto) =>
            new 
            (
                Id: dto.Id.ToTypesafe<Planet>(), 
                Name: dto.Name, 
                Description: dto.Description,
                AntiLandColor: dto.AntiLandColor.ToUnityColor(),
                LandColor: dto.LandColor.ToUnityColor()
            );


        public static Color ToUnityColor(this ColorDto dto)
        {
            return new Color(
                ToUnityColorSpace(dto.r),
                ToUnityColorSpace(dto.g), 
                ToUnityColorSpace(dto.b),
                ToUnityColorSpace(dto.a));
            
            float ToUnityColorSpace(ushort value) => value / 255f;
        }
        
        public static ColorDto ToDto(this Color unityColor)
        {
            return new ColorDto(
                ToDomainColorSpace(unityColor.r),
                ToDomainColorSpace(unityColor.g), 
                ToDomainColorSpace(unityColor.b),
                ToDomainColorSpace(unityColor.a));
            
            byte ToDomainColorSpace(float value) => (byte) Mathf.RoundToInt(value * 255);
        }
        public static ICollection<Region> ToModels(this IEnumerable<RegionDto> dtos)
        {
            return dtos.Select(d => d.ToModel()).ToHashSet();
        }
        public static CreateContinentDto ToCreateContinentDto(List<SphereSurfaceCoordinate> coordinates, IdOf<Continent> parentId)
        {
            return new CreateContinentDto
            (
                Name: "new continent",
                Description: "",
                ParentContinentId: parentId.Value,
                Bounds: coordinates
                    .Select(b => new PlanetCoordinateDto(b.Height, b.Polar.ToRadians(), b.Azimuthal.ToRadians()))
                    .ToList()
            );
        }

        public static CreateRegionDto ToCreateRegionDto(List<SphereSurfaceCoordinate> coordinates,
            RegionType regionType, Color color)
        {
            return new CreateRegionDto 
            (
                Name: "new region",
                Description: "",
                RegionType: regionType,
                Color: color.ToDto(),
                Bounds: coordinates
                    .Select(b => new PlanetCoordinateDto(b.Height, b.Polar.ToRadians(), b.Azimuthal.ToRadians()))
                    .ToList()
            );
        }
    }
}