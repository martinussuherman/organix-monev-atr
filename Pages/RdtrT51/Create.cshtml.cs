using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            selectListUtilities = new SelectListUtilities(context);
            rtrUtilities = new RtrUtilities(context);
        }

        [BindProperty]
        public Models.Atr Rtr { get; set; } = new Models.Atr();

        public IEnumerable<SelectListItem> Provinsi { get; set; }

        public IEnumerable<SelectListItem> KabupatenKota { get; set; }

        public IEnumerable<SelectListItem> ProgressRtr { get; set; }

        public IEnumerable<SelectListItem> TahunPenyusunan { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Provinsi = await selectListUtilities.ProvinsiAsync();
            KabupatenKota = await selectListUtilities.KabupatenKotaAsync();
            ProgressRtr = await selectListUtilities.InputProgressRdtrT51Async();
            TahunPenyusunan = selectListUtilities.InputTahunRequired();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            rtrUtilities.SetCommonRtrPropertiesOnCreate(
                Rtr,
                JenisRtrEnum.RdtrT51,
                StatusRevisi.Kosong,
                User);
            await rtrUtilities.SaveRtr(Rtr, User, EntityState.Added);
            return RedirectToPage("./Index");
        }

        private readonly RtrUtilities rtrUtilities;
        private readonly SelectListUtilities selectListUtilities;
    }
}