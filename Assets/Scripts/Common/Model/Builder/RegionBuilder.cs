using Common.Model;
using Common.Model.Builder;
using Common.Model.Abstractions;
using Common.Model.Abstractions;
using System;
using System;
using UnityEngine;
using Common.Model;
using System;
using System.Collections.Generic;
namespace Common.Model.Builder
{
public class RegionBuilder : IModelBuilder<Region>
{
	public IdOf<Region> Id{ get; private set; }
	public RegionBuilder WithId(IdOf<Region> value)
	{
		Id = value;
		return this;
	}
	public IdOf<Continent> ContinentId{ get; private set; }
	public RegionBuilder WithContinentId(IdOf<Continent> value)
	{
		ContinentId = value;
		return this;
	}
	public String Name{ get; private set; }
	public RegionBuilder WithName(String value)
	{
		Name = value;
		return this;
	}
	public String Description{ get; private set; }
	public RegionBuilder WithDescription(String value)
	{
		Description = value;
		return this;
	}
	public Color Color{ get; private set; }
	public RegionBuilder WithColor(Color value)
	{
		Color = value;
		return this;
	}
	public RegionType Type{ get; private set; }
	public RegionBuilder WithType(RegionType value)
	{
		Type = value;
		return this;
	}
	public Boolean Inverted{ get; private set; }
	public RegionBuilder WithInverted(Boolean value)
	{
		Inverted = value;
		return this;
	}
	public List<SphereSurfaceCoordinate> GlobalBounds{ get; private set; }
	public RegionBuilder WithGlobalBounds(List<SphereSurfaceCoordinate> value)
	{
		GlobalBounds = value;
		return this;
	}
	public Region Build()
	{
		return new
		(
			Id: Id,
			ContinentId: ContinentId,
			Name: Name,
			Description: Description,
			Color: Color,
			Type: Type,
			Inverted: Inverted,
			GlobalBounds: GlobalBounds
		);
	}
}
public class RegionBuilderHolder : BuilderHolder<Region, RegionBuilder> { }
}
namespace Common.Model
{
	public partial record Region
	{
		public static RegionBuilder Builder() => new RegionBuilder();
	}
}
