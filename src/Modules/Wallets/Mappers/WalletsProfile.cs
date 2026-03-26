using AutoMapper;

using ShareXe.Modules.Wallets.Entities;
using ShareXe.Modules.Wallets.Dtos;

namespace ShareXe.Modules.Wallets.Mappers
{
    public class WalletsProfile : Profile
    {
        public WalletsProfile()
        {
            CreateMap<Wallet, WalletDto>();
        }
    }
}
