
## Step 1: Create the Student Model

Create a `Student.cs` file inside the `Models` folder.

```csharp
using System.ComponentModel.DataAnnotations;

namespace MvcAdo.Models
{
    public class Student
    {
        [Display(Name = "Student Id")]
        public int Sid { get; set; }

        public string Name { get; set; }

        public int? Class { get; set; }

        public decimal? Fees { get; set; }

        public string Photo { get; set; }
    }
}
```

**Explanation:**

- `Sid`: Student ID.
- `Name`: Student name.
- `Class`: Student's class.
- `Fees`: Student fees.
- `Photo`: Student photo path or filename.

The nullable properties `Class` and `Fees` can accept `null` values.

## Step 2: Configure the Database Connection

Add the following connection string to the `Web.config` file.

```xml
<connectionStrings>
    <add name="ConStr"
         connectionString="Data Source=YOUR_SERVER_NAME;Database=MVCDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Replace the server name, username, and password with your SQL Server credentials.

**Note:** Do not commit real database passwords to GitHub. Use environment-specific configuration and secure credentials.

## Step 3: Create the StudentDAL Class

Create `StudentDAL.cs` to manage database operations.

```csharp
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MvcAdo.Models;

public class StudentDAL
{
    SqlCommand cmd;
    SqlConnection con;

    public StudentDAL()
    {
        string ConStr =
            ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

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
            else if (Sid != null && Status == null)
            {
                cmd.Parameters.AddWithValue("@Sid", Sid);
            }
            else if (Sid == null && Status != null)
            {
                cmd.Parameters.AddWithValue("@Status", Status);
            }

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Student student = new Student
                {
                    Sid = (int)dr["Sid"],
                    Name = (string)dr["Name"],
                    Class = dr["Class"] == DBNull.Value
                        ? null : (int?)dr["Class"],
                    Fees = dr["Fees"] == DBNull.Value
                        ? null : (decimal?)dr["Fees"],
                    Photo = dr["Photo"] as string
                };

                students.Add(student);
            }

            dr.Close();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            con.Close();
        }

        return students;
    }
}
```

**Explanation:**

- `SqlConnection`: Establishes a connection to SQL Server.
- `SqlCommand`: Executes the stored procedure.
- `SqlDataReader`: Reads student records from the database.
- `Student_Select()`: Retrieves student records based on the supplied student ID and status filters.
- `List<Student>`: Stores the retrieved student records.

The code handles nullable `Class` and `Fees` values from the database.

## Step 4: Retrieve Students in the Controller

Create an action method inside your MVC controller.

```csharp
StudentDAL obj = new StudentDAL();

public ViewResult DisplayStudents()
{
    return View(obj.Student_Select(null, true));
}
```

**Explanation:**

- `StudentDAL obj`: Creates an instance of the data access class.
- `Student_Select(null, true)`: Retrieves records with the status set to `true`, without filtering by student ID.
- `View()`: Passes the list of students to the Razor View.

## Step 5: Create the Razor View

Create `DisplayStudents.cshtml` inside the appropriate controller's view folder.

```cshtml
@model IEnumerable<MvcAdo.Models.Student>

<h2>Student List</h2>

<table class="table table-bordered">
    <thead>
        <tr>
            <th>Student ID</th>
            <th>Name</th>
            <th>Class</th>
            <th>Fees</th>
            <th>Photo</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var student in Model)
        {
            <tr>
                <td>@student.Sid</td>
                <td>@student.Name</td>
                <td>@student.Class</td>
                <td>@student.Fees</td>
                <td>@student.Photo</td>
            </tr>
        }
    </tbody>
</table>
```

**Explanation:**

- `@model IEnumerable<Student>`: Declares that the view receives a collection of students.
- `foreach`: Iterates over the student records.
- `@student.PropertyName`: Displays each student's data in the table.

## Execution Flow

1. The user requests the `DisplayStudents` action.
2. The controller calls `StudentDAL`.
3. The DAL connects to SQL Server.
4. The `Student_Select` stored procedure executes.
5. `SqlDataReader` reads the result set.
6. The DAL converts database records into `Student` objects.
7. The controller passes the list to the Razor View.
8. The view displays the student records in an HTML table.


