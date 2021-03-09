// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace Microsoft.BotFramework.Composer.CustomAction
{
    /// <summary>
    /// Custom command which takes takes 2 data bound arguments (arg1 and arg2) and multiplies them returning that as a databound result.
    /// </summary>
    public class SpotifyPlayArtistRandomDialog : Dialog
    {
        [JsonConstructor]
        public SpotifyPlayArtistRandomDialog([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
            : base()
        {
            // enable instances of this command as debug break point
            this.RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        [JsonProperty("$kind")]
        public const string Kind = "SpotifyPlayArtistRandomDialog";

        [JsonProperty("spotifyToken")]
        public StringExpression SpotifyToken { get; set; }

        [JsonProperty("artist")]
        public StringExpression Artist { get; set; }

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get searched artist
			var artistQuery = Artist.GetValue(dc.State);
            var token = SpotifyToken.GetValue(dc.State);
            // Create Spotify Client
            var spotify = new SpotifyClient(token);

            // Find artist by name and take first match
			var searchResponse = spotify.Search.Item(new SearchRequest(SearchRequest.Types.Artist, artistQuery)).Result;
            var firstArtistId = searchResponse.Artists.Items[0].Id;

            // Get artist top tracks
            var topTracksResponse = spotify.Artists.GetTopTracks(firstArtistId, new ArtistsTopTracksRequest("PL")).Result;

            // Select random track from the top tracks
			Random random = new Random();
            int i = random.Next(topTracksResponse.Tracks.Count);
            var trackToStart = topTracksResponse.Tracks[i];

            // Play track
            var playerResumePlayback = new PlayerResumePlaybackRequest();
            playerResumePlayback.Uris = new List<string> { trackToStart.Uri };
            var isStarted = spotify.Player.ResumePlayback(playerResumePlayback).Result;

            // Return the track as the action result
            var startedArtistName = trackToStart.Artists[0].Name;
            var startedTrackName = trackToStart.Name;
            var result = new StartedTrack(startedArtistName, startedTrackName);
            dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);

            return dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }

        class StartedTrack
        {
            [JsonProperty("artist")]
            public string Artist { get; set; }
            [JsonProperty("track")]
            public string Track { get; set; }

            public StartedTrack(string artist, string track)
            {
                Artist = artist;
                Track = track;
            }
        }
    }
}
