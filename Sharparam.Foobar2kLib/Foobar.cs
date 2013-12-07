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
        private readonly TcpClient _controlserverClient;
        private readonly Settings _settings;
        private readonly SongParser _songParser;

        private NetworkStream _controlserverStream;

        public Foobar(string host, ushort port, string format, string separator)
        {
            _settings = new Settings(host, port, format, separator);

            Host = _settings.Host;
            Port = _settings.Port;

            _songParser = new SongParser(_settings.Format, _settings.Separator);

            _controlserverClient = new TcpClient();
            _controlserverClient.BeginConnect(Host, Port, OnControlServerConnected, _controlserverClient);
        }

        public event EventHandler Connected;

        public string Host { get; private set; }

        public MessageManager MessageManager { get; private set; }

        public ushort Port { get; private set; }

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
