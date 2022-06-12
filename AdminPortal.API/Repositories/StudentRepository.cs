using AdminPortal.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminPortal.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AdminContext context;

        public StudentRepository(AdminContext context)
        {
            this.context = context;
        }
        public async Task<Student> AddStudent(Student addStudentRequest)
        {
            var student = await context.Student.AddAsync(addStudentRequest);
            await context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                context.Student.Remove(student);
                await context.SaveChangesAsync();
                return student;
            }
            return null;
        }

        public async Task<bool> Exists(Guid studentId)
        {
            return await context.Student.AnyAsync(x => x.Id == studentId);
        }
        public async Task<List<Gender>> GetGendersAsync()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studenId)
        {
            return await context.Student.Include(nameof(Address)).Include(nameof(Gender)).FirstOrDefaultAsync(x => x.Id == studenId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await context.Student.Include(nameof(Address)).Include(nameof(Gender)).ToListAsync();
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.Mobile = request.Mobile;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.Email = request.Email;
                existingStudent.GenderId = request.GenderId;
                existingStudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = request.Address.PostalAddress;

                await context.SaveChangesAsync();
                return existingStudent;
            }

            return null;
        }
    }
}
