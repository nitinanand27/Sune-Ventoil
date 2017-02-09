using DotSee.UmbracoExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Portfolio
{

    public static class Extensions
    {

        #region Static Extension Methods

        /// <summary>
        /// Gets the list image for a portfolio item. If the document is not a portfolio item then the function will return null.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IPublishedContent GetPortfolioListImage(this PagePortfolioItem item)
        {

            //TODO: Get image from media and image cropped correctly
            IPublishedContent mediaItem = null;
            if (!string.IsNullOrEmpty(item.ListImage))
            {
                mediaItem = item.GetPickerMediaByValue(item.ListImage).First();
            }
            else if (!string.IsNullOrEmpty(item.ImageGallery))
            {
                //We don't use GetPickerMedia here to avoid the overhead
                mediaItem = ContentHelper.GetHelper().TypedMedia(item.ImageGallery.Split(',')[0]);
            }

            return (mediaItem);
        }


        public static string GetPortfolioCategoryNames(this PagePortfolioItem item)
        {

            StringBuilder sb = new StringBuilder(string.Empty);

            foreach (IPublishedContent category in item.GetPickerItemsByValue(item.Categories))
            {
                sb.Append(category.Name);
                sb.Append(", ");
            }

            string categoryNames = (sb.Length > 0) ? sb.ToString().Substring(0, (sb.ToString().Length - 2)) : "";

            return (categoryNames);
        }

        public static List<PagePortfolioItem> GetPrevNext(this PagePortfolioItem item)
        {

            SectionPortfolio portfolioRoot = item.Ancestor<SectionPortfolio>();

            PortfolioHelper h = new PortfolioHelper(portfolioRoot);

            //Get a portfoliohelper to get items in the same order as defined in portfolio settings
          
            IEnumerable<DGenericCategoryItem> categories = item.GetPickerItemsByValue<DGenericCategoryItem>(item.Categories);

            //Get the next / previous item related to the current item
            IEnumerable<PagePortfolioItem> allItems = h.GetPortfolioItems();

            //See if we must restrict to current item's categories
            if (portfolioRoot.NextPrevRestrictToCategories)
            {
                allItems = allItems.Where(x => x.Categories.IsContainedInCsv(item.Categories));
            }

            List<PagePortfolioItem> sandwitchedItems = allItems.FindSandwichedItem(x => x.Id == item.Id).ToList<PagePortfolioItem>();

            //Remove current page fron list
            sandwitchedItems.RemoveAt(1);

            return (sandwitchedItems);

        }

        public static bool MustShowRelated(this PagePortfolioItem item)
        {

            SectionPortfolio portfolioRoot = item.Ancestor<SectionPortfolio>();
            PortfolioHelper h = new PortfolioHelper(portfolioRoot);

            if (string.IsNullOrEmpty(item.RelatedItems) && !portfolioRoot.RandomRelated) { return false; }

            return true;
        }

        public static IEnumerable<PagePortfolioItem> GetRelatedItems(this PagePortfolioItem item)
        {
          
            SectionPortfolio portfolioRoot = item.Ancestor<SectionPortfolio>();
            PortfolioHelper h = new PortfolioHelper(portfolioRoot);


            if (string.IsNullOrEmpty(item.RelatedItems) && portfolioRoot.RandomRelated)
            {
                IEnumerable<PagePortfolioItem> randomItems = h.GetPortfolioItemsRandom();

                if (portfolioRoot.RandomRelatedRestrictToCategories)
                {
                    return (randomItems.Where(x => x.Categories.IsContainedInCsv(item.Categories)).Take(h.GlobalRandomRelatedNumber));
                }
                else
                {
                    return (randomItems.Take(h.GlobalRandomRelatedNumber));
                }
            }
            else
            {
                return (item.GetPickerItemsByValue<PagePortfolioItem>(item.RelatedItems));
            }

        }
        #endregion

    }
}
