﻿using Pustok_project.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace Pustok_project.ViewModels.TagVM
{
    public class TagListItemVM
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        //public LoadMoreVM<IEnumerable<AdminProductListItemVM>> PaginatedProducts { get; set; }


    }
}
