﻿using BlogProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlogPost> BlogPosts { get; set; } = default!;
        public virtual DbSet<Comment> Comments { get; set; } = default!;
        public virtual DbSet<Category> Categories { get; set; } = default!;
        public virtual DbSet<Tag> Tags { get; set; } = default!;
        public virtual DbSet<Contact> Contact { get; set; } = default!;

    }
}