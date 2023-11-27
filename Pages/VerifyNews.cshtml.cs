using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class VerifyNewsModel : PageModel
    {
        private readonly DataContext _context;

        public VerifyNewsModel(DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<News> NewsDB { get; set; }

        [BindProperty]
        public News metaNews { get; set; }

        public void OnGet(
        [FromQuery] int? Id,
        [FromQuery] bool? publish)
        {
            if (Id.HasValue && publish.HasValue)
            {
                if(publish == true)
                {
                    //Facebook fbook = new Facebook(_context);

                    metaNews = _context.News
                        .Include(news => news.NewsMedia)
                        .FirstOrDefault(n => n.Id == Id);

                    //fbook.PagePostSimple(metaNews.Text, metaNews.NewsMedia.Base64);

                }
                
                _context.Database.ExecuteSqlRaw("UPDATE News SET Is_Approved = 2 WHERE Id = {0}", Id);
                RedirectToPage("/Index");
            }
            else if (Id.HasValue)
            {
                Redirect("Index");
            }
            else
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
        }
    }
}
