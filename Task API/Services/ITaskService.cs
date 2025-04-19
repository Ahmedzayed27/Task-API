using TaskManagment.Models.DTOs;
using TaskManagment.Models;

public interface ITaskService
{
    TaskItem CreateTask(CreateTaskDto dto);
    TaskItem GetById(int id);
    List<TaskItem> GetByUser(int userId);
}
