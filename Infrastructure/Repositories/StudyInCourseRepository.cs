using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces.StudyInCourse;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudyInCourseRepository(DataContext context) : IStudyInCourseRepository
{
    public async Task<List<StudyInCourse>> GetAll()
    {
        return await context.StudyInCourses.ToListAsync();
    }

    public async Task<List<StudyInCourse>> GetByCourseId(int courseId)
    {
        return await context.StudyInCourses
            .Where(s => s.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<StudyInCourse?> GetById(int id)
    {
        return await context.StudyInCourses
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<int> Create(StudyInCourse studyInCourse)
    {
        await context.StudyInCourses.AddAsync(studyInCourse);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Update(StudyInCourse studyInCourse)
    {
        context.StudyInCourses.Update(studyInCourse);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(int id)
    {
        var studyInCourse = await GetById(id);
        if (studyInCourse == null) return 0;
        
        context.StudyInCourses.Remove(studyInCourse);
        return await context.SaveChangesAsync();
    }
} 