using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace AspNetCoreTodo.Controllers
{
  //[Authorize(Roles = Constants.AdministratorRole)]
  [Authorize]
  public class TodoController : Controller
  {
    private readonly ITodoItemService _todoItemService;
    private readonly ICategoryService _categoryService;
    private readonly UserManager<IdentityUser> _userManager;
    public TodoController(
      ITodoItemService todoItemService,
      ICategoryService categoryService,
      UserManager<IdentityUser> userManager
    )
    {
        _todoItemService = todoItemService;
        _categoryService = categoryService;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
      var currentUser = await _userManager.GetUserAsync(User);
      if (currentUser == null) return Challenge();

      // Get to-do items from database
      var items = await _todoItemService
        .GetIncompleteItemsAsync(currentUser);
      
      // Put items into a model
      var model = new TodoViewModel()
      {
        Items = items
      };
      // Render view using the model
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AddItem()
    {
      var currentUser = await _userManager.GetUserAsync(User);
      if (currentUser == null) return Challenge();
      var categories = await _categoryService.GetAsync();
      
      // Put items into a model
      var model = new TodoItemAddViewModel()
      {
        Categories = categories.ToList()
      };
      // Render view using the model
      return View(model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> AddItem(TodoItemAddViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Index");
      }

      var currentUser = await _userManager.GetUserAsync(User);
      if (currentUser == null) return Challenge();

      var newItem = new TodoItem{
        Id = model.Id,
        Title = model.Title,
        DueAt = model.DueAt,
        Category = _categoryService.GetByIdAsync(model.CategoryId).Result
      };
      var successful = await _todoItemService.AddItemAsync(newItem, currentUser);

      if (!successful)
      {
        return BadRequest("Could not add item.");
      }
      return RedirectToAction("Index");
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkDone(Guid id)
    {
      if (id == Guid.Empty)
      {
        return RedirectToAction("Index");
      }

      var currentUser = await _userManager.GetUserAsync(User);
      if (currentUser == null) return Challenge();

      var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

      if (!successful)
      {
        return BadRequest("Could not mark item as done.");
      }
      return RedirectToAction("Index");
    }
  }
}