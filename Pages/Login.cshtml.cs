using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OpenSocials.App_Code;
using System.Threading.Tasks;

namespace OpenSocials.Pages
{
    public class LoginModel : PageModel
    {

        private readonly DataContext _context;

        public LoginModel(DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Login Login { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Login
                .FirstOrDefaultAsync(u => u.Username == Login.Username && u.Password == Login.Password);

            if (user != null)
            {
                var claims = new[]
                {
					new Claim(ClaimTypes.Name, user.Username),
					new Claim(ClaimTypes.Role, user.Is_Admin == 1 ? "Admin" : ""),
					new Claim(ClaimTypes.Role, user.Is_Commenter == 1 ? "Commenter" : ""),
					new Claim(ClaimTypes.Role, user.Is_Reviewer == 1 ? "Reviewer" : "")
				};

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

                return RedirectToPage("Index"); // Redirect to the home page after successful login
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return Page();
        }
    }
}
