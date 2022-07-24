using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrumTrainer.Spotify
{
    public class SpotifyService
    {
        private Token _lastToken;
        private SpotifyWebAPI _spotify;
        private bool _authenticated;
        private AuthorizationCodeAuth _authenticationProvider;
        public void StartUp()
        {
            _authenticationProvider = new AuthorizationCodeAuth("<CODE>", "<CODE>", "http://localhost:8423", "http://localhost:8423",
                   Scope.UserReadPrivate | Scope.PlaylistReadPrivate | Scope.Streaming | Scope.PlaylistReadPrivate | Scope.UserLibraryRead | Scope.UserReadCurrentlyPlaying | Scope.UserReadPlaybackState);
            _authenticationProvider.AuthReceived += async (sender, payload) =>
            {
                var auth = (AuthorizationCodeAuth)sender;
                auth.Stop();

                _lastToken = await auth.ExchangeCode(payload.Code);
                ProcessToken();
                // Do requests with API client
            };
            _authenticationProvider.Start(); // Starts an internal HTTP Server
        }

        private void ProcessToken()
        {
            _spotify = new SpotifyWebAPI
            {
                AccessToken = _lastToken.AccessToken,
                TokenType = _lastToken.TokenType
            };
            OnRefreshTokenRecieved?.Invoke(this, _lastToken.RefreshToken);
            _authenticated = true;
        }

        public void LoginSpotify()
        {
            _authenticationProvider.OpenBrowser();
        }

        public async Task RefreshTokenIfNeeded()
        {
            if (_lastToken.IsExpired())
            {
                Token newToken = await _authenticationProvider.RefreshToken(_lastToken.RefreshToken);
                _spotify.AccessToken = newToken.AccessToken;
                _spotify.TokenType = newToken.TokenType;
                _lastToken = newToken;
            }
        }

        public async Task TryRefreshToken(string token)
        {
            var newToken = await _authenticationProvider.RefreshToken(token);
            if (string.IsNullOrWhiteSpace(newToken.Error))
            {
                _lastToken = newToken;
                ProcessToken();
                _authenticationProvider.Stop();
            }
        }

        public bool IsAuthenticated()
        {
            return _authenticated;
        }


        public async Task<SearchItem> SearchSong(string query)
        {
            return await _spotify.SearchItemsAsync(query, SearchType.Track, 40);
        }

        public async Task<AvailabeDevices> GetDevicesAsync()
        {
            return await _spotify.GetDevicesAsync();
        }

        public delegate void RefreshTokenRecievedHandler(object sender, string newToken);

        // Declare the event.
        public event RefreshTokenRecievedHandler OnRefreshTokenRecieved;

        public void StartSongAsync(string selectedSpotifyDeviceId, string spotifyTrack)
        {
            _spotify.ResumePlaybackAsync(selectedSpotifyDeviceId, "", new List<string>() { spotifyTrack }, "", 0);
        }

        public void StopSongAsync(string selectedSpotifyDeviceId)
        {
            _spotify.PausePlaybackAsync(selectedSpotifyDeviceId);
        }
    }
}
