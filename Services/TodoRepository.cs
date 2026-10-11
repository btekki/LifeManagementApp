using System;
using System.Collections.Generic;
using System.Text;
using LifeManagementApp.Data;
using LifeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.Services
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _dbContext;

        public TodoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TodoItem>> GetAllTodosAsync()
        {
            return await _dbContext.Todos.ToListAsync();
        }

        public async Task<TodoItem?> GetTodoByIdAsync(int id)
        {
            return await _dbContext.Todos.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTodoAsync(TodoItem todo)
        {
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateTodoAsync(TodoItem todo)
        {
            _dbContext.Todos.Update(todo);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteTodoAsync(int id)
        {
            var todo = await GetTodoByIdAsync(id);
            if(todo != null)
            {
                _dbContext.Todos.Remove(todo);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
