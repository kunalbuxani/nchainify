# Chainify

Chainify is a set of functions that publishes a monthly playlist of songs from
[The Chain](https://www.thechain.uk/), a feature on the Radcliffe & Maconie show on BBC Radio 6, to a [Spotify playlist updated monthly](https://open.spotify.com/playlist/1wASzPsoJvtW3HrjMFsYBN). 
Each song in the playlist is somehow linked - in either an obvious or obscure way - to the previous one.

# Implementation

This project is an exercise to learn about cloud-first computing. 
There are two Azure functions; one that pulls data from The Chain's RSS feed and
persists it to an Azure Table, and another that takes each song - or chain link - 
in the table and refreshes 
[my public Spotify playlist](https://open.spotify.com/playlist/1wASzPsoJvtW3HrjMFsYBN).

# GetChainFeed function

The show is broadcast on the mornings of Saturday and Sunday, 
and two new chain links are added each show.
[The website](https://www.thechain.uk/) has an RSS feed containing the last 10 chain links.
So the function is scheduled to run at noon every Sunday to get that weekend's additions. 
The title of the RSS article contains the position in the chain, which we use as the key, 
the artist and the track name.
Those three pieces along with the publish date are stored in an Azure table.

# UpdateTheChainMonthlyPlaylist function

At four songs a week, every month you get roughly 16 tracks. This is a good length
for a playlist, so the function is scheduled to run at the start of every month
and gets the previous month's chain links from table storage 
(based on the RSS publish date). We search for each track using the 
Spotify API and replace the existing public playlist with the search results.

# Environment Settings

The functions require a few environment variables. An example
`local.settings.json` with fake values is shown below, which can be for local testing but 
should be excluded from source control.
When deployed to Azure, the environment variables
are pulled from the [application settings for the function app](https://docs.microsoft.com/bs-latn-ba/azure/azure-functions/functions-how-to-use-azure-function-app-settings).

## Settings

| Environment Setting | Purpose |
| ------------------- | ------- |
| SpotifyClientId     | The client ID issued by Spotify for the application |
| SpotifyClientSecret | The secret key issued by Spotify for the application |
| SpotifyRefreshToken | The refresh token issued by Spotify to obtain a temporary access token (see [Spotify Authorization Code Flow](https://developer.spotify.com/documentation/general/guides/authorization-guide/#authorization-code-flow)) |
| SpotifyPlaylistUri  | The API URI for the playlist to update |

## Example settings file

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "SpotifyClientId": "1a2b3c",
    "SpotifyClientSecret": "f3e2d1",
    "SpotifyRefreshToken": "1A2-Qhe-ffa",
    "SpotifyPlaylistUri": "https://api.spotify.com/v1/playlists/1BaCdD2"
  }
}
```
