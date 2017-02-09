using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.MazelBlog
{
    public static class BlogHelperMaker
    {
        public static BlogHelper GetCachedBlogHelper(ApplicationContext cx, PageBlogList blogRoot)
        {
            return (BlogHelper)cx.ApplicationCache.RuntimeCache.GetCacheItem(string.Format("blogHelper{0}", blogRoot.Id.ToString()), () => new BlogHelper(blogRoot));
        }
    }
}