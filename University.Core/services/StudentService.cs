using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Core.dtos;
using University.Core.forms;
using University.Data.Entities;
using University.Data.Reposetories;

namespace University.Core.services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Create(CreateStudentFrom form)
        {
            if (form == null) 
            { 
                throw new ArgumentNullException(nameof(form)); 
            }
            if (string.IsNullOrWhiteSpace(form.Name)) { 
                throw new ArgumentNullException(nameof(form.Name));
            }
            if (string.IsNullOrWhiteSpace(form.Email))
            {
                throw new ArgumentNullException(nameof(form.Email));
            }
            var student = new Student()
            {
                Name = form.Name,
                Email = form.Email,
            };
           
            _studentRepository.create( student);
            _studentRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null) {
                throw new ArgumentNullException(nameof(student));    
            }
            _studentRepository.delete(student);
        }

        public List<StudentDTO> GetAll()
        {
            var students = _studentRepository.GetAll();
            var dtos = students.Select(students => new StudentDTO()
            {
                Name = students.Name,
                Email = students.Email,
            }).ToList();
            return dtos;
        }

        public StudentDTO GetById(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            var dto = new StudentDTO()
            {
                
                Name = student.Name,
                Email = student.Email,
            };
            return dto;
        }

        public void Update(int id, UpdateStudentForm form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }
            if (string.IsNullOrWhiteSpace(form.Name))
            {
                throw new ArgumentNullException(nameof(form.Name));
            }

            var student = _studentRepository.GetById(id);
            if(student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            student.Name = form.Name;

            _studentRepository.update(student);
            _studentRepository.SaveChanges();
        }
    }
    public interface IStudentService
    {
        StudentDTO GetById(int id);
        List<StudentDTO> GetAll();
        void Create(CreateStudentFrom studentDTO);
        void Update(int id, UpdateStudentForm studentDTO);
        void Delete(int id);
    }
}
