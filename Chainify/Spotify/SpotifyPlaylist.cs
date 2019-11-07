using System;

namespace Chainify.Spotify
{
    public class SpotifyPlaylist
    {
        public Uri PlaylistUri { get; }
        public string Name { get; }
        public bool IsPublic { get; }
        public bool IsCollaborative { get; }
        public string Description { get; }

        public SpotifyPlaylist(Uri playlistUri, string name, bool isPublic, bool isCollaborative, string description)
        {
            PlaylistUri = playlistUri;
            Name = name;
            IsPublic = isPublic;
            IsCollaborative = isCollaborative;
            Description = description;
        }
    }
}