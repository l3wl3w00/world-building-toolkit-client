#nullable enable
namespace Client.Dto
{
    public record ColorDto(byte r, byte g, byte b, byte a = byte.MaxValue);
}