using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class EditNewsModel : PageModel
    {
        private readonly DataContext _context;

        public EditNewsModel(DataContext context)
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
            NewsDB = _context.News.FirstOrDefault(n => n.Id == id);

            if (NewsDB == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public IActionResult OnPost(string handler)
        {
            switch (handler)
            {
                case "Update":

                    _context.Database.ExecuteSqlRaw("UPDATE News SET Title = {0}, Text = {1}, Date_Created = {2} WHERE Id = {3}",
                                   NewsDB.Title, NewsDB.Text, DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00"), NewsDB.Id);

                    return RedirectToPage("/News");

                case "Delete":

                    _context.Database.ExecuteSqlRaw("UPDATE News SET Is_Approved = 2 WHERE Id = {0}", NewsDB.Id);
                    return RedirectToPage("/News");

                default:
                    return RedirectToPage("/Error");
            }
        }
    }
}
