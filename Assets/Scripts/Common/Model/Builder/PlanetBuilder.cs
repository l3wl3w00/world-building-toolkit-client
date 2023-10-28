using Common.Model;
using Common.Model.Builder;
using Common.Model.Abstractions;
using System;
using System;
using UnityEngine;
using UnityEngine;
namespace Common.Model.Builder
{
public class PlanetBuilder : IModelBuilder<Planet>
{
	public IdOf<Planet> Id{ get; private set; }
	public PlanetBuilder WithId(IdOf<Planet> value)
	{
		Id = value;
		return this;
	}
	public String Name{ get; private set; }
	public PlanetBuilder WithName(String value)
	{
		Name = value;
		return this;
	}
	public String Description{ get; private set; }
	public PlanetBuilder WithDescription(String value)
	{
		Description = value;
		return this;
	}
	public Color LandColor{ get; private set; }
	public PlanetBuilder WithLandColor(Color value)
	{
		LandColor = value;
		return this;
	}
	public Color AntiLandColor{ get; private set; }
	public PlanetBuilder WithAntiLandColor(Color value)
	{
		AntiLandColor = value;
		return this;
	}
	public Planet Build()
	{
		return new
		(
			Id: Id,
			Name: Name,
			Description: Description,
			LandColor: LandColor,
			AntiLandColor: AntiLandColor
		);
	}
}
public class PlanetBuilderHolder : BuilderHolder<Planet, PlanetBuilder> { }
}
namespace Common.Model
{
	public partial record Planet
	{
		public static PlanetBuilder Builder() => new PlanetBuilder();
	}
}
