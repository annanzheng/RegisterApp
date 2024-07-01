using System.Web.Mvc;
using RegisterApp.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace RegisterApp.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Students (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Age", student.Age);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Index
        public ActionResult Index()
        {
            List<Student> students = new List<Student>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Student student = new Student
                    {
                        StudentId = (int)reader["StudentId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Age = (int)reader["Age"]
                    };
                    students.Add(student);
                }
                connection.Close();
            }

            return View(students);
        }

        // GET: Student/Search
        public ActionResult Search(string name)
        {
            List<Student> students = new List<Student>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "EXEC GetStudentInformation @StudentName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentName", name);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Student student = new Student
                    {
                        StudentId = (int)reader["StudentId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Age = (int)reader["Age"]
                    };
                    students.Add(student);
                }
                connection.Close();
            }

            return View(students);
        }
    }
}
