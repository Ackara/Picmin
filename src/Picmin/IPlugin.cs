namespace Acklann.Picmin
{
    public interface IPlugin
    {
        string SourceFile { get; }

        CompilerResult Run();
    }
}