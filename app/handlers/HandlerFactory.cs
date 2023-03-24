namespace KDLCompiler
{
    internal class HandlerFactory
    {
        internal static ISectionHandler GetHandler(string handlerClassName)
        {
            switch (handlerClassName)
            {
                case "Import":
                    return new ImportSectionHandler();
                case "CTASection":
                    return new CTASectionHandler();
                case "DefaultNavigationSection":
                    return new DefaultNavigationSectionHandler(); 
                case "HeroSection":
                    return new HeroSectionHandler(); 
                case "ClientLogosSection":
                    return new ClientLogosSection(); 
                case "GroupHeader":
                    return new GroupHeaderHandler(); 
                case "ImageSection":
                    return new ImageSectionHandler(); 
                case "ScheduleDemoSection":
                    return new ScheduleDemoSectionHandler(); 
                case "TestimonialsSection":
                    return new TestimonialsSectionHandler(); 
                case "DefaultFooterSection":
                	return new DefaultFooterSectionHandler(); 
                case "AutoAlignImageSections":
                	return new AutoAlignImageSectionsHandler();
                case "CTABoxRightImageFullPurpleBackSection":
                    return new CTABoxRightImageFullPurpleBackSectionHandler();
                case "CTANoBoxFullLightPurpleBackSection":
                    return new CTANoBoxFullLightPurpleBackSectionHandler();
                case "GridSection":
                    return new GridSectionHandler();
                case "IndustriesSection":
                    return new IndustriesSectionHandler();
                case "NavigationSection":
                    return new NavigationSectionHandler();
                case "PageBeginSection":
                    return new PageBeginSectionHandler();
                case "PageEndSection":
                    return new PageEndSectionHandler();
                case "WatchVideoSection":
                    return new WatchVideoSectionHandler();

                default:
                    throw new KDLParserException($"Unknown section type: {handlerClassName}");
            }
        }
    }
}