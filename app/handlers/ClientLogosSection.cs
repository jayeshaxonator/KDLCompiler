using KdlDotNet;

namespace KDLCompiler
{
    internal class ClientLogosSection : SectionHandler
    {
        protected override string GetTemplateName()
        {
            return "ClientLogosSectionHandler";
        }
    }
}