using DrumTrainer.Spotify;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrumTrainer.ViewModels
{
    class SearchSpotifyViewViewModel : ViewModel
    {
        private readonly IChildView _childView;
        private readonly SpotifyService _spotifyService;

        public AsyncRelayCommand SearchCommand { get; }
        public RelayCommand CancelCommand { get; }

        public string SearchText { get; set; }

        public SearchSpotifyViewViewModel(IChildView childView, SpotifyService spotifyService)
        {
            _childView = childView;
            _spotifyService = spotifyService;
            SearchCommand = new AsyncRelayCommand(SearchExecute);
            CancelCommand = new RelayCommand(CancelEditExecute);
            SelectCommand = new RelayCommand(SaveExecute, CanSaveExecute);
        }

        private bool CanSaveExecute(object obj)
        {
            return SelectedItem != null;
        }

        private async Task SearchExecute(object arg)
        {
            SelectedItem = null;
            ResultItems = (await _spotifyService.SearchSong(SearchText).ConfigureAwait(true)).Tracks.Items;
        }

        public FullTrack SelectedItem { get; set; }
        private void SaveExecute(object obj)
        {
            _childView.Close(SelectedItem);
        }

        private void CancelEditExecute(object obj)
        {
            _childView.Close(null);
        }

        public RelayCommand SelectCommand { get; }

        List<FullTrack> _resultItems;
        public List<FullTrack> ResultItems
        {
            get
            {
                return _resultItems;
            }
            set
            {
                _resultItems = value;
                OnPropertyChanged();
            }
        }

    }
}
