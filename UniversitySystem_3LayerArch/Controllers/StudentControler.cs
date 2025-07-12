using System.Net;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using University.Core.forms;
using University.Core.services;
using University.Data.Context;
using University.Data.Entities;
using University.Data.Reposetories;

namespace UniversitySystem_3LayerArch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentControler : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentControler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id}")]
        public ApiResponse GetId(int id) { 
            var dto = _studentService.GetById(id);
            return new ApiResponse(dto);
        }

        [HttpGet]
        public ApiResponse GetAll(int id)
        {
            var dto = _studentService.GetAll();
            return new ApiResponse(dto);
        }

        [HttpPost]
        public ApiResponse create([FromBody] CreateStudentFrom form) {
            _studentService.Create(form);
            return new ApiResponse(HttpStatusCode.Created);
        }
        [HttpPut("{id}")]
        public ApiResponse update([FromBody] UpdateStudentForm form, int id)
        {
            _studentService.Update(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }
        [HttpDelete]
        public ApiResponse Delete(int id) { 
            _studentService.Delete(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
