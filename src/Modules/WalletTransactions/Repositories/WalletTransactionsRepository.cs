using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.WalletTransactions.Entities;

namespace ShareXe.Modules.WalletTransactions.Repositories
{
    [Injectable]
    public class WalletTransactionsRepository(ShareXeDbContext context) : BaseRepository<WalletTransaction>(context)
    {
    }
}
