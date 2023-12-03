using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenSocials.Pages
{
	[Authorize(Roles = "Admin")]
    public class LogsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
