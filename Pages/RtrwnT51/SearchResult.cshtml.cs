using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonevAtr.Models;
using P.Pager;
using Protaru.Identity;

namespace MonevAtr.Pages.RtrwnT51
{
    public class SearchResultModel : SearchResultPageModel
    {
        public SearchResultModel(
            IAuthorizationService authorizationService,
            PomeloDbContext context)
        {
            _authorizationService = authorizationService;
            _context = context;
        }

        public IActionResult OnGet([FromQuery] AtrSearch rtr, [FromQuery] int page = 1)
        {
            Hasil = _context.Atr
                .ByJenis(JenisRtrEnum.RtrwnT51)
                .ByTahun(rtr.Tahun)
                .ByNama(rtr.Nama)
                .ByNomor(rtr.Nomor)
                .ByProgressList(rtr.ProgressList)
                .RtrInclude()
                .AsNoTracking()
                .ToPagerList(page, PagerUrlHelper.ItemPerPage);
            Rtr = rtr;
            RegulationName = "Perpres";
            IsDisplayRegulation = true;
            IsUseCreateForm = false;
            IsCanCreate = _authorizationService.AuthorizeAsync(
                User,
                Permissions.RtrwnT51.Create).Result.Succeeded;
            IsCanEdit = _authorizationService.AuthorizeAsync(
                User,
                Permissions.RtrwnT51.Edit).Result.Succeeded;

            return Page();
        }

        public async Task<IActionResult> OnGetExport([FromQuery] AtrSearch rtr)
        {
            return await this.RtrZeroFieldExport(_context, JenisRtrEnum.RtrwnT51, rtr);
        }

        private readonly IAuthorizationService _authorizationService;
        private readonly PomeloDbContext _context;
    }
}