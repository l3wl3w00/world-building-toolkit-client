namespace UI.Common
{
    public class AuthenticationContext
    {
        public string AuthToken { get; }

        public AuthenticationContext(string authToken)
        {
            AuthToken = authToken;
        }
    }
}