using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Users.Entities;

namespace ShareXe.Modules.Users.Repositories
{
    [Injectable]
    public class UsersRepository(ShareXeDbContext context) : BaseRepository<User>(context)
    {
    }
}
