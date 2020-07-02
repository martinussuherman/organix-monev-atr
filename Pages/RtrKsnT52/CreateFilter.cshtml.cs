using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonevAtr.Models;
using P.Pager;
using Protaru.Identity;

namespace MonevAtr.Pages.RtrKsnT52
{
    [Authorize(Permissions.RtrKsnT52.Create)]
    public class CreateFilterModel : PageModel
    {
        public CreateFilterModel(MonevAtrDbContext context)
        {
            _context = context;
        }

        public IPager<Models.Atr> Hasil { get; set; }

        public IActionResult OnGet([FromQuery] AtrSearch rtr, [FromQuery] int page = 1)
        {
            Hasil = _context.Atr
                .Where(a => (a.KodeJenisAtr == (int) JenisRtrEnum.RtrKsnT51 ||
                        a.KodeJenisAtr == (int) JenisRtrEnum.RtrKsnT52) &&
                    a.SudahDirevisi == 0)
                .ByKawasan(rtr.Kawasan)
                .ByTahun(rtr.Tahun)
                .ByNama(rtr.Nama)
                .ByNomor(rtr.Nomor)
                .ByProgressList(rtr.ProgressList)
                .RtrKsnInclude()
                .ToPagerList(page, PagerUrlHelper.ItemPerPage);
            return Page();
        }

        private readonly MonevAtrDbContext _context;
    }
}