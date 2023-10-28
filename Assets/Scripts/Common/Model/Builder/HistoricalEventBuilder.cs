using Common.Model;
using Common.Model.Builder;
using Common.Model.Abstractions;
namespace Common.Model.Builder
{
public class HistoricalEventBuilder : IModelBuilder<HistoricalEvent>
{
	public IdOf<HistoricalEvent> Id{ get; private set; }
	public HistoricalEventBuilder WithId(IdOf<HistoricalEvent> value)
	{
		Id = value;
		return this;
	}
	public HistoricalEvent Build()
	{
		return new
		(
			Id: Id
		);
	}
}
public class HistoricalEventBuilderHolder : BuilderHolder<HistoricalEvent, HistoricalEventBuilder> { }
}
namespace Common.Model
{
	public partial record HistoricalEvent
	{
		public static HistoricalEventBuilder Builder() => new HistoricalEventBuilder();
	}
}
