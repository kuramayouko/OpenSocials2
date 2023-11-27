using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OpenSocials.App_Code;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenSocials.Pages
{
    public class NewsModel : PageModel
    {
        private readonly DataContext _context;

        public NewsModel(DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<News> NewsDB { get; set; }

        public void OnGet()
        {

            var newsList = _context.News
                .Include(news => news.NewsMedia)
                .OrderByDescending(news => news.Id)
                .ToList();

            if (newsList != null)
            {
                NewsDB = newsList;
            }
        }

        public string dateTranform(string dateComing)
        {
            var dateTransformed = DateTime.Parse(dateComing);

            return dateTransformed.ToString("dd/MM/yyyy").Replace('-', '/');
        }
    }
}
