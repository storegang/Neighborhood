﻿namespace webapi.Models
{
    public class Like
    {
        public string Id { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
