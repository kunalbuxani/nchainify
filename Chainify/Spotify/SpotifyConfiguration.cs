using System;
using System.Text;

namespace Chainify.Spotify
{
    public class SpotifyConfiguration
    {
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RefreshToken { get; }
        public string EncodedAuthorizationString => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

        public SpotifyConfiguration(string clientId, string clientSecret, string refreshToken)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
        }
    }
}