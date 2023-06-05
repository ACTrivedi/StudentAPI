using Microsoft.Data.SqlClient;
using StudentAPI.Models;

namespace StudentAPI.DAL
{
    public class Student_DAL
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;

        public IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefualtConnection");
        }

        public List<Student> GetAllStudents()
        { 
            List<Student> studentList = new List<Student>();
            using (_connection = new SqlConnection(GetConnectionString()))
            { 
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Get_Students]";
                _connection.Open();

                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read())
                { 
                    Student student = new Student();
                    student.StudentId = Convert.ToInt32(dr["StudentId"]);
                    student.Name = dr["Name"].ToString();
                    student.ContactNumber = Convert.ToInt64(dr["ContactNumber"]);
                    student.Age = Convert.ToInt32(dr["Age"]);
                    studentList.Add(student);
                }

                _connection.Close();

            }
            return studentList;
        
        }


        public bool Insert(Student model)
        {
            int id = 0;
           /* List<Student> studentList = new List<Student>();*/
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Insert_Students]";
                _command.Parameters.AddWithValue("@StudentId", model.StudentId);
                _command.Parameters.AddWithValue("@Name", model.Name);
                _command.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                _command.Parameters.AddWithValue("@Age", model.Age);

                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();               

            }
            return id > 0 ? true:false;

        }

    }
}
