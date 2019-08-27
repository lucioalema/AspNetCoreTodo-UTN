using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.UnitTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreTodo.UnitTests
{
    public class TodoItemServiceTests
    {
        private static readonly IdentityUser FakeUser = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "fake@fake"
        };
        private static readonly List<Category> Categories = new List<Category>
        {
            new Category{
                Id = Guid.NewGuid(),
                Name = "Standard"
            },
            new Category{
                Id = Guid.NewGuid(),
                Name = "Special"
            }
        };

        private static readonly List<TodoItem> Items = new List<TodoItem>
        {
            new TodoItem {
                Id = Guid.NewGuid(),
                Title = "Curso ASP.NET Core",
                DueAt = DateTimeOffset.Now.AddDays(1),
                Category = Categories.FirstOrDefault(x => x.Name == "Standard"),
                UserId = FakeUser.Id,
                IsDone = false
            },
            new TodoItem {
                Id = Guid.NewGuid(),
                Title = "Curso React",
                DueAt = DateTimeOffset.Now.AddDays(1),
                Category = Categories.FirstOrDefault(x => x.Name == "Special"),
                UserId = FakeUser.Id,
                IsDone = false
            }
        };

        [Fact]
        public async Task GetIncompleteItemsAsync_ItemsAreReturned()
        {
            using (var context = new ApplicationDbContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var categories = Categories;
                var expectedTodoItems = Items;
                var service = new TodoItemService(context);

                await context.Categories.AddRangeAsync(categories);
                await context.Items.AddRangeAsync(expectedTodoItems);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetIncompleteItemsAsync(FakeUser);

                // Assert
                var actualTodoItems = Assert.IsAssignableFrom<TodoItem[]>(result);
                Assert.Equal(
                    expectedTodoItems.OrderBy(m => m.Id).Select(m => m.Title),
                    actualTodoItems.OrderBy(m => m.Id).Select(m => m.Title));
            }
        }

        [Fact]
        public async Task AddItemAsync_TodoItemIsAdded()
        {
            using (var context = new ApplicationDbContext(Helpers.Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedTodoItem = Items[0];
                var service = new TodoItemService(context);

                // Act
                await service.AddItemAsync(expectedTodoItem, FakeUser);

                // Assert
                var actualTodoItem = await context.Items.FirstOrDefaultAsync(x => x.Title == "Curso ASP.NET Core");
                Assert.Equal(expectedTodoItem, actualTodoItem);
            }
        }

        [Fact]
        public async Task MarkDone_TodoItemIsMarkedDone()
        {
            using (var context = new ApplicationDbContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedTodoItem = Items[1];
                await context.AddAsync(expectedTodoItem);
                await context.SaveChangesAsync();
                var service = new TodoItemService(context);
                var expectedIsDone = expectedTodoItem.IsDone;

                // Act
                await service.MarkDoneAsync(expectedTodoItem.Id, FakeUser);

                // Assert
                var actualTodoItem = await context.Items.FirstOrDefaultAsync(x => x.Title == "Curso React");
                Assert.NotEqual(expectedIsDone, actualTodoItem.IsDone);
            }
        }
    }
}
