using Microsoft.Data.Sqlite;

namespace AppointmentBot
{
    public class Appointment
    {
        private string _option;
        private string _age;
        private string _patientName;
        private string _reason;

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
        // public void Save(){
        //    using (var connection = new SqliteConnection(DB.GetConnectionString()))
        //     {
        //         connection.Open();

        //         var commandUpdate = connection.CreateCommand();
        //         commandUpdate.CommandText =
        //         @"
        //         UPDATE orders
        //         SET size = $option
        //         WHERE phone = $phone
        //          ";
        //         commandUpdate.Parameters.AddWithValue("$size", Size);
        //        // commandUpdate.Parameters.AddWithValue("$phone", Phone);
        //         int nRows = commandUpdate.ExecuteNonQuery();
        //         if(nRows == 0){
        //             var commandInsert = connection.CreateCommand();
        //             commandInsert.CommandText =
        //             @"
        //     INSERT INTO orders(size, phone)
        //     VALUES($size, $phone)
        // ";
        //             commandInsert.Parameters.AddWithValue("$size", Size);
        //             commandInsert.Parameters.AddWithValue("$phone", Phone);
        //             int nRowsInserted = commandInsert.ExecuteNonQuery();

        //         }
        //     }

       // }
    }
}
