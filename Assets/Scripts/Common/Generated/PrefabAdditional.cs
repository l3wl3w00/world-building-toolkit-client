#nullable enable
using System.Linq;

namespace Common.Generated
{
    public readonly partial struct Prefab
    {
        public string Path { get; }
        public string Name => Path.Split("/").Last();
        private Prefab(string path)
        {
            Path = path;
        }
    }
}