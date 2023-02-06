using LojaJkMisterG.Models;

using Microsoft.EntityFrameworkCore;

namespace LojaJkMisterG.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Roupa> Roupas { get; set; }
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidosDetalhe { get; set; }

    }
}
