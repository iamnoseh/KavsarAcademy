using Domain.Entities;

namespace Infrastructure.Interfaces.StudyInCourse;

public interface IStudyInCourseRepository
{
    Task<List<Domain.Entities.StudyInCourse>> GetAll();
    Task<List<Domain.Entities.StudyInCourse>> GetByCourseId(int courseId);
    Task<Domain.Entities.StudyInCourse?> GetById(int id);
    Task<int> Create(Domain.Entities.StudyInCourse studyInCourse);
    Task<int> Update(Domain.Entities.StudyInCourse studyInCourse);
    Task<int> Delete(int id);
} 