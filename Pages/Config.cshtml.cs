using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly DataContext _context;

        [BindProperty]
        public Config ConfigDB { get; set; }

        public bool check;
        Crypto secret = new Crypto();

        public ConfigModel(DataContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            ConfigDB = _context.Config.FirstOrDefault();

            if(ConfigDB.UserToken != "0" && ConfigDB.PageToken != "0")
            {
                ConfigDB.UserToken = secret.Decrypt(ConfigDB.UserToken);
                ConfigDB.PageToken = secret.Decrypt(ConfigDB.PageToken);
            }
            
            // Checa se app id foi configurado primeiro
            if (ConfigDB != null && ConfigDB.UserToken != "0")
            {
                check = true;
            }
            else
            {
                check = false;
            }
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                Page();
            }

            ConfigDB = _context.Config.FirstOrDefault();

            if (ConfigDB != null)
            {
                // Atualiza o conteudo appid e appsecret

                ConfigDB.UserToken = secret.Encrypt(Request.Form["ConfigDB.UserToken"]);
                ConfigDB.PageToken = secret.Encrypt(Request.Form["ConfigDB.PageToken"]);
                
                //Salva no bd
                _context.Config.Update(ConfigDB);
                _context.SaveChanges();
            }

            RedirectToPage();
        }
    }
}
