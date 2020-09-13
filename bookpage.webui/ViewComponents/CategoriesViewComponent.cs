using System.Collections.Generic;
using bookpage.business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace bookpage.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent//heryerde category var zaten o yüzden sürekli indexten almayayım diye bağımsız bir yapı oluşturuyorum
    {
        private ICategoryServices _categoryServices;
        public CategoriesViewComponent(ICategoryServices CategoryServices)
        {
            this._categoryServices=CategoryServices;
        }
        public IViewComponentResult Invoke()
        {
            if(RouteData.Values["category"]!=null)
            ViewBag.SelectedCategory=RouteData?.Values["category"];
           return View(_categoryServices.GetAll());//metodu categorylerimi getirir

        }
    }
}