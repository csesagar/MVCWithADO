using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCWithADO.Models
{
    public class StudentDAL
    {
        SqlCommand cmd;
        SqlConnection con;

        public StudentDAL()
        {
            string ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            con = new SqlConnection(ConStr);
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;

        }

        public List<Student> Student_Select(int? Sid, bool? Status)
        {
            List<Student> students = new List<Student>();
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "Student_Select";
                if (Sid != null && Status != null)
                {
                    cmd.Parameters.AddWithValue("@Sid", Sid);
                    cmd.Parameters.AddWithValue("@Status", Status);
                }
                else if(Sid != null && Status == null)
            cmd.Parameters.AddWithValue("@Sid", Sid);
                else if(Sid == null && Status != null)
            cmd.Parameters.AddWithValue("@Status", Status);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Student student = new Student
                    {
                        Sid = (int)dr["Sid"],
                        Name = (string)dr["Name"],
                        Class = (int)dr["Class"],
                        Fees = (decimal)dr["Fees"],
                        Photo = dr["Photo"] as string
                    };
                    students.Add(student);
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { con.Close(); }
            return students;
        }

        public int InsertStudent(Student student)
        {
            int Count = 0;
            try
            {
                cmd.CommandText = "Student_Insert";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Sid", student.Sid);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Fees", student.Fees);
                if (student.Photo != null && student.Photo.Length != 0)
                    cmd.Parameters.AddWithValue("@Photo", student.Photo);
                con.Open();
                Count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { con.Close(); }
            return Count;
        }

        public int UpdateStudent(Student student)
        {
            int Count = 0;
            try
            {
                cmd.CommandText = "Student_Update";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Sid", student.Sid);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Fees", student.Fees);
                if (student.Photo != null && student.Photo.Length != 0)
                    cmd.Parameters.AddWithValue("@Photo", student.Photo);
                con.Open();
                Count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { con.Close(); }
            return Count;
        }
        public int DeleteStudent(int Sid)
        {
            int Count = 0;
            try
            {
                cmd.CommandText = "Student_Delete";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Sid", Sid);
                con.Open();
                Count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { con.Close(); }
            return Count;
        }


    }
}