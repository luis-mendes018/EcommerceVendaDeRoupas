using LojaJkMisterG.Areas.Admin.Servicos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaJkMisterG.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminGraficoController : Controller
    {
        private readonly GraficoVendasService _graficoVendas;

        public AdminGraficoController(GraficoVendasService graficoVendas)
        {
            _graficoVendas = graficoVendas ?? 
                throw new ArgumentNullException(nameof(graficoVendas));
        }

        public JsonResult VendasRoupas(int dias)
        {
            var roupasVendasTotais = _graficoVendas.GetVendasRoupas(dias);
            return Json(roupasVendasTotais);
        }

        [HttpGet]
        public IActionResult Index(int dias)
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasMensal(int dias)
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasSemanal(int dias)
        {
            return View();
        }
    }
}
