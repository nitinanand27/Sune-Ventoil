using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Portfolio
{
    public class PortfolioEvents : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            ContentService.Published += ContentService_Published;
            ContentService.Deleted += ContentService_Deleted;
        }

        private void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                if (item.ContentType.Alias == SectionPortfolio.ModelTypeAlias
                    || item.ContentType.Alias == PagePortfolioItem.ModelTypeAlias
                    || item.ContentType.Alias == DGenericCategoryItem.ModelTypeAlias
                    )
                {
                    //Clear the cache.
                    ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch("portfolioHelper");
                }
            }
        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var item in e.PublishedEntities)
            {
                if (item.ContentType.Alias == SectionPortfolio.ModelTypeAlias 
                    || item.ContentType.Alias == PagePortfolioItem.ModelTypeAlias
                    || item.ContentType.Alias == DGenericCategoryItem.ModelTypeAlias
                    )
                {
                    //Clear the cache.
                    ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch("portfolioHelper");
                }
            }
        }
    }

}