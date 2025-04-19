using System.Data;
using System.ComponentModel.DataAnnotations;
using static TaskManagment.Models.Enums;

namespace TaskManagment.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<TaskItem> Tasks { get; set; }
    }
}
