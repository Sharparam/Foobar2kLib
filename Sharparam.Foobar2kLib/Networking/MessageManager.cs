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

        public event EventHandler<MessageEventArgs> MessageReceived;

        public event EventHandler<StringEventArgs> MessageSent;

        public event EventHandler<StringEventArgs> RawMessageReceived;

        public void WriteMessage(string message)
        {
            _sendMessage = message;

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

        private void OnRawMessageReceived(string message)
        {
            var fun = RawMessageReceived;
            if (fun != null)
                fun(this, new StringEventArgs(message));
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

                Message messageObject;

                switch (messageType)
                {
                    case MessageType.Playing:
                        messageObject = new PlayingMessage(messageContent, _settings.Separator, _parser);
                        break;

                    case MessageType.Stopped:
                        messageObject = new StoppedMessage(messageContent, _settings.Separator, _parser);
                        break;

                    case MessageType.Paused:
                        messageObject = new PausedMessage(messageContent, _settings.Separator, _parser);
                        break;

                    case MessageType.Volume:
                        messageObject = new VolumeMessage(messageContent);
                        break;

                    case MessageType.Order:
                        messageObject = new OrderMessage(messageContent);
                        break;

                    case MessageType.PlaylistInfo:
                        messageObject = new PlaylistInfoMessage(messageContent, _settings.Separator);
                        break;

                    case MessageType.PlaylistCount:
                        messageObject = new PlaylistCountMessage(messageContent, _settings.Separator);
                        break;

                    case MessageType.PlaylistEntry:
                        messageObject = new PlaylistEntryMessage(messageContent, _settings.Separator, _parser);
                        break;

                    case MessageType.PlaylistEntryPlaying:
                        messageObject = new PlaylistEntryPlayingMessage(messageContent, _settings.Separator, _parser);
                        break;

                    case MessageType.QueueCount:
                        messageObject = new QueueCountMessage(messageContent);
                        break;

                    case MessageType.QueueEntry:
                        messageObject = new QueueEntryMessage(messageContent, _settings.Separator, _parser);
                        break;

                    case MessageType.Info:
                        messageObject = new InfoMessage(messageContent);
                        break;

                    default:
                        messageObject = new Message(messageType, messageContent);
                        break;
                }

                OnMessageReceived(messageObject);
            }

            StartRead();
        }

        private void OnStreamSent(IAsyncResult result)
        {
            ((Stream)result.AsyncState).EndWrite(result);

            OnMessageSent(_sendMessage);
        }
    }
}
