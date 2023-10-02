using System;

namespace Game.Client
{
    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete,
        Patch
    }

    public static class HttpMethodImpl
    {
        public static string Name(this HttpMethod httpMethod)
        {
            return httpMethod switch
            {
                HttpMethod.Get => "GET",
                HttpMethod.Post => "POST",
                HttpMethod.Put => "PUT",
                HttpMethod.Delete => "DELETE",
                HttpMethod.Patch => "PATCH",
                _ => throw new ArgumentOutOfRangeException(nameof(httpMethod), httpMethod, null)
            };
        }
    }
}