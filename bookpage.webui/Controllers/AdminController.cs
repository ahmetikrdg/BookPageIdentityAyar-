using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using bookpage.business.Abstract;
using bookpage.entity;
using bookpage.webui.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bookpage.webui.Controllers
{
    public class AdminController:Controller
    {
        private IProductServices _productServices;
        private ICategoryServices _categoryServices;
        public AdminController(IProductServices productServices,ICategoryServices categoryServices)
        {
            this._productServices=productServices;
            this._categoryServices=categoryServices;
        }
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products=_productServices.GetAll(),
            });
        }
        [HttpGet]
        public IActionResult ProductCreate()
        {//ProductCreate içindeki bilgileri getirdim.
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
        {
            if(ModelState.IsValid)
            {
            var entity=new Product()
            {
                Name=model.Name,
                Url=model.Url,
                Author=model.Author,
                Pages=model.Pages,
                Description=model.Description,
                ImageUrl=model.ImageUrl,
            };
            _productServices.Create(entity);
            var msg= new AlertMessage()
            {
              Message=$"{entity.Name} isimli ürün eklendi",
              AlertType="success"
            };
            TempData["message"]=JsonConvert.SerializeObject(msg); 
            return RedirectToAction("ProductList");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id)//gelen idye göre sorgular gelen idye göre bilgiyi göstericezb
        {
            if(id==null)
            {
                return NotFound();
            }
            var entity=_productServices.GetByIdWithCategories((int)id);//bunla gittim ürünü buldum entity içine atadım
            var model=new ProductModel()//buradada entity içinden modele atıyorum
            {
                ProductId=entity.ProductId,
                Name=entity.Name,
                Url=entity.Url,
                Author=entity.Author,
                Pages=entity.Pages,
                Description=entity.Description,
                ImageUrl=entity.ImageUrl,
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                categories=entity.ProductCategories.Select(i=>i.Categories).ToList()//seçilmiş olan ürünle ilişkili kategorileri bir listeye çevirip categories içine attım
            };
            //categorieste sadece o idye ait ürünü aldım şimdi ise tüm kategori bilgilerini alacağım
            ViewBag.Category=_categoryServices.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model,int [] CategoryIds,IFormFile file)
        {
            var entity=_productServices.GetById(model.ProductId);//modelin productıdsini buldum getirdim eski modeldekiyle yenisini değiştiricem
            if(entity==null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
            entity.Name=model.Name;
            entity.Author=model.Author;
            entity.Url=model.Url;
            entity.Pages=model.Pages;
            entity.Description=model.Description;
            entity.IsApproved=model.IsApproved;
            entity.IsHome=model.IsHome;
            if(file!=null)//eğer biz file bilgisi göndermişsek
            {
                var extension=Path.GetExtension(file.FileName);//gönderdiğimiz file.filenamenin jph mi png mi old bulur
                var randomName=string.Format($"{Guid.NewGuid()}{extension}");//guid ile benzersiz sayılar gelir.Orjinal resmin uzantısını jpgmi png mi extension ile gönderdik
                entity.ImageUrl=randomName;//vtye bu randomname ile kaydettik
                var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",randomName);//getcurren uygulamanaın çalıştığı yeri getirir ve bu dizinle beraber bizim birleştirmek istediğimiz yer wwwroot ordanda imgye gidicez
               //bu işlemle filename ile resmin ismini kaydettik ve resmin tam yolunuda bu path.combine ve içindekilerle aldık.Peki bu yolada resim kaydetmemiz gerekecek
   //artık resmin uzantısı ve nereye kaydedeceğiz bununla ilgili bilgimiz var şimdi bu resmi kaydedelim
                using(var stream= new FileStream(path,FileMode.Create))//path bilgisini verdim
                {
                   await file.CopyToAsync(stream);//await ile durdumdum file ile oluşturduğumuz streamı ilgili metoda kaydetmiş oluyoruz
                }//public async Task yazdımki bu işlemi görsün asenkron olarak
            }
            _productServices.Update(entity,CategoryIds);
            var msg= new AlertMessage()
            {
              Message=$"{entity.Name} isimli ürün güncellendi",
              AlertType="primary"
            };
            TempData["message"]=JsonConvert.SerializeObject(msg); 
            
            return RedirectToAction("ProductList");
            }
            return View(entity);
        }

        [HttpPost]
        public IActionResult ProductDelete(int productId)
        {
            var entity=_productServices.GetById(productId);
            if(entity!=null)
            {
                _productServices.Delete(entity);
            }           

            var msg= new AlertMessage()
            {
              Message=$"{entity.Name} isimli ürün silindi",
              AlertType="danger"
            };
            TempData["message"]=JsonConvert.SerializeObject(msg); 

            return RedirectToAction("ProductList");         
        }

//-----------------CATEGORY-----------------
         
        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories=_categoryServices.GetAll(),
            });
        }


        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            var entity=new Category()
            {
                Name=model.Name,
                Url=model.Url,
            };
            _categoryServices.Create(entity);
            var msg= new AlertMessage()
            {
              Message=$"{entity.Name} isimli kategori eklendi",
              AlertType="success"
            };
            TempData["message"]=JsonConvert.SerializeObject(msg); 
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult CategoryEdit(int? id)//gelen idye göre sorgular gelen idye göre bilgiyi göstericezb
        {
            if(id==null)
            {
                return NotFound();
            }
            var entity=_categoryServices.GetByIdWithProducts((int)id);//ile entity bize gelir onuda sayfaya göndeririz
            var model=new CategoryModel()//buradada entity içinden modele atıyorum
            {
                CategoryId=entity.CategoryId,
                Name=entity.Name,
                Url=entity.Url,
                Products= entity.ProductCategories.Select(p=>p.products).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            //var entity=_categoryServices.GetById(model.CategoryId);
            var entity=_categoryServices.GetById(model.CategoryId);
            if(entity==null)
            {
                return NotFound();
            }
            entity.Name=model.Name;
            entity.Url=model.Url;

            _categoryServices.Update(entity);
            
            var msg= new AlertMessage()
            {
              Message=$"{entity.Name} isimli kategori güncellendi",
              AlertType="primary"
            };
            TempData["message"]=JsonConvert.SerializeObject(msg); 
            
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult CategoryDelete(int categoryId)
        {
            var entity=_categoryServices.GetById(categoryId);
            if(entity!=null)
            {
                _categoryServices.Delete(entity);
            }           

            var msg= new AlertMessage()
            {
              Message=$"{entity.Name} isimli kategori silindi",
              AlertType="danger"
            };
            TempData["message"]=JsonConvert.SerializeObject(msg); 

            return RedirectToAction("CategoryList");         
        }
        
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryServices.DeleteFromCategory(productId,categoryId);
            return Redirect("/admin/categories/"+categoryId);         
        }

    }
}