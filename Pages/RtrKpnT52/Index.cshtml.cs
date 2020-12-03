using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonevAtr.Models;

namespace MonevAtr.Pages.RtrKpnT52
{
    public class IndexModel : PageModel
    {
        public IndexModel(PomeloDbContext context)
        {
            selectListUtilities = new SelectListUtilities(context);
        }

        [BindProperty]
        public AtrSearch Rtr { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Tahun"] = await selectListUtilities.TahunPerpresRtrKpnT52();
            return Page();
        }

        private readonly SelectListUtilities selectListUtilities;
    }
}