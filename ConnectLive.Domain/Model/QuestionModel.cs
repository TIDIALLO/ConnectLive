using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectLive.Domain.Model;
public class QuestionModel
{
    public Guid QuestionId { get; set; }
    public string Question { get; set; }
    public string Category { get; set; }
    public List<string> Choices { get; set; }
    public string CorrectAnswer { get; set; }
}
