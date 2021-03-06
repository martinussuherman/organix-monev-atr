using Microsoft.AspNetCore.Mvc;
using MonevAtr.Models;

namespace Protaru.ViewComponents.Rtr
{
    public class CreateViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Atr rtr)
        {
            JenisRtrEnum jenis = (JenisRtrEnum)rtr.KodeJenisAtr;

            return View(new ViewModel
            {
                Rtr = rtr,
                DisplayAoi = DisplayAoiByRtr(jenis),
                Location = LocationByRtr(jenis),
                ReadOnlyLocation = ReadOnlyByRtr(jenis)
            });
        }

        public class ViewModel
        {
            public Atr Rtr { get; set; }

            public bool DisplayAoi { get; set; }

            public RtrLocationEnum Location { get; set; }

            public bool ReadOnlyLocation { get; set; }
        }

        private bool DisplayAoiByRtr(JenisRtrEnum jenisRtr)
        {
            if (jenisRtr == JenisRtrEnum.RtrKpnT51
                || jenisRtr == JenisRtrEnum.RtrKpnT52
                || jenisRtr == JenisRtrEnum.RtrKsnT51
                || jenisRtr == JenisRtrEnum.RtrKsnT52
                || jenisRtr == JenisRtrEnum.RtrPulauT51
                || jenisRtr == JenisRtrEnum.RtrPulauT52)
            {
                return false;
            }

            return true;
        }
        private bool ReadOnlyByRtr(JenisRtrEnum jenisRtr)
        {
            if (jenisRtr == JenisRtrEnum.RdtrT51 ||
                jenisRtr == JenisRtrEnum.RtrKpnT51 ||
                jenisRtr == JenisRtrEnum.RtrKsnT51 ||
                jenisRtr == JenisRtrEnum.RtrPulauT51 ||
                jenisRtr == JenisRtrEnum.RtrwnT51 ||
                jenisRtr == JenisRtrEnum.RtrwT50 ||
                jenisRtr == JenisRtrEnum.RtrwT51)
            {
                return false;
            }

            return true;
        }
        private RtrLocationEnum LocationByRtr(JenisRtrEnum jenisRtr)
        {
            if (jenisRtr == JenisRtrEnum.RtrwnT51 ||
                jenisRtr == JenisRtrEnum.RtrwnT52)
            {
                return RtrLocationEnum.None;
            }

            if (jenisRtr == JenisRtrEnum.RtrKsnT51 ||
                jenisRtr == JenisRtrEnum.RtrKsnT52)
            {
                return RtrLocationEnum.Kawasan;
            }

            if (jenisRtr == JenisRtrEnum.RtrPulauT51 ||
                jenisRtr == JenisRtrEnum.RtrPulauT52)
            {
                return RtrLocationEnum.Pulau;
            }

            return RtrLocationEnum.ProvinsiKabKota;
        }
    }
}