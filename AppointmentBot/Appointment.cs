using Microsoft.Data.Sqlite;

namespace AppointmentBot
{
    public class Appointment
    {
        private string _option;
        private string _age;
        private string _patientName;
        private string _reason;
        private string _appDay;
        private string _appTime;
        private string _appDate;
        private string _referenceId;
        private string _phone;

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

        public string appDay{
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
    public void saveAppointmentInfo(){
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            UPDATE orders
            SET size = $option
            WHERE phone = $phone
                ";
            commandUpdate.Parameters.AddWithValue("$size", Size);
            // commandUpdate.Parameters.AddWithValue("$phone", Phone);
            int nRows = commandUpdate.ExecuteNonQuery();
            if(nRows == 0){
                var commandInsert = connection.CreateCommand();
                commandInsert.CommandText =
                @"
        INSERT INTO orders(size, phone)
        VALUES($size, $phone)
    ";
                commandInsert.Parameters.AddWithValue("$size", Size);
                commandInsert.Parameters.AddWithValue("$phone", Phone);
                int nRowsInserted = commandInsert.ExecuteNonQuery();

            }
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
            WHERE P.NAME = $name AND P.CONTACT_NO = $phone AND DATE(A.APP_DATE) > DATE(NOW())
            ORDER BY A.APP_DATE LIMIT 1
                ";
            commandUpdate.Parameters.AddWithValue("$name", patientName);
            commandUpdate.Parameters.AddWithValue("$phone", Phone);

            using(var reader = commandUpdate.ExecuteReader())
            {
                while(reader.Read())
                {
                    patientName = reader.getString(0);
                    age = reader.getString(1);
                    Phone = reader.getString(2);
                    referenceId = reader.getString(3);
                    appTime = reader.getString(4);
                    appDate = reader.getString(5);
                    reason = reader.getString(6);
                    appDay = reader.getString(7); 
                }
            }
        }
        string returnText = "Reference ID: "+referenceId+"\nPatient Name: "+patientName+"\nPatient Age: "+age+"\nAppointment Date: "+appDate+"\nApponintment Time: "+appTime+"\nReason: "+reason;
        retun returnText;
    }
    public string viewAppointmentInfoByID()
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            SELECT APP_TIME, APP_DATE 
            FROM APPOINTMENT
            WHERE REFERENCE_ID = $referenceid
                ";
            commandUpdate.Parameters.AddWithValue("$referenceid", referenceid);
            using(var reader = commandUpdate.ExecuteReader())
            {
                while(reader.Read())
                {
                    appTime = reader.getString(0);
                    appDate = reader.getString(1);
                }
            }
        }
        string returnText = "Your appointment information is : \nReference ID: "+referenceid+"\nAppointment Date: "+appDate+"\nApponintment Time: "+appTime;
        retun returnText;

    }

/////////////////////////////////////////////////////////////////////////////////////////////
    public void savePatientInfo()
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            UPDATE PATIENT
            SET AGE = $age
            WHERE phone = $phone AND NAME = $name
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
                INSERT INTO PATIENT(NAME, AGE, CONTACT_NO)
                VALUES($name, $age, $phone)
                ";
                commandInsert.Parameters.AddWithValue("$name", patientName);
                commandInsert.Parameters.AddWithValue("$age", age);
                commandInsert.Parameters.AddWithValue("$phone", Phone);
                int nRowsInserted = commandInsert.ExecuteNonQuery();  
            } 
        }

    }
    public void updateAppointmentInfo(){
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            UPDATE orders
            SET size = $option
            WHERE phone = $phone
                ";
            commandUpdate.Parameters.AddWithValue("$size", Size);
            // commandUpdate.Parameters.AddWithValue("$phone", Phone);
            int nRows = commandUpdate.ExecuteNonQuery();
            if(nRows == 0){
                var commandInsert = connection.CreateCommand();
                commandInsert.CommandText =
                @"
        INSERT INTO orders(size, phone)
        VALUES($size, $phone)
    ";
                commandInsert.Parameters.AddWithValue("$size", Size);
                commandInsert.Parameters.AddWithValue("$phone", Phone);
                int nRowsInserted = commandInsert.ExecuteNonQuery();

            }
        }

    }

