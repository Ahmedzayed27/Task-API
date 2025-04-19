using TaskManagment.DBContexts;
using TaskManagment.Models.DTOs;
using TaskManagment.Models;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public TaskItem CreateTask(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            UserId = dto.UserId
        };

        _context.Tasks.Add(task);
        _context.SaveChanges();
        return task;
    }

    public TaskItem GetById(int id)
    {
        return _context.Tasks.FirstOrDefault(t => t.Id == id);
    }

    public List<TaskItem> GetByUser(int userId)
    {
        return _context.Tasks.Where(t => t.UserId == userId).ToList();
    }
}
