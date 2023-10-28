using Common.Model;
using Common.Model.Builder;
using Common.Model.Abstractions;
using Common;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
namespace Common.Model.Builder
{
public class ContinentBuilder : IModelBuilder<Continent>
{
	public IdOf<Continent> Id{ get; private set; }
	public ContinentBuilder WithId(IdOf<Continent> value)
	{
		Id = value;
		return this;
	}
	public Option<IdOf<Continent>> ParentIdOpt{ get; private set; }
	public ContinentBuilder WithParentIdOpt(Option<IdOf<Continent>> value)
	{
		ParentIdOpt = value;
		return this;
	}
	public String Name{ get; private set; }
	public ContinentBuilder WithName(String value)
	{
		Name = value;
		return this;
	}
	public String Description{ get; private set; }
	public ContinentBuilder WithDescription(String value)
	{
		Description = value;
		return this;
	}
	public Boolean Inverted{ get; private set; }
	public ContinentBuilder WithInverted(Boolean value)
	{
		Inverted = value;
		return this;
	}
	public ICollection<Region> Regions{ get; private set; }
	public ContinentBuilder WithRegions(ICollection<Region> value)
	{
		Regions = value;
		return this;
	}
	public List<SphereSurfaceCoordinate> GlobalBounds{ get; private set; }
	public ContinentBuilder WithGlobalBounds(List<SphereSurfaceCoordinate> value)
	{
		GlobalBounds = value;
		return this;
	}
	public Continent Build()
	{
		return new
		(
			Id: Id,
			ParentIdOpt: ParentIdOpt,
			Name: Name,
			Description: Description,
			Inverted: Inverted,
			Regions: Regions,
			GlobalBounds: GlobalBounds
		);
	}
}
public class ContinentBuilderHolder : BuilderHolder<Continent, ContinentBuilder> { }
}
namespace Common.Model
{
	public partial record Continent
	{
		public static ContinentBuilder Builder() => new ContinentBuilder();
	}
}
