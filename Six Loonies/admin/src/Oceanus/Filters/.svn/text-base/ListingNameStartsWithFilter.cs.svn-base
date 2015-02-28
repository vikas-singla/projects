using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using MVCControlsToolkit.Controls.DataFilter;
using Oceanus.Models;
using Oceanus.ViewModels;

namespace Oceanus.Filters
{
    public class ListingNameStartsWithFilter : IFilterDescription<ListingViewModel>
    {
        [Required]
        [Display(Prompt = "chars the name of item starts with")]
        public string Name { get; set; }

        public Expression<Func<ListingViewModel, bool>> GetExpression()
        {
            Expression<Func<ListingViewModel, bool>> res = null;
            Name = Name.Trim();
            if (!string.IsNullOrEmpty(Name))
            {
                Name = Name.Trim();
                res = m => (m.Name.StartsWith(Name));

            }
            return res;
        }
    }
}