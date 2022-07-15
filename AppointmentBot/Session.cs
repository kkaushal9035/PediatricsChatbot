using System;

namespace AppointmentBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, OPTION, BOOK, BOOK_DAY, BOOK_TIME, BOOK_PATIENT_NAME, BOOK_PATIENT_AGE, BOOK_REASON, VIEW, VIEW_GET_REFERENCE, RESCHEDULE, RESCHEDULE_GET_REFERENCE, RESCHEDULE_TIME, RESCHEDULE_DATE, CANCEL, CANCEL_REFERENCE, VIEW_OPTIONS
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
                            sMessage = "You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.";
                            break;
                    }
                   }
                   else
                   {
                     sMessage = "You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.";
                          
                   }
                    break;  
                case State.VIEW_GET_REFERENCE:
                    this.aApp.referenceId = sInMessage;
                    bool found = this.aApp.searchByReferenceID(this.aApp.referenceId);
                    if(found == false)
                    {
                        sMessage = "You have entered incorrect reference ID for your appointment.\n Please choose from below:\n1. if you wish to re-enter your reference ID \n2. if you wish to book a new appointment.";
                        this.nCur = State.VIEW_OPTIONS;
                    }
                    else
                    {
                        //display information
                       sMessage = this.aApp.viewAppointmentInfo(this.aApp.referenceId) +"\n.Thank you for contacting Group 4 Pediatrics Clinic.";
                        this.nCur = State.WELCOMING;
                    }
                    
                    break;
                case State.VIEW_OPTIONS:
                    if(int.TryParse(sInMessage, out int viewOption))
                    {
                        if(viewOption == 1)
                        {
                            sMessage = "Enter reference ID for your appointment to view details";
                            this.nCur = State.VIEW_GET_REFERENCE;
                        }
                        else if(viewOption == 2)
                        {
                            sMessage = "Enter patient's name to book new appointment";
                            this.nCur = State.BOOK_PATIENT_NAME;
                        }
                        else
                        {
                            sMessage = "You have chosen incorrect option, please choose from available options 1 or 2.";
                        }
                    }
                    break;
                case State.RESCHEDULE_GET_REFERENCE:
                    break;
                case State.CANCEL_REFERENCE:
                    this.aApp.referenceId = sInMessage;
                    bool found = this.aApp.searchByReferenceID(this.aApp.referenceId);
                    if(found == false)
                    {
                        sMessage = "You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.";
                    }
                    else
                    {
                        //display information
                       sMessage = this.aApp.viewAppointmentInfo(this.aApp.referenceId) +"\n.Thank you for contacting Group 4 Pediatrics Clinic.";
                        this.nCur = State.WELCOMING;
                    }
                    break;
                case State.BOOK_PATIENT_NAME:
                    this.aApp.patientName = sInMessage;
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
                         if(this.aApp.age.IndexOf("newborn",StringComparison.OrdinalIgnoreCase) >= 0)
                         {
                             sMessage = "Enter reason for visit";
                              this.nCur = State.BOOK_REASON;
                         }
                         else
                         {
                             sMessage = "You have entered incorrect input for age, please enter age in months from 0 to 216 or \"newborn\" for patient less than a month old.";  
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
                    this.aApp.appDay = sInMessage;
                    if(this.aApp.appDay == 1 || this.aApp.appDay == 2 || this.aApp.appDay == 3 || this.aApp.appDay == 4 || this.aApp.appDay == 5 || this.aApp.appDay == 6)
                    {
                         //fetch dates from DB based on days selected

                         //add check if the selected day is available else stay in current state and ask user again
                         sMessage = "Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM";
                         this.nCur = State.BOOK_TIME;
                    }
                    else
                    {
                        sMessage = "You have entered incorrect choice for days, please choose between 1 to 6.";
                    }
                    break;  
                case State.BOOK_TIME:
                    this.aApp.appTime = sInMessage;
                    if(this.aApp.appTime == 1 || this.aApp.appTime == 2 || this.aApp.appTime == 3 )
                    {
                        //check DB for available slots between user's selection
                    }
                    else
                    {
                        sMessage = "You have entered incorrect choice for time slots, please choose between 1, 2, or 3.";
                    }
                    break;
            }
            System.Diagnostics.Debug.WriteLine(sMessage);
            return sMessage;
        }
    }
}
