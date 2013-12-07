/* MessageManager.cs
 *
 * Copyright © 2013 by Adam Hellberg.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do
 * so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.IO;
using System.Text;
using Sharparam.Foobar2kLib.Events;

namespace Sharparam.Foobar2kLib.Networking
{
    public class MessageManager
    {
        private static readonly string[] Separators = new[] { "\r\n", "\n\r", "\r", "\n" };

        private readonly Stream _stream;

        private byte[] _readBuffer;

        private string _sendMessage;

        internal MessageManager(Stream stream)
        {
            _stream = stream;
        }

        public event EventHandler<StringEventArgs> MessageReceived;

        public event EventHandler<StringEventArgs> MessageSent;

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

        private void OnMessageReceived(string message)
        {
            var fun = MessageReceived;
            if (fun != null)
                fun(this, new StringEventArgs(message));
        }

        private void OnMessageSent(string message)
        {
            var fun = MessageSent;
            if (fun != null)
                fun(this, new StringEventArgs(message));
        }

        private void OnStreamRead(IAsyncResult result)
        {
            var numRead = ((Stream)result.AsyncState).EndRead(result);

            var data = Encoding.UTF8.GetString(_readBuffer, 0, numRead);

            var messages = data.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (var message in messages)
                OnMessageReceived(message);

            StartRead();
        }

        private void OnStreamSent(IAsyncResult result)
        {
            ((Stream)result.AsyncState).EndWrite(result);

            OnMessageSent(_sendMessage);
        }
    }
}
