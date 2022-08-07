using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AppointmentBot
{
    public class Appointment
    {
        private string _option;
        private string _age;
        private string _patientName;
        private string _reason;
        private int _appDay;
        private string _appTime;
        private string _appDate;
        private string _referenceId;
        private string _phone;
        private string patient_Id;
        private List<string> futureDates = new List<String>(); 

        public string Phone{
            get => _phone;
            set => _phone = value;
        }

        public string option{
            get => _option;
            set => _option = value;
        }

        public string age{
            get => _age;
            set => _age = value;
        }

         public string patientName{
            get => _patientName;
            set => _patientName = value;
        }

        public string reason{
            get => _reason;
            set => _reason = value;
        }

        public int appDay{
            get => _appDay;
            set => _appDay = value;
        }

        public string appTime{
            get => _appTime;
            set => _appTime = value;
        }
        public string appDate{
            get => _appDate;
            set => _appDate = value;
        }

        public string referenceId{
            get => _referenceId;
            set => _referenceId = value;
        }
    
/////////////////////////////////////////////////////////////////////////////////////////////
    public void saveAppointmentInfo(){
        //UUID for Reference ID
        Guid myUUID = Guid.NewGuid();
        referenceId = myUUID.ToString();
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();
            var commandSelect = connection.CreateCommand();
            commandSelect.CommandText = @"SELECT PATIENT_ID FROM PATIENT WHERE NAME=$name AND CONTACT_NO=$phone";
            commandSelect.Parameters.AddWithValue("$name", patientName);
            commandSelect.Parameters.AddWithValue("$phone", Phone);
            using(var reader = commandSelect.ExecuteReader())
            {
                while(reader.Read())
                {
                    patient_Id = reader.GetString(0);
                }
            } 
            var commandInsert = connection.CreateCommand();
            commandInsert.CommandText =
            @"INSERT INTO APPOINTMENT(REFERENCE_ID, APP_DATE, APP_TIME, APP_DAY, REASON, PATIENT_ID)
             VALUES($reference, $appDate, $appTime, $appDay, $reason, $patientId) 
             ";
            commandInsert.Parameters.AddWithValue("$reference", referenceId);
            commandInsert.Parameters.AddWithValue("$appDate", appDate);
            commandInsert.Parameters.AddWithValue("$appTime", appTime);
            commandInsert.Parameters.AddWithValue("$appDay", appDay);
            commandInsert.Parameters.AddWithValue("$reason", reason);
            commandInsert.Parameters.AddWithValue("$patientId", patient_Id);
            int nRowsInserted = commandInsert.ExecuteNonQuery();
        }

    }
/////////////////////////////////////////////////////////////////////////////////////////////
    public string viewAppointmentInfoByName()
    {
         using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            SELECT P.NAME, P.AGE, P.CONTACT_NO, A.REFERENCE_ID, A.APP_TIME, A.APP_DATE, A.REASON, A.APP_DAY 
            FROM APPOINTMENT A JOIN PATIENT P
            ON A.PATIENT_ID = P.PATIENT_ID
            WHERE P.NAME = $name AND P.CONTACT_NO = $phone AND A.APP_DATE > strftime('%m/%d/%Y', 'now')
            ORDER BY A.APP_DATE LIMIT 1
                ";
            commandUpdate.Parameters.AddWithValue("$name", patientName);
            commandUpdate.Parameters.AddWithValue("$phone", Phone);

            using(var reader = commandUpdate.ExecuteReader())
            {
                while(reader.Read())
                {
                    patientName = reader.GetString(0);
                    age = reader.GetString(1);
                    Phone = reader.GetString(2);
                    referenceId = reader.GetString(3);
                    appTime = reader.GetString(4);
                    appDate = reader.GetString(5);
                    reason = reader.GetString(6);
                    appDay = Int32.Parse(reader.GetString(7)); 
                }
            }
        }
        string returnText = "Reference ID: "+referenceId+"\nPatient Name: "+patientName+"\nPatient Age: "+age+"\nAppointment Date: "+appDate+"\nApponintment Time: "+appTime+"\nReason: "+reason;
        return returnText;
    }

