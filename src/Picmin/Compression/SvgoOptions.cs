namespace Acklann.Picmin.Compression
{
    public readonly struct SvgoOptions : ICompressionOptions
    {
        public SvgoOptions(string sourceFile, string outFile = default, bool removeMetadata = true)
        {
            SourceFile = sourceFile;
            OutputFile = (outFile ?? sourceFile.WithSuffix());
            RemoveMetadata = removeMetadata;
        }

        public string SourceFile { get; }

        public string OutputFile { get; }

        public bool RemoveMetadata { get; }

        int ICompressionOptions.Quality
        {
            get => default;
        }

        public override string ToString()
        {
            return string.Concat(
                "\"node_modules/svgo/bin/svgo\" ",
                (RemoveMetadata ? string.Empty : "--disable=removeMetadata --disable=removeTitle --disable=removeDesc --disable=removeComments --pretty "),
                $"--multipass --output=\"{OutputFile}\" ",
                $"\"{SourceFile}\""
                );
        }
    }
}