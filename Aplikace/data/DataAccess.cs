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

        public void CreateAcount(Employee employee, User user)
        {
            InsertEmployeeUser(employee, user);
        }

        // načte všechny uzivatele z db a uloží je do listu
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
        // načte všechny zaměstnance z db a uloží je do listu
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string query = "SELECT * FROM zamestnanec";
                var dataTable = connection.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string name = row["jmeno"].ToString();
                    string surname = row["prijmeni"].ToString();
                    DateTime hireDate = Convert.ToDateTime(row["nastup"]);
                    int roleId = Convert.ToInt32(row["role_id"]);
                    byte[] photo = null;

                    if (!row.IsNull("fotka"))
                    {
                        photo = (byte[])row["fotka"];
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



                    var employee = new Employee(id, name, surname, hireDate, photo, role);
                    employees.Add(employee);
                }
            }
            return employees;
        }
        // nacte zamestnance na zaklade jeho id
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



                var employee = new Employee(id, name, surname, hireDate, photo, role);

                return employee;
            }

            return null;
        }
        public User GetUserWithEmployee(string username, string password)
        {
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "GetEmployeeAndUser";

                using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Vstupní parametry
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = Security.Encrypt(password);

                    // Výstupní parametry
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_name", OracleDbType.Varchar2, 64).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_surname", OracleDbType.Varchar2, 64).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_hire_date", OracleDbType.Date).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_role_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_photo", OracleDbType.Blob).Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        // Zde můžete ošetřit specifickou chybu ORA-01403
                        if (ex.Number == 1403)
                        {
                            return null; // Pokud nenalezen žádný uživatel, vrátíme null
                        }
                        else
                        {
                            throw; // Pokud je to jiná chyba, vrátíme ji dál
                        }
                    }

                    int userId = Convert.ToInt32(cmd.Parameters["v_user_id"].Value);

                    
                        int employeeId = Convert.ToInt32(cmd.Parameters["v_employee_id"].Value);
                        string employeeName = cmd.Parameters["v_employee_name"].Value.ToString();
                        string employeeSurname = cmd.Parameters["v_employee_surname"].Value.ToString();
                        DateTime hireDate = Convert.ToDateTime(cmd.Parameters["v_employee_hire_date"].Value);
                        int roleId = Convert.ToInt32(cmd.Parameters["v_employee_role_id"].Value);
                        byte[] photo = (byte[])cmd.Parameters["v_employee_photo"].Value;

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

                        Employee employee = new Employee(employeeId, employeeName, employeeSurname, hireDate, photo, role);
                        return new User(userId, username, password, employee);
                   
                }
            }
        }


        // vloží zaměstnance a uživatele do db
        //private void InsertEmployee(Employee employee)
        //{
        //    string formattedDate = employee.HireDate.ToString("yyyy-MM-dd");
        //    string insertProcedure = "InsertEmployee";

        //    using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
        //    {
        //        databaseConnection.OpenConnection();

        //        using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;


        //            cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = employee.Name;
        //            cmd.Parameters.Add("p_surname", OracleDbType.Varchar2).Value = employee.Surname;
        //            cmd.Parameters.Add("p_hire_date", OracleDbType.Date).Value = employee.HireDate;
        //            cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = (int)employee.Role;

        //            if (employee.Photo != null)
        //            {
        //                OracleBlob photoBlob = new OracleBlob(databaseConnection.connection);

        //                photoBlob.Write(employee.Photo, 0, employee.Photo.Length);

        //                cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = photoBlob;
        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = DBNull.Value;
        //            }

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
        //private void InsertUser(User user)
        //{
        //    string insertProcedure = "InsertUser";

        //    using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
        //    {
        //        int employeeId = GetLastEmployeeId();
        //        databaseConnection.OpenConnection();

        //        using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Value = employeeId;
        //            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = Security.Encrypt(user.Password);
        //            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = user.Name;




        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
        //private int GetLastEmployeeId()
        //{
        //    int lastEmployeeId = -1;

        //    using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
        //    {
        //        databaseConnection.OpenConnection();

        //        using (OracleCommand cmd = new OracleCommand("GetLastEmployeeId", databaseConnection.connection))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add("p_employee_id", OracleDbType.Int32).Direction = ParameterDirection.Output;

        //            cmd.ExecuteNonQuery();

        //            if (cmd.Parameters["p_employee_id"].Value != DBNull.Value)
        //            {
        //                int parsedId;
        //                if (int.TryParse(cmd.Parameters["p_employee_id"].Value.ToString(), out parsedId))
        //                {
        //                    lastEmployeeId = parsedId;
        //                }
        //            }
        //        }
        //    }

        //    return lastEmployeeId;
        //}
        private void InsertEmployeeUser(Employee employee, User user)
        {
            string insertProcedure = "Register";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = employee.Name;
                    cmd.Parameters.Add("p_surname", OracleDbType.Varchar2).Value = employee.Surname;
                    cmd.Parameters.Add("p_nastup", OracleDbType.Date).Value = employee.HireDate;
                    cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = (int)employee.Role;

                    // Convert byte[] to OracleBlob
                    OracleBlob photoBlob = new OracleBlob(databaseConnection.connection);
                    if (employee.Photo != null)
                    {
                        photoBlob.Write(employee.Photo, 0, employee.Photo.Length);
                    }
                    else
                    {
                        // If no photo, set to NULL
                        cmd.Parameters["p_photo"].Value = DBNull.Value;
                    }
                    cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = photoBlob;

                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = user.Name;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = Security.Encrypt(user.Password);

                    cmd.ExecuteNonQuery();
                }
            }
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
                    var address = new Address(id, city, postalCode, streetNumber, country, street);

                    addresses.Add(address);
                }
            }
            return addresses;
        }
        //načte z db zákroky a uloží je do listu
        public List<Procedure> GetProcedures()
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

                    var procedure = new Procedure(id, name, price, coveredByInsurance, procedureSteps);

                    Procedures.Add(procedure);
                }
            }
            return Procedures;
        }
        private void InsertProcedure(Procedure procedure)
        {


        }

        //TODO načtení ostatních tabulek a uložení do listů. 
        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string query = "SELECT p.ID, p.JMENO, p.PRIJMENI, p.RODNE_CISLO, p.POHLAVI, p.NAROZENI, p.TELEFON, p.EMAIL, p.ADRESY_ID, p.POJISTOVNY_ID FROM ST67060.PACIENT p";
                var dataTable = connection.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    
                    int id = Convert.ToInt32(row["ID"]);
                    string firstName = row["JMENO"].ToString();
                    string lastName = row["PRIJMENI"].ToString();
                    string socialSecurityNumber = row["RODNE_CISLO"].ToString();
                    string gender = row["POHLAVI"].ToString();
                    DateTime dateOfBirth = Convert.ToDateTime(row["NAROZENI"]);
                    string phone = row["TELEFON"].ToString();
                    string email = row["EMAIL"].ToString();

                    
                    Address address = GetAddressById(Convert.ToInt32(row["ADRESY_ID"]));
                    InsuranceCompany insuranceCompany = InsuranceCompanyExtensions.GetInsuranceCompanyById(Convert.ToInt32(row["POJISTOVNY_ID"]));

                    
                    var patient = new Patient(id, firstName, lastName, socialSecurityNumber, gender, dateOfBirth, phone, email, address, insuranceCompany);
                    patients.Add(patient);
                }
            }
            return patients;
        }
        public Address GetAddressById(int id)
        {
            using (var connection = new OracleDatabaseConnection())
            {
                Address address = null;
                connection.OpenConnection();

                string query = $"SELECT ID, MESTO, PSC, CISLO_POPISNE, STAT, ULICE FROM ST67060.ADRESA WHERE ID = {id}";
                var dataTable = connection.ExecuteQuery(query);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    address = new Address(
                        Convert.ToInt32(row["ID"]),
                        row["MESTO"].ToString(),
                        Convert.ToInt32(row["PSC"]),
                        Convert.ToInt32(row["CISLO_POPISNE"]),
                        row["STAT"].ToString(),
                        row["ULICE"].ToString()
                    );
                }

                connection.CloseConnection();
                return address;
            }
        }
        

    }
}