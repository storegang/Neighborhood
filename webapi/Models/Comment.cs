﻿using webapi.Interfaces;

namespace webapi.Models;

public class Comment : BaseEntity, ILikeable, IUserReference
{
    // INHERITS: public string Id { get; set; }
    public string Content { get; set; }

    public DateTime DatePosted { get; set; }
    public DateTime? DateLastEdited { get; set; }

    public User User { get; set; }
    public string ParentPostId { get; set; }
    public Post ParentPost { get; set; }

    public string? ImageUrl { get; set; }
    public ICollection<string>? LikedByUserID { get; set; } = new List<string>();
}
