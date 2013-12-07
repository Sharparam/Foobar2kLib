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
using System.Net.Sockets;
using Sharparam.Foobar2kLib.Events;
using Sharparam.Foobar2kLib.Networking;

namespace Sharparam.Foobar2kLib
{
    public class Foobar
    {
        public readonly Playlists Playlists;

        private readonly TcpClient _controlserverClient;
        private readonly Settings _settings;
        private readonly SongParser _songParser;

        private NetworkStream _controlserverStream;

        private string _order;
        private int _playlistIndex;
        private int _trackIndex;
        private int _volume;

        public Foobar(string host, ushort port, string format, string separator)
        {
            _settings = new Settings(host, port, format, separator);

            Host = _settings.Host;
            Port = _settings.Port;

            _songParser = new SongParser(_settings.Format, _settings.Separator);

            Playlists = new Playlists();

            _playlistIndex = 0;
            _trackIndex = 0;

            _controlserverClient = new TcpClient();
            _controlserverClient.BeginConnect(Host, Port, OnControlServerConnected, _controlserverClient);
        }

        public event EventHandler Connected;

        public string Host { get; private set; }

        public MessageManager MessageManager { get; private set; }

        public string Order
        {
            get { return _order; }
            set { MessageManager.WriteMessage("order {0}", value); }
        }

        public ushort Port { get; private set; }

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

        private void MessageManagerOnMessageReceived(object sender, MessageEventArgs args)
        {
            // TODO: Raise relevant events here
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
            MessageManager.MessageReceived += MessageManagerOnMessageReceived;
            MessageManager.StartRead();

            OnConnected();
        }
    }
}
