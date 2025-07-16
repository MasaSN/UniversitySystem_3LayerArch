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
    public class CourseServeice : ICourseService
    {
        private readonly IcourseRepository _courseRepository;
        private readonly ILogger<CourseServeice> _logger;
        public CourseServeice(IcourseRepository courseRepository, ILogger<CourseServeice> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public void Create(CreateCourseForm form)
        {
            _logger.LogInformation("Creating a new course with name: {CourseName}", form.CourseName);
            var validation = FormValidator.Validate(form);
            if (!validation.isValid)
            {
                throw new BussinessException(validation.Errors);
            }
            var course = new Course()
            {
                CourseName = form.CourseName,
                description = form.description,
            };

            _courseRepository.create(course);
            _courseRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            _courseRepository.delete(course);
        }

        public List<CourseDTO> GetAll()
        {
            var course = _courseRepository.GetAll();
            var dtos = course.Select(course => new CourseDTO()
            {
                CourseName = course.CourseName,
                description = course.description,
            }).ToList();
            return dtos;
        }

        public CourseDTO GetById(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
            var dto = new CourseDTO()
            {

                CourseName = course.CourseName,
                description = course.description,
            };
            return dto;
        }

        public void Update(int id, UpdateCourseForm form)
        {
            var validation = FormValidator.Validate(form);
            if (!validation.isValid)
            {
                throw new BussinessException(validation.Errors);
            }

            var course = _courseRepository.GetById(id);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            course.description = form.description;

            _courseRepository.update(course);
            _courseRepository.SaveChanges();
        }
    }
    public interface ICourseService
    {
        CourseDTO GetById(int id);
        List<CourseDTO> GetAll();
        void Create(CreateCourseForm CourseDTO);
        void Update(int id, UpdateCourseForm CourseDTO);
        void Delete(int id);
    }
}
