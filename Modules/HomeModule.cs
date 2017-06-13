using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Registrar.Objects;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/students"] = _ => {
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
      Get["/courses"] = _ => {
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };
      Get["/courses/new"] = _ => {
        return View["courses_form.cshtml"];
      };
      Post["/courses/new"] = _ => {
        Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
        newCourse.Save();
        return View["success.cshtml"];
      };
      Get["/students/new"] = _ => {
        return View["students_form.cshtml"];
      };
      Post["/students/new"] = _ => {
        Student newStudent = new Student(Request.Form["student-first-name"], Request.Form["student-last-name"], Request.Form["student-enrollment-date"]);
        newStudent.Save();
        return View["success.cshtml"];
      };
      Post["/students/delete"] = _ => {
        Student.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["students/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student SelectedStudent = Student.Find(parameters.id);
        List<Course> StudentCourses = SelectedStudent.GetCourses();
        List<Course> AllCourses = Course.GetAll();
        model.Add("student", SelectedStudent);
        model.Add("studentCourses", StudentCourses);
        model.Add("allCourses", AllCourses);
        return View["student.cshtml", model];
      };
      Get["courses/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course SelectedCourse = Course.Find(parameters.id);
        List<Student> CourseStudents = SelectedCourse.GetStudents();
        List<Student> AllStudents = Student.GetAll();
        model.Add("course", SelectedCourse);
        model.Add("courseStudents", CourseStudents);
        model.Add("allStudents", AllStudents);
        return View["course.cshtml", model];
      };
      // Post["student/add_course"] = _ => {
      //   Course course = Course.Find(Request.Form["course-id"]);
      //   Student student = Student.Find(Request.Form["student-id"]);
      //   student.AddCourse(course);
      //   return View["success.cshtml"];
      // };
      // Post["course/add_student"] = _ => {
      //   Course course = Course.Find(Request.Form["course-id"]);
      //   Student student = Student.Find(Request.Form["student-id"]);
      //   course.AddStudent(student);
      //   return View["success.cshtml"];
      // };
    }
  }
}
