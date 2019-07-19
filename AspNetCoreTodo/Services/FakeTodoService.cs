using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
namespace AspNetCoreTodo.Services
{
    public class FakeTodoItemService : ITodoItemService
    {
        public Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            var item1 = new TodoItem
            {
                Title = "ASP.NET Core - MVC",
                DueAt = DateTimeOffset.Now.AddDays(1)
            };
            var item2 = new TodoItem
            {
                Title = "ASP.NET Core - Web Api",
                DueAt = DateTimeOffset.Now.AddDays(1)
            };
            var item3 = new TodoItem
            {
                Title = "React",
                DueAt = DateTimeOffset.Now.AddMonths(2)
            };
            return Task.FromResult(new[] { item1, item2, item3 });
        }
    }
}