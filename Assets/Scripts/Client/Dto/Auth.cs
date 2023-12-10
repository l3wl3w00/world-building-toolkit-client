namespace Client.Dto
{
    public enum LoginType
    {
        ByEmail,
        ByUsername
    }
    public record LoginDto(LoginType LoginType, string UsernameOrEmail, string Password) : JsonSerializable<LoginDto>;
    public record RegisterDto(string Email, string Username, string Password) : JsonSerializable<RegisterDto>;
    
    public record UserIdentityDto(string Username, string Email) : JsonSerializable<UserIdentityDto>;
    public record UserIdentityDtoWithToken(string Username, string Email, string Token) : JsonSerializable<UserIdentityDtoWithToken>;
}