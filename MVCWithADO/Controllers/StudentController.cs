using MVCWithADO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWithADO.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        #region display the student
        StudentDAL obj = new StudentDAL();
        public ViewResult DisplayStudents()
        {
            return View(obj.Student_Select(null, true));
        }
        public ViewResult DisplayStudent(int Sid)
        {
            return View(obj.Student_Select(Sid, true).Single());
        }
        #endregion
        #region Add Student
        public ViewResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult AddStudent(Student student, HttpPostedFileBase selectedFile)
        {
            if (selectedFile != null)
            {
                string PhysicalPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(PhysicalPath))
                {
                    Directory.CreateDirectory(PhysicalPath);
                }
                selectedFile.SaveAs(PhysicalPath + selectedFile.FileName);
                student.Photo = selectedFile.FileName;
            }
    
            obj.InsertStudent(student);
           return RedirectToAction("DisplayStudents");
    }
        #endregion
        #region update the student

        public RedirectToRouteResult UpdateStudent(Student student, HttpPostedFileBase selectedFile)
        {
            if (selectedFile != null)
            {
                string PhysicalPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(PhysicalPath))
                {
                    Directory.CreateDirectory(PhysicalPath);
                }
                selectedFile.SaveAs(PhysicalPath + selectedFile.FileName);
                student.Photo = selectedFile.FileName;
            }
            else
            {
                student.Photo = TempData["Photo"].ToString();
            }
            obj.UpdateStudent(student);
            return RedirectToAction("DisplayStudents");
        }

        #endregion
        #region edit the student
        public ViewResult EditStudent(int Sid)
        {
            Student student = obj.Student_Select(Sid, true).Single();
            TempData["Photo"] = student.Photo;
            return View(student);
        }
        #endregion
        #region delete the student
        public RedirectToRouteResult DeleteStudent(int Sid)
        {
            obj.DeleteStudent(Sid);
            return RedirectToAction("DisplayStudents");
        }
        #endregion
    }
}