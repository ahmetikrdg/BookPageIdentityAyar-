using System;

namespace bookpage.webui.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }//vtde kaç ürünüm var toplam
        public int ItemsPerPage { get; set; }//her sayfada kaç ürün göstermek istiyorum
        public int CurrentPage { get; set; }//o an hangi sayfayı gösteriyorum
        public string CurrentCategory { get; set; }//linkte kategori varmı yokmu
        //toplam 10 ürün var ve her sayfada 3 olursa 10/3:3.3 olur bunu yuvarlamam lazım 4 e
        public int TotalPages()
        {//kaç ürün varsa kaç sayfa göstereceğimi hesaplayan yardımcı bir metod
            return (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage);//yuvarlamayı mathceilngle yapıyorum
        }
    }
}