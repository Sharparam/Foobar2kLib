using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharparam.Foobar2kLib.Messages
{
    public abstract class SongMessage : Message
    {
        internal SongMessage(MessageType type, string content) : base(type, content)
        {
        }

        public abstract Song Song { get; }
    }
}
