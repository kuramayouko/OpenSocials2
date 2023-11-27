using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class CreateNewsModel : PageModel
    {

		private readonly DataContext _context;
		
		public CreateNewsModel(DataContext context)
		{
			_context = context;
		}

		[BindProperty]
		public News NewsBD { get; set; }
		
		[BindProperty]
		public NewsMedia NewsMediaBD { get; set; }
		
		public String UrlString;
	
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            NewsBD.Is_Approved = 0;
            NewsBD.Date_Created = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00");
            NewsBD.Media_Id = 0;

            // Saving NewsBD to the database
            _context.News.Add(NewsBD);
            _context.SaveChanges();

            // Retrieve the generated ID after saving
            int generatedId = NewsBD.Id;

            // Update News.Media_Id with the same value as Id
            NewsBD.Media_Id = generatedId;

            // Update NewsBD in the database
            _context.News.Update(NewsBD);
            _context.SaveChanges();

            if (!string.IsNullOrEmpty(NewsMediaBD.Base64) && NewsMediaBD.Base64 != "0")
            {
                // Assign the generated ID to NewsMediaBD
                NewsMediaBD.Id = generatedId;

                // Save NewsMediaBD to the database
                _context.NewsMedia.Add(NewsMediaBD);
                _context.SaveChanges();
            }

            return RedirectToPage("/News");
        }

    }
}
