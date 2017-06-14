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
      Get["/departments"] = _ => {
        List<Department> AllDepartments = Department.GetAll();
        return View["departments.cshtml", AllDepartments];
      };
      Get["/courses/new"] = _ => {
        return View["courses_form.cshtml"];
      };
      Post["/courses/new"] = _ => {
        Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
        newCourse.Save();
        return View["success.cshtml"];
      };
      Get["/departments/new"] = _ => {
        return View["departments_form.cshtml"];
      };
      Post["/departments/new"] = _ => {
        Department newDepartment = new Department(Request.Form["department-name"]);
        newDepartment.Save();
        return View["success.cshtml"];
      };
      Get["/student/new"] = _ => {
        return View["students_form.cshtml"];
      };
      Post["/student/new"] = _ => {
        Student newStudent = new Student(Request.Form["student-first-name"], Request.Form["student-last-name"], Request.Form["student-enrollment-date"]);
        newStudent.Save();
        return View["success.cshtml"];
      };
      Get["students/delete/confirm"] = _ => {
        return View["students_delete.cshtml"];
      };
      Post["/students/delete"] = _ => {
        Student.DeleteAll();
        return View["success.cshtml"];
      };
      Get["departments/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Department SelectedDepartment = Department.Find(parameters.id);
        List<Student> DepartmentStudents = SelectedDepartment.GetStudents();
        List<Student> AllStudents = Student.GetAll();
        model.Add("department", SelectedDepartment);
        model.Add("departmentStudents", DepartmentStudents);
        model.Add("allStudents", AllStudents);
        return View["department.cshtml", model];
      };
      Get["student/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student SelectedStudent = Student.Find(parameters.id);
        List<Course> StudentCourses = SelectedStudent.GetCourses();
        List<Course> AllCourses = Course.GetAll();
        List<Department> StudentDepartments = SelectedStudent.GetDepartments();
        List<Department> AllDepartments = Department.GetAll();
        model.Add("student", SelectedStudent);
        model.Add("studentCourses", StudentCourses);
        model.Add("studentDepartments", StudentDepartments);
        model.Add("allCourses", AllCourses);
        model.Add("allDepartments", AllDepartments);
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
      Post["student/add_course"] = _ => {
        Course course = Course.Find(Request.Form["course-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        student.AddCourse(course);
        return View["success.cshtml"];
      };
      Post["student/add_department"] = _ => {
        Department department = Department.Find(Request.Form["department-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        student.AddDepartment(department);
        return View["success.cshtml"];
      };
      Post["course/add_student"] = _ => {
        Course course = Course.Find(Request.Form["course-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        course.AddStudent(student);
        return View["success.cshtml"];
      };
      Post["departments/add_student"] = _ => {
        Department department = Department.Find(Request.Form["department-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        department.AddStudent(student);
        return View["success.cshtml"];
      };
      Delete["/student/delete/{id}"] = parameters => {
        Student currentStudent = Student.Find(parameters.id);
        currentStudent.Delete();
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };
      Delete["/departments/delete/{id}"] = parameters => {
        Department currentDepartment = Department.Find(parameters.id);
        currentDepartment.Delete();
        List<Department> allDepartments = Department.GetAll();
        return View["departments.cshtml", allDepartments];
      };
      Patch["/student/update/{id}"] = parameters => {
        Student currentStudent = Student.Find(parameters.id);
        string newFirstName = Request.Form["new-first-name"];
        string newLastName = Request.Form["new-last-name"];
        if (newFirstName == "" && newLastName != "")
        {
          currentStudent.UpdateLastName(newLastName);
        }
        else if (newFirstName != "" && newLastName == "")
        {
          currentStudent.UpdateFirstName(newFirstName);
        }
        else
        {
          currentStudent.UpdateLastName(newLastName);
          currentStudent.UpdateFirstName(newFirstName);
        }
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Course> allCourses = Course.GetAll();
        List<Course> studentCourses = currentStudent.GetCourses();
        model.Add("student", currentStudent);
        model.Add("allCourses", allCourses);
        model.Add("studentCourses", studentCourses);
        return View["student.cshtml", model];
      };
    }
  }
}
