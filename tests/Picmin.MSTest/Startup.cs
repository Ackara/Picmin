using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Acklann.Diffa.ApprovedFolder("approved-folder")]
[assembly: Acklann.Diffa.Reporters.Reporter(typeof(Acklann.Diffa.Reporters.DiffReporter))]

namespace Acklann.Picmin
{
    [TestClass]
    public class Startup
    {
        public static void Configure(TestContext context)
        {
            
        }
    }
}