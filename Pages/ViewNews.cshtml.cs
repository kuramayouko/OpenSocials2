using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class ViewNewsModel : PageModel
    {
        private readonly DataContext _context;

        public ViewNewsModel(DataContext context)
        {
            _context = context;
        }
        [BindProperty]
        public News NewsDB { get; set; }
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Error");
            }

            // Fetch the news item with the specified ID
            NewsDB = _context.News.Include(n => n.NewsMedia).FirstOrDefault(n => n.Id == id);

            if (NewsDB == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public string dateTranform(string dateComing)
        {
            var dateTransformed = DateTime.Parse(dateComing);

            return dateTransformed.ToString("dd/MM/yyyy").Replace('-', '/');
        }
    }
}
