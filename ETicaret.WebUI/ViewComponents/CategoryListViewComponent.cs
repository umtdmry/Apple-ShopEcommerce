using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Business.Base;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.ViewComponents
{
    //Categoriler List Model
    //Menümüzdeki kategorileri buradan çekmekteyiz.
    public class CategoryListViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IViewComponentResult Invoke() 
        {
            return View(new CategoryListViewModel() 
            { 
                    SelectedCategory = RouteData.Values["category"]?.ToString(),
                    Categories = _categoryService.GetAll()
            });
        }
    }
}