/////////////////////////////////////////////////////////////////////////////////////////////
    public string viewAppointmentInfoByID()
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();
            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            SELECT P.NAME, P.AGE, P.CONTACT_NO, A.REFERENCE_ID, A.APP_TIME, A.APP_DATE, A.REASON, A.APP_DAY 
            FROM APPOINTMENT A JOIN PATIENT P
            ON A.PATIENT_ID = P.PATIENT_ID
            WHERE A.REFERENCE_ID = $reference AND A.APP_DATE > strftime('%m/%d/%Y', 'now')
            ORDER BY A.APP_DATE LIMIT 1
                ";
            commandUpdate.Parameters.AddWithValue("$reference", referenceId);
            
            using(var reader = commandUpdate.ExecuteReader())
            {
                while(reader.Read())
                {
                    patientName = reader.GetString(0);
                    age = reader.GetString(1);
                    Phone = reader.GetString(2);
                    referenceId = reader.GetString(3);
                    appTime = reader.GetString(4);
                    appDate = reader.GetString(5);
                    reason = reader.GetString(6);
                    appDay = Int32.Parse(reader.GetString(7)); 
                }
            }
        }
        string returnText = "Reference ID: "+referenceId+"\nPatient Name: "+patientName+"\nPatient Age: "+age+"\nAppointment Date: "+appDate+"\nApponintment Time: "+appTime+"\nReason: "+reason;
        return returnText;
    }

/////////////////////////////////////////////////////////////////////////////////////////////
    public void savePatientInfo()
    {
        //UUID for PATIENT_ID
        Guid myUUID = Guid.NewGuid();
        patient_Id = myUUID.ToString();
       
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            UPDATE PATIENT
            SET AGE = $age
            WHERE CONTACT_NO = $phone AND NAME = $name
                ";
            commandUpdate.Parameters.AddWithValue("$age", age);
            commandUpdate.Parameters.AddWithValue("$phone", Phone);
            commandUpdate.Parameters.AddWithValue("$name", patientName);

            int nRows = commandUpdate.ExecuteNonQuery();
            if(nRows == 0 )
            {
                var commandInsert = connection.CreateCommand();
                commandInsert.CommandText =
                @"
                INSERT INTO PATIENT(PATIENT_ID, NAME, AGE, CONTACT_NO)
                VALUES($patientid, $name, $age, $phone)
                ";
                commandInsert.Parameters.AddWithValue("$patientid",patient_Id);
                commandInsert.Parameters.AddWithValue("$name", patientName);
                commandInsert.Parameters.AddWithValue("$age", age);
                commandInsert.Parameters.AddWithValue("$phone", Phone);
                int nRowsInserted = commandInsert.ExecuteNonQuery();  
            } 
        }

    }

