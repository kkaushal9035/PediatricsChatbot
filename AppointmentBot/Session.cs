using System;
using System.Collections.Generic;

namespace AppointmentBot
{
    public class Session
    {
        int age = -1, optionVal = -1, opt = -1, ageOption = -1, dayOption = -1, timeChoice = -1, viewOption = -1;
        List<String> availableDates = new List<String>();
        List<String> availableTimeSlot = new List<String>();
           
        private enum State
        {
            WELCOMING, OPTION,
            CANCEL_BYNAME, CANCEL_CONFIRMATIONBYNAME, CANCEL_BYREFERENCE, CANCEL_CONFIRMATIONBYID, CANCEL_OPTION,
            VIEW_GET_PATIENT, VIEW_APP_OPTIONS, VIEW_GET_REFERENCE, VIEW_OPTIONS,
            BOOK_DAY, BOOK_DATE, BOOK_TIME, BOOK_PATIENT_NAME, BOOK_PATIENT_AGE, 
            BOOK_PATIENT_AGE_CHOICE, BOOK_REASON, BOOK_CONFIRMATION, EXISTING_APP_OPTION,
            RESCHEDULE_OPTION, RESCHEDULE_BYREFERENCE, RESCHEDULE_BYNAME, RESCHEDULE_DAY, RESCHEDULE_DATE,
            RESCHEDULE_TIME, RESCHEDULE_CONFIRMATION
        }

        private State nCur = State.WELCOMING;
        private Appointment aApp;

        public Session(string sPhone){
            this.aApp = new Appointment();
            this.aApp.Phone = sPhone;
        }

