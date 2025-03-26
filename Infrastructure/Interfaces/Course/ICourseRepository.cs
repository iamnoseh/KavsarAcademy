using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAll();
    Task<Course?> GetById(int id);
    Task<int> Create(Course course);
    Task<int> Update(Course course);
    Task<int> Delete(Course course);
    
    // Методы для получения курсов с включенными связями
    Task<List<Course>> GetCoursesWithDetails();
    Task<Course?> GetCourseWithDetailsById(int id);
}