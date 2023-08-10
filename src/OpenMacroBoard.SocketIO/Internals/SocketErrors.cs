using System;
using System.IO;

namespace OpenMacroBoard.SocketIO.Internals
{
    internal static class SocketErrors
    {
        internal static void EndOfFile()
        {
            throw new EndOfStreamException();
        }

        internal static void FileNotOpen()
        {
            throw new ObjectDisposedException(null, nameof(FileNotOpen));
        }
    }
}
