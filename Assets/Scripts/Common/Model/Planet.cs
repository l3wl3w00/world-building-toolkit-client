#nullable enable
using System;
using Common.Model.Abstractions;
using UnityEngine;

namespace Common.Model
{
    public partial record Planet(
            IdOf<Planet> Id,
            string Name,
            string Description,
            TimeSpan DayLength,
            Color LandColor,
            Color AntiLandColor) 
        : IModel<Planet>
    {
        // public Planet(IdOf<Planet> id, Color landColor, Color antiLandColor, string name = "", string description = "")
        // {
        //     Name = name;
        //     Description = description;
        //     Id = id;
        //     LandColor = landColor;
        //     AntiLandColor = antiLandColor;
        // }
        //
        // public string Name { get; init; }
        // public string Description { get; init; }
        // public IdOf<Planet> Id { get; init; }
        // public Color LandColor { get; init; }
        // public Color AntiLandColor { get; init; }
    }
}