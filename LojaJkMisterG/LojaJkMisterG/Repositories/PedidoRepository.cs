﻿using LojaJkMisterG.Context;
using LojaJkMisterG.Models;
using LojaJkMisterG.Repositories.Interfaces;

namespace LojaJkMisterG.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoRepository(AppDbContext appDbContext, CarrinhoCompra carrinhoCompra)
        {
            _appDbContext = appDbContext;
            _carrinhoCompra = carrinhoCompra;
        }

        public void CriarPedido(Pedido pedido)
        {
            pedido.PedidoEnviado = DateTime.Now;
            _appDbContext.SaveChanges();

            var carrinhoCompraItens = _carrinhoCompra.CarrinhoCompraItens;

            foreach (var carrinhoItem in carrinhoCompraItens)
            {
                var pedidoDetail = new PedidoDetalhe()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    RoupaId = carrinhoItem.Roupa.RoupaId,
                    PedidoId = pedido.PedidoId,
                    Preco = carrinhoItem.Roupa.Preco

                };

                _appDbContext.PedidosDetalhe.Add(pedidoDetail);
            }

            _appDbContext.SaveChanges();
        }
    }
}
