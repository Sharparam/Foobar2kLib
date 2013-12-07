using System.Text.RegularExpressions;

namespace Sharparam.Foobar2kLib.Messages
{
    public class QueueEntryMessage : Message
    {
        public readonly int PlaylistIndex;
        public readonly int QueueIndex;
        public readonly Song Song;
        public readonly int SongIndex;
        private const string MessageRegexFormat = @"(\d+)\{0}(\d+)\{0}(\d+)\{0}(.+)";

        internal QueueEntryMessage(string content, string separator, SongParser parser)
            : base(MessageType.QueueEntry, content)
        {
            var messageRegex = new Regex(string.Format(MessageRegexFormat, separator), RegexOptions.IgnoreCase);

            var match = messageRegex.Match(content);

            QueueIndex = int.Parse(match.Groups[1].Value);
            PlaylistIndex = int.Parse(match.Groups[2].Value);
            SongIndex = int.Parse(match.Groups[3].Value);
            Song = parser.ParseSong(match.Groups[4].Value);
        }
    }
}
