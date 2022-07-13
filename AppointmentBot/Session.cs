using System;

namespace AppointmentBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, OPTION, BOOK, BOOK_DAY, BOOK_TIME, BOOK_PATIENT_NAME, BOOK_PATIENT_AGE, BOOK_REASON, VIEW, VIEW_GET_REFERENCE, RESCHEDULE, RESCHEDULE_GET_REFERENCE, RESCHEDULE_TIME, RESCHEDULE_DATE, CANCEL, CANCEL_REFERENCE
        }

        private State nCur = State.WELCOMING;
        private Appointment aApp;

        public Session(string sPhone){
            this.aApp = new Appointment();
        }

        public String OnMessage(String sInMessage)
        {
            String  sMessage = "Welcome to Group 4 Pediatrics Clinic! Please choose one of the below options to proceed: \n1.Book an appointment\n2.View an existing appointment\n3.Reschedule an appointment\n4.Cancel an appointment";
            int optionVal;
            switch (this.nCur)
            {
                case State.WELCOMING:
                    this.nCur = State.OPTION;
                    break;
                case State.OPTION:
                    this.aApp.option = sInMessage;
                    Console.WriteLine("user input :" +sInMessage);
                   if(int.TryParse(this.aApp.option, out optionVal))
                   {
                    //based on option selected fetch information from user
                    switch (optionVal)
                    {
                        case 1:
                           sMessage = "Enter patient's name to book new appointment";
                           this.nCur = State.BOOK_PATIENT_NAME;
                            break;
                        case 2:
                            sMessage = "Enter reference ID for your appointment to view details";
                            this.nCur = State.VIEW_GET_REFERENCE;
                            break;
                        case 3:
                            sMessage = "Enter reference ID for your appointment to reschedule";
                            this.nCur = State.RESCHEDULE_GET_REFERENCE;
                            break;
                        case 4:
                            sMessage = "Enter reference ID for your appointment to cancel appointment";
                            this.nCur = State.CANCEL_REFERENCE;
                            break;
                        default:
                            sMessage = "You have chosen incorrect option, thank you for contacting Pediatrics clinic.";
                            this.nCur = State.WELCOMING;
                            break;
                    }
                   }
                   else
                   {
                     sMessage = "You have chosen incorrect option, thank you for contacting Pediatrics clinic.";
                     this.nCur = State.WELCOMING;       
                   }
                    break;  
                case State.BOOK_PATIENT_NAME:
                    this.aApp.name = sInMessage;
                    sMessage = "Enter Patient's Age in months.\nFor example: For a 2 year old, enter 24. \nIf Patient is less than 1 month old, enter \"newborn\".";
                    this.nCur = State.BOOK_PATIENT_AGE;
                    break;
                case State.BOOK_PATIENT_AGE:
                    this.aApp.age = sInMessage;
                    if(int.TryParse(this.aApp.age, out optionVal))
                    {
                        if(optionVal < 216 && optionVal > 0)
                        {
                            sMessage = "Enter reason for visit";
                            this.nCur = State.BOOK_REASON;
                        }
                        else
                        {
                            sMessage = "The patient is not eligible for a pediatric checkup. The age limit for pediatrics patients is 18 years (216 months).\nThank you for contacting Pediatrics clinic.";
                            this.nCur = State.WELCOMING;
                        }
                    }
                    else
                    {
                        if(this.app.age.indexOf("newborn",StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            sMessage = "Enter reason for visit";
                             this.nCur = State.BOOK_REASON;
                        }
                        else
                        {
                            sMessage = "You have entered incorrect input for age, thank you for contacting Pediatrics clinic.";
                            this.nCur = State.WELCOMING;  
                        }
                    }
                    break;  
                case State.BOOK_REASON:
                    this.aApp.reason = sInMessage;
                    //display options to user for working days
                    sMessage = "Choose option to select your preferred day from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday";
                    this.nCur = State.BOOK_DAY;
                    break; 
                case State.BOOK_DAY:
                    break;  
            }
            System.Diagnostics.Debug.WriteLine(sMessage);
            return sMessage;
        }
    }
}
