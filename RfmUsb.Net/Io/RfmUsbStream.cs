﻿/*
* MIT License
*
* Copyright (c) 2023 Derek Goslin
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using RfmUsb.Net.Extensions;
using System;
using System.IO;
using System.Linq;

namespace RfmUsb.Net.Io
{
    /// <summary>
    /// The stream for reading and writing to the RfmUsb buffered IO
    /// </summary>
    public class RfmUsbStream : Stream
    {
        private readonly IRfm _rfm;

        internal RfmUsbStream(IRfm rfm)
        {
            _rfm = rfm ?? throw new ArgumentNullException(nameof(rfm));
        }

        ///<inheritdoc/>
        public override bool CanRead => true;

        ///<inheritdoc/>
        public override bool CanSeek => false;

        ///<inheritdoc/>
        public override bool CanWrite => true;

        ///<inheritdoc/>
        public override long Length => _rfm.BufferedIoInfo.Count;

        ///<inheritdoc/>
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        ///<inheritdoc/>
        public override void Flush()
        {
        }

        ///<inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "value must not be negative");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "value must not be negative");
            if (buffer.Length - offset < count)
                throw new ArgumentException("invalid value", nameof(offset));

            int bytesAvailable = _rfm.BufferedIoInfo.Count;
            int bytesToRead = Math.Min(bytesAvailable, count);
            var bytes = _rfm.ReadFromBuffer(bytesToRead);

            Buffer.BlockCopy(bytes.ToArray(), 0, buffer, offset, bytesToRead);

            return bytesToRead;
        }

        ///<inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        ///<inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        ///<inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "value must not be negative");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "value must not be negative");
            if (buffer.Length - offset < count)
                throw new ArgumentException("invalid value", nameof(offset));

            var ioBufferInfo = _rfm.BufferedIoInfo;
            var capacity = ioBufferInfo.Capacity - ioBufferInfo.Count;

            if (count > capacity)
                throw new ArgumentOutOfRangeException(nameof(count), $"Not enough buffer space {capacity}");

            var chunks = buffer.Skip(offset).Take(count).Split(64).ToList();

            foreach (var chunk in chunks)
            {
                _rfm.WriteToBuffer(chunk);
            }

            _rfm.TransmitBuffer();
        }
    }
}