/////////////////////////////////////////////////////////////////////////////////////////////
    public bool updateAppointmentInfo(){
        if(string.IsNullOrEmpty(referenceId))
        {
            using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();
                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText =
                @"
                UPDATE TABLE APPOINTMENT SET 
                APP_DATE = $appDate, 
                APP_TIME = $appTime,
                APP_DAY = $appDay
                WHERE REFERENCE_ID = $reference AND APP_DATE > strftime('%m/%d/%Y', 'now')
                ";
                commandUpdate.Parameters.AddWithValue("$reference", referenceId);
                commandUpdate.Parameters.AddWithValue("$appDate", appDate);
                commandUpdate.Parameters.AddWithValue("$appTime", appTime);
                commandUpdate.Parameters.AddWithValue("$appDay", appDay);
                int nRowsInserted = commandUpdate.ExecuteNonQuery();
                if(nRowsInserted <= 0 )
                {
                    return false;
                }
            }
        }
        else if(string.IsNullOrEmpty(patientName))
        {
            using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();
                var commandSelect = connection.CreateCommand();
                commandSelect.CommandText = @"SELECT PATIENT_ID FROM PATIENT WHERE NAME=$name AND CONTACT_NO=$phone;";
                commandSelect.Parameters.AddWithValue("$name", patientName);
                commandSelect.Parameters.AddWithValue("$phone", Phone);
                using(var reader = commandSelect.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        patient_Id = reader.GetString(0);
                    }
                } 
                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText =
                @"
                UPDATE TABLE APPOINTMENT SET 
                APP_DATE = $appDate, 
                APP_TIME = $appTime,
                APP_DAY = $appDay
                WHERE REFERENCE_ID = (
                    SELECT REFERENCE_ID FROM APPOINTMENT 
                    WHERE PATIENT_ID = $patientid AND APP_DATE > strftime('%m/%d/%Y', 'now')
                    ORDER BY APP_DATE LIMIT 1)
                ";
                commandUpdate.Parameters.AddWithValue("$appDate", appDate);
                commandUpdate.Parameters.AddWithValue("$appTime", appTime);
                commandUpdate.Parameters.AddWithValue("$appDay", appDay);
                commandUpdate.Parameters.AddWithValue("$patientId", patient_Id);
                int nRowsInserted = commandUpdate.ExecuteNonQuery();
                if(nRowsInserted <= 0 )
                {
                    return false;
                }
            }
        }
        return true;
    }

/////////////////////////////////////////////////////////////////////////////////////////////
    public bool deleteAppointmentInfoByName()
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();
            var commandSelect = connection.CreateCommand();
            commandSelect.CommandText = @"SELECT PATIENT_ID FROM PATIENT WHERE NAME=$name AND CONTACT_NO=$phone;";
            commandSelect.Parameters.AddWithValue("$name", patientName);
            commandSelect.Parameters.AddWithValue("$phone", Phone);
            
            using(var reader = commandSelect.ExecuteReader())
            {
                while(reader.Read())
                {
                    patient_Id = reader.GetString(0);
                }
            } 
            if(string.IsNullOrEmpty(patient_Id))
            {
                return false;
            }
            else
            {
                var commandDelete = connection.CreateCommand();
                commandDelete.CommandText =
                @"
                DELETE FROM APPOINTMENT
                WHERE REFERENCE_ID = (
                    SELECT REFERENCE_ID FROM APPOINTMENT 
                    WHERE PATIENT_ID = $patientid AND APP_DATE > strftime('%m/%d/%Y', 'now')
                    ORDER BY APP_DATE LIMIT 1
                    )
                ";
                commandDelete.Parameters.AddWithValue("$patientid", patient_Id);
                int nRowsInserted = commandDelete.ExecuteNonQuery();
                if(nRowsInserted <= 0)
                {
                    return false;
                }
            }
            return true;
        }

    }

     public bool deleteAppointmentAndPatientInfoByName(string patient, string contact)
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            bool found = false;
            connection.Open();
            var commandSelect = connection.CreateCommand();
            commandSelect.CommandText = @"SELECT PATIENT_ID FROM PATIENT WHERE NAME=$name AND CONTACT_NO=$phone;";
            commandSelect.Parameters.AddWithValue("$name", patient);
            commandSelect.Parameters.AddWithValue("$phone", contact);
            
            using(var reader = commandSelect.ExecuteReader())
            {
                while(reader.Read())
                {
                    patient_Id = reader.GetString(0);
                }
            } 
            if(string.IsNullOrEmpty(patient_Id))
            {
                return false;
            }
            else
            {
                var commandDelete = connection.CreateCommand();
                commandDelete.CommandText =
                @"
                DELETE FROM APPOINTMENT
                WHERE PATIENT_ID = $patientid
                ";
                commandDelete.Parameters.AddWithValue("$patientid", patient_Id);
                int nRowsInserted = commandDelete.ExecuteNonQuery();
                if(nRowsInserted > 0)
                {
                    found = true;
                    var commandDelete2 = connection.CreateCommand();
                    commandDelete2.CommandText =
                    @"
                    DELETE FROM PATIENT
                    WHERE PATIENT_ID = $patientid
                    ";
                    commandDelete2.Parameters.AddWithValue("$patientid", patient_Id);
                    nRowsInserted = commandDelete2.ExecuteNonQuery();
                    if(nRowsInserted > 0)
                    {
                        found = true;
                    }
                }
            }
            return found;
        }

    }

