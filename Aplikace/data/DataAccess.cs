using Aplikace.data.Entity;
using Aplikace.data.Enum;
using Aplikace.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Xps;
using System.Xml.Linq;
using static Aplikace.data.DataAccess;

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
            return GetLogs().Result;

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
        public bool InsertPatient(Patient patient)
        {
            return InsertPatientWithDetails(patient);
        }
        public bool DeletePatient(Patient patient)
        {
            return DeletePatientWithDetails(patient);

        }
        public bool UpdatePatient(Patient patient)
        {
            return UpdatePatientWithDetails(patient);
        }
        public List<Reservation> GetReservations()
        {

            return GetAllReservations().Result;
        }
        public class ReservationProcedureLink
        {
            public int ReservationId { get; set; }
            public int ProcedureId { get; set; }

            public ReservationProcedureLink(int reservationId, int procedureId)
            {
                ReservationId = reservationId;
                ProcedureId = procedureId;
            }

        }
        public class AlergyHealthCardLink
        {
            public int AlergyId { get; set; }
            public int HealthCardId { get; set; }

            public AlergyHealthCardLink(int alergyId, int alergyHealthCardId)
            {
                HealthCardId = alergyHealthCardId;
                AlergyId = alergyId;
            }

        }

        // ==============================================
        // Metody pro Quest
        // ==============================================

        public Task<Employee> GetSuperior()
        {
            return Task.Run(() =>
            {
                Employee employee = null;

                using (var connection = new OracleDatabaseConnection())
                {
                    connection.OpenConnection();

                    using (OracleCommand cmd = new OracleCommand("SELECT_EMPLOYEES_LEVEL_1", connection.connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Výstupní parametr
                        cmd.Parameters.Add("v_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        // Vykonání příkazu
                        cmd.ExecuteNonQuery();

                        // Čtení dat ze vstupního kurzoru
                        OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["v_cursor"].Value).GetDataReader();

                        reader.Read();

                        string employeeName = reader["v_employee_name"].ToString();
                        string employeeSurname = reader["v_employee_surname"].ToString();
                        int roleId = Convert.ToInt32(reader["v_employee_role_id"]);

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

                        employee = new Employee(0, employeeName, employeeSurname, DateTime.Now, role);

                    }
                }

                return employee;
            });
        }

        // nacteni zaměstnanců pro listView

        public Task<List<Employee>> GetEmployees()
        {
            return Task.Run(() =>
            {
                List<Employee> employees = new List<Employee>();
                Dictionary<int, int?> superiorMapping = new Dictionary<int, int?>();

                using (var connection = new OracleDatabaseConnection())
                {
                    connection.OpenConnection();

                    using (OracleCommand cmd = new OracleCommand("SELECT_ZAMESTNANCI", connection.connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Definice výstupních parametrů pro proceduru
                        cmd.Parameters.Add("v_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        // Spuštění procedury
                        cmd.ExecuteNonQuery();

                        // Čtení výsledků z kurzoru
                        OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["v_cursor"].Value).GetDataReader();

                        while (reader.Read())
                        {
                            int employeeId = Convert.ToInt32(reader["v_employee_id"]);
                            string employeeName = reader["v_employee_name"].ToString();
                            string employeeSurname = reader["v_employee_surname"].ToString();
                            DateTime hireDate = Convert.ToDateTime(reader["v_employee_hire_date"]);
                            int roleId = Convert.ToInt32(reader["v_employee_role_id"]);
                            byte[] photo = reader["v_employee_photo"] != DBNull.Value ? (byte[])reader["v_employee_photo"] : null;

                            int? superiorId = reader["v_employee_employee"] != DBNull.Value ? (int?)Convert.ToInt32(reader["v_employee_employee"]) : null;

                            if (superiorId != null)
                            {
                                superiorMapping.Add(employeeId, superiorId);
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

                            var employee = new Employee(employeeId, employeeName, employeeSurname, hireDate, photo, role);
                            employees.Add(employee);
                        }
                    }

                    // Přiřazení nadřízených zaměstnanců
                    foreach (var employee in employees)
                    {
                        if (superiorMapping.ContainsKey(employee.Id))
                        {
                            int? superiorId = superiorMapping[employee.Id];
                            employee.Superior = superiorId.HasValue
                                ? employees.FirstOrDefault(e => e.Id == superiorId)
                                : null;
                        }
                    }
                }

                return employees;
            });
        }



        // ==============================================
        // Metody pro příhlášení a odhlášení
        // ==============================================

        // login -> kontrola jmena a hesla, vraceni uzivatele s zamestnancem
        public User GetUserWithEmployee(string username, string password)
        {
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "SELECT_ZAMESTNANCE_UZIVATELE";

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
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        return null;
                    }



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


        // ==============================================
        // Metody pro Doctor,Nurse atd
        // ==============================================

        public async Task<List<Patient>> GetAllPatients()
        {
            return await Task.Run(() =>
            {
                List<Patient> patients = new List<Patient>();
                List<Alergy> alergies = GetAllAllergies();
                List<AlergyHealthCardLink> alergyHealthCardLinks = GetAllAlergyHealthCardLinks();
                List<Anamnesis> anamneses = GetAllAnamnesis();
                List<Insurance> insurances = GetAllInsurances();

                using (var connection = new OracleDatabaseConnection())
                {
                    connection.OpenConnection();
                    string procedureName = "SELECT_PACIENTI";

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
                                long IDNumber = reader["PATIENT_ID_NUMBER"] != DBNull.Value ? (long)Convert.ToDouble(reader["PATIENT_ID_NUMBER"]) : 0;
                                string Gender = reader["GENDER"].ToString();
                                DateTime BirthDate = reader["BIRTH_DATE"] != DBNull.Value ? Convert.ToDateTime(reader["BIRTH_DATE"]) : DateTime.MinValue;
                                long Phone = reader["PHONE"] != DBNull.Value ? (long)Convert.ToDouble(reader["PHONE"]) : 0;
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
                                bool Smokes = ConvertDbCharToBool(reader["SMOKES"].ToString());
                                bool Pregnancy = ConvertDbCharToBool(reader["PREGNANCY"].ToString());
                                bool Alcohol = ConvertDbCharToBool(reader["ALCOHOL"].ToString());
                                string Sport = reader["SPORT"].ToString();
                                int Fillings = reader["FILLINGS"] != DBNull.Value ? Convert.ToInt32(reader["FILLINGS"]) : 0;
                                string anamnesisName = reader["ANAMNESIS_NAME"].ToString();

                                Insurance insurance = insurances.FirstOrDefault(insurance => insurance.Id == InsuranceID);
                                Anamnesis anamnesis = anamneses.FirstOrDefault(anamnesis => anamnesis.Name == anamnesisName);
                                Address address = new Address(AddressID, City, ZipCode, StreetNumber, Country, Street);
                                HealthCard healthCard = new HealthCard(HealthCardID, Smokes, Pregnancy, Alcohol, Sport, Fillings, anamnesis);
                                Patient patient = new Patient(ID, FirstName, LastName, IDNumber, Gender, BirthDate, Phone, Email, address, healthCard, insurance);
                                var linkedAlergies = alergyHealthCardLinks.Where(link => link.HealthCardId == HealthCardID)
                                                        .Select(link => alergies.FirstOrDefault(a => a.Id == link.AlergyId))
                                                        .ToList();
                                patient.HealthCard.Alergies.Clear();
                                foreach (var alergy in linkedAlergies)
                                {
                                    patient.HealthCard.Alergies.Add(alergy);
                                }


                                patients.Add(patient);


                            }
                        }
                    }
                }

                return patients;
            });

        }
        // zapsání Pacient, adresy a zdravotní karty do databáze
        private bool InsertPatientWithDetails(Patient patient)
        {
            string insertProcedure = "INSERT_PACIENTI_S_PODROBNOSTMI";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = patient.FirstName;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = patient.LastName;
                    cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.SocialSecurityNumber);
                    cmd.Parameters.Add("p_pohlavi", OracleDbType.Varchar2).Value = patient.Gender;
                    cmd.Parameters.Add("p_narozeni", OracleDbType.Date).Value = patient.DateOfBirth;
                    cmd.Parameters.Add("p_telefon", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Phone);
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = patient.Email;
                    cmd.Parameters.Add("p_pojistovny_id", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.InsuranceCompany.Id);
                    cmd.Parameters.Add("p_kouri", OracleDbType.Char).Value = ConvertBoolToDbChar(patient.HealthCard.Smokes);
                    cmd.Parameters.Add("p_tehotenstvi", OracleDbType.Char).Value = ConvertBoolToDbChar(patient.HealthCard.Pregnancy);
                    cmd.Parameters.Add("p_alkohol", OracleDbType.Char).Value = ConvertBoolToDbChar(patient.HealthCard.Alcohol);
                    cmd.Parameters.Add("p_sport", OracleDbType.Varchar2).Value = patient.HealthCard.Sport;
                    cmd.Parameters.Add("p_plomby", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.HealthCard.Fillings);
                    cmd.Parameters.Add("p_anamnezy_id", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.HealthCard.Anamnesis.Id);

                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = patient.Address.City;
                    cmd.Parameters.Add("p_psc", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Address.PostalCode);
                    cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Address.StreetNumber);
                    cmd.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = patient.Address.Country;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = patient.Address.Street;



                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        return false;
                    }


                }
            }
        }
        // editace pacienta,adresy a zdrav karty
        private bool UpdatePatientWithDetails(Patient patient)
        {
            string insertProcedure = "UPDATE_PACIENTI_S_PODROBNOSTMI";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_patient_id", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Id);
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = patient.FirstName;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = patient.LastName;
                    cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.SocialSecurityNumber);
                    cmd.Parameters.Add("p_pohlavi", OracleDbType.Varchar2).Value = patient.Gender;
                    cmd.Parameters.Add("p_narozeni", OracleDbType.Date).Value = patient.DateOfBirth;
                    cmd.Parameters.Add("p_telefon", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Phone);
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = patient.Email;
                    cmd.Parameters.Add("p_pojistovny_id", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.InsuranceCompany.Id);
                    cmd.Parameters.Add("p_kouri", OracleDbType.Char).Value = ConvertBoolToDbChar(patient.HealthCard.Smokes);
                    cmd.Parameters.Add("p_tehotenstvi", OracleDbType.Char).Value = ConvertBoolToDbChar(patient.HealthCard.Pregnancy);
                    cmd.Parameters.Add("p_alkohol", OracleDbType.Char).Value = ConvertBoolToDbChar(patient.HealthCard.Alcohol);
                    cmd.Parameters.Add("p_sport", OracleDbType.Varchar2).Value = patient.HealthCard.Sport;
                    cmd.Parameters.Add("p_plomby", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.HealthCard.Fillings);
                    cmd.Parameters.Add("p_anamnezy_id", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.HealthCard.Anamnesis.Id);

                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = patient.Address.City;
                    cmd.Parameters.Add("p_psc", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Address.PostalCode);
                    cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Decimal).Value = Convert.ToDecimal(patient.Address.StreetNumber);
                    cmd.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = patient.Address.Country;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = patient.Address.Street;



                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        return false;
                    }


                }
            }
        }
        // vymazani pacienta z db
        private bool DeletePatientWithDetails(Patient patient)
        {
            string deleteProcedure = "DELETE_PACIENT";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_pacient_id", OracleDbType.Decimal);
                    paramId.Value = patient.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public List<HealthCard> GetAllAlergiesHealthCard()
        {
            List<HealthCard> healthCardList = new List<HealthCard>();
            List<Alergy> alergyList = GetAllAllergies();
            List<AlergyHealthCardLink> allergyHealthCardLinks = GetAllAlergyHealthCardLinks();
            List<Anamnesis> anamneses = GetAllAnamnesis();
            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "SELECT_PACIENTI";

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


                            int HealthCardID = reader["HEALTH_CARD_ID"] != DBNull.Value ? Convert.ToInt32(reader["HEALTH_CARD_ID"]) : 0;
                            bool Smokes = ConvertDbCharToBool(reader["SMOKES"].ToString());
                            bool Pregnancy = ConvertDbCharToBool(reader["PREGNANCY"].ToString());
                            bool Alcohol = ConvertDbCharToBool(reader["ALCOHOL"].ToString());
                            string Sport = reader["SPORT"].ToString();
                            int Fillings = reader["FILLINGS"] != DBNull.Value ? Convert.ToInt32(reader["FILLINGS"]) : 0;
                            int anamnesisID = reader["ANAMNESIS_ID"] != DBNull.Value ? Convert.ToInt32(reader["ANAMNESIS_ID"]) : 0;
                            Anamnesis anamnesis = anamneses.FirstOrDefault(anamnesis => anamnesis.Id == anamnesisID);


                            healthCardList.Add(new HealthCard(HealthCardID, Smokes, Pregnancy, Alcohol, Sport, Fillings, anamnesis));




                        }
                    }
                }
            }



            return healthCardList;
        }




        public async Task<List<Reservation>> GetAllReservations()
        {
            return await Task.Run(() =>
            {
                List<Reservation> reservationList = new List<Reservation>();
                List<Patient> patientList = GetAllPatients().Result;
                List<Employee> employeeList = GetEmployees().Result;
                List<Procedure> procedureList = GetAllProcedures();
                List<ReservationProcedureLink> reservationProcedureLinks = GetAllReservationProcedureLinks();

                using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
                {
                    databaseConnection.OpenConnection();

                    using (OracleCommand cmd = new OracleCommand("SELECT_REZERVACI", databaseConnection.connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                        cursorParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(cursorParam);

                        cmd.ExecuteNonQuery();

                        // Zpracování výsledků pomocí OracleDataReader
                        using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                        {
                            while (reader.Read())
                            {
                                int reservationId = reader.GetInt32(reader.GetOrdinal("RESERVATION_ID"));
                                string reservationNotes = reader.GetOrdinal("NOTES").ToString();
                                DateTime reservationDate = reader.GetDateTime(reader.GetOrdinal("RESERVATION_DATE"));
                                int employeeId = reader.GetInt32(reader.GetOrdinal("EMPLOYEE_ID"));
                                int patientId = reader.GetInt32(reader.GetOrdinal("PATIENT_ID"));

                                Patient patientTemp = patientList.FirstOrDefault(p => p.Id == patientId);
                                Employee employeeTemp = employeeList.FirstOrDefault(e => e.Id == employeeId);

                                if (employeeTemp != null && patientTemp != null)
                                {
                                    Reservation reservation = new Reservation(reservationId, reservationNotes, reservationDate, patientTemp, employeeTemp);


                                    var linkedProcedures = reservationProcedureLinks.Where(link => link.ReservationId == reservationId)
                                                                                     .Select(link => procedureList.FirstOrDefault(p => p.Id == link.ProcedureId))
                                                                                     .ToList();
                                    reservation.Procedures = new ObservableCollection<Procedure>(linkedProcedures);

                                    reservationList.Add(reservation);
                                }

                            }
                        }
                    }
                }



                return reservationList;
            });
        }
        public List<Procedure> GetAllProcedures()
        {
            List<Procedure> procedureList = new List<Procedure>();

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_ZAKROKY", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    // Zpracování výsledků pomocí OracleDataReader
                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int procedureId = reader.GetInt32(reader.GetOrdinal("ZAKROK_ID"));
                            string procedureName = reader["ZAKROK_NAME"].ToString();
                            int procedureCost = reader.GetInt32(reader.GetOrdinal("ZAKROK_PRICE"));
                            bool isPayed = ConvertDbCharToBool(reader["COVERED_BY_INSURANCE"].ToString());
                            string process = reader.GetOrdinal("PROCEDURE_DESCRIPTION").ToString();
                            procedureList.Add(new Procedure(procedureId, procedureName, procedureCost, isPayed, process));


                        }
                    }
                }
            }


            return procedureList;


        }
        public List<ReservationProcedureLink> GetAllReservationProcedureLinks()
        {
            List<ReservationProcedureLink> reservationProcedureList = new List<ReservationProcedureLink>();

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_REZERVACI_ZAKROKY", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    // Zpracování výsledků pomocí OracleDataReader
                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int reservatinId = reader.GetInt32(reader.GetOrdinal("RESERVATION_ID"));
                            int procedureId = reader.GetInt32(reader.GetOrdinal("PROCEDURE_ID"));
                            reservationProcedureList.Add(new ReservationProcedureLink(reservatinId, procedureId));


                        }
                    }
                }
            }


            return reservationProcedureList;


        }

        private List<AlergyHealthCardLink> GetAllAlergyHealthCardLinks()
        {
            List<AlergyHealthCardLink> allergyHealthCardList = new List<AlergyHealthCardLink>();
            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_ALERGIE_ZDRAVOTNI_KARTA", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    // Zpracování výsledků pomocí OracleDataReader
                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int alergyId = reader.GetInt32(reader.GetOrdinal("ALLERGY_ID"));
                            int alergyHealthCardId = reader.GetInt32(reader.GetOrdinal("HEALTH_CARD_ID"));
                            allergyHealthCardList.Add(new AlergyHealthCardLink(alergyId, alergyHealthCardId));


                        }
                    }
                }
            }


            return allergyHealthCardList;
        }
        public List<Alergy> GetAllAllergies()
        {
            List<Alergy> allergyList = new List<Alergy>();

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_ALERGIE", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int allergyId = reader.GetInt32(reader.GetOrdinal("ALLERGY_ID"));
                            string allergyName = reader.GetString(reader.GetOrdinal("ALLERGY_NAME"));
                            allergyList.Add(new Alergy(allergyId, allergyName));
                        }
                    }
                }
            }

            return allergyList;
        }

        public async Task<List<Visit>> GetAllVisits()
        {
            return await Task.Run(() =>
            {
                List<Visit> visits = new List<Visit>();
                List<Patient> patientList = GetAllPatients().Result;

                using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
                {
                    databaseConnection.OpenConnection();

                    using (OracleCommand cmd = new OracleCommand("SELECT_NAVSTEVY", databaseConnection.connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                        cursorParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(cursorParam);

                        cmd.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                        {
                            while (reader.Read())
                            {
                                int visitId = reader.GetInt32(reader.GetOrdinal("VISIT_ID"));
                                DateTime visitDate = reader.GetDateTime(reader.GetOrdinal("VISIT_DATE"));
                                string visitNotes = reader.IsDBNull(reader.GetOrdinal("VISIT_NOTES")) ? null : reader.GetString(reader.GetOrdinal("VISIT_NOTES"));
                                int patientId = reader.GetInt32(reader.GetOrdinal("PACIENT_ID"));

                                Patient patientTemp = patientList.FirstOrDefault(p => p.Id == patientId);

                                if (patientTemp != null)
                                {
                                    Visit visit = new Visit(visitId, visitDate, visitNotes, patientTemp);
                                    visits.Add(visit);
                                }
                            }
                        }
                    }
                }

                return visits;
            });
        }
        //public Task<List<Document>> GetAllDocuments()
        //{
        //    return Task.Run(async () =>
        //    {
        //        List<Document> documents = new List<Document>();
        //        using (var connection = new OracleDatabaseConnection())
        //        {
        //            connection.OpenConnection();
        //            string procedureName = "SELECT_DOKUMENTYY";

        //            using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                // Výstupní parametr pro kurzor
        //                OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
        //                cursorParam.Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add(cursorParam);

        //                using (OracleDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        int Id = reader["DOCUMENT_ID"] != DBNull.Value ? Convert.ToInt32(reader["DOCUMENT_ID"]) : 0;
        //                        byte[] file = null;
        //                        if (!reader.IsDBNull(reader.GetOrdinal("FILE")))
        //                        {
        //                            OracleBlob oracleBlob = reader.GetOracleBlob(reader.GetOrdinal("FILE"));
        //                            file = new byte[oracleBlob.Length];
        //                            oracleBlob.Read(file, 0, (int)oracleBlob.Length);
        //                        }
        //                        string fileName = reader.IsDBNull(reader.GetOrdinal("FILE_NAME")) ? null : reader.GetString(reader.GetOrdinal("FILE_NAME"));
        //                        string fileSuffix = reader.IsDBNull(reader.GetOrdinal("FILE_SUFFIX")) ? null : reader.GetString(reader.GetOrdinal("FILE_SUFFIX"));
        //                        MemoryStream memoryStream = new MemoryStream(file);
        //                        PdfDocument document = PdfReader.Open(memoryStream, PdfDocumentOpenMode.Import);
        //                        Document doc = new Document(Id, document, fileName, fileSuffix);
        //                        documents.Add(doc);




        //                    }
        //                }
        //            }
        //        }
        //        return documents;
        //    });
        //}
        public List<Document> GetAllDocuments()
        {
            List<Document> documents = new List<Document>();
            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT * FROM ST67060.DOKUMENT", databaseConnection.connection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0;
                            byte[] file = reader["SOUBOR"] != DBNull.Value ? (byte[])reader["SOUBOR"] : null;
                            string fileName = reader["NAZEV_SOUBORU"] != DBNull.Value ? reader["NAZEV_SOUBORU"].ToString() : null;
                            string fileSuffix = reader["PRIPONA_SOUBORU"] != DBNull.Value ? reader["PRIPONA_SOUBORU"].ToString() : null;

                            using (MemoryStream memoryStream = new MemoryStream(file))
                            {
                                PdfDocument document = PdfReader.Open(memoryStream, PdfDocumentOpenMode.Import);
                                Document doc = new Document(Id, document, fileName, fileSuffix);
                                documents.Add(doc);
                            }
                        }
                    }
                }
            }
            return documents;
        }
        public List<Anamnesis> GetAllAnamnesis()
        {
            List<Anamnesis> anamneses = new List<Anamnesis>();
            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_ANAMNEZY", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int anamnesisId = reader.GetInt32(reader.GetOrdinal("ANAMNESIS_ID"));
                            string anamnesisName = reader.IsDBNull(reader.GetOrdinal("DISEASE")) ? null : reader.GetString(reader.GetOrdinal("DISEASE"));
                            Entity.Anamnesis anamnesis = new Entity.Anamnesis(anamnesisId, anamnesisName);
                            anamneses.Add(anamnesis);


                        }
                    }
                }
            }
            return anamneses;
        }
        public Task<List<Prescription>> GetAllPrescriptions()
        {
            return Task.Run(async () =>
            {
                List<Document> documents = GetAllDocuments();
                List<Patient> patients = GetAllPatients().Result;
                List<Employee> employees = GetEmployees().Result;

                List<Prescription> prescriptions = new List<Prescription>();


                using (var connection = new OracleDatabaseConnection())
                {
                    connection.OpenConnection();
                    string procedureName = "SELECT_LEKARSKE_PREDPISY";

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


                                int ID = reader["PRESCRIPTION_ID"] != DBNull.Value ? Convert.ToInt32(reader["PRESCRIPTION_ID"]) : 0;
                                string drugName = reader["DRUG_NAME"].ToString();
                                decimal supplement = reader["SUPPLEMENT"] != DBNull.Value ? Convert.ToDecimal(reader["SUPPLEMENT"]) : 0;
                                DateTime date = reader["DATE_PRESCRIBED"] != DBNull.Value ? Convert.ToDateTime(reader["DATE_PRESCRIBED"]) : DateTime.MinValue;
                                int patientId = reader["PATIENT_ID"] != DBNull.Value ? Convert.ToInt32(reader["PATIENT_ID"]) : 0;
                                int employeeId = reader["EMPLOYEE_ID"] != DBNull.Value ? Convert.ToInt32(reader["EMPLOYEE_ID"]) : 0;
                                int documentId = reader["DOCUMENT_ID"] != DBNull.Value ? Convert.ToInt32(reader["DOCUMENT_ID"]) : 0;
                                Document doc = documents.FirstOrDefault(docu => docu.Id == documentId);

                                Patient patientTemp = patients.FirstOrDefault(p => p.Id == patientId);
                                Employee employeeTemp = employees.FirstOrDefault(p => p.Id == employeeId);
                                Prescription prescription = new Prescription(ID, drugName, supplement, employeeTemp, patientTemp, date);
                                prescription.File = doc;
                                prescriptions.Add(prescription);


                            }
                        }
                    }
                }

                return prescriptions;
            });

        }
        public bool InsertProcedureToReservation(Reservation reservation, Procedure procedure)
        {
            string insertProcedure = "INSERT_REZERVACE_ZAKROK";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_rezervace_id", OracleDbType.Decimal).Value = Convert.ToDecimal(reservation.Id);
                    cmd.Parameters.Add("v_zakrok_id", OracleDbType.Decimal).Value = Convert.ToDecimal(procedure.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        return false;
                    }


                }
            }
        }


        // ==============================================
        // Metody pro Admin
        // ==============================================


        // nacteni logu - ADMIN

        private Task<List<Log>> GetLogs()
        {
            return Task.Run(async () =>
            {
                List<Log> logs = new List<Log>();
                using (var connection = new OracleDatabaseConnection())
                {
                    connection.OpenConnection();
                    string procedureName = "SELECT_LOGY";

                    using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Výstupní parametr pro kurzor
                        cmd.Parameters.Add("v_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int Id = Convert.ToInt32(reader["LOG_ID"]);
                                string before = reader["BEFORE_VALUES"].ToString();
                                DateTime timeStamp = Convert.ToDateTime(reader["TIMESTAMP"]);
                                string eventType = reader["EVENT_TYPE"].ToString();
                                string tableName = reader["TABLE_NAME"].ToString();
                                string after = reader["AFTER_VALUES"].ToString();
                                ChangeType changeType = ChangeType.Insert;
                                switch (eventType)
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
                                Log log = new Log(Id, tableName, changeType, timeStamp, before, after);
                                logs.Add(log);
                            }
                        }
                    }
                }
                return logs;
            });
        }


        // nacteni uzivatelů - ADMIN
        public List<User> GetUsers()
        {
            List<User> userList = new List<User>();

            using (var connection = new OracleDatabaseConnection())
            {
                connection.OpenConnection();
                string procedureName = "SELECT_UZIVATELE";

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
        public List<Insurance> GetAllInsurances()
        {
            List<Insurance> insurances = new List<Insurance>();

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_POJISTOVNY", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int insuranceId = reader.GetInt32(reader.GetOrdinal("INSURANCE_ID"));
                            string insuranceName = reader.IsDBNull(reader.GetOrdinal("INSURANCE_NAME")) ? null : reader.GetString(reader.GetOrdinal("INSURANCE_NAME"));
                            string insuranceAbbreviation = reader.IsDBNull(reader.GetOrdinal("INSURANCE_ABBREVIATION")) ? null : reader.GetString(reader.GetOrdinal("INSURANCE_ABBREVIATION"));

                            Insurance insurance = new Insurance(insuranceId, insuranceName, insuranceAbbreviation);
                            insurances.Add(insurance);
                        }
                    }
                }
            }

            return insurances;
        }

        public List<Address> GetAllAddresses()
        {
            List<Address> addresses = new List<Address>();

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand("SELECT_ADRESY", databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("v_cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cursorParam);

                    cmd.ExecuteNonQuery();

                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        while (reader.Read())
                        {
                            int addressId = reader.GetInt32(reader.GetOrdinal("ADDRESS_ID"));
                            string city = reader.IsDBNull(reader.GetOrdinal("CITY")) ? null : reader.GetString(reader.GetOrdinal("CITY"));
                            int postalCode = reader.IsDBNull(reader.GetOrdinal("ZIP_CODE")) ? 0 : int.Parse(reader.GetString(reader.GetOrdinal("ZIP_CODE")));
                            int streetNumber = reader.IsDBNull(reader.GetOrdinal("STREET_NUMBER")) ? 0 : int.Parse(reader.GetString(reader.GetOrdinal("STREET_NUMBER")));
                            string country = reader.IsDBNull(reader.GetOrdinal("COUNTRY")) ? null : reader.GetString(reader.GetOrdinal("COUNTRY"));
                            string street = reader.IsDBNull(reader.GetOrdinal("STREET")) ? null : reader.GetString(reader.GetOrdinal("STREET"));

                            Address address = new Address(addressId, city, postalCode, streetNumber, country, street);
                            addresses.Add(address);
                        }
                    }
                }
            }

            return addresses;
        }

        public bool InsertReservation(Reservation reservation)
        {
            string insertProcedure = "INSERT_REZERVACE";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_poznamky_rez", OracleDbType.Varchar2).Value = reservation.Notes;
                    cmd.Parameters.Add("p_datum_rez", OracleDbType.Date).Value = reservation.Date;
                    cmd.Parameters.Add("p_pacient_id_rez", OracleDbType.Decimal).Value = Convert.ToDecimal(reservation.Patient.Id);
                    cmd.Parameters.Add("p_zamestnanec_id_rez", OracleDbType.Decimal).Value = Convert.ToDecimal(reservation.Employee.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        return false;
                    }

                }
            }
        }
        public bool UpdateReservation(Reservation reservation)
        {
            string updateProcedure = "UPDATE_RESERVACE";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_rezervace_id", OracleDbType.Decimal).Value = reservation.Id;
                    cmd.Parameters.Add("p_poznamky_rez", OracleDbType.Varchar2).Value = reservation.Notes;
                    cmd.Parameters.Add("p_datum_rez", OracleDbType.Date).Value = reservation.Date;
                    cmd.Parameters.Add("p_pacient_id_rez", OracleDbType.Decimal).Value = reservation.Patient.Id;
                    cmd.Parameters.Add("p_zamestnanec_id_rez", OracleDbType.Decimal).Value = reservation.Employee.Id;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public bool DeleteReservation(Reservation reservation)
        {
            string deleteProcedure = "DELETE_REZERVACE";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_rezervace_id", OracleDbType.Decimal);
                    paramId.Value = reservation.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool InsertPrescription(Prescription prescription)
        {
            string insertProcedure = "INSERT_LEKARSKY_PREDPIS";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_nazev_leku", OracleDbType.Varchar2).Value = prescription.DrugName;
                    cmd.Parameters.Add("p_doplatek", OracleDbType.Decimal).Value = prescription.Supplement;
                    cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Decimal).Value = Convert.ToDecimal(prescription.Employee.Id);
                    cmd.Parameters.Add("p_pacient_id", OracleDbType.Decimal).Value = Convert.ToDecimal(prescription.Patient.Id);
                    cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = prescription.Date;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeletePrescription(Prescription prescription)
        {
            string deleteProcedure = "DELETE_LEKARSKY_PREDPIS";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter paramId = new OracleParameter("p_predpis_id", OracleDbType.Decimal);
                    paramId.Value = prescription.ID;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdatePrescription(Prescription prescription)
        {
            string updateProcedure = "UPDATE_LEKARSKY_PREDPIS";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_predpis_id", OracleDbType.Decimal).Value = prescription.ID;
                    cmd.Parameters.Add("p_nazev_leku", OracleDbType.Varchar2).Value = prescription.DrugName;
                    cmd.Parameters.Add("p_doplatek", OracleDbType.Decimal).Value = prescription.Supplement;
                    cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Decimal).Value = Convert.ToDecimal(prescription.Employee.Id);
                    cmd.Parameters.Add("p_pacient_id", OracleDbType.Decimal).Value = Convert.ToDecimal(prescription.Patient.Id);
                    cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = prescription.Date;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public bool InsertVisit(Visit visit)
        {
            string insertProcedure = "INSERT_NAVSTEVY";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = visit.Date;
                    cmd.Parameters.Add("p_poznamky", OracleDbType.Varchar2).Value = visit.Notes;
                    cmd.Parameters.Add("p_pacient_id", OracleDbType.Decimal).Value = Convert.ToDecimal(visit.Patient.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteVisit(Visit visit)
        {
            string deleteProcedure = "DELETE_NAVSTEVY";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    OracleParameter paramId = new OracleParameter("p_navsteva_id", OracleDbType.Decimal);
                    paramId.Value = visit.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateVisit(Visit visit)
        {
            string updateProcedure = "UPDATE_NAVSTEVY";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_navsteva_id", OracleDbType.Decimal).Value = visit.Id;
                    cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = visit.Date;
                    cmd.Parameters.Add("p_poznamky", OracleDbType.Varchar2).Value = visit.Notes;
                    cmd.Parameters.Add("p_pacient_id", OracleDbType.Decimal).Value = Convert.ToDecimal(visit.Patient.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool InsertAddress(Address address)
        {
            string insertProcedure = "INSERT_ADRESA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = address.City;
                    cmd.Parameters.Add("p_psc", OracleDbType.Decimal).Value = address.PostalCode;
                    cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Decimal).Value = address.StreetNumber;
                    cmd.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = address.Country;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = address.Street;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteAddress(Address address)
        {
            string deleteProcedure = "DELETE_ADRESA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_adresa_id", OracleDbType.Decimal);
                    paramId.Value = address.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateAddress(Address address)
        {
            string updateProcedure = "UPDATE_ADRESA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = address.Id;
                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = address.City;
                    cmd.Parameters.Add("p_psc", OracleDbType.Decimal).Value = address.PostalCode;
                    cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Decimal).Value = address.StreetNumber;
                    cmd.Parameters.Add("p_stat", OracleDbType.Varchar2).Value = address.Country;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = address.Street;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool InsertAlergy(Alergy alergy)
        {
            string insertProcedure = "INSERT_ALERGIE";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = alergy.Name;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteAlergy(Alergy alergy)
        {
            string deleteProcedure = "DELETE_ALERGIE";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_alergie_id", OracleDbType.Decimal);
                    paramId.Value = alergy.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public bool UpdateAlergy(Alergy alergy)
        {
            string updateProcedure = "UPDATE_ALERGIE";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = alergy.Id;
                    cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = alergy.Name;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public bool InsertInsurance(Insurance insurance)
        {
            string insertProcedure = "INSERT_POJISTOVNY";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = insurance.Name;
                    cmd.Parameters.Add("p_zkratka", OracleDbType.Varchar2).Value = insurance.Abbreviation;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteInsurance(Insurance insurance)
        {
            string deleteProcedure = "DELETE_POJISTOVNY";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_pojistovna_id", OracleDbType.Decimal);
                    paramId.Value = insurance.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateInsurance(Insurance insurance)
        {
            string updateProcedure = "UPDATE_POJISTOVNY";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = insurance.Id;
                    cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = insurance.Name;
                    cmd.Parameters.Add("p_zkratka", OracleDbType.Varchar2).Value = insurance.Abbreviation;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public bool InsertAnamnesis(Anamnesis anamnesis)
        {
            string insertProcedure = "INSERT_ANAMNEZA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_nemoc", OracleDbType.Varchar2).Value = anamnesis.Name;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public bool DeleteAnamnesis(Anamnesis anamnesis)
        {
            string deleteProcedure = "DELETE_ANAMNEZA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_anamneza_id", OracleDbType.Decimal);
                    paramId.Value = anamnesis.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public bool UpdateAnamnesis(Anamnesis anamnesis)
        {
            string updateProcedure = "UPDATE_ANAMNEZA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = anamnesis.Id;
                    cmd.Parameters.Add("p_nemoc", OracleDbType.Varchar2).Value = anamnesis.Name;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool InsertUser(User user)
        {
            string insertProcedure = "INSERT_UZIVATEL";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Decimal).Value = Convert.ToDecimal(user.Employee?.Id);
                    cmd.Parameters.Add("p_heslo", OracleDbType.Varchar2).Value = user.Password;
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = user.Name;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteUser(User user)
        {
            string deleteProcedure = "DELETE_UZIVATEL";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_uzivatel_id", OracleDbType.Decimal);
                    paramId.Value = user.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateUser(User user)
        {
            string updateProcedure = "UPDATE_UZIVATEL";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = user.Id;
                    cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Decimal).Value = Convert.ToDecimal(user.Employee?.Id);
                    cmd.Parameters.Add("p_heslo", OracleDbType.Varchar2).Value = user.Password;
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = user.Name;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public bool InsertEmployee(Employee employee)
        {
            string insertProcedure = "INSERT_ZAMESTNANEC";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = employee.Name;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = employee.Surname;
                    cmd.Parameters.Add("p_nastup", OracleDbType.Date).Value = employee.HireDate;
                    cmd.Parameters.Add("p_role_id", OracleDbType.Decimal).Value = Convert.ToDecimal((int)employee.Role);
                    cmd.Parameters.Add("p_fotka", OracleDbType.Blob).Value = employee.Photo;
                    cmd.Parameters.Add("p_nadrizeny_id", OracleDbType.Decimal).Value = Convert.ToDecimal(employee.Superior?.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteEmployee(Employee employee)
        {
            string deleteProcedure = "DELETE_ZAMESTNANEC";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_zamestnanec_id", OracleDbType.Decimal);
                    paramId.Value = employee.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            string updateProcedure = "UPDATE_ZAMESTNANEC";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = employee.Id;
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = employee.Name;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = employee.Surname;
                    cmd.Parameters.Add("p_nastup", OracleDbType.Date).Value = employee.HireDate;
                    cmd.Parameters.Add("p_role_id", OracleDbType.Decimal).Value = Convert.ToDecimal((int)employee.Role);
                    cmd.Parameters.Add("p_fotka", OracleDbType.Blob).Value = employee.Photo;
                    cmd.Parameters.Add("p_nadrizeny_id", OracleDbType.Decimal).Value = Convert.ToDecimal(employee.Superior?.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool InsertHealthCard(HealthCard healthCard)
        {
            string insertProcedure = "INSERT_ZDRAVOTNI_KARTA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_kouri", OracleDbType.Char).Value = healthCard.Smokes ? 'Y' : 'N';
                    cmd.Parameters.Add("p_tehotenstvi", OracleDbType.Char).Value = healthCard.Pregnancy ? 'Y' : 'N';
                    cmd.Parameters.Add("p_alkohol", OracleDbType.Char).Value = healthCard.Alcohol ? 'Y' : 'N';
                    cmd.Parameters.Add("p_sport", OracleDbType.Varchar2).Value = healthCard.Sport;
                    cmd.Parameters.Add("p_plomby", OracleDbType.Decimal).Value = healthCard.Fillings;
                    cmd.Parameters.Add("p_anamnezy_id", OracleDbType.Decimal).Value = healthCard.Anamnesis?.Id;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteHealthCard(HealthCard healthCard)
        {
            string deleteProcedure = "DELETE_ZDRAVOTNI_KARTA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter paramId = new OracleParameter("p_zdravotni_karta_id", OracleDbType.Decimal);
                    paramId.Value = healthCard.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateHealthCard(HealthCard healthCard)
        {
            string updateProcedure = "UPDATE_ZDRAVOTNI_KARTA";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = healthCard.Id;
                    cmd.Parameters.Add("p_kouri", OracleDbType.Char).Value = healthCard.Smokes ? 'Y' : 'N';
                    cmd.Parameters.Add("p_tehotenstvi", OracleDbType.Char).Value = healthCard.Pregnancy ? 'Y' : 'N';
                    cmd.Parameters.Add("p_alkohol", OracleDbType.Char).Value = healthCard.Alcohol ? 'Y' : 'N';
                    cmd.Parameters.Add("p_sport", OracleDbType.Varchar2).Value = healthCard.Sport;
                    cmd.Parameters.Add("p_plomby", OracleDbType.Decimal).Value = healthCard.Fillings;
                    cmd.Parameters.Add("p_anamnezy_id", OracleDbType.Decimal).Value = healthCard.Anamnesis?.Id;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool InsertProcedure(Procedure procedure)
        {
            string insertProcedure = "INSERT_ZAKROK";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(insertProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = procedure.Name;
                    cmd.Parameters.Add("p_cena", OracleDbType.Decimal).Value = procedure.Price;
                    cmd.Parameters.Add("p_hradi_pojistovna", OracleDbType.Char).Value = procedure.CoveredByInsurance ? 'Y' : 'N';
                    cmd.Parameters.Add("p_postup", OracleDbType.Varchar2).Value = procedure.ProcedureSteps;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool DeleteProcedure(Procedure procedure)
        {
            string deleteProcedure = "DELETE_ZAKROK";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(deleteProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметр ID
                    OracleParameter paramId = new OracleParameter("p_zakrok_id", OracleDbType.Decimal);
                    paramId.Value = procedure.Id;
                    cmd.Parameters.Add(paramId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool UpdateProcedure(Procedure procedure)
        {
            string updateProcedure = "UPDATE_ZAKROK";

            using (OracleDatabaseConnection databaseConnection = new OracleDatabaseConnection())
            {
                databaseConnection.OpenConnection();

                using (OracleCommand cmd = new OracleCommand(updateProcedure, databaseConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("p_id", OracleDbType.Decimal).Value = procedure.Id;
                    cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = procedure.Name;
                    cmd.Parameters.Add("p_cena", OracleDbType.Decimal).Value = procedure.Price;
                    cmd.Parameters.Add("p_hradi_pojistovna", OracleDbType.Char).Value = procedure.CoveredByInsurance ? 'Y' : 'N';
                    cmd.Parameters.Add("p_postup", OracleDbType.Varchar2).Value = procedure.ProcedureSteps;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public Task<List<Catalog>> GetCatalog()
        {
            return Task.Run(async () =>
            {
                List<Catalog> catalogItems = new List<Catalog>();
                using (var connection = new OracleDatabaseConnection())
                {
                    connection.OpenConnection();
                    string procedureName = "SYSTEM_KATALOG";

                    using (OracleCommand cmd = new OracleCommand(procedureName, connection.connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Výstupní parametr pro kurzor
                        cmd.Parameters.Add("v_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string owner = reader["OWNER"].ToString();
                                string name = reader["OBJECT_NAME"].ToString();
                                string type = reader["OBJECT_TYPE"].ToString();

                                Catalog catalog = new Catalog(owner, name, type);

                                catalogItems.Add(catalog);
                            }
                        }
                    }
                }
                return catalogItems;
            });
        }


        // ==============================================
        // Interní metody
        // ==============================================



        // metody pro prevadeni databazovych bool na boolean v c#
        private char ConvertBoolToDbChar(bool value)
        {
            return value ? 'Y' : 'N';
        }

        private bool ConvertDbCharToBool(string value)
        {
            return value == "Y";
        }


    }
}