using Itm.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MonevAtr.Models;
using Newtonsoft.Json;
using Syncfusion.EJ2.DropDowns;
using Syncfusion.EJ2.Maps;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonevAtr.Pages
{
    public class RtrIndexNasionalModel : CustomPageModel
    {
        public RtrIndexNasionalModel(
            IHttpClientFactory httpClientFactory,
            PomeloDbContext context,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _configuration = configuration;
            selectListUtilities = new SelectListUtilities(_context);
        }

        public int StatusYear { get; set; }

        public int StatusMonth { get; set; }

        public string BingKey => _configuration.GetValue<string>("BingKey");

        public object MapsMarkerData;

        public MapsBorder Border => new MapsBorder
        {
            Color = "#374557",
            Width = 14
        };

        public MapsCenterPosition CenterPosition => new MapsCenterPosition
        {
            Latitude = -2.8,
            Longitude = 118
        };

        public MapsMarkerClusterSettings ClusterSettings => new MapsMarkerClusterSettings
        {
            AllowClusterExpand = true,
            AllowClustering = true,
            Shape = MarkerType.Image,
            Height = 40,
            Width = 40,
            ImageUrl = Url.Content("~/cluster.svg"),
            LabelStyle = new MapsFont
            {
                Color = "White"
            }
        };

        public List<MapsMarker> MarkerSettings => new List<MapsMarker>
        {
            new MapsMarker
            {
                AnimationDuration = 0,
                DataSource = MapsMarkerData,
                ColorValuePath = "color",
                Height = 10,
                Shape = MarkerType.Circle,
                TooltipSettings=_tooltipSettings,
                Visible = true,
                Width = 10
            }
        };

        public MapsZoomSettings ZoomSettings => new MapsZoomSettings
        {
            Enable = true,
            HorizontalAlignment = Alignment.Near,
            MouseWheelZoom = false,
            PinchZooming = true,
            ShouldZoomInitially = false,
            Toolbars = _toolbars,
            ToolBarOrientation = Orientation.Vertical,
            ZoomFactor = 5
        };

        public List<Models.Provinsi> Provinsi { get; set; }

        public AutoCompleteFieldSettings FieldSettings => new AutoCompleteFieldSettings
        {
            Text = "Nama",
            Value = "Kode"
        };

        public async Task<IActionResult> OnGetAsync()
        {
            MapsMarkerData = await ReadMarkerFromApi();
            StatusYear = DateTime.Today.Year;
            StatusMonth = DateTime.Today.Month;
            Provinsi = await _context.Provinsi.ToListAsync();
            ViewData["TahunLintasSektorDanPersetujuanSubstansi"] =
                await selectListUtilities.TahunRapatLintasSektorDanPersetujuanSubstansi();
            return Page();
        }

        private async Task<object> ReadMarkerFromApi()
        {
            string url = $"{Request.Scheme}://{Request.Host.ToUriComponent()}" +
                $"{Request.PathBase.ToUriComponent()}/api/Progress/NasionalMap";
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, url));
            return JsonConvert.DeserializeObject(
                await response.Content.ReadAsStringAsync());
        }

        private readonly string[] _toolbars = new string[]
        {
            "Zoom",
            "ZoomIn",
            "ZoomOut",
            "Pan",
            "Reset"
        };
        private readonly MapsTooltipSettings _tooltipSettings = new MapsTooltipSettings
        {
            Template = "#tooltip-template",
            Visible = true,
            ValuePath = "nama"
        };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PomeloDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SelectListUtilities selectListUtilities;
    }
}