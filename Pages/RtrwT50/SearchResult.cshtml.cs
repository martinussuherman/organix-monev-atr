using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MonevAtr.Models;
using P.Pager;

namespace MonevAtr.Pages.RtrwT50
{
    public class SearchResultModel : PageModel
    {
        public SearchResultModel(MonevAtrDbContext context)
        {
            _context = context;
        }

        public IPager<Models.Atr> Hasil { get; set; }

        [BindProperty]
        public Models.AtrSearch AtrSearch { get; set; }

        public List<Models.Atr> HasilPencarian { get; set; } = new List<Models.Atr>();

        public IActionResult OnGet([FromQuery] AtrSearch rtr, [FromQuery] int page = 1)
        {
            Hasil = _context.Atr
                .ByJenis(JenisRtrEnum.RtrwT50)
                .ByProvinsi(rtr.Prov, rtr.KabKota)
                .ByKabupatenKota(rtr.KabKota)
                .ByTahun(rtr.Tahun)
                .ByNama(rtr.Nama)
                .ByNomor(rtr.Nomor)
                .ByProgressList(rtr.ProgressList)
                .RtrInclude()
                .ToPagerList(page, PagerUrlHelper.ItemPerPage);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            IQueryable<Models.Atr> query = _context.Atr
                .Where(a => a.KodeJenisAtr == (int) JenisRtrEnum.RtrwT50);

            query = filter.QueryAtrByProvinsi(query, this.AtrSearch);
            query = filter.QueryAtrByKabupatenKota(query, this.AtrSearch);
            query = filter.QueryAtrByTahun(query, this.AtrSearch);
            query = filter.QueryAtrByNama(query, this.AtrSearch);
            query = filter.QueryAtrByNomor(query, this.AtrSearch);
            query = filter.QueryAtrByProgress(query, this.AtrSearch);

            this.HasilPencarian = await query
                .Include(a => a.Provinsi)
                .Include(a => a.KabupatenKota)
                .Include(a => a.KabupatenKota.Provinsi)
                .Include(a => a.ProgressAtr)
                .OrderBy(a => a.KabupatenKota.Provinsi.Nama)
                .ThenBy(a => a.KabupatenKota.Nama)
                .ThenBy(a => a.Provinsi.Nama)
                .ToListAsync();
            return Page();
        }

        private readonly FilterUtilities filter = new FilterUtilities();

        private readonly MonevAtrDbContext _context;
    }
}