// <copyright file="Foobar.cs" company="Adam Hellberg">
//     Copyright © 2013 by Adam Hellberg.
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy of
//     this software and associated documentation files (the "Software"), to deal in
//     the Software without restriction, including without limitation the rights to
//     use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//     of the Software, and to permit persons to whom the Software is furnished to do
//     so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//     CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.ComponentModel;
using System.Net.Sockets;
using Sharparam.Foobar2kLib.Events;
using Sharparam.Foobar2kLib.Networking;

namespace Sharparam.Foobar2kLib
{
    public class Foobar : INotifyPropertyChanged
    {
        public readonly Playlists Playlists;

        private readonly TcpClient _controlserverClient;
        private readonly Settings _settings;
        private readonly SongParser _songParser;

        private NetworkStream _controlserverStream;

        private string _order;
        private int _playlistIndex;
        private PlayState _state;
        private float _time;
        private int _trackIndex;
        private float _volume;

        public Foobar(string host, ushort port, string format, string separator)
        {
            _settings = new Settings(host, port, format, separator);

            Host = _settings.Host;
            Port = _settings.Port;

            _songParser = new SongParser(_settings.Format, _settings.Separator);

            Playlists = new Playlists();

            _controlserverClient = new TcpClient();
            _controlserverClient.BeginConnect(Host, Port, OnControlServerConnected, _controlserverClient);
        }

        public event EventHandler Connected;

        public event PropertyChangedEventHandler PropertyChanged;

        public Playlist CurrentPlaylist
        {
            get { return Playlists[_playlistIndex]; }
        }

        public int CurrentPlaylistIndex
        {
            get
            {
                return _playlistIndex;
            }
            private set
            {
                bool changed = value == _playlistIndex;
                _playlistIndex = value;
                if (changed)
                {
                    OnPropertyChanged("CurrentPlaylistIndex");
                    OnPropertyChanged("CurrentPlaylist");
                }
            }
        }

        public Song CurrentSong
        {
            get
            {
                var list = Playlists[_playlistIndex];
                return list == null ? null : list[_trackIndex];
            }
        }

        public int CurrentTrackIndex
        {
            get
            {
                return _trackIndex;
            }
            private set
            {
                bool changed = value == _trackIndex;
                _trackIndex = value;
                if (changed)
                {
                    OnPropertyChanged("CurrentTrackIndex");
                    OnPropertyChanged("CurrentSong");
                }
            }
        }

        public string Host { get; private set; }

        public MessageManager MessageManager { get; private set; }

        public string Order
        {
            get { return _order; }
            set { MessageManager.WriteMessage("order {0}", value); }
        }

        public ushort Port { get; private set; }

        public PlayState State
        {
            get
            {
                return _state;
            }
            set
            {
                switch (value)
                {
                    case PlayState.Playing:
                        if (_state == PlayState.Paused)
                            Resume();
                        else
                            Play();
                        break;

                    case PlayState.Paused:
                        if (_state != PlayState.Paused)
                            Pause();
                        break;

                    case PlayState.Stopped:
                        Stop();
                        break;
                }
            }
        }

        public float Time
        {
            get { return _time; }
            set { Seek((int)Math.Floor(value + 0.5)); }
        }

        public float Volume
        {
            get { return _volume; }
            set { MessageManager.WriteMessage("vol {0}", value); }
        }

        public void DecreaseVolume()
        {
            MessageManager.WriteMessage("vol down");
        }

        public void IncreaseVolume()
        {
            MessageManager.WriteMessage("vol up");
        }

        public void Mute()
        {
            MessageManager.WriteMessage("vol mute");
        }

        public void Pause()
        {
            MessageManager.WriteMessage("pause");
        }

        public void Play()
        {
            Play(_playlistIndex, _trackIndex);
        }

        public void Play(int track)
        {
            MessageManager.WriteMessage("play {0}", track);
        }

        public void Play(int playlist, int track)
        {
            MessageManager.WriteMessage("play {0} {1}", playlist, track);
        }

        public void PlayNext()
        {
            PlayNext(_playlistIndex);
        }

        public void PlayNext(int playlist)
        {
            MessageManager.WriteMessage("next {0}", playlist);
        }

        public void PlayPrevious()
        {
            PlayPrevious(_playlistIndex);
        }

        public void PlayPrevious(int playlist)
        {
            MessageManager.WriteMessage("prev {0}", playlist);
        }

