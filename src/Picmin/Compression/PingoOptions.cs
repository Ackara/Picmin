namespace Acklann.Picmin.Compression
{
    public readonly struct PingoOptions
    {
        public PingoOptions(string sourceFile, string outputFile = default, int quality = 80, int speed = -1, bool removeMetadata = true)
        {
            SourceFile = sourceFile;
            OutputFile = (outputFile ?? sourceFile.WithSuffix());
            RemoveMetadata = removeMetadata;
            Quality = quality;
            Speed = speed;
        }

        public string SourceFile { get; }

        public string OutputFile { get; }

        public bool RemoveMetadata { get; }

        public int Quality { get; }

        public int Speed { get; }

        public override string ToString()
        {
            int range(int x, int min, int max)
            {
                if (x < min) return min;
                else if (x > max) return max;
                else return x;
            }

            return string.Format("-noconversion {2}{3} -auto={1} \"{0}\"",
                OutputFile,
                range(Quality, 1, 100),
                (Speed <= 0 ? string.Empty : $"-s{range(Speed, 0, 9)} "),
                (RemoveMetadata ? string.Empty : "-nostrip")
                );
        }
    }
}