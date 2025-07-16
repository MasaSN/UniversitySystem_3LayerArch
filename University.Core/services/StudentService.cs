using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using University.Core.dtos;
using University.Core.Exceptions;
using University.Core.forms;
using University.Core.Validations;
using University.Data.Entities;
using University.Data.Reposetories;

namespace University.Core.services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentService> _logger;
        public StudentService(IStudentRepository studentRepository, ILogger<StudentService>logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public  void Create(CreateStudentFrom form)
        {
            var validation = FormValidator.Validate(form);
            if (!validation.isValid)
            {
                throw new BussinessException(validation.Errors);
            }
            var existingStudent = _studentRepository.GetByEmail(form.Email);
            if (existingStudent)
            {
                throw new BussinessException(new Dictionary<string, List<string>>()
                {
                    { "Email", new List<string>() { "Email already exists" } }
                });
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
                throw new NotFoundException("student not found");    
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
            _logger.LogInformation($"Getting student by id {id}", id);
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                throw new NotFoundException("unable to find student");
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
            var validation = FormValidator.Validate(form);
            if (!validation.isValid)
            {
                throw new BussinessException(validation.Errors);
            }

            var student = _studentRepository.GetById(id);
            if(student == null)
            {
                throw new NotFoundException("unable to find student");
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
        //void GetByEmail(string email);
    }
}
