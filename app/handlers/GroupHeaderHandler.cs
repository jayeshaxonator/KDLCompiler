using KdlDotNet;

namespace KDLCompiler
{
    internal class GroupHeaderHandler : SectionHandler
    {
        protected override string GetTemplateName()
        {
            return "GroupHeaderSectionHandler";
        }
    }
}