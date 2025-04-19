using System.ComponentModel.DataAnnotations;

namespace TaskManagment.Models.DTOs
{

    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }  //id of user which is 1 or 2
    }

}
