using ShareXe.Base.Attributes;
using ShareXe.Base.Repositories;
using ShareXe.DAL;
using ShareXe.Modules.Auth.Entities;

namespace ShareXe.Modules.Auth.Repositories
{
    [Injectable]
    public class AccountsRepository(ShareXeDbContext context) : BaseRepository<Account>(context)
    {
        public Task<Account?> GetByFirebaseUidAsync(string firebaseUid, string includeProperties = "")
          => GetOneAsync(a => a.FirebaseUid == firebaseUid, includeProperties);

        public Task<Account?> GetByEmailAsync(string email, string includeProperties = "")
          => GetOneAsync(a => a.Email == email, includeProperties);
    }
}
