using System;

namespace WorldBuilder.Client.UI.Common
{
    public class AuthenticationContext
    {
        private readonly String authToken;

        public string AuthToken => authToken;

        public AuthenticationContext(string authToken)
        {
            this.authToken = authToken;
        }
    }
}