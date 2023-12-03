using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
	[Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly DataContext _context;

        public RegisterModel(DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Login LoginDB { get; set; }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
        
            _context.Login.Add(LoginDB);
            _context.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}
