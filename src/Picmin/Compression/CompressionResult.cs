using System;

namespace Acklann.Picmin.Compression
{
    public readonly struct CompressionResult
    {
        public CompressionResult(string tool, bool success, string sourceFile, string outFile, long originalSize, long newSize, long ticks, string message = default)
        {
            Tool = tool;
            SourceFile = sourceFile;
            OriginalSize = originalSize;
            OuputFile = outFile;
            NewSize = newSize;
            Elapse = TimeSpan.FromTicks(ticks);
            Success = success;
            Message = message;
        }

        public readonly string Tool;

        public readonly string Message;

        public readonly string SourceFile;

        public readonly string OuputFile;

        public readonly long OriginalSize;

        public readonly long NewSize;

        public readonly bool Success;

        public readonly TimeSpan Elapse;
    }
}