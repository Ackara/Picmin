namespace Acklann.Picmin.Compression
{
    public readonly struct PngquantOptions : ICompressionOptions
    {
        public PngquantOptions(string sourceFile, string outFile = default, int min = 65, int max = 80, bool removeMetadata = true, int speed = 3)
        {
            SourceFile = sourceFile;
            OutputFile = (outFile ?? sourceFile.WithSuffix());
            RemoveMetadata = removeMetadata;
            Speed = speed;
            Min = min;
            Max = max;
        }

        public string SourceFile { get; }

        public string OutputFile { get; }

        public int Min { get; }

        public int Max { get; }

        public int Speed { get; }

        public bool RemoveMetadata { get; }

        public int Quality { get => Min; }

        public override string ToString()
        {
            int range(int x, int min, int max)
            {
                if (x < min) return min;
                else if (x > max) return max;
                else return x;
            }

            return string.Format("{5} --quality {2}-{3} --speed {4} --force --output \"{1}\" -- \"{0}\"",
                SourceFile,
                OutputFile,
                range(Min, 0, 100),
                range(Max, 0, 100),
                (range(Speed, 1, 11)),
                (RemoveMetadata ? "--strip" : string.Empty)
                ).Trim();
        }
    }
}