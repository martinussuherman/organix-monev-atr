using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MonevAtr.Models;
using Protaru.Identity;

namespace MonevAtr.Pages.RdtrT51
{
    [Authorize(Permissions.RdtrT51.Create)]
    public class CreateModel : PageModel
    {
        public CreateModel(PomeloDbContext context)
        {
            _context = context;
            selectListUtilities = new SelectListUtilities(context);
            rtrUtilities = new RtrUtilities(context);
        }

        [BindProperty]
        public Models.Atr Atr { get; set; } = new Models.Atr();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ProgressAtr"] = await selectListUtilities.ProgressRdtrT51();
            ViewData["Provinsi"] = await selectListUtilities.Provinsi();
            ViewData["KabupatenKota"] = selectListUtilities.EmptyKabupatenKota;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            rtrUtilities.SetCommonRtrPropertiesOnCreate(
                this.Atr,
                JenisRtrEnum.RdtrT51,
                StatusRevisi.Kosong,
                User);

            // if (!ModelState.IsValid)
            // {
            //     return await OnGetAsync();
            // }

            _context.Atr.Attach(this.Atr);
            _context.Entry(this.Atr).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private readonly RtrUtilities rtrUtilities;

        private readonly SelectListUtilities selectListUtilities;

        private readonly PomeloDbContext _context;
    }
}