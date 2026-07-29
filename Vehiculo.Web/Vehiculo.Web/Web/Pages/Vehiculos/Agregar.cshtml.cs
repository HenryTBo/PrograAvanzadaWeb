using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    public class AgregarModel : PageModel
    {
        private readonly Abstracciones.Interfaces.Reglas.IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public VehiculoRequest vehiculo { get; set; }

        [BindProperty]
        public List<SelectListItem> marcas { get; set; }

        [BindProperty]
        public List<SelectListItem> modelos { get; set; }

        [BindProperty]
        public Guid marcaseleccionada { get; set; }

        public async Task<ActionResult> OnGet()
        {
            await ObtenerMarcas();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await ObtenerMarcas();
                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarVehiculo");
            var cliente = new HttpClient();
            var respuesta = await cliente.PostAsJsonAsync(endpoint, vehiculo);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerMarcas");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadodeserializado = JsonSerializer.Deserialize<List<JsonElement>>(resultado, opciones);

            marcas = resultadodeserializado.Select(m =>
                new SelectListItem
                {
                    Value = m.GetProperty("id").GetString(),
                    Text = m.GetProperty("nombre").GetString(),
                }
            ).ToList();
        }

        private async Task<List<SelectListItem>> ObtenerModelos(Guid marcaId)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerModelos");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, $"{endpoint}?marcaId={marcaId}");

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadodeserializado = JsonSerializer.Deserialize<List<JsonElement>>(resultado, opciones);

                return resultadodeserializado.Select(m =>
                    new SelectListItem
                    {
                        Value = m.GetProperty("id").GetString(),
                        Text = m.GetProperty("nombre").GetString(),
                    }
                ).ToList();
            }

            return new List<SelectListItem>();
        }

        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaId)
        {
            var modelos = await ObtenerModelos(marcaId);
            return new JsonResult(modelos);
        }
    }
}