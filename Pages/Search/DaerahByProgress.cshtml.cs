using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonevAtr.Models;

namespace MonevAtr.Pages.Search
{
    public class DaerahByProgressModel : PageModel
    {
        public DaerahByProgressModel(PomeloDbContext context)
        {
            _context = context;
            selectListUtilities = new SelectListUtilities(context);
        }

        public AtrSearch Rtr { get; set; }

        [ViewData]
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int provinsi, string returnUrl)
        {
            ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "./Index" : returnUrl;
            Rtr = new AtrSearch
            {
                Prov = provinsi
            };

            var prov = await _context.Provinsi.FindAsync(provinsi);
            ViewData["Title"] = $"Pencarian RTR Provinsi {prov.Nama}";
            ViewData["PageTitle"] = $"RTR Provinsi {prov.Nama}";
            ViewData["KabupatenKota"] = await selectListUtilities.KabupatenKota(provinsi);
            return Page();
        }

        private readonly SelectListUtilities selectListUtilities;

        private readonly PomeloDbContext _context;
    }
}