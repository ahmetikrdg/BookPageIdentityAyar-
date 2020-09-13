using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using bookpage.entity;

namespace bookpage.webui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Display(Name="Kitap Adı",Prompt="Kitap Adını Giriniz")]
        [Required(ErrorMessage="Kitap Adı Zorunlu Bir Alandır")]
        [StringLength(60,MinimumLength=2,ErrorMessage="2 ile 60 arası karakter giriniz")]
        public string Name { get; set; }
        [Display(Prompt="Url Adını Giriniz")]
        [Required(ErrorMessage="Url Adı Zorunlu Bir Alandır")]
        public string Url { get; set; }
        [Display(Name="Yazar",Prompt="Yazar Adını Giriniz")]
        [Required(ErrorMessage="Yazar Adı Zorunlu Bir Alandır")]
        public string Author { get; set; }
        [Display(Name="Kitap Sayfası",Prompt="Sayfa Sayısını Giriniz")]
        [Required(ErrorMessage="Sayfa Sayısı Zorunlu Bir Alandır")]
        public double? Pages { get; set; }//? koymasaydım 0 olarak alırdı ve işlevsel olmazdı
         [Display(Name="Özet",Prompt="Kitabın Özerini Giriniz")]
        [Required(ErrorMessage="Özet Zorunlu Bir Alandır")]
        public string Description { get; set; }
        // public string Door { get; set; }
        // public string Moduls1 { get; set; }
        // public string Moduls2 { get; set; }
        // public string Moduls3 { get; set; }
        // public string Moduls4 { get; set; }
        public string ImageUrl { get; set; }
        public bool  IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> categories { get; set; }
    }
}