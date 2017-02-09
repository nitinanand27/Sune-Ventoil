using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Portfolio
{
    public static class PortfolioHelperMaker
    {
        public static PortfolioHelper GetCachedPortfolioHelper(ApplicationContext cx, SectionPortfolio portfolioRoot)
        {
            return (PortfolioHelper)cx.ApplicationCache.RuntimeCache.GetCacheItem(string.Format("portfolioHelper{0}", portfolioRoot.Id.ToString()), () => new PortfolioHelper(portfolioRoot));
        }
    }
}