/////////////////////////////////////////////////////////////////////////////////////////////
    public bool deleteAppointmentInfoByID()
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();
            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            DELETE FROM APPOINTMENT 
            WHERE REFERENCE_ID = $reference AND APP_DATE > strftime('%m/%d/%Y', 'now')
            ";
            commandUpdate.Parameters.AddWithValue("$reference", referenceId);
            int nRowsInserted = commandUpdate.ExecuteNonQuery();
            if(nRowsInserted <= 0 )
            {
                return false;
            }
        }
        return true;

    }

/////////////////////////////////////////////////////////////////////////////////////////////////
       public List<String> searchAvailableDay()
       {
            List<String> bookedDateList = new List<String>();
            List<String> availableDateList = new List<String>();

            getDate();
            using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();

                var commandSelect = connection.CreateCommand();
                commandSelect.CommandText =
                @"
                SELECT APP_DATE FROM APPOINTMENT 
                WHERE APP_DAY = $appDay AND APP_DATE > strftime('%m/%d/%Y', 'now')
                ORDER BY APP_DATE
                    ";
                commandSelect.Parameters.AddWithValue("$appDay", appDay);
                int nRows = commandSelect.ExecuteNonQuery();
                using(var reader = commandSelect.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        bookedDateList.Add(reader.GetString(0));
                    }
                }
            }
            availableDateList = futureDates.Except(bookedDateList).ToList();   
            return availableDateList;
       }

//////////////////////////////////////////////////////////////////////////////////////
       public List<String> searchAvailableTime(int optionForTime)
       {
          
            List<String> bookedTimeList = new List<String>();
            List<String> availableTimeList = new List<String>();
            List<String> timeList = new List<String>();
            switch(optionForTime)
            {
                case 1:
                    timeList.Add("9AM");
                    timeList.Add("9:30AM");
                    timeList.Add("10AM");
                    timeList.Add("10:30AM");
                    break;
                case 2:
                    timeList.Add("12PM");
                    timeList.Add("12:30PM");
                    timeList.Add("1PM");
                    timeList.Add("1:30PM");
                    break;
                case 3:
                    timeList.Add("3PM");
                    timeList.Add("3:30PM");
                    timeList.Add("4PM");
                    timeList.Add("4:30PM");
                    break;
            }

            using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();

                var commandSelect = connection.CreateCommand();
                commandSelect.CommandText =
                @"
                SELECT APP_TIME FROM APPOINTMENT 
                WHERE APP_DAY = $appDay AND DATE(APP_DATE) = $appDate
                    ";
                commandSelect.Parameters.AddWithValue("$appDay", appDay);
                commandSelect.Parameters.AddWithValue("$appDate",appDate);

                int nRows = commandSelect.ExecuteNonQuery();
                using(var reader = commandSelect.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        bookedTimeList.Add(reader.GetString(0));
                    }
                }
            }
            availableTimeList = timeList.Except(bookedTimeList).ToList();   
            return availableTimeList;

       }

/////////////////////////////////////////////////////////////////////////////////////////////
       public bool searchPatientInfo()
       {
           using (var connection = new SqliteConnection(DB.GetConnectionString()))
           {
                connection.Open();
                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText =
                @"
                SELECT NAME, AGE, CONTACT_NO 
                FROM PATIENT 
                WHERE CONTACT_NO = $phone AND NAME = $name;
                    ";
                commandUpdate.Parameters.AddWithValue("$phone", Phone);
                commandUpdate.Parameters.AddWithValue("$name", patientName);
                
                using(var reader = commandUpdate.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        return true;
                    }
                }
            }
        return false;

       }

