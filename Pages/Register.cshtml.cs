using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
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
            // You can add any logic needed when the page is initially loaded
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
        
            _context.Login.Add(LoginDB);
            _context.SaveChanges();

            // Redirect to another page or perform additional actions after successful registration
            return RedirectToPage("/Index");
        }
    }
}
