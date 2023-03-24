using KdlDotNet;

namespace KDLCompiler
{
    internal interface ISectionHandler
    {
        string RenderSection(KDLNode node);
    }
}