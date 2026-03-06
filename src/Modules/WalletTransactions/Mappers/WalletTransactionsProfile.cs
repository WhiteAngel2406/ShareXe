using AutoMapper;

using ShareXe.Modules.WalletTransactions.Entities;
using ShareXe.src.Modules.WalletTransactions.Dtos;

namespace ShareXe.src.Modules.WalletTransactions.Mappers
{
    public class WalletTransactionsProfile : Profile
    {
        public WalletTransactionsProfile()
        {
            CreateMap<WalletTransaction, WalletTransactionDto>();
        }
    }
}
