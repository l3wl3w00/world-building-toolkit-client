#nullable enable
namespace Game.SceneChange
{
    public class EmptySceneChangeParameters : ISceneChangeParameters
    {
        public void Add(SceneParamKey key, object value)
        {
        }

        public void Destroy()
        {
        }

        public T Get<T>(SceneParamKey key)
        {
            return default;
        }
    }
}