        public void PlayRandom()
        {
            PlayRandom(_playlistIndex);
        }

        public void PlayRandom(int playlist)
        {
            MessageManager.WriteMessage("rand {0}", playlist);
        }

        public void RequestAllPlaylistInfo()
        {
            MessageManager.WriteMessage("listinfo all");
        }

        public void RequestOrder()
        {
            MessageManager.WriteMessage("order");
        }

        public void RequestPlaylistEntries()
        {
            RequestPlaylistEntries(_playlistIndex);
        }

        public void RequestPlaylistEntries(int playlist)
        {
            MessageManager.WriteMessage("list {0}", playlist);
        }

        public void RequestPlaylistEntries(int start, int end)
        {
            MessageManager.WriteMessage("list {0} {1}", start, end);
        }

        public void RequestPlaylistInfo()
        {
            RequestPlaylistInfo(_playlistIndex);
        }

        public void RequestPlaylistInfo(int playlist)
        {
            MessageManager.WriteMessage("listinfo {0}", playlist);
        }

        public void RequestPlaylistInfo(int start, int end)
        {
            MessageManager.WriteMessage("listinfo {0} {1}", start, end);
        }

        public void RequestTrackinfo()
        {
            MessageManager.WriteMessage("trackinfo");
        }

        public void RequestVolume()
        {
            MessageManager.WriteMessage("vol");
        }

        public void Resume()
        {
            // Pause just toggles, so you write pause again to resume
            MessageManager.WriteMessage("pause");
        }

        public void Search(string term)
        {
            Search(_playlistIndex, term);
        }

        public void Search(int playlist, string term)
        {
            MessageManager.WriteMessage("search {0} {1}", playlist, term);
        }

        public void Seek(int seconds)
        {
            MessageManager.WriteMessage("seek {0}", seconds);
        }

