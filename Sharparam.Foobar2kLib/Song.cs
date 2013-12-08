// <copyright file="Song.cs" company="Adam Hellberg">
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

using System.Collections.Generic;

namespace Sharparam.Foobar2kLib
{
    public class Song
    {
        public readonly string Album;
        public readonly string Artist;
        public readonly int Bitrate;
        public readonly int Channels;
        public readonly string Codec;
        public readonly string CodecProfile;
        public readonly string Date;
        public readonly string Directory;
        public readonly int DiscCount;
        public readonly int DiscIndex;
        public readonly string Filename;
        public readonly string FilenameExtension;
        public readonly int Filesize;
        public readonly string Genre;
        public readonly string LastModified;
        public readonly string Length;
        public readonly int LengthSeconds;
        public readonly string NaturalFilesize;
        public readonly string Path;
        public readonly string PathSort;
        public readonly int SampleRate;
        public readonly int SubIndex;
        public readonly string Title;
        public readonly int TrackCount;
        public readonly int TrackIndex;

        internal Song(
            string title,
            string artist,
            string album,
            string date,
            string genre,
            int trackIndex,
            int trackCount,
            int discIndex,
            int discCount,
            string codec,
            string codecProfile,
            string filename,
            string filenameExtension,
            string directory,
            string path,
            int subIndex,
            string pathSort,
            string length,
            int lengthSeconds,
            int bitrate,
            int channels,
            int sampleRate,
            int filesize,
            string naturalFilesize,
            string lastModified)
        {
            Title = title;
            Artist = artist;
            Album = album;
            Date = date;
            Genre = genre;
            TrackIndex = trackIndex;
            TrackCount = trackCount;
            DiscIndex = discIndex;
            DiscCount = discCount;
            Codec = codec;
            CodecProfile = codecProfile;
            Filename = filename;
            FilenameExtension = filenameExtension;
            Directory = directory;
            Path = path;
            SubIndex = subIndex;
            PathSort = pathSort;
            Length = length;
            LengthSeconds = lengthSeconds;
            Bitrate = bitrate;
            Channels = channels;
            SampleRate = sampleRate;
            Filesize = filesize;
            NaturalFilesize = naturalFilesize;
            LastModified = lastModified;
        }

        internal Song(IDictionary<string, string> data)
        {
            Title = data.ContainsKey("title") ? data["title"] : null;
            Artist = data.ContainsKey("artist") ? data["artist"] : null;
            Album = data.ContainsKey("album") ? data["album"] : null;
            Date = data.ContainsKey("date") ? data["date"] : null;
            Genre = data.ContainsKey("genre") ? data["genre"] : null;

            if (data.ContainsKey("tracknumber"))
            {
                if (!int.TryParse(data["tracknumber"], out TrackIndex))
                    TrackIndex = -1;
            }
            else
                TrackIndex = -1;

            if (data.ContainsKey("totaltracks"))
            {
                if (!int.TryParse(data["totaltracks"], out TrackCount))
                    TrackCount = -1;
            }
            else
                TrackCount = -1;

            if (data.ContainsKey("discnumber"))
            {
                if (!int.TryParse(data["discnumber"], out DiscIndex))
                    DiscIndex = -1;
            }
            else
                DiscIndex = -1;

            if (data.ContainsKey("discnumber"))
            {
                if (!int.TryParse(data["discnumber"], out DiscIndex))
                    DiscIndex = -1;
            }
            else
                DiscIndex = -1;

            if (data.ContainsKey("totaldiscs"))
            {
                if (!int.TryParse(data["totaldiscs"], out DiscCount))
                    DiscCount = -1;
            }
            else
                DiscCount = -1;

            Codec = data.ContainsKey("codec") ? data["codec"] : null;
            CodecProfile = data.ContainsKey("codec_profile") ? data["codec_profile"] : null;
            Filename = data.ContainsKey("filename") ? data["filename"] : null;
            FilenameExtension = data.ContainsKey("filename_ext") ? data["filename_ext"] : null;
            Directory = data.ContainsKey("directoryname") ? data["directoryname"] : null;
            Path = data.ContainsKey("path") ? data["path"] : null;

            if (data.ContainsKey("subsong"))
            {
                if (!int.TryParse(data["subsong"], out SubIndex))
                    SubIndex = -1;
            }
            else
                SubIndex = -1;

            PathSort = data.ContainsKey("path_sort") ? data["path_sort"] : null;
            Length = data.ContainsKey("length") ? data["length"] : null;

            if (data.ContainsKey("length_seconds"))
            {
                if (!int.TryParse(data["length_seconds"], out LengthSeconds))
                    LengthSeconds = -1;
            }
            else
                LengthSeconds = -1;

            if (data.ContainsKey("bitrate"))
            {
                if (!int.TryParse(data["bitrate"], out Bitrate))
                    Bitrate = -1;
            }
            else
                Bitrate = -1;

            if (data.ContainsKey("channels"))
            {
                if (!int.TryParse(data["channels"], out Channels))
                    Channels = -1;
            }
            else
                Channels = -1;

            if (data.ContainsKey("samplerate"))
            {
                if (!int.TryParse(data["samplerate"], out SampleRate))
                    SampleRate = -1;
            }
            else
                SampleRate = -1;

            if (data.ContainsKey("filesize"))
            {
                if (!int.TryParse(data["filesize"], out Filesize))
                    Filesize = -1;
            }
            else
                Filesize = -1;

            NaturalFilesize = data.ContainsKey("filesize_natural") ? data["filesize_natural"] : null;
            LastModified = data.ContainsKey("last_modified") ? data["last_modified"] : null;
        }
    }
}
