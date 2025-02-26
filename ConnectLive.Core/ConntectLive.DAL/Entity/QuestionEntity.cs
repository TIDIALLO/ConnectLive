using System.ComponentModel.DataAnnotations.Schema;


namespace ConntectLive.DAL;

[Table("questions")]
public class QuestionEntity : IEntity
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [ForeignKey("user-key")]
    public Guid UserId { get; set; }

    [Column("question")]
    public string Question { get; set; }
    [Column("category")]
    public string Category { get; set; }
    [Column("choices")]
    public List<string> Choices { get; set; }
    [Column("correct-answer")]
    public string CorrectAnswer { get; set; }
}