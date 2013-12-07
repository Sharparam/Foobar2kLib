// <copyright file="Playlists.cs" company="Adam Hellberg">
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
    public class Playlists : IEnumerable<Playlist>, IEnumerable<KeyValuePair<int, Playlist>>
    {
        private readonly Dictionary<int, Playlist> _playlists;

        internal Playlists()
        {
            _playlists = new Dictionary<int, Playlist>();
        }

        public int Count
        {
            get { return _playlists.Count; }
        }

        public Playlist this[int index]
        {
            get { return _playlists.ContainsKey(index) ? _playlists[index] : null; }
            internal set { _playlists[index] = value; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _playlists.Values.GetEnumerator();
        }

        IEnumerator<KeyValuePair<int, Playlist>> IEnumerable<KeyValuePair<int, Playlist>>.GetEnumerator()
        {
            return _playlists.GetEnumerator();
        }

        IEnumerator<Playlist> IEnumerable<Playlist>.GetEnumerator()
        {
            return _playlists.Values.GetEnumerator();
        }

        internal void Clear()
        {
            _playlists.Clear();
        }
    }
}
