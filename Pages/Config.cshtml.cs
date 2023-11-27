using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly DataContext _context;

        [BindProperty]
        public Config Config { get; set; }

        public bool check;

        public ConfigModel(DataContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Config = _context.Config.FirstOrDefault();

            // Checa se app id foi configurado primeiro
            if (Config != null && Config.AppId != "0" && Config.AppSecret != "0")
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

            Config = _context.Config.FirstOrDefault();

            if (Config != null)
            {
                // Atualiza o conteudo appid e appsecret
                Config.AppId = Request.Form["Config.AppId"];
                Config.AppSecret = Request.Form["Config.AppSecret"];
                Config.FbPageToken = Request.Form["Config.FbPageToken"];
                Config.FbPageId = Request.Form["Config.FbPageId"];

                //Salva no bd
                _context.Config.Update(Config);
                _context.SaveChanges();
            }

            RedirectToPage();
        }
    }
}
