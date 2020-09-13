using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using bookpage.business.Abstract;
using bookpage.business.Concrate;
using bookpage.data.Abstract;
using bookpage.data.Concrate.EfCore;
using bookpage.webui.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace bookpage.webui
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {//serisleri proje içine dahil ederiz
            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();//ICategryRepository çağırıldığı zaman EfCoreCategory repository gelsin
            services.AddScoped<ICategoryServices,CategoryManager>();
            services.AddScoped<IProductRepository,EfCoreProductRepository>();//mysql kullanmak istiyosam onu yazarım
            services.AddScoped<IProductServices,ProductManager>();
            services.AddControllersWithViews();//mvc yapısını kullandım controlleri kullanacağım dedim
//User
            services.AddDbContext<ApplicationContext>(options=>options.UseSqlite("Data Source=BookDb"));//benim applicationcontexti kullan optionsum sqlite kullanıcam yoluda içindeki ""
            services.AddIdentity<User,IdentityRole>() // ıdentityRole userin içindeki kalıtım gibidir yani biz user yerine direk IdentityUser'de yazabilirdik ama yapmadık çünkü User içinde isim soy isim gibi istediğimiz alanlar vardı
            .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();//adddefault parola resetlemek için gerekli olan benzersiz sayı üretecek yapı
//bunları ekledimki klasörün içindekileride tanısın yoksa applicationcontexti falan uygulama klasörden tanımaz buradan tanır. Servisleri ekledik kullan dediğimiz yer ise routenin hemen üstüdür.
            //IDENTİTY AYARLARI
                //password
            services.Configure<IdentityOptions>(options=>{
            options.Password.RequireDigit=true;//kullanıcı şifresinde mutlaka sayısal değer olmalı
            options.Password.RequireLowercase=true;//mutlaka küçük harf olmak zorunda
            options.Password.RequireUppercase=true;//büyük harf olmak zorunda
            options.Password.RequiredLength=6;//min 6 karakter olsun
            options.Password.RequireNonAlphanumeric=true;// @ _ falan olacak
                 //Lockout: Kullanıcı hesabının kitlenip kitlenmemesiyle alakalı bir durum
            options.Lockout.MaxFailedAccessAttempts=5;//kullanıcı en fazla 5 yanlış şifre girer sonrasında kilitleriz.
            options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);//5 Dk sonra kullanıcı giriş yapmayı deneyebilir.
            options.Lockout.AllowedForNewUsers=true;//Lockout aktif oldu

   //         options.User.AllowedUserNameCharacters="";//usernamenin içerisinde olmasını istediğin karakterleri yazabilirsin.
            options.User.RequireUniqueEmail=true;//her kullanıcının birbirinden farklı mail adresi olmalı
            options.SignIn.RequireConfirmedEmail=false;// kullanıcı üye olur ve mutlaka hesabını onaylamalı
   //         options.SignIn.RequireConfirmedPhoneNumber=true;//verdiği telno içinse onay
            });
                        //COOKİE Ayarları
