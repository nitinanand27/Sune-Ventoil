using System.Text;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.UmbracoExtensions
{
    public static class SectionEffectParser
    {
        public static string GetSectionEffectCssClass (this IAbstractSectionEffects item) {

            if (!item.AnimationEnable) { return null; }

            StringBuilder sb = new StringBuilder("wow fadeIn");

            if (item.AnimationDirection != null)
            {
                sb.Append(item.AnimationDirection.ToString());
            }
            
            if (item.AnimationLarger)
            {
                sb.Append("Big");
            }

            return (sb.ToString());
        }
    }
}