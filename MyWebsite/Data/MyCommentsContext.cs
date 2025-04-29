using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Models;

namespace MyWebsite.Data
{
    public class MyCommentsContext : DbContext
    {
        public MyCommentsContext (DbContextOptions<MyCommentsContext> options)
            : base(options)
        {
        }

        public DbSet<MyWebsite.Models.Comment> CommentModel { get; set; } = default!;
    }
}
