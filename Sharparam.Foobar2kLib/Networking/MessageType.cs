// <copyright file="MessageType.cs" company="Adam Hellberg">
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

namespace Sharparam.Foobar2kLib.Networking
{
    public enum MessageType
    {
        Playing = 111,
        Stopped = 112,
        Paused = 113,
        Volume = 222,
        Order = 333,
        PlaylistCount = 400,
        PlaylistInfo = 401,
        PlaylistInfoPlaying = 402,
        SearchResultCount = 500,
        SearchResultEntry = 501,
        SearchResultEntryPlaying = 502,
        PlaylistSongCount = 600,
        PlaylistSong = 601,
        PlaylistSongPlaying = 602,
        QueueCount = 800,
        QueueEntry = 801,
        Info = 999
    }
}
