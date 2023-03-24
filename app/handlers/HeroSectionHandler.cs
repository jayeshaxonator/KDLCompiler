using System.Dynamic;
using Cottle;
using KdlDotNet;
using SimpleJinja2DotNet;

namespace KDLCompiler
{
    internal class HeroSectionHandler : SectionHandler
    {
        protected override string GetTemplateName()
        {
            return "HeroSectionHandler";
        }
    }
}