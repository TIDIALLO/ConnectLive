using Microsoft.EntityFrameworkCore;

namespace ConntectLive.DAL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    //public virtual DbSet<LeaderboardEntity> Leaderboards { get; set; }
    //public virtual DbSet<QuestionEntity> Questions { get; set; }
    //public virtual DbSet<SessionEntity> Sessions { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<QuestionEntity> Questions { get; set; }

}