        public List<String> OnMessage(String sInMessage)
        {
            List<String> sMessage = new List<String>();
            bool found = false;
            switch (this.nCur)
            {
                case State.WELCOMING:
                     sMessage.Add("Welcome to Group 4 Pediatrics Clinic!");
                     sMessage.Add("Please choose one of the below options to proceed: \n1.Book an appointment\n2.View an existing appointment\n3.Reschedule an appointment\n4.Cancel an appointment");
                     this.nCur = State.OPTION;
                     break;
                case State.OPTION:
                    this.aApp.option = sInMessage;
                    if(int.TryParse(this.aApp.option, out optionVal))
                   {
                    //based on option selected fetch information from user
                    switch (optionVal)
                    {
                        case 1:
                           sMessage.Add("Enter patient's name to book new appointment");
                           this.nCur = State.BOOK_PATIENT_NAME;
                            break;
                        case 2:
                            sMessage.Add("Choose from below to view details for your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and view details with Patient Name.");
                            this.nCur = State.VIEW_APP_OPTIONS;
                            break;
                        case 3:
                            sMessage.Add("Choose from below to reschedule your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and reschedule with Patient Name.");
                            this.nCur = State.RESCHEDULE_OPTION;
                            break;
                        case 4:
                            sMessage.Add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                            this.nCur = State.CANCEL_OPTION;
                            break;
                        default:
                            sMessage.Add("You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.");
                            break;
                    }
                   }
                   else
                   {
                     sMessage.Add("You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.");
                          
                   }
                    break;  
                case State.RESCHEDULE_OPTION:
                    if(int.TryParse(sInMessage, out opt))
                        {
                            switch (opt)
                            {
                                case 1:
                                    sMessage.Add("Enter the REFERENCE ID for your appointment");
                                    this.nCur = State.RESCHEDULE_BYREFERENCE;
                                    break;
                                case 2:
                                    sMessage.Add("Enter Patient's Name for your appointment");
                                    this.nCur = State.RESCHEDULE_BYNAME;
                                    break;
                                default:
                                    sMessage.Add("You have chosen incorrect option");
                                    sMessage.Add("Choose from below to reschedule your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and reschedule with Patient Name.");
                                    break;
                            }
                        }
                        else
                        {
                            sMessage.Add("You have chosen incorrect option");
                            sMessage.Add("Choose from below to reschedule your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and reschedule with Patient Name.");
                        }
                    break;
                case State.RESCHEDULE_BYREFERENCE:
                    this.aApp.referenceId = sInMessage;
                    found = this.aApp.searchByReferenceID();
                    if(found == false)
                    {
                        sMessage.Add("We could not find any record of upcoming appointment to reschedule for reference ID "+this.aApp.referenceId);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.Add("Your upcoming appointment information is:");
                        sMessage.Add(this.aApp.viewAppointmentInfoByID());
                        sMessage.Add("Choose option to select your preferred day to reschedule from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                        this.nCur = State.RESCHEDULE_DAY;
                    }
                    break;
                case State.RESCHEDULE_BYNAME:
                    this.aApp.patientName = sInMessage;
                    found = this.aApp.searchAppointmentByName();
                    if(found == false)
                    {
                        sMessage.Add("We could not find any record of upcoming appointment to reschedule for patient "+this.aApp.patientName);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.Add("Your upcoming appointment information is:");
                        sMessage.Add(this.aApp.viewAppointmentInfoByName());
                        sMessage.Add("Choose option to select your preferred day to reschedule from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                        this.nCur = State.RESCHEDULE_DAY;
                    }
                    break;
                case State.RESCHEDULE_DAY:
                    if(int.TryParse(sInMessage, out dayOption))
                    {
                        this.aApp.appDay = dayOption;
                        if(dayOption == 1 || dayOption == 2 || dayOption == 3 || dayOption == 4 || dayOption == 5 || dayOption == 6)
                        {
                            //fetch dates from DB based on days selected
                            availableDates = this.aApp.searchAvailableDay();
                            if(availableDates.Count > 4)
                            {
                                sMessage.Add("Please choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);
                            }
                            else
                            {
                               string msg = "";
                                for(int i=0; i<availableDates.Count; i++)
                                {
                                   msg = msg+ (i+1)+". "+availableDates[i]+"\n";
                                }
                                sMessage.Add("Please choose your availability for below dates for appointment:\n"+msg);
                            }
                            this.nCur = State.RESCHEDULE_DATE;
                        }
                        else
                        {
                            sMessage.Add("You have entered incorrect choice for days, please choose between 1 to 6.");
                        }
                    }
                    else
                    {
                        sMessage.Add("You have entered incorrect choice for days, please choose between 1 to 6.");
                    }
                    break;
                case State.RESCHEDULE_DATE:
                     if(int.TryParse(sInMessage, out optionVal))
                    {
                        if(availableDates.Count > 4)
                        {
                            switch(optionVal)
                            {
                                case 1:
                                    this.aApp.appDate = availableDates[0];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.RESCHEDULE_TIME;
                                    break;
                                case 2:
                                    this.aApp.appDate = availableDates[1];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.RESCHEDULE_TIME;
                                    break;
                                case 3:
                                    this.aApp.appDate = availableDates[2];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.RESCHEDULE_TIME;
                                    break;
                                case 4:
                                    this.aApp.appDate = availableDates[3];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.RESCHEDULE_TIME;
                                    break;
                                default:
                                    sMessage.Add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                                    break;
                            } 
                        }
                        else
                        {
                            if(optionVal >= 0 && optionVal < availableDates.Count)
                            {
                                this.aApp.appDate = availableDates[optionVal];
                                 sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                 this.nCur = State.RESCHEDULE_TIME;
                            }
                            else
                            {
                                string msg = "";
                                for(int i=0; i<availableDates.Count; i++)
                                {
                                   msg = msg+ (i+1)+". "+availableDates[i]+"\n";
                                }
                                sMessage.Add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n"+msg);  
                            }
                        }
                    }
                    else
                    {
                        sMessage.Add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                    }
                    break;
                case State.RESCHEDULE_TIME:
                    if(int.TryParse(sInMessage, out timeChoice ))
                    {
                        if(timeChoice == 1 || timeChoice == 2 || timeChoice == 3 )
                        {
                            //check DB for available slots between user's selection
                            availableTimeSlot = this.aApp.searchAvailableTime(timeChoice);
                            if(availableTimeSlot.Count > 0)
                            {
                                this.aApp.appTime = availableTimeSlot[0];
                                sMessage.Add("Please enter Y to confirm the below details to finalise your booking:");
                                sMessage.Add("Patient Name: "+this.aApp.patientName+"\nAge: "+this.aApp.age+"\nAppointment Date: "+this.aApp.appDate+"\nAppointment Time: "+this.aApp.appTime+"\nReason: "+this.aApp.reason);
                                this.nCur = State.RESCHEDULE_CONFIRMATION;
                            }
                            else
                            {
                                sMessage.Add("Sorry, no available slots for your chosen time slot.\nPlease choose another day for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.RESCHEDULE_DAY;
                            }
                        }
                        else
                        {
                            sMessage.Add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                        }
                    }
                    else
                    {
                       sMessage.Add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                    }
                    break;
                case State.RESCHEDULE_CONFIRMATION:
                    if(sInMessage.Equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        found = this.aApp.updateAppointmentInfo();
                        if(found == true)
                        {
                            if(!string.IsNullOrEmpty(this.aApp.patientName))
                            {
                                sMessage.Add(this.aApp.viewAppointmentInfoByName());
                            }
                            else if(!string.IsNullOrEmpty(this.aApp.referenceId))
                            {
                                sMessage.Add(this.aApp.viewAppointmentInfoByID());
                            }
                             sMessage.Add("Your appointment has been successfully rescheduled.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        }
                        else
                        {
                            sMessage.Add("Sorry, we did not find any upcoming appointment for you.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        }
                    } 
                    else
                    {
                        sMessage.Add("You have chosen to not reschedule an appointment with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    this.nCur = State.WELCOMING;
                    break;
                case State.VIEW_APP_OPTIONS:
                    if(int.TryParse(sInMessage, out opt))
                    {
                        switch (opt)
                        {
                            case 1:
                                sMessage.Add("Enter the REFERENCE ID for your appointment");
                                this.nCur = State.VIEW_GET_REFERENCE;
                                break;
                            case 2:
                                sMessage.Add("Enter Patient's Name for your appointment");
                                this.nCur = State.VIEW_GET_PATIENT;
                                break;
                            default:
                                sMessage.Add("You have chosen incorrect option");
                                sMessage.Add("Choose from below to view details for your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and view details with Patient Name.");
                                break;
                        }
                    }
                    else
                    {
                        sMessage.Add("You have chosen incorrect option");
                        sMessage.Add("Choose from below to view details for your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and view details with Patient Name.");
                    }
                    break;
                case State.VIEW_GET_PATIENT:
                    this.aApp.patientName = sInMessage;
                    found = this.aApp.searchAppointmentByName();
                    if(found == false)
                    {
                        sMessage.Add("We could not find any upcoming appointment for this patient from this phone number.\nThank you for contacting Group 4 Pediatrics Clinic.");
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.Add(this.aApp.viewAppointmentInfoByName() +"\n.Thank you for contacting Group 4 Pediatrics Clinic.");
                        this.nCur = State.WELCOMING;
                    }
                    break;
                case State.VIEW_GET_REFERENCE:
                    this.aApp.referenceId = sInMessage;
                    found = this.aApp.searchByReferenceID();
                    if(found == false)
                    {
                        sMessage.Add("You have entered incorrect reference ID for your appointment.\n Please choose from below:\n1. if you wish to re-enter your reference ID \n2. if you wish to book a new appointment.");
                        this.nCur = State.VIEW_OPTIONS;
                    }
                    else
                    {
                        //display information
                       sMessage.Add(this.aApp.viewAppointmentInfoByID() +"\n.Thank you for contacting Group 4 Pediatrics Clinic.");
                        this.nCur = State.WELCOMING;
                    }
                    
                    break;
                case State.VIEW_OPTIONS:
                    if(int.TryParse(sInMessage, out viewOption))
                    {
                        if(viewOption == 1)
                        {
                            sMessage.Add("Enter reference ID for your appointment to view details");
                            this.nCur = State.VIEW_GET_REFERENCE;
                        }
                        else if(viewOption == 2)
                        {
                            sMessage.Add("Enter patient's name to book new appointment");
                            this.nCur = State.BOOK_PATIENT_NAME;
                        }
                        else
                        {
                            sMessage.Add("You have chosen incorrect option, please choose from available options 1 or 2.");
                        }
                    }
                    break;
                case State.CANCEL_OPTION:
                    if(int.TryParse(sInMessage, out opt))
                        {
                            switch (opt)
                            {
                                case 1:
                                    sMessage.Add("Enter the REFERENCE ID for your appointment");
                                    this.nCur = State.CANCEL_BYREFERENCE;
                                    break;
                                case 2:
                                    sMessage.Add("Enter Patient's Name for your appointment");
                                    this.nCur = State.CANCEL_BYNAME;
                                    break;
                                default:
                                    sMessage.Add("You have chosen incorrect option");
                                    sMessage.Add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                                    break;
                            }
                        }
                        else
                        {
                            sMessage.Add("You have chosen incorrect option");
                            sMessage.Add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                        }
                    break;
                case State.CANCEL_BYNAME:
                    this.aApp.patientName = sInMessage;
                    found = this.aApp.searchAppointmentByName();
                    if(found == false)
                    {
                        sMessage.Add("We could not find any record of upcoming appointment for patient "+this.aApp.referenceId);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.Add(this.aApp.viewAppointmentInfoByName() +"\n.Please enter Y to cancel this appointment.");
                        this.nCur = State.CANCEL_CONFIRMATIONBYNAME;
                    }
                    break;
                case State.CANCEL_BYREFERENCE:
                    this.aApp.referenceId = sInMessage;
                    found = this.aApp.searchByReferenceID();
                    if(found == false)
                    {
                        sMessage.Add("We could not find any record of upcoming appointment for reference ID "+this.aApp.referenceId);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.Add(this.aApp.viewAppointmentInfoByID() +"\n.Please enter Y to cancel this appointment.");
                        this.nCur = State.CANCEL_CONFIRMATIONBYID;
                    }
                    break;
                case State.CANCEL_CONFIRMATIONBYID:
                    if(sInMessage.Equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        found = this.aApp.deleteAppointmentInfoByID();
                        if(found == true)
                        {
                            sMessage.Add("Your appointment has been successfully cancelled.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        }
                        else
                        {
                            sMessage.Add("Sorry, we are unable to find any upcoming appointment for this reference ID to cancel");
                        }
                    }
                    else
                    {
                        sMessage.Add("You have chosen to not cancel your appointment "+this.aApp.referenceId+" with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    this.nCur = State.WELCOMING;
                    break;
                case State.CANCEL_CONFIRMATIONBYNAME:
                    if(sInMessage.Equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        found = this.aApp.deleteAppointmentInfoByName();
                        if(found == true)
                        {
                            sMessage.Add("Your appointment has been successfully cancelled.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        }
                        else
                        {
                            sMessage.Add("Sorry, we are unable to find any upcoming appointment for this patient to cancel");
                        }
                    }
                    else
                    {
                        sMessage.Add("You have chosen to not cancel your appointment "+this.aApp.patientName+" with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    this.nCur = State.WELCOMING;
                    break;
                case State.BOOK_PATIENT_NAME:
                    this.aApp.patientName = sInMessage;
                    found=this.aApp.searchAppointmentByName();
                    if(found == true)
                    {
                        //display information
                        sMessage.Add("You already have an upcoming appointment");
                        sMessage.Add(this.aApp.viewAppointmentInfoByName());
                        sMessage.Add("Enter 1 to reschedule the existing appointment\nEnter 2 to keep the appointment as it is and exit.");
                        this.nCur = State.EXISTING_APP_OPTION;
                    }
                    else
                    {
                        //check if appointment already exists for this patient
                        sMessage.Add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                        this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                    }
                    break;
                case State.EXISTING_APP_OPTION:
                    if(int.TryParse(sInMessage, out opt))
                    {
                        switch (opt)
                        {
                            case 1:
                                sMessage.Add("Choose option to select your preferred day from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.RESCHEDULE_DAY;
                                break;
                            case 2:
                                sMessage.Add("Your appointment is scheduled as before.\nThank you for contacting Group 4 Pediatrics Clinic!");
                                this.nCur = State.WELCOMING;
                                break;
                            default:
                                sMessage.Add("You have chosen incorrect option");
                                sMessage.Add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                                break;
                        }
                    }
                    else
                    {
                        sMessage.Add("You have chosen incorrect option");
                        sMessage.Add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                    }
                    break;
                case State.BOOK_PATIENT_AGE_CHOICE:
                    if(int.TryParse(sInMessage, out ageOption))
                    {
                        switch (ageOption)
                        {
                            case 1:
                                sMessage.Add("Enter age in months from 1 to 11. Expected Format is numeric value only like 1, 2, 3,.. till 11.");
                                this.nCur = State.BOOK_PATIENT_AGE;
                                break;
                            case 2:
                                sMessage.Add("Enter age in days from 1 to 29. Expected Format is numeric value only like 1, 2, 3,.. till 29.");
                                this.nCur = State.BOOK_PATIENT_AGE;
                                break;
                            case 3:
                                sMessage.Add("Enter age in years from 1 to 18. Expected Format is numeric value only like 1, 2, 3,.. till 18.");
                                this.nCur = State.BOOK_PATIENT_AGE;
                                break;
                            default:
                                sMessage.Add("You have entered incorrect input option. \n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");  
                                break;
                        }
                    }
                    else
                    {
                      sMessage.Add("You have entered incorrect input option. \n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");  
                    }
                    break;
                case State.BOOK_PATIENT_AGE:
                    if(int.TryParse(sInMessage, out age))
                    {
                        switch (ageOption)
                        {
                            case 1:
                                if(age < 12 && age >= 1)
                                {
                                    this.aApp.age = sInMessage + " month";
                                    this.aApp.savePatientInfo();
                                    sMessage.Add("Please enter the reason for your visit.");
                                    this.nCur = State.BOOK_REASON;
                                }
                                else if(age >= 12)
                                {
                                    sMessage.Add("You have entered age greater than or equal to 12 months for this category.");
                                    sMessage.Add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                else
                                {
                                    sMessage.Add("You have entered incorrect age for this category.");
                                    sMessage.Add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                break;
                            case 2:
                                if(age <= 29 && age >= 1)
                                {
                                    this.aApp.age = sInMessage + " day";
                                    this.aApp.savePatientInfo();
                                    sMessage.Add("Please enter the reason for your visit.");
                                    this.nCur = State.BOOK_REASON;
                                }
                                else if(age > 29)
                                {
                                    sMessage.Add("You have entered age greater than 29 days for this category.");
                                    sMessage.Add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                else
                                {
                                    sMessage.Add("You have entered incorrect age for this category.");
                                    sMessage.Add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                break;
                            case 3:
                                if(age <= 18 && age >=1)
                                {
                                    this.aApp.age = sInMessage + " year";
                                    this.aApp.savePatientInfo();
                                    sMessage.Add("Please enter the reason for your visit.");
                                    this.nCur = State.BOOK_REASON;
                                }
                                else if(age > 18)
                                {
                                    sMessage.Add("Sorry, the pediatrics clinic only deals with patients under the age of 18 years.\n Thank you for contacting Group 4 Pediatrics Clinic.");
                                    this.nCur = State.WELCOMING;
                                }
                                else
                                {
                                    sMessage.Add("You have entered incorrect age for this category.");
                                    sMessage.Add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                break;
                        }
                    }
                    else
                    {
                        sMessage.Add("You have entered incorrect input for age.\n Please enter numeric value only for age.");  
                    }
                    break;  
                case State.BOOK_REASON:
                    this.aApp.reason = sInMessage;
                    //display options to user for working days
                    sMessage.Add("Choose option to select your preferred day from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                    this.nCur = State.BOOK_DAY;
                    break; 
                case State.BOOK_DAY:
                    if(int.TryParse(sInMessage, out dayOption))
                    {
                        this.aApp.appDay = dayOption;
                        if(dayOption == 1 || dayOption == 2 || dayOption == 3 || dayOption == 4 || dayOption == 5 || dayOption == 6)
                        {
                            //fetch dates from DB based on days selected
                            availableDates = this.aApp.searchAvailableDay();
                            if(availableDates.Count > 4)
                            {
                                sMessage.Add("Please choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);
                            }
                            else if(availableDates.Count <= 0)
                            {
                                sMessage.Add("Sorry, there are no available dates for appointment on the day of your selection.");
                                sMessage.Add("Choose option to select your preferred day from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.BOOK_DAY;
                            }
                            else
                            {
                                string msg = "";
                                for(int i=0; i<availableDates.Count; i++)
                                {
                                   msg = msg+ (i+1)+". "+availableDates[i]+"\n";
                                }
                                sMessage.Add("Please choose your availability for below dates for appointment:\n"+msg);
                            }
                            this.nCur = State.BOOK_DATE;
                        }
                        else
                        {
                            sMessage.Add("You have entered incorrect choice for days, please choose between 1 to 6.");
                        }
                    }
                    else
                    {
                        sMessage.Add("You have entered incorrect choice for days, please choose between 1 to 6.");
                    }
                    break;  
                case State.BOOK_DATE:
                    if(int.TryParse(sInMessage, out optionVal))
                    {
                        if(availableDates.Count > 4)
                        {
                            switch(optionVal)
                            {
                                case 1:
                                    this.aApp.appDate = availableDates[0];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.BOOK_TIME;
                                    break;
                                case 2:
                                    this.aApp.appDate = availableDates[1];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.BOOK_TIME;
                                    break;
                                case 3:
                                    this.aApp.appDate = availableDates[2];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.BOOK_TIME;
                                    break;
                                case 4:
                                    this.aApp.appDate = availableDates[3];
                                    sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                    this.nCur = State.BOOK_TIME;
                                    break;
                                default:
                                    sMessage.Add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                                    break;
                            } 
                        }
                        else
                        {
                            if(optionVal > 0 && optionVal < availableDates.Count+1)
                            {
                                this.aApp.appDate = availableDates[optionVal];
                                sMessage.Add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.BOOK_TIME;
                            }
                            else
                            {
                                string msg = "";
                                for(int i=0; i<availableDates.Count; i++)
                                {
                                   msg = msg+ (i+1)+". "+availableDates[i]+"\n";
                                }
                                sMessage.Add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n"+msg);  
                            }
                        }
                    }
                    else
                    {
                        sMessage.Add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                    }
                    break;
                case State.BOOK_TIME:
                    if(int.TryParse(sInMessage, out timeChoice ))
                    {
                        if(timeChoice == 1 || timeChoice == 2 || timeChoice == 3 )
                        {
                            //check DB for available slots between user's selection
                            availableTimeSlot = this.aApp.searchAvailableTime(timeChoice);
                            if(availableTimeSlot.Count > 0)
                            {
                                this.aApp.appTime = availableTimeSlot[0];
                                sMessage.Add("Please enter Y to confirm the below details to finalise your booking:");
                                sMessage.Add("Patient Name: "+this.aApp.patientName+"\nAge: "+this.aApp.age+"\nAppointment Date: "+this.aApp.appDate+"\nAppointment Time: "+this.aApp.appTime+"\nReason: "+this.aApp.reason);
                                this.nCur = State.BOOK_CONFIRMATION;
                            }
                            else
                            {
                                sMessage.Add("Sorry, no available slots for your chosen time slot.\nPlease choose another day for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.BOOK_DAY;
                            }
                        }
                        else
                        {
                            sMessage.Add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                        }
                    }
                    else
                    {
                       sMessage.Add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                    }
                    break;
                case State.BOOK_CONFIRMATION:
                    if(sInMessage.Equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        this.aApp.saveAppointmentInfo();
                        sMessage.Add("Your appointment has been successfully booked.\n\n"+this.aApp.viewAppointmentInfoByID() + "\n\nThank you for contacting Group 4 Pediatrics Clinic!");
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        sMessage.Add("You have chosen to not book an appointment with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        this.nCur = State.WELCOMING;
                    }
                    break;
            }
            sMessage.ForEach(delegate (String sMessages)
            {
                System.Diagnostics.Debug.WriteLine(sMessages);
            });
            return sMessage;
        }
    }
}
