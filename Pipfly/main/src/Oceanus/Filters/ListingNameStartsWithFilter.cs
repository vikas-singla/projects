﻿//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Linq.Expressions;
//using MVCControlsToolkit.Controls.DataFilter;
//using MVCControlsToolkit.Linq;
//using Oceanus.ViewModels;

//namespace Oceanus.Filters
//{
//    public class ListingNameStartsWithFilter : IFilterDescription<ListItemViewModel>
//    {
//        [Required]
//        [Display(Prompt = "Name starts with...")]
//        public string Name { get; set; }
//        public bool IsNameEnabled { get; set; }

//        public Expression<Func<ListItemViewModel, bool>> GetExpression()
//        {
//            Expression<Func<ListItemViewModel, bool>> res = new FilterBuilder<ListItemViewModel>()
//                .Add(IsNameEnabled && !string.IsNullOrWhiteSpace(Name), m => m.Name.StartsWith(Name))
//                .Get();
            
//            return res;
//        }
//    }
//}