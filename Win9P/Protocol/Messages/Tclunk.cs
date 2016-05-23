﻿using System;
using Win9P.Exceptions;

namespace Win9P.Protocol.Messages
{
    public sealed class Tclunk : Message
    {
        public Tclunk(uint fid)
        {
            Type = (byte) MessageType.Tclunk;
            Fid = fid;
            Length += Constants.BIT32SZ;
        }

        public Tclunk(byte[] bytes) : base(bytes)
        {
            var offset = Constants.HeaderOffset;
            Fid = Protocol.readUInt(bytes, offset);
            offset += Constants.BIT32SZ;
            if (offset < Length)
            {
                throw new InsufficientDataException(Length, offset);
            }
        }

        public uint Fid { get; set; }

        public override byte[] ToBytes()
        {
            var bytes = new byte[Length];
            var offset = Protocol.writeUint(bytes, Length, 0);
            bytes[offset] = Type;
            offset += Constants.BIT8SZ;
            offset += Protocol.writeUshort(bytes, Tag, offset);

            offset += Protocol.writeUint(bytes, Fid, offset);
            if (offset < Length)
            {
                throw new InsufficientDataException(Length, offset);
            }
            return bytes;
        }

        private bool Equals(Tclunk other)
        {
            return base.Equals(other) && Fid == other.Fid;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Tclunk) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (int) Fid;
            }
        }
    }
}