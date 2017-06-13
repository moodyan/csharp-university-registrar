using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Registrar;

namespace Registrar.Objects
{
  public class Student
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private DateTime _enrollmentDate;

    public Student(string FirstName, string LastName, DateTime EnrollmentDate, int Id = 0)
    {
      _id = Id;
      _firstName = FirstName;
      _lastName = LastName;
      _enrollmentDate = EnrollmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool firstNameEquality = this.GetFirstName() == newStudent.GetFirstName();
        bool lastNameEquality = this.GetLastName() == newStudent.GetLastName();
        bool enrollmentDateEquality = this.GetEnrollmentDate() == newStudent.GetEnrollmentDate();
        return (idEquality && firstNameEquality && lastNameEquality && enrollmentDateEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetFirstName()
    {
      return _firstName;
    }
    public void SetFirstName(string newFirstName)
    {
      _firstName = newFirstName;
    }
    public string GetLastName()
    {
      return _lastName;
    }
    public void SetLastName(string newLastName)
    {
      _lastName = newLastName;
    }
    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }
    public void SetEnrollmentDate(DateTime newEnrollmentDate)
    {
      _enrollmentDate = newEnrollmentDate;
    }
    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentFirstName = rdr.GetString(1);
        string studentLastName = rdr.GetString(2);
        DateTime studentEnrollmentDate = rdr.GetDateTime(3);
        Student newStudent = new Student(studentFirstName, studentLastName, studentEnrollmentDate, studentId);
        allStudents.Add(newStudent);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allStudents;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (first_name, last_name, enrollment_date) OUTPUT INSERTED.id VALUES (@StudentFirstName, @StudentLastName, @StudentEnrollmentDate);", conn);

      SqlParameter firstNameParameter = new SqlParameter();
      firstNameParameter.ParameterName = "@StudentFirstName";
      firstNameParameter.Value = this.GetFirstName();
      cmd.Parameters.Add(firstNameParameter);

      SqlParameter lastNameParameter = new SqlParameter();
      lastNameParameter.ParameterName = "@StudentLastName";
      lastNameParameter.Value = this.GetLastName();
      cmd.Parameters.Add(lastNameParameter);

      SqlParameter enrollmentDateParameter = new SqlParameter();
      enrollmentDateParameter.ParameterName = "@StudentEnrollmentDate";
      enrollmentDateParameter.Value = this.GetEnrollmentDate();
      cmd.Parameters.Add(enrollmentDateParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundFirstName = null;
      string foundLastName = null;
      DateTime foundEnrollmentDate = default(DateTime);

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundFirstName = rdr.GetString(1);
        foundLastName = rdr.GetString(2);
        foundEnrollmentDate = rdr.GetDateTime(3);
      }
      Student foundStudent = new Student(foundFirstName, foundLastName, foundEnrollmentDate, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM students_courses WHERE students_id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
