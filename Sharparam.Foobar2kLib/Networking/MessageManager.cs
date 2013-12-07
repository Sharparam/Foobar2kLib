// <copyright file="MessageManager.cs" company="Adam Hellberg">
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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sharparam.Foobar2kLib.Events;
using Sharparam.Foobar2kLib.Messages;

namespace Sharparam.Foobar2kLib.Networking
{
    public class MessageManager
    {
        private static readonly Regex MessageRegex = new Regex(@"(\d+)\|(.+)\|");
        private static readonly string[] Separators = new[] { "\r\n", "\n\r", "\r", "\n" };
        private readonly SongParser _parser;
        private readonly Settings _settings;
        private readonly Stream _stream;
        private byte[] _readBuffer;

        private string _sendMessage;

        internal MessageManager(Stream stream, SongParser parser, Settings settings)
        {
            _stream = stream;
            _parser = parser;
            _settings = settings;
        }

        public event EventHandler<InfoMessageEventArgs> InfoMessageReceived;

        public event EventHandler<MessageEventArgs> MessageReceived;

        public event EventHandler<StringEventArgs> MessageSent;

        public event EventHandler<OrderMessageEventArgs> OrderMessageReceived;

        public event EventHandler<PausedMessageEventArgs> PausedMessageReceived;

        public event EventHandler<PlayingMessageEventArgs> PlayingMessageReceived;

        public event EventHandler<PlaylistCountMessageEventArgs> PlaylistCountMessageReceived;

        public event EventHandler<PlaylistInfoMessageEventArgs> PlaylistInfoMessageReceived;

        public event EventHandler<PlaylistInfoPlayingMessageEventArgs> PlaylistInfoPlayingMessageReceived;

        public event EventHandler<PlaylistSongCountMessageEventArgs> PlaylistSongCountMessageReceived;

        public event EventHandler<PlaylistSongMessageEventArgs> PlaylistSongMessageReceived;

        public event EventHandler<PlaylistSongPlayingMessageEventArgs> PlaylistSongPlayingMessageReceived;

        public event EventHandler<QueueCountMessageEventArgs> QueueCountMessageReceived;

        public event EventHandler<QueueEntryMessageEventArgs> QueueEntryMessageReceived;

        public event EventHandler<StringEventArgs> RawMessageReceived;

        public event EventHandler<SearchResultCountMessageEventArgs> SearchResultCountMessageReceived;

        public event EventHandler<SearchResultEntryMessageEventArgs> SearchResultEntryMessageReceived;

        public event EventHandler<SearchResultEntryPlayingMessageEventArgs> SearchResultEntryPlayingMessageReceived;

        public event EventHandler<StoppedMessageEventArgs> StoppedMessageReceived;

        public event EventHandler<VolumeMessageEventArgs> VolumeMessageReceived;

        public void WriteMessage(string format, params object[] args)
        {
            _sendMessage = string.Format(format, args);

            if (!_sendMessage.EndsWith("\r\n"))
                _sendMessage = _sendMessage += "\r\n";

            var buffer = Encoding.UTF8.GetBytes(_sendMessage);
            _stream.BeginWrite(buffer, 0, buffer.Length, OnStreamSent, _stream);
        }

        internal void StartRead()
        {
            _readBuffer = new byte[4096];
            _stream.BeginRead(_readBuffer, 0, _readBuffer.Length, OnStreamRead, _stream);
        }

        private void OnInfoMessageReceived(InfoMessage message)
        {
            var fun = InfoMessageReceived;
            if (fun != null)
                fun(this, new InfoMessageEventArgs(message));
        }

        private void OnMessageReceived(Message message)
        {
            var fun = MessageReceived;
            if (fun != null)
                fun(this, new MessageEventArgs(message));
        }

        private void OnMessageSent(string message)
        {
            var fun = MessageSent;
            if (fun != null)
                fun(this, new StringEventArgs(message));
        }

        private void OnOrderMessageReceived(OrderMessage message)
        {
            var fun = OrderMessageReceived;
            if (fun != null)
                fun(this, new OrderMessageEventArgs(message));
        }

        private void OnPausedMessageReceived(PausedMessage message)
        {
            var fun = PausedMessageReceived;
            if (fun != null)
                fun(this, new PausedMessageEventArgs(message));
        }

        private void OnPlayingMessageReceived(PlayingMessage message)
        {
            var fun = PlayingMessageReceived;
            if (fun != null)
                fun(this, new PlayingMessageEventArgs(message));
        }

        private void OnPlaylistCountMessageReceived(PlaylistCountMessage message)
        {
            var fun = PlaylistCountMessageReceived;
            if (fun != null)
                fun(this, new PlaylistCountMessageEventArgs(message));
        }

        private void OnPlaylistInfoMessageReceived(PlaylistInfoMessage message)
        {
            var fun = PlaylistInfoMessageReceived;
            if (fun != null)
                fun(this, new PlaylistInfoMessageEventArgs(message));
        }

        private void OnPlaylistInfoPlayingMessageReceived(PlaylistInfoPlayingMessage message)
        {
            var fun = PlaylistInfoPlayingMessageReceived;
            if (fun != null)
                fun(this, new PlaylistInfoPlayingMessageEventArgs(message));
        }

        private void OnPlaylistSongCountMessageReceived(PlaylistSongCountMessage message)
        {
            var fun = PlaylistSongCountMessageReceived;
            if (fun != null)
                fun(this, new PlaylistSongCountMessageEventArgs(message));
        }

