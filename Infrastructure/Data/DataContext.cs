using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User,IdentityRole<int>,int>(options)
{
    public DbSet<Banner> Banners { get; set; } 
    public DbSet<Request> Requests { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<ChooseUs> ChooseUss{ get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<Colleague> Colleagues { get; set; }
    public DbSet<VideoReview> VideoReviews { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 

        builder.Entity<Comment>()
            .HasMany(c => c.PatternComments)
            .WithOne(c => c.PatternComment)
            .HasForeignKey(c => c.PatternCommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}