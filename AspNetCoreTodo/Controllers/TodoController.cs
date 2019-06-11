using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Mvc;
namespace AspNetCoreTodo.Controllers
{
  public class TodoController : Controller
  {
    public IActionResult Index()
    {
        // Get to-do items from database
      var item1 = new TodoItem
      {
        Title = "Curso ASP.NET Core",
        DueAt = DateTimeOffset.Now.AddDays(1)
      };
      var item2 = new TodoItem
      {
        Title = "Curso React",
        DueAt = DateTimeOffset.Now.AddDays(2)
      };
      var items = new[] { item1, item2 };
      // Put items into a model
      var model = new TodoViewModel()
      {
        Items = items
      };
      // Render view using the model
        return View(model);
    } 
  }
}