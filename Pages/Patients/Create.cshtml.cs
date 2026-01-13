//using HCAMiniEHR.Data;
//using HCAMiniEHR.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace HCAMiniEHR.Pages.Patients
//{
//    public class CreateModel : PageModel
//    {
//        private readonly MINIDbContext _context;

//        public CreateModel(MINIDbContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public Patient Patient { get; set; } = new();

//        public void OnGet() { }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//                return Page();

//            _context.Patients.Add(Patient);
//            await _context.SaveChangesAsync();

//            return RedirectToPage("Index");
//        }
//    }
//}



using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace HCAMiniEHR.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly MINIDbContext _context;

        public CreateModel(MINIDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patient Patient { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC [Mini].[CreatePatient] @FirstName, @LastName, @DateOfBirth, @Gender, @PhoneNumber",
                    new SqlParameter("@FirstName", Patient.FirstName),
                    new SqlParameter("@LastName", Patient.LastName),
                    new SqlParameter("@DateOfBirth", Patient.DateOfBirth),
                    new SqlParameter("@Gender", (object?)Patient.Gender ?? DBNull.Value),
                    new SqlParameter("@PhoneNumber", (object?)Patient.PhoneNumber ?? DBNull.Value)
                );

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // Show real SQL error on UI (for debugging/demo)
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
