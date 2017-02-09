using DotSee.UmbracoExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.MazelBlog
{
    /// <summary>
    /// A class to rule all blog-related things.
    /// Yes, it's all in memory - don't hit me for that. It is supposed to handle a blog-inside-a-website
    /// which typically has a limited number of posts.
    /// </summary>
    public class BlogHelper
    {
        #region Private Members

        private List<PageArticleItem> _postItems = null;
        private List<DGenericCategoryItem> _categoryItems = null;
        private List<string> _tagItems = null;
        private Dictionary<int, int> _categoryItemCount = null;
        private Dictionary<string, int> _tagItemCount = null;

        #endregion

        #region Properties 

        //private UmbracoHelper _umbHelper;
        private PageBlogList _currPage;

        public bool GlobalTagsActive { get; private set; }

        public bool GlobalAuthorsActive { get; private set; }

        public bool GlobalCategoriesActive { get; private set; }

        public bool GlobalRecentArticlesActive { get; private set; }

        public bool GlobalHideDates { get; private set; }

        public string GlobalDateFormat { get; private set; }

        public bool ShowItemCount { get; private set; }

        #endregion

        #region Ctor and relevant methods
        public BlogHelper(PageBlogList currPage)
        {
            _currPage = currPage;
            LoadSettings();

            _postItems = _currPage.Descendants<PageArticleItem>().ToList();

            if (GlobalTagsActive)
            {
                _tagItemCount = new Dictionary<string, int>();
                IPublishedContent homePage = currPage.AncestorOrSelf(1);
                var allTags = ContentHelper.GetHelper().TagQuery.GetAllContentTags("blogtags")
                                       .Where(x => x.NodeCount > 0)
                                       .Where(x => ContentHelper.GetHelper().TagQuery.GetContentByTag(x.Text, "blogtags").Where(y => y.IsInPath(homePage)).Any());

                foreach (var t in allTags) {
                    _tagItemCount.Add(t.Text, _postItems.Where(x => x.Tags!=null && x.Tags.ToString().Split(',').Contains(t.Text)).Count());
                }

                

            }
            //Load categories and blog posts.
            if (GlobalCategoriesActive)
            {
                _categoryItems = _currPage.Descendant<FolderGenericCategories>().Children<DGenericCategoryItem>().ToList();
            }

            

            //If categories enabled, count blog posts for each category
            if (GlobalCategoriesActive)
            {
                _categoryItemCount = new Dictionary<int, int>();

                //Get only the non-empty categories (GetCategories() gets only categories with items) and count items in each
                foreach (DGenericCategoryItem categoryItem in GetCategories())
                {
                    _categoryItemCount.Add(
                        categoryItem.Id,
                        _postItems.Where(x => !string.IsNullOrEmpty(x.Categories) && categoryItem.Id.ToString().IsContainedInCsv(x.Categories)).Count()
                    );
                }
            }
        }

        private void LoadSettings()
        {
            //Get user-defined global settings
            GlobalCategoriesActive =
                (
                    !_currPage.HideCategories
                    && _currPage.Descendant(FolderGenericCategories.ModelTypeAlias).HasChildren(includeWithPrefix: DGenericCategoryItem.ModelTypeAlias)
                );

            GlobalRecentArticlesActive =
                (
                    !_currPage.HideRecentArticles
                    && _currPage.Descendant(FolderArticlePosts.ModelTypeAlias).HasChildren(includeWithPrefix: PageArticleItem.ModelTypeAlias)
                );
            GlobalAuthorsActive =
                (
                    !_currPage.HideAuthors
                    && _currPage.Descendant(FolderAuthors.ModelTypeAlias).HasChildren(includeWithPrefix: PageBlogAuthor.ModelTypeAlias)
                );

            GlobalTagsActive = !_currPage.HideTags;

            GlobalHideDates = _currPage.HideDates;

            GlobalDateFormat = _currPage.DateFormat;

            //Show counters
            ShowItemCount = _currPage.ShowItemCount;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the total count of all blog posts for this blog
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            return (_postItems.Count());
        }

        /// <summary>
        /// Gets a count of blog posts for the given category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int GetCountPerCategory(DGenericCategoryItem category)
        {
            return (_categoryItemCount[category.Id]);
        }

        public int GetCountPerCategory(int categoryId) {
            return (_categoryItemCount[categoryId]);
        }

        public int GetCountPerCategory(string categoryId) {
            int cid;
            int.TryParse(categoryId, out cid);
            if (cid == 0) return 0; 
            return (_categoryItemCount[cid]);
        }

        public int GetCountPerTag(string tag)
        {
            return _tagItemCount[tag];
        }
        
        /// <summary>
        /// Gets the list of blog posts sorted according to settings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PageArticleItem> GetPostItems()
        {

            //Get all blog posts
            return _postItems.OrderByDescending(x => x.PostDate);

        }

        /// <summary>
        /// Gets the list of blog posts sorted randomly
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PageArticleItem> GetPostItemsRandom()
        {

            //Get all blog posts
            return _postItems.OrderBy(x => Guid.NewGuid());

        }

        /// <summary>
        /// Gets the list of categories. Empty categories are excluded.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DGenericCategoryItem> GetCategories()
        {
            IEnumerable<DGenericCategoryItem> categories = Enumerable.Empty<DGenericCategoryItem>();

            if (GlobalCategoriesActive)
            {
                //Check if category is contained in at least one blog post's picker. If not, don't add to list since it's an empty category.
                categories = _categoryItems
                    .Where(x => x.IsContainedInAnyPicker(_postItems.AsEnumerable(), "categories"));
            }
            return (categories);
        }
        #endregion

    }

}
