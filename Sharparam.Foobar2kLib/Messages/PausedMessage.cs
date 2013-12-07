// <copyright file="PausedMessage.cs" company="Adam Hellberg">
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

using System.Globalization;
using System.Text.RegularExpressions;

namespace Sharparam.Foobar2kLib.Messages
{
    public class PausedMessage : SongMessage
    {
        public readonly int PlaylistIndex;
        public readonly int SongIndex;
        public readonly float Time;
        private const string MessageRegexFormat = @"(\d+)\{0}(\d+)\{0}(\d+\.\d+)\{0}(.+)";

        private readonly Song _song;

        internal PausedMessage(string content, string separator, SongParser parser)
            : base(MessageType.Paused, content)
        {
            var messageRegex = new Regex(string.Format(MessageRegexFormat, separator), RegexOptions.IgnoreCase);

            var match = messageRegex.Match(content);

            PlaylistIndex = int.Parse(match.Groups[1].Value);
            SongIndex = int.Parse(match.Groups[2].Value);
            Time = float.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
            _song = parser.ParseSong(match.Groups[4].Value);
        }

        public override Song Song
        {
            get { return _song; }
        }
    }
}
