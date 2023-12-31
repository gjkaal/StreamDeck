﻿using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;

namespace OpenMacroBoard.SocketIO.Internals
{
    public static class StreamExtensions
    {
        public static async Task<int> ReadByteAsync(this Stream stream, CancellationToken cancellationToken)
        {
            var buffer = new byte[1];
            var read = await stream.ReadAsync(buffer.AsMemory(0, 1), cancellationToken).ConfigureAwait(false);
            if (read == 0)
                return -1;

            return buffer[0];
        }

        public static Task WriteByteAsync(this Stream stream, byte value, CancellationToken cancellationToken)
        {
            return stream.WriteAsync(new[] { value }, 0, 1, cancellationToken);
        }
    }
}
