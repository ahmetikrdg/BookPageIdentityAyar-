
using bookpage.business.Abstract;
using bookpage.data.Abstract;
using bookpage.entity;
using bookpage.webui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bookpage.webui.Controllers
{
    public class ProductController:Controller
    {
        private IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            this._productServices=productServices;
        }
        public IActionResult Index()
        {
            var productsViewModel = new ProductListViewModel
            {
               Products = _productServices.GetHomePageProducts()
            };

            return View(productsViewModel);
        }
        public IActionResult Search(string q)
        {
            var productsViewModel = new ProductListViewModel
            {
               
               Products = _productServices.GetSearchResult(q)
            };

            return View(productsViewModel);
        }

        public IActionResult List(int? id,string q)
        {
           //var products=ProductRepository.Products;
            // if(id!=null)
            // {
            //     products=products.Where(p=>p.CategoryId==id).ToList();
            // }

            // if(!string.IsNullOrEmpty(q))
            // {
            //     products=products.Where(i=>i.Name.ToLower().Contains(q.ToLower())).ToList();
            // }
           
            var productsViewModel = new ProductListViewModel
             {
                Products = _productServices.GetAll()
             };

            return View(productsViewModel);
        }

        public IActionResult Details(int id)
        {
            return View();
            //id parametresini gönderdim ve viewüzerine bir pdourct nesnesi gönderiyor
        }

        public IActionResult Read(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Categories=new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            //return View(new Product());
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product p)
        {
           // if(ModelState.IsValid)//beklediğimiz tüm bilgiler uyguladığımız kurallara göre oluştu.Productta
            //{
             // ProductRepository.Addproducts(p);
              return RedirectToAction("list");
            //}
             //ViewBag.Categories=new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            //return View(p);//eğer yanlış girerse girdiği bilgilerle geldiği ekrana gitsin
           
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //ViewBag.Categories=new SelectList(CategoryRepository.Categories,"CategoryId","Name");
           // return View(ProductRepository.GetProductById(id));
                       return View();

        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            //ProductRepository.EditProduct(p);
            return RedirectToAction("list");
        }

        [HttpPost]
        public IActionResult Delete(int ProductId)
        {
           // ProductRepository.DeleteProduct(ProductId);
            return RedirectToAction("list");
        }
    }
}