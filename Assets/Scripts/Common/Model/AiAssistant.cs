#nullable enable
using Common.Model.Abstractions;
using UnityEditorInternal;

namespace Common.Model
{
    public record AiAssistant(string Question, Option<string> Answer)
    {
        public static AiAssistant Default() => new(string.Empty, Option<string>.None);
    }
}