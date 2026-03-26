using AutoMapper;

using ShareXe.Modules.WalletTransactions.Entities;
using ShareXe.Modules.WalletTransactions.Dtos;

namespace ShareXe.Modules.WalletTransactions.Mappers
{
    public class WalletTransactionsProfile : Profile
    {
        public WalletTransactionsProfile()
        {
            CreateMap<WalletTransaction, WalletTransactionDto>();
        }
    }
}
