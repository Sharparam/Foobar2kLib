// <copyright file="SongParser.cs" company="Adam Hellberg">
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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sharparam.Foobar2kLib
{
    public class SongParser
    {
        public const string DefaultFormat =
            "%codec%|%bitrate%|$if(%album artist%,%album artist%,%artist%)|%album%|%date%|%genre%|%tracknumber%|%title%";

        public const string DefaultSeparator = "|";

        private const string RegexPatternFormat = @"(?:%([^%]+)%|[^%].+%([^%]+)%\)+)\{0}?";

        private static readonly Dictionary<string, string> FieldMapping = new Dictionary<string, string>
        {
            { "album artist", "artist" },
            { "track artist", "artist" }
        };

        private readonly Regex _fieldCheckRegex;

        private readonly string[] _fields;

        internal SongParser(string format = DefaultFormat, string separator = DefaultSeparator)
        {
            Format = format;
            Separator = separator;

            _fieldCheckRegex = new Regex(string.Format(RegexPatternFormat, Separator), RegexOptions.IgnoreCase);

            var matches = _fieldCheckRegex.Matches(Format);

            _fields = new string[matches.Count];

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                var field = match.Groups[1].Value;

                if (string.IsNullOrEmpty(field))
                    field = match.Groups[2].Value;

                _fields[i] = FieldMapping.ContainsKey(field) ? FieldMapping[field] : field;
            }
        }

        public string Format { get; private set; }

        public string Separator { get; private set; }

        public Dictionary<string, string> ParseData(string input)
        {
            var output = new Dictionary<string, string>();

            var data = input.Split(new[] { Separator }, StringSplitOptions.None);

            for (var i = 0; i < _fields.Length; i++)
                output[_fields[i]] = data[i];

            return output;
        }

        public Song ParseSong(string input)
        {
            return new Song(ParseData(input));
        }
    }
}
