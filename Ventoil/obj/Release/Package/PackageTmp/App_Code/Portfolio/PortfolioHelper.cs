using DotSee.PropSense;
using DotSee.UmbracoExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Portfolio
{

    public class PortfolioHelper
    {

        #region Private Members

        private List<PagePortfolioItem> _portfolioItems = null;
        private List<DGenericCategoryItem> _categoryItems = null;
        private Dictionary<int, int> _categoryItemCount = null;
        private UmbracoHelper _umbHelper;
        private SectionPortfolio _currPage;

        #endregion

        #region Properties 

        public int GlobalRandomRelatedNumber { get; private set; }

        public string GlobalDateFormat { get; private set; }

        private string _allOptionLabel;

        private bool _hasCategories;

        public bool HasCategories
        {
            get
            {

                return _hasCategories;

            }

        }

        private int _noOfCols;

        public int NoOfCols { get { return _noOfCols; } }

        public bool Masonry { get; private set; }
        public bool Spacing { get; private set; }
        public bool UpscaleImages { get; private set; }
        public bool ShowItemCount { get; private set; }
        public bool Wide { get; private set; }

        #endregion

        #region Ctor and relevant methods
        public PortfolioHelper(SectionPortfolio currPage)
        {

            _currPage = currPage;
            _umbHelper = ContentHelper.GetHelper();

            LoadSettings();

            //Load categories and portfolio items.
            if (_hasCategories)
            {
                _categoryItems = _currPage.Descendant<FolderGenericCategories>().Children<DGenericCategoryItem>().ToList();
            }

            _portfolioItems = _currPage.Descendants<PagePortfolioItem>().ToList();

            //If show counters enabled and categories are enabled, count portfolio items for each category
            //This is done here to preserve processing if hide categories is enabled.
            if (_currPage.ShowItemCount && !_currPage.DoNotDisplayCategories)
            {
                _categoryItemCount = new Dictionary<int, int>();

                //Get only the non-empty categories (GetCategories() gets only categories with items) and count items in each
                foreach (IPublishedContent categoryItem in this.GetCategories())
                {
                    _categoryItemCount.Add(
                        categoryItem.Id,
                        _portfolioItems.Where(x => categoryItem.Id.ToString().IsContainedInCsv(x.Categories)).Count()
                    );
                }
            }
        }

        private void LoadSettings()
        {
            //Get user-defined global settings
           
            GlobalRandomRelatedNumber = (_currPage.RandomRelatedNumber<=0)?3:_currPage.RandomRelatedNumber;
            
            GlobalDateFormat = (string.IsNullOrEmpty(_currPage.DateFormat)) ? "dd/MM/yyyy" : _currPage.DateFormat;

            //Get label for "All" category menu option
            _allOptionLabel = string.IsNullOrEmpty(_currPage.AllOptionLabel)? _umbHelper.GetDictionaryValue("Theme.Gallery.All").ToString() : _currPage.AllOptionLabel;
 
            //See if there are any categories defined at all
            _hasCategories = _currPage.Descendant<FolderGenericCategories>().HasChildren<DGenericCategoryItem>();

            //Get number of columns
            _noOfCols = _currPage.Columns <= 0 ? 2 : _currPage.Columns;

            //Decide if masonry or grid
            Masonry = _currPage.Masonry;

            //Are images spaced?
            Spacing = _currPage.Spacing;

            //Get image upscaling
            UpscaleImages = _currPage.GetPropertyValue<bool>("upscaleImages", false);

            //Wide page
            Wide = _currPage.Wide;

            //Show counters
            ShowItemCount = _currPage.ShowItemCount;

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the total count of all portfolio items for this portfolio
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            return (_portfolioItems.Count());
        }

        /// <summary>
        /// Gets a count of portfolio items for the given category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int GetCountPerCategory(DGenericCategoryItem category)
        {
            if (!ShowItemCount) { return 0; }
            return (_categoryItemCount[category.Id]);

        }
        /// <summary>
        /// Gets the list of portfolio items sorted according to settings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PagePortfolioItem> GetPortfolioItems()
        {

            //Get all portfolio items
            return (_currPage.SortByDate) ? _portfolioItems
                .OrderByDescending(x => x.ReleaseDate) : _portfolioItems.AsEnumerable();

        }

        /// <summary>
        /// Gets the list of portfolio items sorted randomly
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PagePortfolioItem> GetPortfolioItemsRandom()
        {

            //Get all portfolio items
            return _portfolioItems.OrderBy(x => Guid.NewGuid());

        }

        /// <summary>
        /// Gets the list of categories. Empty categories are excluded.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DGenericCategoryItem> GetCategories()
        {

            IEnumerable<DGenericCategoryItem> categories = null;

            if (_hasCategories)
            {
                //Check if category is contained in at least one portfolio item's picker. If not, don't add to list since it's an empty category.
                categories = _categoryItems
                    .Where(x => x.IsContainedInAnyPicker(_portfolioItems.AsEnumerable(), Psn_pagePortfolioItem.categories));
            }
            return (categories);
        }
        
        #endregion

    }

}