using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagment.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }

        public int TaskItemId { get; set; }

        [ForeignKey("TaskItemId")]
        public TaskItem TaskItem { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
