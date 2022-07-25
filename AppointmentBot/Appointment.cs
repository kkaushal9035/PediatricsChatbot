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

        public void savePatientInfo()
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

    public string viewAppointmentInfo()
    {
        
    }
       public string viewAppointmentInfoByID(string referenceid)
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

       public void updateAppointment(){
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

    public void deleteAppointmentInfo()
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

       public void searchPatientInfo(){
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

       public void searchForUpcomingAppointment(){
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

       public bool searchByReferenceID(string reference){
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
                return true;
            }

       }
}
