using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Wallets.Entities;

namespace ShareXe.src.Modules.Wallets.Repositories
{
    [Injectable]
    public class WalletsRepository(ShareXeDbContext context) : BaseRepository<Wallet>(context)
    {
    }
}
