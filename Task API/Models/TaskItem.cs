using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagment.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }


        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User AssignedUser { get; set; }

        public ICollection<TaskComment> Comments { get; set; }
    }
}
