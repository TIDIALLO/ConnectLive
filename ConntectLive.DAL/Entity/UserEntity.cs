using System.ComponentModel.DataAnnotations.Schema;
using EF = Microsoft.EntityFrameworkCore;


namespace ConntectLive.DAL;

[Table("users")]
[EF.Index(nameof(Email), IsUnique = true)]
public class UserEntity : IEntity
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Column("firstname")]
    public string FirstName { get; set; }
    [Column("lastname")]
    public string LastName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("nickname")]
    public string Nickname { get; set; }

    public virtual List<QuestionEntity> Questions { get; set; }
}