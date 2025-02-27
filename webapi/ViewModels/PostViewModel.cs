﻿using webapi.Models;

namespace webapi.ViewModels;

public class PostViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DatePosted { get; set; }

    public User User { get; set; }

    public Category Category { get; set; }
    public Neighborhood Neighborhood { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<string>? Images { get; set; }
    public ICollection<string>? LikedByUserID { get; set; }

    // TODO: Add image urls to POST

    // TODO: Add events and polls
}