        private void OnPlaylistSongMessageReceived(PlaylistSongMessage message)
        {
            var fun = PlaylistSongMessageReceived;
            if (fun != null)
                fun(this, new PlaylistSongMessageEventArgs(message));
        }

        private void OnPlaylistSongPlayingMessageReceived(PlaylistSongPlayingMessage message)
        {
            var fun = PlaylistSongPlayingMessageReceived;
            if (fun != null)
                fun(this, new PlaylistSongPlayingMessageEventArgs(message));
        }

        private void OnQueueCountMessageReceived(QueueCountMessage message)
        {
            var fun = QueueCountMessageReceived;
            if (fun != null)
                fun(this, new QueueCountMessageEventArgs(message));
        }

        private void OnQueueEntryMessageReceived(QueueEntryMessage message)
        {
            var fun = QueueEntryMessageReceived;
            if (fun != null)
                fun(this, new QueueEntryMessageEventArgs(message));
        }

        private void OnRawMessageReceived(string message)
        {
            var fun = RawMessageReceived;
            if (fun != null)
                fun(this, new StringEventArgs(message));
        }

        private void OnSearchResultCountMessageReceived(SearchResultCountMessage message)
        {
            var fun = SearchResultCountMessageReceived;
            if (fun != null)
                fun(this, new SearchResultCountMessageEventArgs(message));
        }

        private void OnSearchResultEntryMessageReceived(SearchResultEntryMessage message)
        {
            var fun = SearchResultEntryMessageReceived;
            if (fun != null)
                fun(this, new SearchResultEntryMessageEventArgs(message));
        }

        private void OnSearchResultEntryPlayingMessageReceived(SearchResultEntryPlayingMessage message)
        {
            var fun = SearchResultEntryPlayingMessageReceived;
            if (fun != null)
                fun(this, new SearchResultEntryPlayingMessageEventArgs(message));
        }

        private void OnStoppedMessageReceived(StoppedMessage message)
        {
            var fun = StoppedMessageReceived;
            if (fun != null)
                fun(this, new StoppedMessageEventArgs(message));
        }

        private void OnStreamRead(IAsyncResult result)
        {
            var numRead = ((Stream)result.AsyncState).EndRead(result);

            var data = Encoding.UTF8.GetString(_readBuffer, 0, numRead);

            var messages = data.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (var message in messages)
            {
                OnRawMessageReceived(message);

                // Parse message
                var match = MessageRegex.Match(message);
                var messageId = int.Parse(match.Groups[1].Value);
                var messageContent = match.Groups[2].Value;

                var messageType = (MessageType)messageId;

                var messageObject = new Message(messageType, messageContent);

                OnMessageReceived(messageObject);

                switch (messageType)
                {
                    case MessageType.Playing:
                        OnPlayingMessageReceived(new PlayingMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.Stopped:
                        OnStoppedMessageReceived(new StoppedMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.Paused:
                        OnPausedMessageReceived(new PausedMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.Volume:
                        OnVolumeMessageReceived(new VolumeMessage(messageContent));
                        break;

                    case MessageType.Order:
                        OnOrderMessageReceived(new OrderMessage(messageContent));
                        break;

                    case MessageType.PlaylistCount:
                        OnPlaylistCountMessageReceived(new PlaylistCountMessage(messageContent));
                        break;

                    case MessageType.PlaylistInfo:
                        OnPlaylistInfoMessageReceived(new PlaylistInfoMessage(messageContent, _settings.Separator));
                        break;

                    case MessageType.PlaylistInfoPlaying:
                        OnPlaylistInfoPlayingMessageReceived(
                            new PlaylistInfoPlayingMessage(messageContent, _settings.Separator));
                        break;

                    case MessageType.SearchResultCount:
                        OnSearchResultCountMessageReceived(
                            new SearchResultCountMessage(messageContent, _settings.Separator));
                        break;

                    case MessageType.SearchResultEntry:
                        OnSearchResultEntryMessageReceived(
                            new SearchResultEntryMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.SearchResultEntryPlaying:
                        OnSearchResultEntryPlayingMessageReceived(
                            new SearchResultEntryPlayingMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.PlaylistSongCount:
                        OnPlaylistSongCountMessageReceived(
                            new PlaylistSongCountMessage(messageContent, _settings.Separator));
                        break;

                    case MessageType.PlaylistSong:
                        OnPlaylistSongMessageReceived(
                            new PlaylistSongMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.PlaylistSongPlaying:
                        OnPlaylistSongPlayingMessageReceived(
                            new PlaylistSongPlayingMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.QueueCount:
                        OnQueueCountMessageReceived(new QueueCountMessage(messageContent));
                        break;

                    case MessageType.QueueEntry:
                        OnQueueEntryMessageReceived(new QueueEntryMessage(messageContent, _settings.Separator, _parser));
                        break;

                    case MessageType.Info:
                        OnInfoMessageReceived(new InfoMessage(messageContent));
                        break;
                }
            }

            StartRead();
        }

        private void OnStreamSent(IAsyncResult result)
        {
            ((Stream)result.AsyncState).EndWrite(result);

            OnMessageSent(_sendMessage);
        }

        private void OnVolumeMessageReceived(VolumeMessage message)
        {
            var fun = VolumeMessageReceived;
            if (fun != null)
                fun(this, new VolumeMessageEventArgs(message));
        }
    }
}
