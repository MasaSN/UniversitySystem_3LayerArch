using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Data.Context;
using University.Data.Entities;

namespace University.Data.Reposetories
{
    public class CourseRepository : IcourseRepository

    {
        private readonly UniversityContext _context;
        public CourseRepository(UniversityContext context)
        {
            _context = context;

        }
        public void create(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            _context.Add(course);

        }

        public void delete(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            _context.Courses.Remove(course);
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetById(int id)
        {
            return _context.Courses.Find(id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void update(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            
            _context.Courses.Update(course);

        }

    }
    public interface IcourseRepository
    {
        Course GetById(int id);

        List<Course> GetAll();
        void create(Course course);
        void update(Course course);
        void delete(Course course);
        void SaveChanges();
    }
}
