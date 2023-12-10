#nullable enable
namespace Client.Dto
{
    public record AiPromptDto(string Prompt) : JsonSerializable<AiPromptDto>;
    public record AiAnswerDto(string Answer) : JsonSerializable<AiAnswerDto>;
}