using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Task_API.Services
{
    public class TaskServicesFixCode
    {
        private readonly DbContext _dbContext;

        public TaskServicesFixCode(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //use asyncrouns for i/o

        public async Task<MyTask> GetTask(int id)
        {
            //should use  await  because of FirstOrDefaultAsync
            return await _dbContext.Set<MyTask>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<MyTask>> GetAllTasks()
        {
            //should use  await  because of ToListAsync

            return await _dbContext.Set<MyTask>().ToListAsync();
        }
    }
}
public class MyTask
{
    public int Id { get; set; }
    public string Title { get; set; }
}
