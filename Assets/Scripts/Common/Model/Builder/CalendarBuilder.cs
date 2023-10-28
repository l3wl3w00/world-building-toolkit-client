using Common.Model;
using Common.Model.Builder;
using Common.Model.Abstractions;
using Common.Model.Abstractions;
using System;
using System;
using System;
using System.Collections.Generic;
namespace Common.Model.Builder
{
public class CalendarBuilder : IModelBuilder<Calendar>
{
	public IdOf<Calendar> Id{ get; private set; }
	public CalendarBuilder WithId(IdOf<Calendar> value)
	{
		Id = value;
		return this;
	}
	public IdOf<Planet> PlanetId{ get; private set; }
	public CalendarBuilder WithPlanetId(IdOf<Planet> value)
	{
		PlanetId = value;
		return this;
	}
	public String Name{ get; private set; }
	public CalendarBuilder WithName(String value)
	{
		Name = value;
		return this;
	}
	public String Description{ get; private set; }
	public CalendarBuilder WithDescription(String value)
	{
		Description = value;
		return this;
	}
	public UInt32 FirstYear{ get; private set; }
	public CalendarBuilder WithFirstYear(UInt32 value)
	{
		FirstYear = value;
		return this;
	}
	public List<YearPhase> YearPhases{ get; private set; }
	public CalendarBuilder WithYearPhases(List<YearPhase> value)
	{
		YearPhases = value;
		return this;
	}
	public Calendar Build()
	{
		return new
		(
			Id: Id,
			PlanetId: PlanetId,
			Name: Name,
			Description: Description,
			FirstYear: FirstYear,
			YearPhases: YearPhases
		);
	}
}
public class CalendarBuilderHolder : BuilderHolder<Calendar, CalendarBuilder> { }
}
namespace Common.Model
{
	public partial record Calendar
	{
		public static CalendarBuilder Builder() => new CalendarBuilder();
	}
}
