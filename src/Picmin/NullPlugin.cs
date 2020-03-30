namespace Acklann.Picmin
{
    public class NullPlugin : IPlugin
    {
        public string SourceFile { get => null; }

        public CompilerResult Run()
        {
            return new CompilerResult(
                string.Empty,
                true,
                string.Empty,
                string.Empty,
                0,
                0,
                0
                );
        }
    }
}