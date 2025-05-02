using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.DTOs;
using webapi.Identity;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DemoController(INeighborhoodService neighborhoodService, ICategoryService CategoryService, IPostService PostService, ICommentService CommentService, UserManager<User> userManager) : ControllerBase
{
    private readonly INeighborhoodService _neighborhoodService = neighborhoodService;
    private readonly ICategoryService _categoryService = CategoryService;
    private readonly IPostService _postService = PostService;
    private readonly ICommentService _commentService = CommentService;
    private readonly UserManager<User> _userManager = userManager;

    // POST api/<NeighborhoodController>
    [HttpPost("&DemoData")]
    public async Task<ActionResult<NeighborhoodDTO>> CreateWithDemoData(NeighborhoodDTO neighborhoodData)
    {
        string newGuid;
        do
        {
            newGuid = Guid.NewGuid().ToString();
        }
        while (await _neighborhoodService.GetById(newGuid) != null);

        neighborhoodData.Id = newGuid;
        Neighborhood neighborhood = new()
        {
            Id = neighborhoodData.Id,
            Name = neighborhoodData.Name,
            Description = neighborhoodData.Description
        };

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        User? existingUser = await _userManager.FindByIdAsync(claimsId);
        if (existingUser == null)
        {
            return Unauthorized("User does not exist.");
        }

        if (!string.IsNullOrEmpty(existingUser.NeighborhoodId))
        {
            return BadRequest("Can not create a new neighborhood while being a member of another.");
        }

        neighborhood.Users.Add(existingUser);
        await _neighborhoodService.Create(neighborhood);

        existingUser.NeighborhoodId = neighborhood.Id;
        await _userManager.AddToRolesAsync(existingUser, [UserRoles.Tenant, UserRoles.BoardMember]);
        await _userManager.UpdateAsync(existingUser);

        // Create demo data
        User demoUser1 = new() { Id = Guid.NewGuid().ToString(), Name = "Frans", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/men/31.jpg" };
        User demoUser2 = new() { Id = Guid.NewGuid().ToString(), Name = "Peter", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/men/3.jpg" };
        User demoUser3 = new() { Id = Guid.NewGuid().ToString(), Name = "Amalie", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/women/0.jpg" };
        User demoUser4 = new() { Id = Guid.NewGuid().ToString(), Name = "Erik", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/men/15.jpg" };
        User demoUser5 = new() { Id = Guid.NewGuid().ToString(), Name = "Benedicte", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/women/28.jpg" };
        User demoUser6 = new() { Id = Guid.NewGuid().ToString(), Name = "Siri", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/women/38.jpg" };
        User demoUser7 = new() { Id = Guid.NewGuid().ToString(), Name = "Arne", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/men/22.jpg" };
        User demoUser8 = new() { Id = Guid.NewGuid().ToString(), Name = "Mathias", NeighborhoodId = neighborhood.Id, Avatar = "https://randomuser.me/api/portraits/men/43.jpg" };
        neighborhood.Users.Add(demoUser1);
        neighborhood.Users.Add(demoUser2);
        neighborhood.Users.Add(demoUser3);
        neighborhood.Users.Add(demoUser4);
        neighborhood.Users.Add(demoUser5);
        neighborhood.Users.Add(demoUser6);
        neighborhood.Users.Add(demoUser7);
        neighborhood.Users.Add(demoUser8);

        Category category1 = new() { Id = Guid.NewGuid().ToString(), Name = "Information", NeighborhoodId = neighborhood.Id };
        Category category2 = new() { Id = Guid.NewGuid().ToString(), Name = "General", NeighborhoodId = neighborhood.Id };
        Category category3 = new() { Id = Guid.NewGuid().ToString(), Name = "Events", NeighborhoodId = neighborhood.Id };
        Category category4 = new() { Id = Guid.NewGuid().ToString(), Name = "Lending", NeighborhoodId = neighborhood.Id };
        neighborhood.Categories.Add(category1);
        neighborhood.Categories.Add(category2);
        neighborhood.Categories.Add(category3);
        neighborhood.Categories.Add(category4);

        Post demoPost1 = new() { Id = Guid.NewGuid().ToString(), Title = "Spring cleaning soon", Description = "We're doing spring cleaning on the coming Saturday, hope some of you can come and help out!", CategoryId = category1.Id, User = demoUser1 };
        Post demoPost2 = new() { Id = Guid.NewGuid().ToString(), Title = "I like squirrels :)", Description = "I just think they're neat.", CategoryId = category2.Id, User = demoUser2 };
        Post demoPost3 = new() { Id = Guid.NewGuid().ToString(), Title = "We're celebrating my uncles birthday", Description = "Anyone is free to come visit in the party room, got plenty of cake :)", CategoryId = category3.Id, User = demoUser3 };
        Post demoPost4 = new() { Id = Guid.NewGuid().ToString(), Title = "I got a hammer if anyone wants to borrow", Description = "Just come knock on my door if you need it.", CategoryId = category4.Id, User = demoUser4 };
        Post demoPost5 = new() { Id = Guid.NewGuid().ToString(), Title = "Demo Post 5", Description = "This is a fifth demo post.", CategoryId = category1.Id, User = demoUser5 };
        Post demoPost6 = new() { Id = Guid.NewGuid().ToString(), Title = "Demo Post 6", Description = "This is a sixth demo post.", CategoryId = category2.Id, User = demoUser6 };
        Post demoPost7 = new() { Id = Guid.NewGuid().ToString(), Title = "Demo Post 7", Description = "This is a seventh demo post.", CategoryId = category1.Id, User = demoUser7 };
        category1.Posts.Add(demoPost1);
        category2.Posts.Add(demoPost2);
        category1.Posts.Add(demoPost3);
        category2.Posts.Add(demoPost4);
        category1.Posts.Add(demoPost5);
        category2.Posts.Add(demoPost6);
        category1.Posts.Add(demoPost7);

        Comment demoComment1 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a demo comment.", ParentPost = demoPost1, User = demoUser1 };
        Comment demoComment2 = new() { Id = Guid.NewGuid().ToString(), Content = "This is another demo comment.", ParentPost = demoPost2, User = demoUser2 };
        Comment demoComment3 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a third demo comment.", ParentPost = demoPost3, User = demoUser3 };
        Comment demoComment4 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a fourth demo comment.", ParentPost = demoPost4, User = demoUser4 };
        Comment demoComment5 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a fifth demo comment.", ParentPost = demoPost5, User = demoUser5 };
        Comment demoComment6 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a sixth demo comment.", ParentPost = demoPost6, User = demoUser6 };
        Comment demoComment7 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a seventh demo comment.", ParentPost = demoPost7, User = demoUser7 };
        Comment demoComment8 = new() { Id = Guid.NewGuid().ToString(), Content = "This is an eighth demo comment.", ParentPost = demoPost1, User = demoUser8 };
        Comment demoComment9 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a ninth demo comment.", ParentPost = demoPost2, User = demoUser1 };
        Comment demoComment10 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a tenth demo comment.", ParentPost = demoPost3, User = demoUser2 };
        Comment demoComment11 = new() { Id = Guid.NewGuid().ToString(), Content = "This is an eleventh demo comment.", ParentPost = demoPost4, User = demoUser3 };
        Comment demoComment12 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a twelfth demo comment.", ParentPost = demoPost5, User = demoUser4 };
        Comment demoComment13 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a thirteenth demo comment.", ParentPost = demoPost6, User = demoUser5 };
        Comment demoComment14 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a fourteenth demo comment.", ParentPost = demoPost7, User = demoUser6 };
        Comment demoComment15 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a fifteenth demo comment.", ParentPost = demoPost1, User = demoUser7 };
        Comment demoComment16 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a sixteenth demo comment.", ParentPost = demoPost2, User = demoUser8 };
        Comment demoComment17 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a seventeenth demo comment.", ParentPost = demoPost3, User = demoUser1 };
        Comment demoComment18 = new() { Id = Guid.NewGuid().ToString(), Content = "This is an eighteenth demo comment.", ParentPost = demoPost4, User = demoUser2 };
        Comment demoComment19 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a nineteenth demo comment.", ParentPost = demoPost5, User = demoUser3 };
        Comment demoComment20 = new() { Id = Guid.NewGuid().ToString(), Content = "This is a twentieth demo comment.", ParentPost = demoPost6, User = demoUser4 };
        demoPost1.Comments.Add(demoComment1);
        demoPost2.Comments.Add(demoComment2);
        demoPost3.Comments.Add(demoComment3);
        demoPost4.Comments.Add(demoComment4);
        demoPost5.Comments.Add(demoComment5);
        demoPost6.Comments.Add(demoComment6);
        demoPost7.Comments.Add(demoComment7);
        demoPost1.Comments.Add(demoComment8);
        demoPost2.Comments.Add(demoComment9);
        demoPost3.Comments.Add(demoComment10);
        demoPost4.Comments.Add(demoComment11);
        demoPost5.Comments.Add(demoComment12);
        demoPost6.Comments.Add(demoComment13);
        demoPost7.Comments.Add(demoComment14);
        demoPost1.Comments.Add(demoComment15);
        demoPost2.Comments.Add(demoComment16);
        demoPost3.Comments.Add(demoComment17);
        demoPost4.Comments.Add(demoComment18);
        demoPost5.Comments.Add(demoComment19);
        demoPost6.Comments.Add(demoComment20);


        _neighborhoodService.Update(neighborhood);

        await _userManager.CreateAsync(demoUser1);
        await _userManager.CreateAsync(demoUser2);
        await _userManager.CreateAsync(demoUser3);
        await _userManager.CreateAsync(demoUser4);
        await _userManager.CreateAsync(demoUser5);
        await _userManager.CreateAsync(demoUser6);
        await _userManager.CreateAsync(demoUser7);
        await _userManager.CreateAsync(demoUser8);

        await _userManager.AddToRolesAsync(demoUser1, [UserRoles.Tenant, UserRoles.BoardMember]);
        await _userManager.AddToRolesAsync(demoUser2, [UserRoles.Tenant]);
        await _userManager.AddToRolesAsync(demoUser3, [UserRoles.Tenant]);
        await _userManager.AddToRolesAsync(demoUser4, [UserRoles.Tenant]);
        await _userManager.AddToRolesAsync(demoUser5, [UserRoles.Tenant, UserRoles.BoardMember]);
        await _userManager.AddToRolesAsync(demoUser6, [UserRoles.Tenant]);
        await _userManager.AddToRolesAsync(demoUser7, [UserRoles.Tenant]);
        await _userManager.AddToRolesAsync(demoUser8, [UserRoles.Tenant, UserRoles.BoardMember]);

        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser1.Id), [UserRoles.Tenant, UserRoles.BoardMember]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser2.Id), [UserRoles.Tenant]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser3.Id), [UserRoles.Tenant]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser4.Id), [UserRoles.Tenant]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser5.Id), [UserRoles.Tenant, UserRoles.BoardMember]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser6.Id), [UserRoles.Tenant]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser7.Id), [UserRoles.Tenant]);
        //await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(demoUser8.Id), [UserRoles.Tenant, UserRoles.BoardMember]);

        _categoryService.Create(category1);
        _categoryService.Create(category2);

        _postService.Create(demoPost1);
        _postService.Create(demoPost2);
        _postService.Create(demoPost3);
        _postService.Create(demoPost4);
        _postService.Create(demoPost5);
        _postService.Create(demoPost6);
        _postService.Create(demoPost7);

        _commentService.Create(demoComment1);
        _commentService.Create(demoComment2);
        _commentService.Create(demoComment3);
        _commentService.Create(demoComment4);
        _commentService.Create(demoComment5);
        _commentService.Create(demoComment6);
        _commentService.Create(demoComment7);
        _commentService.Create(demoComment8);
        _commentService.Create(demoComment9);
        _commentService.Create(demoComment10);
        _commentService.Create(demoComment11);
        _commentService.Create(demoComment12);
        _commentService.Create(demoComment13);
        _commentService.Create(demoComment14);
        _commentService.Create(demoComment15);
        _commentService.Create(demoComment16);
        _commentService.Create(demoComment17);
        _commentService.Create(demoComment18);
        _commentService.Create(demoComment19);
        _commentService.Create(demoComment20);

        // TODO: Add roles to users

        //return CreatedAtAction(nameof(GetById), new { id = neighborhoodData.Id }, neighborhoodData);
        return Ok(neighborhoodData);
    }

    //[Authorize(Roles = UserRoles.BoardMember)]
    // DELETE api/<NeighborhoodController>/{id}
    [HttpDelete("{id}&DeleteUsers")]
    public async Task<IActionResult> Delete(string id)
    {
        Neighborhood? existingNeighborhood = await _neighborhoodService.GetById(id, [query => query.Include(c => c.Users)]);
        if (existingNeighborhood == null)
        {
            return NotFound("Neighborhood not found.");
        }

        string claimsId = User.Claims.First(c => c.Type.Equals("user_id"))?.Value ?? "";
        User? claimsUser = await _userManager.FindByIdAsync(claimsId);
        if (claimsUser == null)
        {
            return Unauthorized("Requesting user does not exist.");
        }

        ICollection<User> usersCopy = existingNeighborhood.Users;

        await _neighborhoodService.Delete(id);

        foreach (User user in usersCopy)
        {
            await _userManager.RemoveFromRolesAsync(user, [UserRoles.Tenant, UserRoles.BoardMember]);
            await _userManager.UpdateAsync(user);

            if (user.Id == claimsUser.Id)
            {
                continue; // Don't delete the user making the request.
            }
            await _userManager.DeleteAsync(user);
        }

        return NoContent();
    }
}
