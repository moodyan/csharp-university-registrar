using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Registrar;

namespace Registrar.Objects
{
  public class Department
  {
    private int _id;
    private string _departmentName;

    public Department(string DepartmentName, int Id = 0)
    {
      _id = Id;
      _departmentName = DepartmentName;
    }

    public override bool Equals(System.Object otherDepartment)
    {
      if (!(otherDepartment is Department))
      {
        return false;
      }
      else
      {
        Department newDepartment = (Department) otherDepartment;
        bool idEquality = this.GetId() == newDepartment.GetId();
        bool departmentNameEquality = this.GetDepartmentName() == newDepartment.GetDepartmentName();
        return (idEquality && departmentNameEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetDepartmentName()
    {
      return _departmentName;
    }
    public void SetDepartmentName(string newDepartmentName)
    {
      _departmentName = newDepartmentName;
    }
    public static List<Department> GetAll()
    {
      List<Department> allDepartments = new List<Department>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments ORDER BY department_name;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int departmentId = rdr.GetInt32(0);
        string departmentName = rdr.GetString(1);
        Department newDepartment = new Department(departmentName, departmentId);
        allDepartments.Add(newDepartment);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allDepartments;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments (department_name) OUTPUT INSERTED.id VALUES (@DepartmentName);", conn);

      SqlParameter departmentNameParameter = new SqlParameter();
      departmentNameParameter.ParameterName = "@DepartmentName";
      departmentNameParameter.Value = this.GetDepartmentName();
      cmd.Parameters.Add(departmentNameParameter);

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
    public static Department Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(departmentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundDepartmentId = 0;
      string foundDepartmentName = null;

      while(rdr.Read())
      {
        foundDepartmentId = rdr.GetInt32(0);
        foundDepartmentName = rdr.GetString(1);
      }
      Department foundDepartment = new Department(foundDepartmentName, foundDepartmentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundDepartment;
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM departments WHERE id = @DepartmentId; DELETE FROM departments_courses WHERE departments_id = @DepartmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(departmentIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddCourse(Course newCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments_courses (departments_id, courses_id) VALUES (@DepartmentId, @CourseId);", conn);
      SqlParameter DepartmentIdParameter = new SqlParameter();
      DepartmentIdParameter.ParameterName = "@DepartmentId";
      DepartmentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(DepartmentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = newCourse.GetId();
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses_id FROM departments_courses WHERE departments_id = @DepartmentId;", conn);

      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@DepartmentId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> courseIds = new List<int> {};

      while (rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        courseIds.Add(courseId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Course> courses = new List<Course> {};

      foreach (int courseId in courseIds)
      {
        SqlCommand courseQuery = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);

        SqlParameter courseIdParameter = new SqlParameter();
        courseIdParameter.ParameterName = "@CourseId";
        courseIdParameter.Value = courseId;
        courseQuery.Parameters.Add(courseIdParameter);

        SqlDataReader queryReader = courseQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisCourseId = queryReader.GetInt32(0);
          string courseName = queryReader.GetString(1);
          string courseNumber = queryReader.GetString(2);
          Course foundCourse = new Course(courseName, courseNumber, thisCourseId);
          courses.Add(foundCourse);
        }
        if (queryReader != null)
        {
          queryReader.Close();
        }
      }
      if (conn != null)
      {
        conn.Close();
      }
      return courses;
    }
    // public void UpdateDepartmentName(string newDepartmentName)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand ("UPDATE departments SET department_name = @NewDepartmentName OUTPUT INSERTED.department_name WHERE id = @DepartmentId;", conn);
    //
    //   cmd.Parameters.AddWithValue("@NewDepartmentName", newDepartmentName);
    //   cmd.Parameters.AddWithValue("@DepartmentId", _id);
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     this._departmentName = rdr.GetString(0);
    //   }
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
