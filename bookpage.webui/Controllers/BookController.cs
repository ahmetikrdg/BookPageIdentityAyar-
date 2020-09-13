using bookpage.business.Abstract;
using bookpage.entity;
using bookpage.webui.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace bookpage.webui.Controllers
{
    public class BookController:Controller
    {
        private IProductServices _productServices;
        public BookController(IProductServices productServices)
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
        public IActionResult List(string category, int page=1)
        {
            const int pageSize=3;
            var productsViewModel = new ProductListViewModel
            {
               pageInfo= new PageInfo()
               {
                   TotalItems=_productServices.GetCountByCategory(category),//kategoriye göre tüm ürün sayılarımı aldım kategori yoksa hepsi gelsin
                   CurrentPage=page,//metoda gönderdiğimiz page bilgisi
                   ItemsPerPage=pageSize,//kaç ürün göstermek istiyorum
                   CurrentCategory=category//hangi kategori varsa gelir
               },
               Products = _productServices.GetProductsByCategory(category,page,pageSize)
            };//bunu productviewmodel.csye gönderiyo onu viewde kullanıyo sonra onu ekrana basıyo

            return View(productsViewModel);
        }

        public IActionResult Details(string Url)
        {
           if(Url==null)//eğer id yoksa hata sayfası ver
            {
                return NotFound();
            }
            //Product productt=_productServices.GetProductDetails((int)id);//bunu getall dan getproddeta ile değiştirdim.
            Product productt=_productServices.GetProductDetails(Url);
            if(productt==null)//eğer product yoksa hata sayfası
            {
                return NotFound();
            }
            //return View(productt);//buldunduysa bulduğu product bilgisini sayfaya mdel olarak gönderelim şimdi details cshtml düzenleyelim
               return View(new ProductDetailModel{//viewmodel içine paketledim
                 product = productt,
                 Category = productt.ProductCategories
                .Select(i=>i.Categories)// zaten vt'den sorguladığımız bir bilgi üstüne select işlemi yapabiliriz her gelen bilgi üzerinden kategoriye geçeriz
                .ToList()//ve almış olduğum her kategoriyi foreace kullanıyomuşum gibi i içine kopyaliycam ve gitmiş olduğum productun categorysine gidicem
            });//ürünü aldım kategorilerinide getirdim 1. telefonlar o veriyi post edeceksin ama elinde product ve category var ve başka categoryde varsa o ürnün hangi kategoriden geldiğini bilmen lazım oda bu
        }

        public IActionResult Read(string Url)
        {
            if(Url==null)//eğer id yoksa hata sayfası ver
            {
                return NotFound();
            }
            Product productt=_productServices.GetProductDetails(Url);//bize product bilgisi geliyor product bilgisi içerisinden productcategories ve ordanda categori bilgilerine geçiş yapabiliyoruz

            if(productt==null)//eğer product yoksa hata sayfası
            {
                return NotFound();
            }
           // return View(product);//buldunduysa bulduğu product bilgisini sayfaya mdel olarak gönderelim ve readcshtml'de bilgiler gözüksün
               return View(new ProductDetailModel{
                 product = productt,
                 Category = productt.ProductCategories.Select(i=>i.Categories).ToList()
             });
        }
        public IActionResult Search(string q)
        {
            var productsViewModel = new ProductListViewModel
            {
               
               Products = _productServices.GetSearchResult(q)
            };

            return View(productsViewModel);
        }
        /*public IActionResult deneme()
        {
            
            var productsViewModel = new ProductListViewModel
            {
               
               Products = _productServices.GetAll()
            };
            return View(productsViewModel);
        }*/



    }
}