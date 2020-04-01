namespace Acklann.Picmin
{
    public interface ICommand
    {
        string SourceFile { get; }

        CompilerResult Run();
    }
}