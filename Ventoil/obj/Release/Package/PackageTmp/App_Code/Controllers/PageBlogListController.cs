using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Core.Models;
using DotSee.MazelBlog;
using Umbraco.Core;
using DotSee.Models;

namespace DotSee.Controllers
{
    public class PageBlogListController : Umbraco.Web.Mvc.RenderMvcController
    {

        public ActionResult PageBlogList(PageBlogList model)
        {
            PageBlogListViewModel viewModel = new PageBlogListViewModel(model);
            BlogHelper bh = BlogHelperMaker.GetCachedBlogHelper(ApplicationContext.Current, viewModel);

            IPublishedContent categoryFilter = null;
            if (!string.IsNullOrEmpty(Request.QueryString["c"]))
            {
                try
                {
                    categoryFilter = Umbraco.TypedContent(Request.QueryString["c"]);
                    if (!categoryFilter.ContentType.Alias.Equals(DGenericCategoryItem.ModelTypeAlias)) { categoryFilter = null; }
                }
                catch { }
            }
            string customMessage = null;

            string tagFilter = (!string.IsNullOrEmpty(Request.QueryString["t"])) ? Request.QueryString["t"] : null;

            int recordsPerPage = (viewModel.PostsPerPage >= 1) ? viewModel.PostsPerPage : 5;
            int page; int.TryParse(Request.QueryString["page"], out page);

            page = page < 1 ? 1 : page;

            IEnumerable<PageArticleItem> posts = bh.GetPostItems()
                .Where(
                x => (categoryFilter != null && x.Categories.Split(',').Contains(categoryFilter.Id.ToString()))
                
                || (tagFilter!=null && ((x.Tags != null) ? x.Tags : "").ToString().Split(',').Contains(tagFilter))

                || (tagFilter==null && categoryFilter == null)

                )
                .Skip(recordsPerPage * (page - 1))
                .Take(recordsPerPage);

            int filteredTotalItems = 0;
            if (categoryFilter != null)
            {
                filteredTotalItems = bh.GetCountPerCategory(categoryFilter.Id);
                customMessage = string.Concat("(", Umbraco.GetDictionaryValue("Blog.FilterBy.Category"), categoryFilter.Name, ") ");
            }

            else if (tagFilter != null)
            {
                filteredTotalItems = bh.GetCountPerTag(tagFilter);
                 customMessage = string.Concat("(", Umbraco.GetDictionaryValue("Blog.FilterBy.Tag"), tagFilter, ") ");
            }
            else
            {
                filteredTotalItems = bh.GetTotalCount();
            }

            viewModel.TotalItems = filteredTotalItems;
            viewModel.TotalPages = (int)Math.Ceiling((double)viewModel.TotalItems / (double)recordsPerPage);
            viewModel.Page = page;
            viewModel.IsFirstPage = (page == 1);
            viewModel.IsLastPage = (page == viewModel.TotalPages);
            viewModel.BlogPosts = posts;
            viewModel.CustomMessage = customMessage;

            return CurrentTemplate(viewModel);
        }
    }
}