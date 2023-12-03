using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace OpenSocials.Pages
{
    public class LogoutModel : PageModel
    {

        public void OnGet()
        {
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			RedirectToPage("Index");
        }
    }
}