/////////////////////////////////////////////////////////////////////////////////////////////
       public bool searchAppointmentByName()
       {
           using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            SELECT APP_TIME, APP_DATE 
            FROM APPOINTMENT A JOIN PATIENT P ON A.PATIENT_ID = P.PATIENT_ID
            WHERE P.NAME = $patient AND CONTACT_NO = $phone AND A.APP_DATE > strftime('%m/%d/%Y', 'now')
            ORDER BY A.APP_DATE LIMIT 1
                ";
            commandUpdate.Parameters.AddWithValue("$patient", patientName);
            commandUpdate.Parameters.AddWithValue("$phone",Phone);
            using(var reader = commandUpdate.ExecuteReader())
            {
                while(reader.Read())
                {
                    return true;
                }
            }
            }
        return false;

       }

/////////////////////////////////////////////////////////////////////////////////////////////
public string getReferenceID(string patient, string contact)
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            SELECT REFERENCE_ID 
            FROM APPOINTMENT A JOIN PATIENT P ON A.PATIENT_ID = P.PATIENT_ID
            WHERE P.NAME = $patient AND CONTACT_NO = $phone AND A.APP_DATE > strftime('%m/%d/%Y', 'now')
            ORDER BY A.APP_DATE LIMIT 1
                ";
            commandUpdate.Parameters.AddWithValue("$patient", patient);
            commandUpdate.Parameters.AddWithValue("$phone",contact);
            using(var reader = commandUpdate.ExecuteReader())
            {
                while(reader.Read())
                {
                    return reader.GetString(0);
                }
            }
        }
    return "false";

    }
/////////////////////////////////////////////////////////////////////////////////////////////
       public bool searchByReferenceID()
       {
           using (var connection = new SqliteConnection(DB.GetConnectionString()))
           {
                connection.Open();

                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText =
                @"
                SELECT APP_TIME, APP_DATE 
                FROM APPOINTMENT 
                WHERE REFERENCE_ID = $reference AND APP_DATE > strftime('%m/%d/%Y', 'now')
                ORDER BY APP_DATE;
                    ";
                commandUpdate.Parameters.AddWithValue("$reference", referenceId);
                using(var reader = commandUpdate.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        return true;
                    }
                }
            }
        return false;

       }


///////////////////////////////////////////////////////////////////////////////////////////////////    
        public void getDate()
        {
            DateTime d1 = DateTime.Today;
            DateTime d2 = d1.AddYears(1);
            var StartDate = DateTime.Parse(d1.ToString("d"));
            var SeriesEndDate = DateTime.Parse(d2.ToString("d"));
            List<DateTime> dates = new List<DateTime>();
            for (DateTime day = StartDate.Date; day.Date <= SeriesEndDate.Date; day = day.AddDays(1))
            {
                dates.Add(day);
            }
            var query = dates.Where(d => d.DayOfWeek == (DayOfWeek)appDay).GroupBy(d => d.Month).Select(e => e.Take(4));
            foreach (var item in query)
            {
                foreach (var date in item)
                {
                   futureDates.Add(date.ToString("d"));
                }
            }
        }

        public void deleteAllDataFromSqlLite()
        {
            using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();
                var commandDelete = connection.CreateCommand();
                commandDelete.CommandText =
                @"
                DELETE FROM APPOINTMENT 
                ";
                int nRowsInserted = commandDelete.ExecuteNonQuery();

                var commandDelete2 = connection.CreateCommand();
                commandDelete2.CommandText =
                @"
                DELETE FROM PATIENT 
                ";
                nRowsInserted = commandDelete2.ExecuteNonQuery();
                
            }
        }
    }
}
