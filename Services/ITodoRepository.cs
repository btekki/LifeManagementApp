using System;
using System.Collections.Generic;
using System.Text;
using LifeManagementApp.Models;

namespace LifeManagementApp.Services
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllTodosAsync();

        Task<TodoItem?> GetTodoByIdAsync(int id);

        Task AddTodoAsync(TodoItem todo);

        Task DeleteTodoAsync(int id);

        Task UpdateTodoAsync(TodoItem todo);
    }
}
