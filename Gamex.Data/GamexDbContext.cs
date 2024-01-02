using Gamex.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gamex.Data;

public class GamexDbContext : IdentityDbContext<ApplicationUser>
{
    public GamexDbContext(DbContextOptions<GamexDbContext> options) :
        base(options)
    { }
}
