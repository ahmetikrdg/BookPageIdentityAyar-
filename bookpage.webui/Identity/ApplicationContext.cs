using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bookpage.webui.Identity
{
    public class ApplicationContext:IdentityDbContext<User>
    {//kaydettim startupta işlem görücem
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            
        }
    }
}