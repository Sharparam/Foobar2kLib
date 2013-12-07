// <copyright file="Program.cs" company="Adam Hellberg">
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
using Sharparam.Foobar2kLib.Messages;

namespace Sharparam.Foobar2kLib.Tester
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "Foobar2kLib Tester";

            var foobar = new Foobar(
                "127.0.0.1",
                3333,
                "%codec%|%bitrate%|$if(%album artist%,%album artist%,%artist%)|%album%|%date%|%genre%|%tracknumber%|%title%",
                "|");

            foobar.Connected += (s, a) =>
            {
                foobar.MessageManager.MessageReceived += (sender, args) =>
                {
                    var msg = args.Message;

                    var songMsg = msg as SongMessage;

                    if (songMsg == null)
                        Console.WriteLine("{0}: {1}", msg.Type, msg.Content);
                    else
                    {
                        var plMsg = songMsg as PlayingMessage;
                        var paMsg = songMsg as PausedMessage;
                        var sMsg = songMsg as StoppedMessage;
                        Console.WriteLine(
                            "{0} \"{1}\" ({3}) by {2}",
                            songMsg.Type,
                            songMsg.Song.Title,
                            songMsg.Song.Artist,
// ReSharper disable PossibleNullReferenceException
                            plMsg == null ? paMsg == null ? sMsg.SongIndex : paMsg.SongIndex : plMsg.SongIndex);
// ReSharper restore PossibleNullReferenceException
                    }
                };
            };

            while (true)
            {
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                    foobar.MessageManager.WriteMessage(input);
            }
        }
    }
}
