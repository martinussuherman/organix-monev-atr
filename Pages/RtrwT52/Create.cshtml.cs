using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonevAtr.Models;
using Protaru.Identity;

namespace MonevAtr.Pages.RtrwT52
{
    [Authorize(Permissions.RtrwT52.Create)]
    public class CreateModel : PageModel
    {
        public CreateModel(PomeloDbContext context)
        {
            _context = context;
            selectListUtilities = new SelectListUtilities(context);
            rtrUtilities = new RtrUtilities(context);
        }

        [BindProperty]
        public Models.Atr Rtr { get; set; }

        [BindProperty]
        public int KodeReferensiAtr { get; set; }

public IEnumerable<SelectListItem> TahunPenyusunan { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            KodeReferensiAtr = (int)id;

            Rtr = await _context.Atr
                .Include(a => a.Provinsi)
                .Include(a => a.KabupatenKota)
                .Include(a => a.KabupatenKota.Provinsi)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Kode == KodeReferensiAtr);

            ViewData["ProgressAtr"] = await selectListUtilities.ProgressRtrwT52();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            rtrUtilities.SetCommonRtrPropertiesOnCreate(
                Rtr,
                JenisRtrEnum.RtrwT52,
                StatusRevisi.RevisiT52,
                User);
            await rtrUtilities.SaveRtr(Rtr, User, EntityState.Added);
            await rtrUtilities.UpdateReferensiRtr(KodeReferensiAtr);
            return RedirectToPage("./Index");
        }

        private readonly RtrUtilities rtrUtilities;
        private readonly SelectListUtilities selectListUtilities;
        private readonly PomeloDbContext _context;
    }
}