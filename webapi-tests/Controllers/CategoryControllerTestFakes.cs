using Microsoft.AspNetCore.Mvc;
using webapi.Controllers;
using webapi.DTOs;
using webapi.Tests.Services;

namespace webapi.Tests.Controllers;

public class CategoryControllerTestFakes
{
    private readonly CategoryServiceFake _categoryService;
    private readonly NeighborhoodServiceFake _neighborhoodService;

    private readonly CategoryController _categoryController;

    public CategoryControllerTestFakes()
    {
        _categoryService = new CategoryServiceFake();
        _neighborhoodService = new NeighborhoodServiceFake();

        _categoryController = new CategoryController(_categoryService, _neighborhoodService);
    }

    [Fact]
    public void GetAll_WhenCalled_ReturnsOkResult()
    {
        var okResult = _categoryController.GetAll().Result;

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void GetAll_WhenCalled_ReturnsAllItems()
    {
        var okResult = _categoryController.GetAll().Result as OkObjectResult;
        var items = Assert.IsType<CategoryCollectionDTO>(okResult.Value).Categories;

        Assert.Equal(3, items.Count());
    }



    [Fact]
    public void GetById_WhenCalled_ReturnsOkResult()
    {
        var okResult = _categoryController.GetById("1").Result;

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
    {
        var notFoundResult = _categoryController.GetById(Guid.NewGuid().ToString()).Result;

        Assert.IsType<NotFoundResult>(notFoundResult);
    }

    [Fact]
    public void GetById_ExistingGuidPassed_ReturnsOkResult()
    {
        var okResult = _categoryController.GetById("2").Result as OkObjectResult;

        Assert.IsType<OkObjectResult>(okResult);
    }
    [Fact]
    public void GetById_ExistingGuidPassed_ReturnsRightItem()
    {
        var testId = "2";
        var okResult = _categoryController.GetById(testId).Result as OkObjectResult;

        Assert.IsType<CategoryDTO>(okResult.Value);
        Assert.Equal(testId, (okResult.Value as CategoryDTO).Id);
    }



    [Fact]
    public void Create_ValidObjectPassed_ReturnsCreatedResponse()
    {
        var testItem = new CategoryDTO()
        {
            Id = "1",
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "1",
        };

        var createdResponse = _categoryController.Create(testItem).Result as CreatedAtActionResult;

        Assert.IsType<CreatedAtActionResult>(createdResponse);
    }

    [Fact]
    public void Create_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
        var testItem = new CategoryDTO()
        {
            Id = "1",
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "1",
        };

        var createdResponse = _categoryController.Create(testItem).Result as CreatedAtActionResult;
        var item = createdResponse.Value as CategoryDTO;

        Assert.IsType<CategoryDTO>(item);
        Assert.Equal("Category 1", item.Name);
    }



    [Fact]
    public void Delete_NotExistingGuidPassed_ReturnsNotFoundResponse()
    {
        var badResponse = _categoryController.Delete(Guid.NewGuid().ToString());

        Assert.IsType<NotFoundResult>(badResponse);
    }

    [Fact]
    public void Delete_ExistingGuidPassed_ReturnsNoContentResult()
    {
        var noContentResponse = _categoryController.Delete("2");

        Assert.IsType<NoContentResult>(noContentResponse);
    }

    [Fact]
    public void Delete_ExistingGuidPassed_RemovesOneItem()
    {
        var okResponse = _categoryController.Delete("2");

        Assert.Equal(2, _categoryService.GetAllCategories().Count());
    }
}
