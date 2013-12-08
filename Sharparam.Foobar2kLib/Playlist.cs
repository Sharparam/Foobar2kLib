// <copyright file="Playlist.cs" company="Adam Hellberg">
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

using System.Collections;
using System.Collections.Generic;

namespace Sharparam.Foobar2kLib
{
    public class Playlist : IEnumerable<KeyValuePair<int, Song>>
    {
        public readonly int Index;

        private readonly Dictionary<int, Song> _songs;

        internal Playlist(int index, string name = "<Unknown>")
        {
            Index = index;
            Name = name;
            _songs = new Dictionary<int, Song>();
        }

        public int Count
        {
            get { return _songs.Count; }
        }

        public string Name { get; internal set; }

        public Song this[int index]
        {
            get { return _songs.ContainsKey(index) ? _songs[index] : null; }
            internal set { _songs[index] = value; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _songs.Values.GetEnumerator();
        }

        IEnumerator<KeyValuePair<int, Song>> IEnumerable<KeyValuePair<int, Song>>.GetEnumerator()
        {
            return _songs.GetEnumerator();
        }

        internal void Clear()
        {
            _songs.Clear();
        }
    }
}
