using Microsoft.AspNetCore.Identity;

namespace bookpage.webui.Identity
{
    public class User:IdentityUser//zaten ıdentityUser içinde email şifreli password falan var burda olmasını istediğim özel alanı tanımlıyorum
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }//şimdi context tanımlıycam klasörde
}