using Aplikace.data.Entity;
using Aplikace.data.Enum;
using Aplikace.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
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
                byte[] photo = null;

                if (!employeeRow.IsNull("fotka"))
                {
                    photo = (byte[])employeeRow["fotka"];
                }



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
                
                
      
                var employee = new Employee(id, name, surname, hireDate,photo, role);

                return employee;
            }

            return null;
        }
        public void CreateAcount(Employee employee, User user) {
            InsertEmployee(employee);
            InsertUser(user);
        }
        private void InsertEmployee(Employee employee)
        {
            string formattedDate = employee.HireDate.ToString("yyyy-MM-dd");
            string insertProcedure = "InsertEmployee";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = employee.Name;
                    cmd.Parameters.Add("p_surname", OracleDbType.Varchar2).Value = employee.Surname;
                    cmd.Parameters.Add("p_hire_date", OracleDbType.Date).Value = employee.HireDate;
                    cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = (int)employee.Role;

                    if (employee.Photo != null)
                    {
                        OracleBlob photoBlob = new OracleBlob(databaseConnection.connection);

                       photoBlob.Write(employee.Photo, 0, employee.Photo.Length);

                        cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = photoBlob;
                    }
                    else
                    {
                        cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = DBNull.Value;
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }




        private void InsertUser(User user)
        {
            string insertProcedure = "InsertUser";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                int employeeId = GetLastEmployeeId();
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Value = employeeId;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = Security.Encrypt(user.Password);
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = user.Name;
                    
                    
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private int GetLastEmployeeId()
        {
            int lastEmployeeId = -1;

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("GetLastEmployeeId", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if (cmd.Parameters["p_employee_id"].Value != DBNull.Value)
                    {
                        int parsedId;
                        if (int.TryParse(cmd.Parameters["p_employee_id"].Value.ToString(), out parsedId))
                        {
                            lastEmployeeId = parsedId;
                        }
                    }
                }
            }

            return lastEmployeeId;
        }

        public List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string query = "SELECT * FROM adresa";
                var dataTable = connection.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {

                    int id = Convert.ToInt32(row["id"]);
                    string city = row["mesto"].ToString();
                    int postalCode = Convert.ToInt32(row["psc"]);
                    int streetNumber = Convert.ToInt32(row["cislo_popisne"]);
                    string country = row["stat"].ToString();
                    string street = row["ulice"].ToString();
                    var address = new Address(id, city, postalCode, streetNumber, country,street);

                    addresses.Add(address);
                }
            }
            return addresses;
        }
        public  List<Procedure> GetProcedures()
        {
            List<Procedure> Procedures = new List<Procedure>();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string query = "SELECT * FROM zakrok";
                var dataTable = connection.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {

                    int id = Convert.ToInt32(row["id"]);
                    string name = row["nazev"].ToString();
                    decimal price = Convert.ToDecimal(row["cena"]);
                    bool coveredByInsurance = Convert.ToBoolean(row["hradi_pojistovna"]);
                    string procedureSteps = row["postup"].ToString();
                    
                    var procedure = new Procedure(id,name,price,coveredByInsurance,procedureSteps  );

                    Procedures.Add(procedure);
                }
            }
            return Procedures;
        }
    }


}