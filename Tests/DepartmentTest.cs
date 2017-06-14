using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
  [Collection("Registrar")]
  public class DepartmentTest : IDisposable
  {
    public DepartmentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DepartmentsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Department.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameDepartment()
    {
      //Arrange, Act
      Department firstDepartment = new Department("Computer Science");
      Department secondDepartment = new Department("Computer Science");

      //Assert
      Assert.Equal(firstDepartment, secondDepartment);
    }
    [Fact]
    public void Test_Save_SavesDepartmentToDatabase()
    {
      //Arrange
      Department testDepartment = new Department("Computer Science");
      testDepartment.Save();

      //Act
      List<Department> result = Department.GetAll();
      List<Department> testList = new List<Department>{testDepartment};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToDepartmentObject()
    {
      //Arrange
      Department testDepartment = new Department("Computer Science");
      testDepartment.Save();

      //Act
      Department savedDepartment = Department.GetAll()[0];

      int result = savedDepartment.GetId();
      int testId = testDepartment.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsDepartmentInDatabase()
    {
      //Arrange
      Department testDepartment = new Department("Computer Science");
      testDepartment.Save();

      //Act
      Department foundDepartment = Department.Find(testDepartment.GetId());

      //Assert
      Assert.Equal(testDepartment, foundDepartment);
    }
    [Fact]
    public void Delete_DeletesDepartmentFromDatabase_DepartmentList()
    {
      //Arrange
      string departmentName1 = "Computer Science";
      Department testDepartment1 = new Department(departmentName1);
      testDepartment1.Save();

      string departmentName2 = "Philosophy";
      Department testDepartment2 = new Department(departmentName2);
      testDepartment2.Save();

      //Act
      testDepartment1.Delete();
      List<Department> resultDepartments = Department.GetAll();
      List<Department> testDepartmentList = new List<Department> {testDepartment2};

      //Assert
      Assert.Equal(testDepartmentList, resultDepartments);
    }
    [Fact]
    public void Test_AddCourse_AddsCourseToDepartment()
    {
      //Arrange
      Department testDepartment = new Department("English");
      testDepartment.Save();

      Course testCourse = new Course("English 101", "ENG101");
      testCourse.Save();

      Course testCourse2 = new Course("Biology 101", "BIO101");
      testCourse2.Save();

      //Act
      testDepartment.AddCourse(testCourse);
      testDepartment.AddCourse(testCourse2);

      List<Course> result = testDepartment.GetCourses();
      List<Course> testList = new List<Course>{testCourse, testCourse2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetCourses_ReturnsAllDepartmentCourses_CourseList()
    {
      //Arrange
      Department testDepartment = new Department("English");
      testDepartment.Save();

      Course testCourse1 = new Course("English 101", "ENG101");
      testCourse1.Save();

      Course testCourse2 = new Course("Biology 101", "BIO101");
      testCourse2.Save();

      //Act
      testDepartment.AddCourse(testCourse1);
      List<Course> savedCourses = testDepartment.GetCourses();
      List<Course> testList = new List<Course> {testCourse1};

      //Assert
      Assert.Equal(testList, savedCourses);
    }

    [Fact]
    public void Delete_DeletesDepartmentAssociationsFromDatabase_DepartmentList()
    {
      //Arrange
      Course testCourse = new Course("English 101", "ENG101");
      testCourse.Save();

      string testDepartmentName = "English";
      Department testDepartment = new Department(testDepartmentName);
      testDepartment.Save();

      //Act
      testDepartment.AddCourse(testCourse);
      testDepartment.Delete();

      List<Department> resultCourseDepartments = testCourse.GetDepartments();
      List<Department> testCourseDepartments = new List<Department> {};

      //Assert
      Assert.Equal(testCourseDepartments, resultCourseDepartments);
    }

    [Fact]
    public void Test_AddStudent_AddsStudentToDepartment()
    {
      //Arrange
      Department testDepartment = new Department("English");
      testDepartment.Save();

      Student testStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent.Save();

      Student testStudent2 = new Student("Daisy", "Duke", new DateTime (2016, 04, 17));
      testStudent2.Save();

      //Act
      testDepartment.AddStudent(testStudent);
      testDepartment.AddStudent(testStudent2);

      List<Student> result = testDepartment.GetStudents();
      List<Student> testList = new List<Student>{testStudent, testStudent2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetStudents_ReturnsAllDepartmentStudents_StudentList()
    {
      //Arrange
      Department testDepartment = new Department("English");
      testDepartment.Save();

      Student testStudent1 = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent1.Save();

      Student testStudent2 = new Student("Daisy", "Duke", new DateTime (2016, 04, 17));
      testStudent2.Save();

      //Act
      testDepartment.AddStudent(testStudent1);
      List<Student> savedStudents = testDepartment.GetStudents();
      List<Student> testList = new List<Student> {testStudent1};

      //Assert
      Assert.Equal(testList, savedStudents);
    }

    public void Dispose()
    {
      Department.DeleteAll();
      Course.DeleteAll();
      Student.DeleteAll();
    }
  }
}
