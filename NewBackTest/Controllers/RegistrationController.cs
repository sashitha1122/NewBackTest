using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewBackTest.Models;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NewBackTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public string registration(Registration registration)
        {
            if (registration == null)
            {
                return "Registration object is null";
            }

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Employees").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO Registration(UseName,Password,IsActive) VALUES('" + registration.Username + "','" + registration.Password + "','" + registration.IsActive + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Data inserted";
            }
            else
            {
                return "error";
            }
        }

        [HttpPost]
        [Route("login")]
        public string login(Registration registration)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Employees").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Registration WHERE Password = '" + registration.Password + "' AND IsActive = '" + registration.IsActive + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return "Valid User";
            }
            else
            {
                return "Invalid User";
            }
        }
    }
}
