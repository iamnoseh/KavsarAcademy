using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository (DataContext context) : ICourseRepository
{
    public async Task<List<Course>> GetAll()
    {
        var courses = await context.Courses.Where(x => x.IsDeleted == false).ToListAsync();
        return courses;
    }

    public async Task<Course?> GetById(int id)
    {
        var course = await context.Courses.Where(x=> x.IsDeleted == false)
            .FirstOrDefaultAsync(x => x.Id == id);
        return course;
    }

    public async Task<int> Create(Course course)
    {
        await context.Courses.AddAsync(course);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Update(Course course)
    {
        context.Courses.Update(course);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(Course course)
    {
        var deletedCourse = await context.Courses.Where(x=> x.IsDeleted == false)
            .FirstOrDefaultAsync(x => x.Id == course.Id);
        if (deletedCourse == null) return 0;
         deletedCourse.IsDeleted = true;
        return await context.SaveChangesAsync();
    }
    
    // Получение всех курсов с преподавателями, материалами и StudyInCourse
    public async Task<List<Course>> GetCoursesWithDetails()
    {
        return await context.Courses
            .Where(x => x.IsDeleted == false)
            .Include(c => c.Colleague)
            .ToListAsync();
    }
    
    // Получение курса по ID с преподавателем, материалами и StudyInCourse
    public async Task<Course?> GetCourseWithDetailsById(int id)
    {
        return await context.Courses
            .Where(x => x.IsDeleted == false && x.Id == id)
            .Include(c => c.Colleague)
            .FirstOrDefaultAsync();
    }
}