public void deleteAppointmentInfoByID()
{
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            UPDATE orders
            SET size = $option
            WHERE phone = $phone
                ";
            commandUpdate.Parameters.AddWithValue("$size", Size);
            // commandUpdate.Parameters.AddWithValue("$phone", Phone);
            int nRows = commandUpdate.ExecuteNonQuery();
            if(nRows == 0){
                var commandInsert = connection.CreateCommand();
                commandInsert.CommandText =
                @"
        INSERT INTO orders(size, phone)
        VALUES($size, $phone)
    ";
                commandInsert.Parameters.AddWithValue("$size", Size);
                commandInsert.Parameters.AddWithValue("$phone", Phone);
                int nRowsInserted = commandInsert.ExecuteNonQuery();

            }
        }

    }

    public void deleteAppointmentInfoByName()
    {
        using (var connection = new SqliteConnection(DB.GetConnectionString()))
        {
            connection.Open();

            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText =
            @"
            UPDATE orders
            SET size = $option
            WHERE phone = $phone
                ";
            commandUpdate.Parameters.AddWithValue("$size", Size);
            // commandUpdate.Parameters.AddWithValue("$phone", Phone);
            int nRows = commandUpdate.ExecuteNonQuery();
            if(nRows == 0){
                var commandInsert = connection.CreateCommand();
                commandInsert.CommandText =
                @"
            INSERT INTO orders(size, phone)
            VALUES($size, $phone)
            ";
            commandInsert.Parameters.AddWithValue("$size", Size);
            commandInsert.Parameters.AddWithValue("$phone", Phone);
            int nRowsInserted = commandInsert.ExecuteNonQuery();

            }
        }

    }

       public List<String> searchAvailableDay(){
           using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();

                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText =
                @"
                UPDATE orders
                SET size = $option
                WHERE phone = $phone
                 ";
                commandUpdate.Parameters.AddWithValue("$size", Size);
               // commandUpdate.Parameters.AddWithValue("$phone", Phone);
                int nRows = commandUpdate.ExecuteNonQuery();
                if(nRows == 0){
                    var commandInsert = connection.CreateCommand();
                    commandInsert.CommandText =
                    @"
            INSERT INTO orders(size, phone)
            VALUES($size, $phone)
        ";
                    commandInsert.Parameters.AddWithValue("$size", Size);
                    commandInsert.Parameters.AddWithValue("$phone", Phone);
                    int nRowsInserted = commandInsert.ExecuteNonQuery();

                }
            }

       }

       public List<String> searchAvailableTime(int optionForTime){
           using (var connection = new SqliteConnection(DB.GetConnectionString()))
            {
                connection.Open();

                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText =
                @"
                UPDATE orders
                SET size = $option
                WHERE phone = $phone
                 ";
                commandUpdate.Parameters.AddWithValue("$size", Size);
               // commandUpdate.Parameters.AddWithValue("$phone", Phone);
                int nRows = commandUpdate.ExecuteNonQuery();
                if(nRows == 0){
                    var commandInsert = connection.CreateCommand();
                    commandInsert.CommandText =
                    @"
            INSERT INTO orders(size, phone)
            VALUES($size, $phone)
        ";
                    commandInsert.Parameters.AddWithValue("$size", Size);
                    commandInsert.Parameters.AddWithValue("$phone", Phone);
                    int nRowsInserted = commandInsert.ExecuteNonQuery();

                }
            }

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
            WHERE P.NAME = $patient AND CONTACT_NO = $phone AND DATE(A.APP_DATE) > DATE(NOW())
            ORDER BY A.APP_DATE LIMIT 1;
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
                WHERE REFERENCE_ID = $reference AND DATE(A.APP_DATE) > DATE(NOW())
                ORDER BY A.APP_DATE;
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
    }
}
