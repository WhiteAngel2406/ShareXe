using AutoMapper;

using ShareXe.Modules.Wallets.Entities;
using ShareXe.src.Modules.Wallets.Dtos;

namespace ShareXe.src.Modules.Wallets.Mappers
{
    public class WalletsProfile : Profile
    {
        public WalletsProfile()
        {
            CreateMap<Wallet, WalletDto>();
        }
    }
}
