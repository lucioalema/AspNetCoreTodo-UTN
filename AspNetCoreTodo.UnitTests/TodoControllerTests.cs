using AspNetCoreTodo.Controllers;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreTodo.UnitTests
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoItemService> mockTodoItemService;
        private readonly Mock<ICategoryService> mockCategoryService;
        private readonly Mock<UserManager<IdentityUser>> mockUserManager;
        public TodoControllerTests()
        {
            mockTodoItemService = new Mock<ITodoItemService>();
            mockCategoryService = new Mock<ICategoryService>();
            mockUserManager = MockUserManager.Create();
        }

        [Fact]
        public async Task Index_ShouldReturn_ChallengeResult_IfUserIsUnauthorize()
        {
            // Arrange
            // Make the mockUserManager always return null for GetUserAsync
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(null));
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task Index_ShouldReturn_ViewResult_TodoViewModel()
        {
            // Arrange
            // Make the mockUserManager always return fake user for GetUserAsync
            var fakeUser = new IdentityUser();
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(fakeUser));
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<TodoViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task AddItemGet_ShouldReturn_ChallengeResult_IfUserIsUnauthorize()
        {
            // Arrange
            // Make the mockUserManager always return null for GetUserAsync
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(null));
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var result = await controller.AddItem();

            // Assert
            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task AddItemGet_ShouldReturn_ViewResult_TodoItemAddViewModel()
        {
            // Arrange
            // Make the mockUserManager always return fake user for GetUserAsync
            var fakeUser = new IdentityUser();
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(fakeUser));
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var result = await controller.AddItem();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<TodoItemAddViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task AddItemPost_ShouldReturn_BadRequest_IfModelIsInvalid()
        {
            // Arrange
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);
            var model = new TodoItemAddViewModel();
            controller.ModelState.AddModelError("error", "testerror");

            //Act
            var result = await controller.AddItem(model);
            var objectResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.IsType<SerializableError>(objectResult.Value);
        }

        [Fact]
        public async Task AddItemPost_ShouldReturn_ChallengeResult_IfUserIsUnauthorize()
        {
            // Arrange
            // Make the mockUserManager always return null for GetUserAsync
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(null));
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);
            var model = new TodoItemAddViewModel();

            // Act
            var result = await controller.AddItem(model);

            // Assert
            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task AddItemPost_ShouldReturn_BadRequest_IfFailed()
        {
            // Arrange
            var fakeUser = new IdentityUser();
            // Make the mockUserManager always return a fake user for GetUserAsync
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(fakeUser));

            // Make the mockTodoItemService always fails
            mockTodoItemService
                .Setup(x => x.AddItemAsync(It.IsAny<TodoItem>(), It.IsAny<IdentityUser>()))
                .Returns(Task.FromResult(false));

            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);
            var model = new TodoItemAddViewModel();

            //Act
            var result = await controller.AddItem(model);
            var objectResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.IsType<string>(objectResult.Value);
        }

        [Fact]
        public async Task AddItemPost_ShouldReturn_RedirectToActionIndex_IfModelIsValid()
        {
            // Arrange
            var fakeUser = new IdentityUser();
            // Make the mockUserManager return a fake user
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(fakeUser));

            // Make the mockTodoItemService always succeed
            mockTodoItemService
                .Setup(x => x.AddItemAsync(It.IsAny<TodoItem>(), It.IsAny<IdentityUser>()))
                .Returns(Task.FromResult(true));

            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);
            var model = new TodoItemAddViewModel();

            //Act
            var result = await controller.AddItem(model);
            var objectResult = result as RedirectToActionResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.Equal(nameof(TodoController.Index), objectResult.ActionName);
        }

        [Fact]
        public async Task MarkDone_ShouldReturn_BadRequest_IfIdIsEmpty()
        {
            // Arrange
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var result = await controller.MarkDone(Guid.Empty);

            // Assert
            Assert.NotNull(result as BadRequestResult);
        }

        [Fact]
        public async Task MarkDone_ShouldReturn_ChallengeResult_IfUserIsUnauthorize()
        {
            // Arrange
            // Make the mockUserManager always return null for GetUserAsync
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(null));
            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var randomId = Guid.NewGuid();
            var result = await controller.MarkDone(randomId);

            // Assert
            Assert.IsType<ChallengeResult>(result);
        }

        [Fact]
        public async Task MarkDone_ShouldReturn_BadRequest_IfFailed()
        {
            // Arrange
            var fakeUser = new IdentityUser();
            // Make the mockUserManager return a fake user
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(fakeUser));

            // Make the mockTodoItemService always fails
            mockTodoItemService
                .Setup(x => x.MarkDoneAsync(It.IsAny<Guid>(), It.IsAny<IdentityUser>()))
                .Returns(Task.FromResult(false));

            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            //Act
            var result = await controller.MarkDone(Guid.NewGuid());
            var objectResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.IsType<string>(objectResult.Value);
        }

        [Fact]
        public async Task MarkDone_ShouldReturn_RedirectToActionIndex()
        {
            // Arrange
            // Make the mockUserManager return a fake user
            var fakeUser = new IdentityUser();
            mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult<IdentityUser>(fakeUser));

            // Make the mockTodoItemService always succeed
            mockTodoItemService
                .Setup(x => x.MarkDoneAsync(It.IsAny<Guid>(), It.IsAny<IdentityUser>()))
                .Returns(Task.FromResult(true));

            var controller = new TodoController(mockTodoItemService.Object, mockCategoryService.Object, mockUserManager.Object);

            // Act
            var result = await controller.MarkDone(Guid.NewGuid());
            var statusCodeResult = result as StatusCodeResult;

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(TodoController.Index), redirectResult.ActionName);
        }
    }
}
