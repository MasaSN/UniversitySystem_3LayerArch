using System.Net;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using University.Core.dtos;
using University.Core.Exceptions;
using University.Core.forms;
using University.Core.services;
using University.Data.Context;
using University.Data.Entities;
using University.Data.Reposetories;
using UniversitySystem_3LayerArch.Filters;

namespace UniversitySystem_3LayerArch.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class StudentControler : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentControler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetId(int id)
        {
            var dto = _studentService.GetById(id);
            return new ApiResponse(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetAll()
        {
            var dto = _studentService.GetAll();
            return new ApiResponse(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse create([FromBody] CreateStudentFrom form)
        {
            _studentService.Create(form);
            return new ApiResponse(HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse update([FromBody] UpdateStudentForm form, int id)
        {
            _studentService.Update(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse Delete(int id)
        {
            _studentService.Delete(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
