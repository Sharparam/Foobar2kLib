﻿using System.Text.RegularExpressions;

namespace Sharparam.Foobar2kLib.Messages
{
    public class PlaylistEntryMessage : Message
    {
        public readonly int PlaylistIndex;
        public readonly int SongIndex;
        public readonly Song Song;

        private const string MessageRegexFormat = @"(\d+)\{0}(\d+)\{0}(.+)";

        internal PlaylistEntryMessage(string content, string separator, SongParser parser) : base(MessageType.PlaylistEntry, content)
        {
            var messageRegex = new Regex(string.Format(MessageRegexFormat, separator), RegexOptions.IgnoreCase);

            var match = messageRegex.Match(content);

            PlaylistIndex = int.Parse(match.Groups[1].Value);
            SongIndex = int.Parse(match.Groups[2].Value);
            Song = parser.ParseSong(match.Groups[3].Value);
        }
    }
}