namespace Acklann.Picmin.Compression
{
    public interface ICompressionOptions
    {
        string SourceFile { get; }

        string OutputFile { get; }

        bool RemoveMetadata { get; }

        int Quality { get; }
    }
}