#nullable enable
namespace Common.Model.Builder
{
    public partial class HistoricalEventBuilder
    {
        protected override void OnConstruct()
        {
            base.OnConstruct();
            this.WithDescription(string.Empty)
                .WithName(string.Empty)
                .WithBeginning(Date.Default())
                .WithEnd(Date.Default());
        }
    }
}