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

    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
