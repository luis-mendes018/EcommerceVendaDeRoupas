using LojaJkMisterG.Models;
using LojaJkMisterG.Repositories.Interfaces;
using LojaJkMisterG.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace LojaJkMisterG.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly IRoupaRepository _roupaRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(IRoupaRepository roupaRepository, CarrinhoCompra carrinhoCompra)
        {
            _roupaRepository = roupaRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Carrinho()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal(),

            };
            return View(carrinhoCompraVM);
        }

        public RedirectToActionResult AdicionarItemNoCarrinhoCompra(int roupaId)
        {
            var roupaSelecionada = _roupaRepository.Roupas
                .FirstOrDefault(p => p.RoupaId == roupaId);

            if (roupaSelecionada != null)
            {
                _carrinhoCompra.AdicionarAoCarrinho(roupaSelecionada);
            }

            return RedirectToAction("Carrinho");
        }

        public IActionResult RemoverItemDoCarrinho(int roupaId)
        {
            var roupaSelecionada = _roupaRepository.Roupas
                .FirstOrDefault(p => p.RoupaId == roupaId);

            if (roupaSelecionada != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(roupaSelecionada);
            }

            return RedirectToAction("Carrinho");
        }
    }
}