//COOKİE: Kullanıcnın tarayıcında uygulama tarafından bırakılan bilgi. Bir uygulamayı ziyaret edersin ve o bilgileri server tarafından belli bilgileri tarayına bırakırki server tarafı daha sonra seni tanır.Mesele telefon ararsın daha sonra girdiğin yerlerde sitelerde telefon reklamları çıkar işte bu cookiedir.Cookie bilgi bırakıyo ve daha sonra bu tarayıcı tanıyor bizi.                        
     //uygulama sen login olunca benzersiz sayı üretir ve tarayıcına bırakır o ziyaret ettiğin sayfada server tarafındada sesion oluşturulur. Sessin nedir global değişken cookie ile session birbiriyle iletişim halinde.       
            services.ConfigureApplicationCookie(options=>{
              options.LoginPath="/account/login";//cookie session tarafından tanınmıyosa cookienin süresi bitmişse bu durumda bizi uygulama logine göndersin
              options.LogoutPath="/account/logout";//kullanıcı çıkış yapınca cookiem tarayıcıdan silinsin
              options.AccessDeniedPath="/account/accessdenied";//her login olan kullanıcı yönetici sayfalarına erişememeli yetki söz konusu login ama yönetici sayfasına gitmek istiyo bizde bunu engelliyoruz
              options.SlidingExpiration=true;//cookie süresi 20 dk sen 20 dk hiçbir istek yapmazsan 21. dakika cookie tarayıdan silinir ve tekrar login olman gerekir.FALSE dersen sen istek yapsanda yapmasanda 20 dk sonra seni atarım demek :D
              options.ExpireTimeSpan=TimeSpan.FromMinutes(60);//days diyip 365 günde diyebilirsin seni 1 senede tanır. ben minutes dedim 60 dk yeter dedim
              options.Cookie=new CookieBuilder{
               HttpOnly=true,//cookie sadece http talebiyle elde edebiliriz
               Name="BookPage.Security.Cookie"//bu şekilde bi isim tarayıcıda gösterilecek
              };

            });





        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();//wwwroot altındaki klasörler açılır
            app.UseStaticFiles(new StaticFileOptions{
                FileProvider=new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"
               
            });
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();//süreç içerisine bir servis yapısını ekledim
            app.UseRouting();
           

            app.UseEndpoints(endpoints =>
            {
                
               
    //--------------PRODUCTS
                
                //burada çok işime yarayacak bir hata yaptım productedit öndeydi ve çalışmadı bende listi başa çektim neden çalışmadı çünkü ilk olarak productsa girmeliyim daha sonra isteğe göre id verilir ve edite girer direk edite gitmez
                endpoints.MapControllerRoute(
                    name:"adminproductlist",
                    pattern:"admin/products",//admin products dediğimizde gitmek istediğim yer
                    defaults:new {Controller="Admin",action="ProductList"}
                );

                endpoints.MapControllerRoute(
                    name:"adminproductedit",
                    pattern:"admin/products/{id?}", //id verilirse ProductEdite git
                    defaults:new {Controller="Admin",action="ProductEdit"}
                );

                 endpoints.MapControllerRoute(
                    name:"adminproductcreate",
                    pattern:"admin/products/create",//admin products dediğimizde gitmek istediğim yer
                    defaults:new {Controller="Admin",action="ProductCreate"}
                );


                //---------- CATEGORİES
                 endpoints.MapControllerRoute(
                    name:"admincategorylist",
                    pattern:"admin/categories",
                    defaults:new {Controller="Admin",action="CategoryList"}
                );
                

                endpoints.MapControllerRoute(
                    name:"admincategorycreate",
                    pattern:"admin/categories/create",
                    defaults:new {Controller="Admin",action="CategoryCreate"}
                );

                endpoints.MapControllerRoute(
                    name:"admincategoryedit",
                    pattern:"admin/categories/{id?}",
                    defaults:new {Controller="Admin",action="CategoryEdit"}
                );
                //----------------
                 endpoints.MapControllerRoute(
                    name:"search",
                    pattern:"search",
                    defaults:new {Controller="Book",action="search"}
                );

                 endpoints.MapControllerRoute(
                    name:"productdetails",
                    pattern:"{Url}",
                    defaults:new {Controller="Book",action="details"}
                );

                endpoints.MapControllerRoute(
                    name:"products",
                    pattern:"products/{category?}",//Güncelledim. Kullanıcı category değeri verirse o kategori bilgileri gelsin--pattern:"products kullanıcı productsu çağırırsa
                    defaults:new {Controller="Book",action="list"}//bura gelsin
                );

                endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Book}/{action=Index}/{id?}"
                //controller=home dedim yani sen birşey çağırmasan bile ilk olarak home çıkar karşına actionu ise ındex
                );
            });
        }
    }
}