        public void SeekDelta(int seconds)
        {
            MessageManager.WriteMessage("seek delta {0}", seconds);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var fun = PropertyChanged;
            if (fun != null)
                fun(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MessageManagerOnOrderMessageReceived(object sender, OrderMessageEventArgs args)
        {
            _order = args.Message.Order;
        }

        private void MessageManagerOnPausedMessageReceived(object sender, PausedMessageEventArgs args)
        {
            UpdatePlaylistSong(args.Message.PlaylistIndex, args.Message.SongIndex, args.Message.Song);
            CurrentPlaylistIndex = args.Message.PlaylistIndex;
            CurrentTrackIndex = args.Message.SongIndex;
            UpdateState(PlayState.Paused);
            _time = args.Message.Time;
            OnPropertyChanged("Time");
        }

        private void MessageManagerOnPlayingMessageReceived(object sender, PlayingMessageEventArgs args)
        {
            UpdatePlaylistSong(args.Message.PlaylistIndex, args.Message.SongIndex, args.Message.Song);
            CurrentPlaylistIndex = args.Message.PlaylistIndex;
            CurrentTrackIndex = args.Message.SongIndex;
            UpdateState(PlayState.Playing);
            _time = args.Message.Time;
            OnPropertyChanged("Time");
        }

        private void MessageManagerOnPlaylistInfoCurrentMessageReceived(object sender, PlaylistInfoCurrentMessageEventArgs args)
        {
            UpdatePlaylist(args.Message.PlaylistIndex, args.Message.PlaylistName);
            CurrentPlaylistIndex = args.Message.PlaylistIndex;
            //RequestPlaylistEntries(args.Message.PlaylistIndex);
        }

        private void MessageManagerOnPlaylistInfoMessageReceived(object sender, PlaylistInfoMessageEventArgs args)
        {
            UpdatePlaylist(args.Message.PlaylistIndex, args.Message.PlaylistName);
            //RequestPlaylistEntries(args.Message.PlaylistIndex);
        }

        private void MessageManagerOnPlaylistInfoSpecificMessageReceived(object sender, PlaylistInfoSpecificMessageEventArgs args)
        {
            UpdatePlaylist(args.Message.PlaylistIndex, args.Message.PlaylistName);
            //RequestPlaylistEntries(args.Message.PlaylistIndex);
        }

        private void MessageManagerOnPlaylistSongMessageReceived(object sender, PlaylistSongMessageEventArgs args)
        {
            UpdatePlaylistSong(args.Message.PlaylistIndex, args.Message.SongIndex, args.Message.Song);
        }

        private void MessageManagerOnPlaylistSongPausedMessageReceived(object sender, PlaylistSongPausedMessageEventArgs args)
        {
            UpdatePlaylistSong(args.Message.PlaylistIndex, args.Message.SongIndex, args.Message.Song);
            CurrentPlaylistIndex = args.Message.PlaylistIndex;
            CurrentTrackIndex = args.Message.SongIndex;
            UpdateState(PlayState.Paused);
            RequestTrackinfo();
        }

        private void MessageManagerOnPlaylistSongPlayingMessageReceived(object sender, PlaylistSongPlayingMessageEventArgs args)
        {
            UpdatePlaylistSong(args.Message.PlaylistIndex, args.Message.SongIndex, args.Message.Song);
            CurrentPlaylistIndex = args.Message.PlaylistIndex;
            CurrentTrackIndex = args.Message.SongIndex;
            UpdateState(PlayState.Playing);
            RequestTrackinfo();
        }

        private void MessageManagerOnStoppedMessageReceived(object sender, StoppedMessageEventArgs args)
        {
            UpdatePlaylistSong(args.Message.PlaylistIndex, args.Message.SongIndex, args.Message.Song);
            CurrentPlaylistIndex = args.Message.PlaylistIndex;
            CurrentTrackIndex = args.Message.SongIndex;
            UpdateState(PlayState.Stopped);
            _time = args.Message.Time;
            OnPropertyChanged("Time");
        }

        private void MessageManagerOnVolumeMessageReceived(object sender, VolumeMessageEventArgs args)
        {
            _volume = args.Message.Value;
            OnPropertyChanged("Volume");
        }

        private void OnConnected()
        {
            var fun = Connected;
            if (fun != null)
                fun(this, null);
        }

        private void OnControlServerConnected(IAsyncResult result)
        {
            ((TcpClient)result.AsyncState).EndConnect(result);

            // Set up stream and start listening
            _controlserverStream = _controlserverClient.GetStream();

            MessageManager = new MessageManager(_controlserverStream, _songParser, _settings);

            MessageManager.PlayingMessageReceived += MessageManagerOnPlayingMessageReceived;
            MessageManager.PausedMessageReceived += MessageManagerOnPausedMessageReceived;
            MessageManager.StoppedMessageReceived += MessageManagerOnStoppedMessageReceived;
            MessageManager.VolumeMessageReceived += MessageManagerOnVolumeMessageReceived;
            MessageManager.OrderMessageReceived += MessageManagerOnOrderMessageReceived;
            MessageManager.PlaylistInfoMessageReceived += MessageManagerOnPlaylistInfoMessageReceived;
            MessageManager.PlaylistInfoCurrentMessageReceived += MessageManagerOnPlaylistInfoCurrentMessageReceived;
            MessageManager.PlaylistInfoSpecificMessageReceived += MessageManagerOnPlaylistInfoSpecificMessageReceived;
            MessageManager.PlaylistSongMessageReceived += MessageManagerOnPlaylistSongMessageReceived;
            MessageManager.PlaylistSongPlayingMessageReceived += MessageManagerOnPlaylistSongPlayingMessageReceived;
            MessageManager.PlaylistSongPausedMessageReceived += MessageManagerOnPlaylistSongPausedMessageReceived;

            MessageManager.MessageSent += (sender, args) => Console.WriteLine(">>> {0}", args.Data);

            MessageManager.StartRead();

            OnConnected();

            RequestVolume();
            RequestTrackinfo();
            RequestOrder();
            RequestAllPlaylistInfo();
        }

        private void Stop()
        {
            MessageManager.WriteMessage("stop");
        }

        private void UpdatePlaylist(int index, string name = "<Unknown>")
        {
            var playlist = Playlists[index];

            if (playlist == null)
                Playlists[index] = new Playlist(index, name);
            else
                playlist.Name = name;
        }

        private void UpdatePlaylistSong(int playlistIndex, int songIndex, Song song)
        {
            var playlist = Playlists[playlistIndex];

            if (playlist == null)
            {
                playlist = new Playlist(playlistIndex);
                Playlists[playlistIndex] = playlist;
            }

            playlist[songIndex] = song;
        }

        private void UpdateState(PlayState newState)
        {
            bool changed = newState == _state;
            _state = newState;
            if (changed)
                OnPropertyChanged("State");
        }
    }
}
