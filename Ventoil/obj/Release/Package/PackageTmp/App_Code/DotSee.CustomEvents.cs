using Umbraco.Core;
using Umbraco.Web.PublishedContentModels;

namespace DotSee
{
    public class CustomEvents : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            AutoNode au = AutoNode.Instance;
            au.RegisterRule(new AutoNodeRule(Pagehome.ModelTypeAlias, FolderConfiguration.ModelTypeAlias, "Site Configuration", true, false));
            au.RegisterRule(new AutoNodeRule(Pagehome.ModelTypeAlias, FolderHomeAssets.ModelTypeAlias, "Homepage Assets", true, false));
            au.RegisterRule(new AutoNodeRule(Pagehome.ModelTypeAlias, FolderSections.ModelTypeAlias, "Page Elements", true, false));
            au.RegisterRule(new AutoNodeRule(FolderConfiguration.ModelTypeAlias, ConfigGlobalSettings.ModelTypeAlias, "Global Settings", true, false));
            au.RegisterRule(new AutoNodeRule(FolderConfiguration.ModelTypeAlias, FolderSectionsReusable.ModelTypeAlias, "Reusable Content Sections", false, false));
            au.RegisterRule(new AutoNodeRule(PageContent.ModelTypeAlias, FolderSections.ModelTypeAlias, "Page Elements", true, false));
            au.RegisterRule(new AutoNodeRule(PagePortfolioItem.ModelTypeAlias, FolderPortfolioSections.ModelTypeAlias, "Additional Portfolio Elements", true, true));
            au.RegisterRule(new AutoNodeRule(SectionImageGallery.ModelTypeAlias, FolderImageGalleryImages.ModelTypeAlias, "Images", true, false));
            au.RegisterRule(new AutoNodeRule(SectionImageGallery.ModelTypeAlias, FolderGenericCategories.ModelTypeAlias, "Categories", true, false));
            au.RegisterRule(new AutoNodeRule(SectionPortfolio.ModelTypeAlias, FolderPortfolioItems.ModelTypeAlias, "Portfolio Items", true, false));
            au.RegisterRule(new AutoNodeRule(SectionPortfolio.ModelTypeAlias, FolderGenericCategories.ModelTypeAlias, "Categories", true, false));
            au.RegisterRule(new AutoNodeRule(FolderHomeAssets.ModelTypeAlias, FolderSliderImages.ModelTypeAlias, "Slider Images", true, false));
            au.RegisterRule(new AutoNodeRule(FolderHomeAssets.ModelTypeAlias, FolderLinkButtons.ModelTypeAlias, "Link Buttons", true, false));
            au.RegisterRule(new AutoNodeRule(DSliderImage.ModelTypeAlias, FolderLinkButtons.ModelTypeAlias, "Buttons", true, false));
            au.RegisterRule(new AutoNodeRule(PageBlogList.ModelTypeAlias, FolderGenericCategories.ModelTypeAlias, "Categories", true, false));
            au.RegisterRule(new AutoNodeRule(PageBlogList.ModelTypeAlias, FolderAuthors.ModelTypeAlias, "Authors", true, false));
            au.RegisterRule(new AutoNodeRule(PageBlogList.ModelTypeAlias, FolderArticlePosts.ModelTypeAlias, "Posts", true, false));

          
        }
    }
}