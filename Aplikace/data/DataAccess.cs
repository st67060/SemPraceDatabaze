using Aplikace.data.Entity;
using Aplikace.data.Enum;
using Aplikace.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aplikace.data
{
    internal class DataAccess
    {
        private OracleDatabaseConnection databaseConnection;

        public DataAccess()
        {
            databaseConnection = new OracleDatabaseConnection();
        }
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string query = "SELECT * FROM uzivatel";
                var dataTable = connection.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    int zamestnanecId = Convert.ToInt32(row["zamestnanec_id"]);
                    Employee emplyee = GetEmployeeById(zamestnanecId, connection);
                    int id = Convert.ToInt32(row["id"]);
                    string name = row["jmeno"].ToString();
                    string password = Security.Decrypt(row["heslo"].ToString());
                    var user = new User(id, name, password, emplyee);

                    users.Add(user);
                }
            }
            return users;
        }
        private Employee GetEmployeeById(int employeeId, OracleDatabaseConnection connection)
        {
            string employeeQuery = "SELECT * FROM zamestnanec WHERE id = :employeeId";
            var parameter = new OracleParameter("employeeId", employeeId);
            var employeeDataTable = connection.ExecuteQuery(employeeQuery, new[] { parameter });

            if (employeeDataTable.Rows.Count > 0)
            {
                var employeeRow = employeeDataTable.Rows[0];
                int id = Convert.ToInt32(employeeRow["id"]);
                string name = employeeRow["jmeno"].ToString();
                string surname = employeeRow["prijmeni"].ToString();
                DateTime hireDate = Convert.ToDateTime(employeeRow["nastup"]);
                int roleId = Convert.ToInt32(employeeRow["role_id"]);
                Role role;
                switch (roleId)
                {
                    case 1:
                        role = Role.Admin;
                        break;
                    case 2:
                        role = Role.Doctor;
                        break;
                    case 3:
                        role = Role.Nurse;
                        break;
                    case 4:
                        role = Role.Employee;
                        break;
                    default:
                        role = Role.Employee;
                        break;
                }

                var employee = new Employee(id, name, surname, hireDate, role);

                return employee;
            }

            return null;
        }
        public void InsertEmployee(Employee employee)
        {
            string formattedDate = employee.HireDate.ToString("yyyy-MM-dd");
            string insertQuery = "INSERT INTO zamestnanec (jmeno, prijmeni, nastup, role_id, fotka) VALUES (:name, :surname, TO_DATE(:hireDate, 'YYYY-MM-DD'), :roleId, :photo)";

            OracleParameter[] parameters = new OracleParameter[]
            {
    new OracleParameter("name", employee.Name),
    new OracleParameter("surname", employee.Surname),
    new OracleParameter("hireDate", formattedDate),
    new OracleParameter("roleId", (int)employee.Role),
     new OracleParameter("photo", OracleDbType.Blob, ParameterDirection.Input) { Value = employee.Photo }
            };

            databaseConnection.ExecuteQuery(insertQuery, parameters);
        }

        public void InsertUser(User user)
        {
            int id = GetLastEmployeeId();
            string encryptedPassword = Security.Encrypt(user.Password);
            string insertQuery = "INSERT INTO uzivatel (zamestnanec_id, heslo, jmeno) VALUES (:employeeId, :password, :name)";

            OracleParameter[] parameters = new OracleParameter[]
            {
            new OracleParameter("employeeId", id),
            new OracleParameter("password", encryptedPassword),
            new OracleParameter("name", user.Name)
            };

            databaseConnection.ExecuteQuery(insertQuery, parameters);
        }
        public int GetLastEmployeeId()
        {
            string query = "SELECT MAX(id) FROM zamestnanec";
            var result = databaseConnection.ExecuteQuery(query);

            if (result.Rows.Count > 0)
            {
                var maxId = result.Rows[0][0];
                if (maxId != DBNull.Value)
                {
                    return Convert.ToInt32(maxId);
                }
            }

            return -1;
        }
    }


}