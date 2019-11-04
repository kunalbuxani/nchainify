using Chainify.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chainify.Spotify
{
    public class SpotifyClient
    {
        private readonly SpotifyConfiguration _config;
        private readonly ILogger _log;

        public SpotifyClient(SpotifyConfiguration config, ILogger log)
        {
            _config = config;
            _log = log;
        }

        public async Task RefreshPlaylist(IList<ChainLink> lastMonthsChainLinks, SpotifyPlaylist playlist)
        {
            var accessToken = await GetAccessToken();

            var trackIds = GetTrackIds(lastMonthsChainLinks, accessToken);

            var updatePlayListDescriptionTask = UpdatePlaylistDescription(playlist, accessToken);

            var updatePlaylistTask = UpdatePlaylist(playlist, accessToken, await trackIds);

            await Task.WhenAll(new List<Task> { updatePlayListDescriptionTask, updatePlaylistTask });
        }

        private async Task UpdatePlaylistDescription(SpotifyPlaylist playlist, string accessToken)
        {
            _log.LogInformation($"Updating playlist description for {playlist.PlaylistUri}...");
            var playlistDescription = new
            {
                name = playlist.Name,
                @public = playlist.IsPublic,
                collaborative = playlist.IsCollaborative,
                description = playlist.Description,
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.PutAsync(playlist.PlaylistUri,
                new StringContent(JsonConvert.SerializeObject(playlistDescription)));
            if (response.IsSuccessStatusCode)
                _log.LogInformation("Updated playlist description.");
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _log.LogError($"Error updating playlist description: {response.StatusCode} {errorMessage}", response);
            }
        }

        private async Task UpdatePlaylist(SpotifyPlaylist playlist, string accessToken, List<string> trackIds)
        {
            _log.LogInformation($"Updating Spotify playlist {playlist.PlaylistUri}...");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var payload = new { uris = trackIds };
            var response = await client.PutAsync($"{playlist.PlaylistUri}/tracks",
                new StringContent(JsonConvert.SerializeObject(payload)));
            if (response.IsSuccessStatusCode)
                _log.LogInformation("Updated Spotify playlist");
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _log.LogError($"Error updating playlist: {response.StatusCode} {errorMessage}", response);
            }
        }

        private async Task<List<string>> GetTrackIds(IList<ChainLink> lastMonthsChainLinks, string accessToken)
        {
            var spotifyTrackIds = new List<string>();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            foreach (var chainLink in lastMonthsChainLinks.OrderBy(f => f.Position))
            {
                _log.LogInformation($"Looking for {chainLink.Position} - {chainLink.Artist} - {chainLink.Track}...");
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["q"] = $"artist:{chainLink.Artist} track:{chainLink.Track}";
                query["type"] = "track";
                var responseJson = JObject.Parse(await client.GetStringAsync($"https://api.spotify.com/v1/search?{query}"));

                if (responseJson["tracks"]["total"].Value<int>() != 0)
                {
                    var firstResultTrackId = responseJson["tracks"]["items"].First["id"].Value<string>();
                    _log.LogInformation($"Found {chainLink.Position} - {chainLink.Artist} - {chainLink.Track} ({firstResultTrackId})");
                    spotifyTrackIds.Add($"spotify:track:{firstResultTrackId}");
                }
                else
                    _log.LogInformation($"Couldn't find {chainLink.Position} - {chainLink.Artist} - {chainLink.Track}");
            }

            return spotifyTrackIds;
        }

        private async Task<string> GetAccessToken()
        {
            _log.LogInformation("Getting Spotify access token...");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", _config.EncodedAuthorizationString);
            var tokenResponse = client
                .PostAsync("https://accounts.spotify.com/api/token",
                    new StringContent($"grant_type=refresh_token&refresh_token={_config.RefreshToken}", Encoding.UTF8,
                        "application/x-www-form-urlencoded"));

            var accessToken =
                JsonConvert.DeserializeAnonymousType(await (await tokenResponse).Content.ReadAsStringAsync(), new { access_token = string.Empty }).access_token;

            _log.LogInformation("Got Spotify access token.");

            return accessToken;
        }
    }
}
