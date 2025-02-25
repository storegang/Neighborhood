using Microsoft.AspNetCore.Mvc;
using webapi.Controllers;
using webapi.DTOs;
using webapi.Models;
using webapi.Services;
using Moq;

namespace webapi.Tests.Controllers;

public class CategoryControllerTestMoq
{
    private readonly Mock<ICategoryService> _categoryService;
    private readonly Mock<INeighborhoodService> _neighborhoodService;

    private readonly CategoryController _categoryController;

    public CategoryControllerTestMoq()
    {
        _categoryService = new Mock<ICategoryService>();
        _neighborhoodService = new Mock<INeighborhoodService>();

        _categoryController = new CategoryController(_categoryService.Object, _neighborhoodService.Object);
    }

    [Fact]
    public void GetAll_WhenCalled_ReturnsOkResult()
    {
        _categoryService.Setup(x => x.GetAllCategories()).Returns(new List<Category>());

        var okResult = _categoryController.GetAll().Result as OkObjectResult;

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void GetAll_WhenCalled_ReturnsAllItems()
    {
        _categoryService.Setup(x => x.GetAllCategories())
            .Returns(new List<Category>() { new Category(), new Category(), new Category() });

        var okResult = _categoryController.GetAll().Result as OkObjectResult;
        var items = Assert.IsType<CategoryCollectionDTO>(okResult.Value).Categories;

        Assert.Equal(3, items.Count());
    }



    [Fact]
    public void GetById_WhenCalled_ReturnsOkResult()
    {
        string testId = "3";
        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        var okResult = _categoryController.GetById(testId).Result as OkObjectResult;

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
    {
        string testId = "1";
        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        var notFoundResult = _categoryController.GetById(Guid.NewGuid().ToString()).Result as NotFoundResult;

        Assert.IsType<NotFoundResult>(notFoundResult);
    }

    [Fact]
    public void GetById_ExistingGuidPassed_ReturnsOkResult()
    {
        string testId = "2";
        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        var okResult = _categoryController.GetById(testId).Result as OkObjectResult;

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void GetById_ExistingGuidPassed_ReturnsRightItem()
    {
        string testId = "2";
        _categoryService.Setup(x => x.GetCategoryById(testId))
            .Returns(new Category { Id = testId, Name = "Cat", Color = "#000000", NeighborhoodId = "2" });

        var okResult = _categoryController.GetById(testId).Result as OkObjectResult;

        Assert.IsType<CategoryDTO>(okResult.Value);
        Assert.Equal(testId, (okResult.Value as CategoryDTO).Id);
    }



    [Fact]
    public void Create_NotExistingNeighborhoodGuid_ReturnsNotFound()
    {
        var testItem = new CategoryDTO()
        {
            Id = "1",
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "2",
        };

        _neighborhoodService.Setup(x => x.GetNeighborhoodById("1")).Returns(new Neighborhood());

        var notFoundResult = _categoryController.Create(testItem).Result as NotFoundResult;

        Assert.IsType<NotFoundResult>(notFoundResult);
    }

    [Fact]
    public void Create_ValidObjectPassed_ReturnsCreatedResult()
    {
        var testItem = new CategoryDTO()
        {
            Id = "1",
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "1",
        };

        _neighborhoodService.Setup(x => x.GetNeighborhoodById("1")).Returns(new Neighborhood());

        var createdResult = _categoryController.Create(testItem).Result as CreatedAtActionResult;

        Assert.IsType<CreatedAtActionResult>(createdResult);
    }

    [Fact]
    public void Create_ValidObjectPassed_ReturnsCreatedItem()
    {
        var testItem = new CategoryDTO()
        {
            Id = "1",
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "1",
        };

        _neighborhoodService.Setup(x => x.GetNeighborhoodById("1")).Returns(new Neighborhood());

        var createdResult = _categoryController.Create(testItem).Result as CreatedAtActionResult;
        var item = createdResult.Value as CategoryDTO;

        Assert.IsType<CategoryDTO>(item);
        Assert.Equal("Category 1", item.Name);
    }



    [Fact]
    public void Update_NotExistingGuid_ReturnsNotFound()
    {
        string testId = "1";
        var testItem = new CategoryDTO()
        {
            Id = testId,
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "2",
        };

        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        var notFoundResult = _categoryController.Update("2", testItem) as NotFoundResult;

        Assert.IsType<NotFoundResult>(notFoundResult);
    }

    [Fact]
    public void Update_ExistingGuidPassed_ReturnsNoContent()
    {
        string testId = "1";
        var testItem = new CategoryDTO()
        {
            Id = testId,
            Name = "Category 1",
            Color = "#000000",
            NeighborhoodId = "2",
        };

        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        var noContentResult = _categoryController.Update(testId, testItem) as NoContentResult;

        Assert.IsType<NoContentResult>(noContentResult);
    }



    [Fact]
    public void Delete_NotExistingGuidPassed_ReturnsNotFoundResult()
    {
        string testId = "2";
        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        _categoryService.Setup(x => x.DeleteCategory(It.IsAny<string>()));

        var notFoundResult = _categoryController.Delete(Guid.NewGuid().ToString()) as NotFoundResult;

        Assert.IsType<NotFoundResult>(notFoundResult);
    }

    [Fact]
    public void Delete_ExistingGuidPassed_ReturnsNoContentResult()
    {
        string testId = "2";
        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        _categoryService.Setup(x => x.DeleteCategory(It.IsAny<string>()));

        var noContentResult = _categoryController.Delete(testId) as NoContentResult;

        Assert.IsType<NoContentResult>(noContentResult);
    }

    [Fact]
    public void Delete_ExistingGuidPassed_ServiceCalledOnce()
    {
        string testId = "2";
        _categoryService.Setup(x => x.GetCategoryById(testId)).Returns(new Category());

        _categoryService.Setup(x => x.DeleteCategory(It.IsAny<string>()));

        var okResult = _categoryController.Delete(testId);

        _categoryService.Verify(x => x.DeleteCategory(testId), Times.Once);
    }
}
