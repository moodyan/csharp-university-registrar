using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
  [Collection("Registrar")]
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CoursesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameCourse()
    {
      //Arrange, Act
      Course firstCourse = new Course("English 101", "ENG101");
      Course secondCourse = new Course("English 101", "ENG101");

      //Assert
      Assert.Equal(firstCourse, secondCourse);
    }
    [Fact]
    public void Test_Save_SavesCourseToDatabase()
    {
      //Arrange
      Course testCourse = new Course("Biology 101", "BIO101");
      testCourse.Save();

      //Act
      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToCourseObject()
    {
      //Arrange
      Course testCourse = new Course("English 101", "ENG101");
      testCourse.Save();

      //Act
      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      //Arrange
      Course testCourse = new Course("Biology 101", "BIO101");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, foundCourse);
    }
    [Fact]
    public void Delete_DeletesCourseFromDatabase_CourseList()
    {
      //Arrange
      string courseName1 = "English 101";
      string courseNumber1 = "ENG101";
      Course testCourse1 = new Course(courseName1, courseNumber1);
      testCourse1.Save();

      string courseName2 = "Biology 101";
      string courseNumber2 = "BIO101";
      Course testCourse2 = new Course(courseName2, courseNumber2);
      testCourse2.Save();

      //Act
      testCourse1.Delete();
      List<Course> resultCourses = Course.GetAll();
      List<Course> testCourseList = new List<Course> {testCourse2};

      //Assert
      Assert.Equal(testCourseList, resultCourses);
    }
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
