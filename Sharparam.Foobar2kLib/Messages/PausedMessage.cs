using System.Globalization;
using System.Text.RegularExpressions;

namespace Sharparam.Foobar2kLib.Messages
{
    public class PausedMessage : Message
    {
        public readonly int PlaylistIndex;
        public readonly Song Song;
        public readonly int SongIndex;
        public readonly float Time;
        private const string MessageRegexFormat = @"(\d+)\{0}(\d+)\{0}(\d+\.\d+)\{0}(.+)";

        internal PausedMessage(string content, string separator, SongParser parser)
            : base(MessageType.Paused, content)
        {
            var messageRegex = new Regex(string.Format(MessageRegexFormat, separator), RegexOptions.IgnoreCase);

            var match = messageRegex.Match(content);

            PlaylistIndex = int.Parse(match.Groups[1].Value);
            SongIndex = int.Parse(match.Groups[2].Value);
            Time = float.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
            Song = parser.ParseSong(match.Groups[4].Value);
        }
    }
}
