using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminPortal.API.Entities.Dto_s;
using System;
using AdminPortal.API.Entities.Dto;
using AdminPortal.API.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository ımageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository ımageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.ımageRepository = ımageRepository;
        }
        [HttpGet]
        [Route("[Controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentRepository.GetStudentsAsync();
            return Ok(mapper.Map<List<StudentDto>>(students));
        }

        [HttpGet]
        [Route("[Controller]/{studentId:guid}"), ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            var student = await studentRepository.GetStudentAsync(studentId);

            if (student != null)
            {
                return Ok(mapper.Map<StudentDto>(student));
            }
            return NotFound();
        }

        [HttpPut]
        [Route("[Controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId))
            {
                var updatedStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<Student>(request));

                if (updatedStudent != null)
                {
                    return Ok(mapper.Map<StudentDto>(updatedStudent));

                }
            }
            return NotFound();

        }

        [HttpDelete]
        [Route("[Controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
                var student = await studentRepository.DeleteStudent(studentId);
                return Ok(mapper.Map<StudentDto>(student));
            }
            return NotFound();
        }
        [HttpPost]
        [Route("[Controller]/add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest addStudentRequest)
        {
            var student = await studentRepository.AddStudent(mapper.Map<Student>(addStudentRequest));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id }, mapper.Map<StudentDto>(student));

        }
        [HttpPost]
        [Route("[Controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            if (await studentRepository.Exists(studentId))
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                var fileImagePath = await ımageRepository.Upload(profileImage, fileName);

                if (await studentRepository.UpdateProfileImage(studentId, fileImagePath))
                {
                    return Ok(fileImagePath);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
            }

            return NotFound();
        }
    }

}
