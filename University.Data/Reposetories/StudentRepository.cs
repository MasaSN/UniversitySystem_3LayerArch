using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Data.Context;
using University.Data.Entities;

namespace University.Data.Reposetories
{
    public class StudentRepository : IStudentRepository

    {
        private readonly UniversityContext _context;
        public StudentRepository(UniversityContext context) {
            _context = context;
            
        }
        public void create(Student student)
        {
            if(student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            student.CreatedAt = DateTime.Now;
            _context.Students.Add(student);
            
        }

        public void delete(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            _context.Students.Remove(student);
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student GetById(int id)
        {
            return _context.Students.Find( id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void update(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            student.UpdatedAt = DateTime.Now;
            _context.Students.Update(student);

        }

    }
    public interface IStudentRepository
        {
        Student GetById(int id);
        
        List <Student> GetAll();
        void create(Student student);
        void update(Student student);
        void delete(Student student);
        void SaveChanges();
        }
}
