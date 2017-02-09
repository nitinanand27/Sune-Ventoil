using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Models
{
    public partial class PageBlogListViewModel : PageBlogList
    {
        //Due to exception "Type DotSee.Models.BlogListModel is missing a public constructor with one argument of type IPublishedContent."
        public PageBlogListViewModel(IPublishedContent currPage) : base(currPage) { }
        public PageBlogListViewModel(PageBlogList currPage) : base(currPage) { }
        public int Page { get; set; }

        public int TotalPages { get; set; }
        
        public int TotalItems { get; set; }

        public int PreviousPage { get; set; }

        public int NextPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }
        
        public string CustomMessage { get; set; }

        public IEnumerable<PageArticleItem> BlogPosts { get; set; }
    }
}
