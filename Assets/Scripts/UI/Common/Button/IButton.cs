namespace WorldBuilder.Client.UI.Common.Button
{
    public interface IButton
    {
        string Name { get; }
        void OnClicked();
    }
}