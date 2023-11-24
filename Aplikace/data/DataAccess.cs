﻿using Aplikace.data.Entity;
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
        public List<Log> GetAllLogs()
        {
            return GetLogs();

        }
        public bool CreateAcount(Employee employee, User user)
        {
            return InsertEmployeeUser(employee, user);
        }
        public User LoginAcount(string username, string password)
        {
            return GetUserWithEmployee(username, password);
        }
        public List<User> GetAllUsers()
        {
            return GetUsers();
        }
        public bool InsertPatient(Patient patient, Address address, HealthCard healthCard)
        {
            return InsertPatientWithDetails(patient, healthCard, address);
        }


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


        // login -> kontrola jmena a hesla, vraceni uzivatele s zamestnancem
        public User GetUserWithEmployee(string username, string password)
        {
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "GetEmployeeAndUser";

                using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string uss = username;
                    string pass = Security.Encrypt(password);
                    // Vstupní parametry
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;

                    // Výstupní parametry
                    cmd.Parameters.Add("v_user_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_name", OracleDbType.Varchar2, 32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_surname", OracleDbType.Varchar2, 64).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_hire_date", OracleDbType.Date).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_role_id", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_employee_photo", OracleDbType.Blob).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("v_password", OracleDbType.Varchar2, 128).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();


                    int userId = Convert.ToInt32(cmd.Parameters["v_user_id"].Value.ToString());
                    int employeeId = Convert.ToInt32(cmd.Parameters["v_employee_id"].Value.ToString());
                    string employeeName = cmd.Parameters["v_employee_name"].Value.ToString();
                    string employeeSurname = cmd.Parameters["v_employee_surname"].Value.ToString();
                    DateTime hireDate = Convert.ToDateTime(cmd.Parameters["v_employee_hire_date"].Value.ToString());
                    int roleId = Convert.ToInt32(cmd.Parameters["v_employee_role_id"].Value.ToString());
                    byte[] photo = null;
                    OracleBlob blob = (OracleBlob)cmd.Parameters["v_employee_photo"].Value;

                    if (blob != null && blob.Length > 0)
                    {
                        photo = new byte[blob.Length];
                        blob.Read(photo, 0, (int)blob.Length);
                    }
                    string dbPassword = cmd.Parameters["v_password"].Value.ToString();

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
                    if (Security.Decrypt(dbPassword) == password)
                    {
                        Employee employee = new Employee(employeeId, employeeName, employeeSurname, hireDate, photo, role);
                        return new User(userId, username, password, employee);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
        }
        private List<User> GetUsers()
        {
            List<User> userList = new List<User>();

            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "GetAllUsers";

                using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Výstupní parametr pro kurzor
                    cmd.Parameters.Add("v_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["USER_ID"]);
                            string username = reader["USERNAME"].ToString();
                            int employeeId = Convert.ToInt32(reader["EMPLOYEE_ID"]);
                            string employeeName = reader["EMPLOYEE_NAME"].ToString();
                            string employeeSurname = reader["EMPLOYEE_SURNAME"].ToString();
                            DateTime hireDate = Convert.ToDateTime(reader["EMPLOYEE_HIRE_DATE"]);
                            int roleId = Convert.ToInt32(reader["EMPLOYEE_ROLE_ID"]);
                            byte[] photo = reader["EMPLOYEE_PHOTO"] as byte[];
                            string password = reader["PASSWORD"].ToString();
                            password = Security.Decrypt(password);
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

                            // Vytvoření a přidání User objektu do seznamu
                            Employee employee = new Employee(employeeId, employeeName, employeeSurname, hireDate, photo, role);
                            User user = new User(userId, username, password, employee);
                            userList.Add(user);
                        }
                    }
                }
            }

            return userList;
        }



        // registrace -> zapsani zamestnance a uzivatele do db, kontrola duplicit
        private bool InsertEmployeeUser(Employee employee, User user)
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

                    OracleBlob photoBlob = new OracleBlob(databaseConnection.connection);
                    if (employee.Photo != null)
                    {
                        photoBlob.Write(employee.Photo, 0, employee.Photo.Length);
                        cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = photoBlob;
                    }
                    else
                    {
                        cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = DBNull.Value;
                    }


                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = user.Name;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = Security.Encrypt(user.Password);

                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch (OracleException ex)
                    {
                        return false;
                    }
                    return true;

                }
            }
        }

        // zapsání Pacient, adresy a zdravotní karty do databáze
        private bool InsertPatientWithDetails(Patient patient, HealthCard healthCard, Address address)
        {
            string insertProcedure = "InsertPatientWithDetails";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = patient.FirstName;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = patient.LastName;
                    cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = patient.SocialSecurityNumber;
                    cmd.Parameters.Add("p_pohlavi", OracleDbType.Varchar2).Value = patient.Gender;
                    cmd.Parameters.Add("p_narozeni", OracleDbType.Date).Value = patient.DateOfBirth;
                    cmd.Parameters.Add("p_telefon", OracleDbType.Decimal).Value = patient.Phone;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = patient.Email;

                    cmd.Parameters.Add("p_kouri", OracleDbType.Char).Value = healthCard.Smokes;
                    cmd.Parameters.Add("p_tehotenstvi", OracleDbType.Char).Value = healthCard.Pregnancy;
                    cmd.Parameters.Add("p_alkohol", OracleDbType.Char).Value = healthCard.Alcohol;
                    cmd.Parameters.Add("p_sport", OracleDbType.Varchar2).Value = healthCard.Sport;
                    cmd.Parameters.Add("p_plomby", OracleDbType.Decimal).Value = healthCard.Fillings;
                    cmd.Parameters.Add("p_anamnezy_id", OracleDbType.Decimal).Value = (int)healthCard.Anamnesis;

                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = address.City;
                    cmd.Parameters.Add("p_psc", OracleDbType.Decimal).Value = address.PostalCode;
                    cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Decimal).Value = address.StreetNumber;
                    cmd.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = address.Country;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = address.Street;
                    cmd.Parameters.Add("p_pojistovny_id", OracleDbType.Decimal).Value = (int)patient.InsuranceCompany;

                    // Execute the procedure
                    //try
                    //{
                        cmd.ExecuteNonQuery();
                    //}
                    //catch (OracleException ex)
                    //{
                    //    // Handle exception if needed
                    //    return false;
                    //}

                    return true;
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
        

      
        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "GetAllPatients";

                using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Výstupní parametr pro kurzor
                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            int ID = reader["PATIENT_ID"] != DBNull.Value ? Convert.ToInt32(reader["PATIENT_ID"]) : 0;
                            string FirstName = reader["PATIENT_NAME"].ToString();
                            string LastName = reader["PATIENT_SURNAME"].ToString();
                            string IDNumber =  reader["PATIENT_ID_NUMBER"].ToString();
                            string Gender = reader["GENDER"].ToString();
                            DateTime BirthDate = reader["BIRTH_DATE"] != DBNull.Value ? Convert.ToDateTime(reader["BIRTH_DATE"]) : DateTime.MinValue;
                            string Phone = reader["PHONE"].ToString();
                            string Email = reader["EMAIL"].ToString();
                            string City = reader["CITY"].ToString();
                            string Street = reader["STREET"].ToString();
                            int StreetNumber = reader["STREET_NUMBER"] != DBNull.Value ? Convert.ToInt32(reader["STREET_NUMBER"]) : 0;
                            int ZipCode = reader["ZIP_CODE"] != DBNull.Value ? Convert.ToInt32(reader["ZIP_CODE"]) : 0;
                            string Country = reader["COUNTRY"].ToString();
                            string InsuranceName = reader["INSURANCE_NAME"].ToString();
                            int HealthCardID = reader["HEALTH_CARD_ID"] != DBNull.Value ? Convert.ToInt32(reader["HEALTH_CARD_ID"]) : 0;
                            int AddressID = reader["ADDRESS_ID"] != DBNull.Value ? Convert.ToInt32(reader["ADDRESS_ID"]) : 0;
                            int InsuranceID = reader["INSURANCE_ID"] != DBNull.Value ? Convert.ToInt32(reader["INSURANCE_ID"]) : 0;
                            bool Smokes =  ConvertDbCharToBool(reader["SMOKES"].ToString());
                            bool Pregnancy = ConvertDbCharToBool(reader["PREGNANCY"].ToString());
                            bool Alcohol = ConvertDbCharToBool(reader["ALCOHOL"].ToString());
                            string Sport = reader["SPORT"].ToString();
                            int Fillings = reader["FILLINGS"] != DBNull.Value ? Convert.ToInt32(reader["FILLINGS"]) : 0;
                            int anamnesisID = reader["ANAMNESIS_ID"] != DBNull.Value ? Convert.ToInt32(reader["ANAMNESIS_ID"]) : 0;
                            Anamnesis an = (Anamnesis)anamnesisID;
                            InsuranceCompany company = (InsuranceCompany)InsuranceID;

                            Address address = new Address(AddressID, City, ZipCode, StreetNumber, Country, Street);
                            HealthCard healthCard = new HealthCard(HealthCardID, Smokes, Pregnancy, Alcohol, Sport, Fillings, an);
                            Patient patient = new Patient(ID, FirstName, LastName, IDNumber, Gender, BirthDate, Phone, Email, address, healthCard, company);
                            patients.Add(patient);
                        }
                    }
                }
            }

            return patients;
        }

        
        // metody pro prevadeni databazovych bool na boolean v c#
        private string ConvertBoolToDbChar(bool value)
        {
            return value ? "Y" : "N";
        }

        private bool ConvertDbCharToBool(string value)
        {
            return value == "Y";
        }
        // 
        private List<Log> GetLogs()
        {
            List<Log> logs = new List<Log>();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string query = "SELECT * FROM LOG_ZMEN";
                var dataTable = connection.ExecuteQuery(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    ChangeType changeType = ChangeType.Insert;
                    int id = Convert.ToInt32(row["ID"]);
                    string tableName = row["NAZEV_TABULKY"].ToString();
                    string s_changeType = row["TYP_ZMENY"].ToString();
                    DateTime changeTime = Convert.ToDateTime(row["CAS_ZMENY"]);
                    switch (s_changeType)
                    {
                        case "INSERT":
                            changeType = ChangeType.Insert;
                            break;
                        case "UPDATE":
                            changeType = ChangeType.Update;
                            break;
                        case "DELETE":
                            changeType = ChangeType.Delete;
                            break;
                        default: break;
                    }

                    var Log = new Log(id, tableName, changeType, changeTime);

                    logs.Add(Log);
                }
            }
            return logs;

        }
    }
}