using DotSee.UmbracoExtensions;
using System;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.MazelBlog
{
    public static class Extensions
    {
        public static IPublishedContent GetPostListImage(this PageArticleItem item)
        {
            IPublishedContent mediaItem = null;
            if (!string.IsNullOrEmpty(item.ListImage))
            {
                mediaItem = item.GetPickerMediaByValue(item.ListImage).First();
            }
            else if (!string.IsNullOrEmpty(item.Image))
            {
                //We don't use GetPickerMedia here to avoid the overhead
                mediaItem = ContentHelper.GetHelper().TypedMedia(item.Image.Split(',')[0]);
            }
            return (mediaItem);
        }

        public static string GetPostDate(this PageArticleItem item)
        {
            string retVal = null;
            DateTime postDate = (item.PostDate == DateTime.MinValue) ? item.CreateDate : item.PostDate;
            retVal = item.RenderDateLiteral(postDate, item.Ancestor<PageBlogList>().DateFormat);

            return (retVal);
        }

    }
}
