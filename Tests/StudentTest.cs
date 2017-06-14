using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
  [Collection("Registrar")]
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_StudentsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameStudent()
    {
      //Arrange, Act
      Student firstStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      Student secondStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));

      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }
    [Fact]
    public void Test_Save_SavesStudentToDatabase()
    {
      //Arrange
      Student testStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent.Save();

      //Act
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToStudentObject()
    {
      //Arrange
      Student testStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent.Save();

      //Act
      Student savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent.Save();

      //Act
      Student foundStudent = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, foundStudent);
    }
    [Fact]
    public void Delete_DeletesStudentFromDatabase_StudentList()
    {
      //Arrange
      string firstName1 = "Jane";
      string lastName1 = "Doe";
      DateTime enrollmentDate1 = new DateTime (2017, 04, 17);
      Student testStudent1 = new Student(firstName1, lastName1, enrollmentDate1);
      testStudent1.Save();

      string firstName2 = "John";
      string lastName2 = "Doe";
      DateTime enrollmentDate2 = new DateTime (2016, 04, 17);
      Student testStudent2 = new Student(firstName2, lastName2, enrollmentDate2);
      testStudent2.Save();

      //Act
      testStudent1.Delete();
      List<Student> resultStudents = Student.GetAll();
      List<Student> testStudentList = new List<Student> {testStudent2};

      //Assert
      Assert.Equal(testStudentList, resultStudents);
    }
    [Fact]
    public void Test_AddCourse_AddsCourseToStudent()
    {
      //Arrange
      Student testStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent.Save();

      Course testCourse = new Course("English 101", "ENG101");
      testCourse.Save();

      Course testCourse2 = new Course("Biology 101", "BIO101");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse);
      testStudent.AddCourse(testCourse2);

      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course>{testCourse, testCourse2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetCourses_ReturnsAllStudentCourses_CourseList()
    {
      //Arrange
      Student testStudent = new Student("John", "Doe", new DateTime (2017, 04, 17));
      testStudent.Save();

      Course testCourse1 = new Course("English 101", "ENG101");
      testCourse1.Save();

      Course testCourse2 = new Course("Biology 101", "BIO101");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse1);
      List<Course> savedCourses = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1};

      //Assert
      Assert.Equal(testList, savedCourses);
    }

    [Fact]
    public void Delete_DeletesStudentAssociationsFromDatabase_StudentList()
    {
      //Arrange
      Course testCourse = new Course("English 101", "ENG101");
      testCourse.Save();

      string testFirstName = "Daisy";
      string testLastName = "Duke";
      DateTime testEnrollmentDate = new DateTime(2016, 01, 01);
      Student testStudent = new Student(testFirstName, testLastName, testEnrollmentDate);
      testStudent.Save();

      //Act
      testStudent.AddCourse(testCourse);
      testStudent.Delete();

      List<Student> resultCourseStudents = testCourse.GetStudents();
      List<Student> testCourseStudents = new List<Student> {};

      //Assert
      Assert.Equal(testCourseStudents, resultCourseStudents);
    }

    public void Dispose()
    {
      Department.DeleteAll();
      Course.DeleteAll();
      Student.DeleteAll();
    }
  }
}
