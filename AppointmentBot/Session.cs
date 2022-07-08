using System;

namespace AppointmentBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, OPTION, BOOK, VIEW, RESCHEDULE, CANCEL
        }

        private State nCur = State.WELCOMING;
        private Appointment aApp;

        public Session(string sPhone){
            this.aApp = new Appointment();
            this.aApp.Phone = sPhone;
        }

        public String OnMessage(String sInMessage)
        {
            String sMessage = "Welcome to Group 4 Pediatrics Clinic! Please choose one of the below options to proceed: \n1. Book an appointment\n2.View an existing appointment\n3.Reschedule an appointment\n4.Cancel an appointment";
            switch (this.nCur)
            {
                case State.WELCOMING:
                    this.nCur = State.OPTION;
                    break;
                // case State.OPTION:
                //     this.oOrder.Size = sInMessage;
                //     this.oOrder.Save();
                //     sMessage = "What protein would you like on this  " + this.oOrder.Size + " Shawarama?";
                //     this.nCur = State.PROTEIN;
                //     break;
                // case State.PROTEIN:
                //     string sProtein = sInMessage;
                //     sMessage = "What toppings would you like on this  " + this.oOrder.Size + " " + sProtein + " Shawarama?";
                //     break;
                    

            }
            System.Diagnostics.Debug.WriteLine(sMessage);
            return sMessage;
        }

    }
}
