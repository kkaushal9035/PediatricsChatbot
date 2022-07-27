using System;

namespace AppointmentBot
{
    public class Session
    {
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
            List<String> availableDates = new List<String>();
            List<String> availableTimeSlot = new List<String>();
            int age, optionVal, ageOption;
            switch (this.nCur)
            {
                case State.WELCOMING:
                     sMessage.add("Welcome to Group 4 Pediatrics Clinic!");
                     sMessage.add("Please choose one of the below options to proceed: \n1.Book an appointment\n2.View an existing appointment\n3.Reschedule an appointment\n4.Cancel an appointment");
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
                           sMessage.add("Enter patient's name to book new appointment");
                           this.nCur = State.BOOK_PATIENT_NAME;
                            break;
                        case 2:
                            sMessage.add("Choose from below to view details for your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and view details with Patient Name.");
                            this.nCur = State.VIEW_APP_OPTIONS;
                            break;
                        case 3:
                            sMessage.add("Choose from below to reschedule your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and reschedule with Patient Name.");
                            this.nCur = State.RESCHEDULE_OPTION;
                            break;
                        case 4:
                            sMessage.add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                            this.nCur = State.CANCEL_OPTION;
                            break;
                        default:
                            sMessage.add("You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.");
                            break;
                    }
                   }
                   else
                   {
                     sMessage.add("You have chosen incorrect option, please choose from available options 1, 2, 3, or 4.");
                          
                   }
                    break;  
                case State.RESCHEDULE_OPTION:
                    if(int.TryParse(sInMessage, out int opt))
                        {
                            switch (opt)
                            {
                                case 1:
                                    sMessage.add("Enter the REFERENCE ID for your appointment");
                                    this.nCur = State.RESCHEDULE_BYREFERENCE;
                                    break;
                                case 2:
                                    sMessage.add("Enter Patient's Name for your appointment");
                                    this.nCur = State.RESCHEDULE_BYNAME;
                                    break;
                                default:
                                    sMessage.add("You have chosen incorrect option");
                                    sMessage.add("Choose from below to reschedule your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and reschedule with Patient Name.");
                                    break;
                            }
                        }
                        else
                        {
                            sMessage.add("You have chosen incorrect option");
                            sMessage.add("Choose from below to reschedule your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and reschedule with Patient Name.");
                        }
                    break;
                case State.RESCHEDULE_BYREFERENCE:
                    this.aApp.referenceId = sInMessage;
                    bool found = this.aApp.searchByReferenceID(this.aApp.referenceId);
                    if(found == false)
                    {
                        sMessage.add("We could not find any record of upcoming appointment to reschedule for reference ID "+this.aApp.referenceId);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.add("Your upcoming appointment information is:");
                        sMessage.add(this.aApp.viewAppointmentInfoByID(this.aApp.referenceId));
                        sMessage.add("Choose option to select your preferred day to reschedule from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                        this.nCur = State.RESCHEDULE_DAY;
                    }
                    break;
                case State.RESCHEDULE_BYNAME:
                    this.aApp.patientName = sInMessage;
                    bool found = this.aApp.searchAppointmentByName();
                    if(found == false)
                    {
                        sMessage.add("We could not find any record of upcoming appointment to reschedule for patient "+this.aApp.patientName);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.add("Your upcoming appointment information is:");
                        sMessage.add(this.aApp.viewAppointmentInfoByName());
                        sMessage.add("Choose option to select your preferred day to reschedule from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                        this.nCur = State.RESCHEDULE_DAY;
                    }
                    break;
                case State.RESCHEDULE_DAY:
                    if(int.TryParse(sInMessage, out int dayOption))
                    {
                        this.aApp.appDay = sInMessage;
                        if(dayOption == 1 || dayOption == 2 || dayOption == 3 || dayOption == 4 || dayOption == 5 || dayOption == 6)
                        {
                            //fetch dates from DB based on days selected
                            availableDates = searchAvailableDay(this.aApp.appDay);
                            sMessage.add("Please choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);
                            this.nCur = State.RESCHEDULE_DATE;
                        }
                        else
                        {
                            sMessage.add("You have entered incorrect choice for days, please choose between 1 to 6.");
                        }
                    }
                    else
                    {
                        sMessage.add("You have entered incorrect choice for days, please choose between 1 to 6.");
                    }
                    break;
                case State.RESCHEDULE_DATE:
                     if(int.TryParse(sInMessage, out optionVal))
                    {
                        switch(optionVal)
                        {
                            case 1:
                                this.aApp.appDate = availableDates[0];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.RESCHEDULE_TIME;
                                break;
                            case 2:
                                this.aApp.appDate = availableDates[1];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.RESCHEDULE_TIME;
                                break;
                            case 3:
                                this.aApp.appDate = availableDates[2];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.RESCHEDULE_TIME;
                                break;
                            case 4:
                                this.aApp.appDate = availableDates[3];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.RESCHEDULE_TIME;
                                break;
                            default:
                                sMessage.add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                                break;
                        } 
                    }
                    else
                    {
                        sMessage.add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                    }
                    break;
                case State.RESCHEDULE_TIME:
                    if(int.TryParse(sInMessage, out int timeChoice ))
                    {
                        if(timeChoice == 1 || timeChoice == 2 || timeChoice == 3 )
                        {
                            //check DB for available slots between user's selection
                            availableTimeSlot = aApp.searchAvailableTime(timeChoice);
                            if(availableTimeSlot[0].equals("false", StringComparison.OrdinalIgnoreCase))
                            {
                                sMessage.add("Sorry, no available slots for your chosen time slot.\nPlease choose another day for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.RESCHEDULE_DAY;
                            }
                            else
                            {
                                this.aApp.appTime = availableTimeSlot[0];
                                sMessage.add("Please enter Y to confirm the below details to finalise your booking:");
                                sMessage.add("Patient Name: "+this.aApp.patientName+"\nAge: "+this.aApp.age+"\nAppointment Date: "+this.aApp.appDate+"\nAppointment Time: "+this.aApp.appTime+"\nReason: "+this.aApp.reason);
                                this.nCur = State.RESCHEDULE_CONFIRMATION;
                            }
                        }
                        else
                        {
                            sMessage.add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                        }
                    }
                    else
                    {
                       sMessage.add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                    }
                    break;
                case State.RESCHEDULE_CONFIRMATION:
                    if(sInMessage.equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        this.aApp.updateAppointmentInfo();
                        sMessage.add("Your appointment has been successfully rescheduled.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        if(this.aApp.patientName != null)
                        {
                            sMessage.add(this.aApp.viewAppointmentInfoByName());
                        }
                        else if(this.aApp.referenceId !=null)
                        {
                            sMessage.add(this.aApp.viewAppointmentInfoByID());
                        }
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        sMessage.add("You have chosen to not reschedule an appointment with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        this.nCur = State.WELCOMING;
                    }
                    break;
                case State.VIEW_APP_OPTIONS:
                    if(int.TryParse(sInMessage, out int opt))
                    {
                        switch (opt)
                        {
                            case 1:
                                sMessage.add("Enter the REFERENCE ID for your appointment");
                                this.nCur = State.VIEW_GET_REFERENCE;
                                break;
                            case 2:
                                sMessage.add("Enter Patient's Name for your appointment");
                                this.nCur = State.VIEW_GET_PATIENT;
                                break;
                            default:
                                sMessage.add("You have chosen incorrect option");
                                sMessage.add("Choose from below to view details for your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and view details with Patient Name.");
                                break;
                        }
                    }
                    else
                    {
                        sMessage.add("You have chosen incorrect option");
                        sMessage.add("Choose from below to view details for your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and view details with Patient Name.");
                    }
                    break;
                case State.VIEW_GET_PATIENT:
                    this.aApp.patientName = sInMessage;
                    bool found = this.aApp.searchAppointmentByName();
                    if(found == false)
                    {
                        sMessage.add("We could not find any upcoming appointment for this patient from this phone number.\nThank you for contacting Group 4 Pediatrics Clinic.");
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                       sMessage.add(this.aApp.viewAppointmentInfoByName() +"\n.Thank you for contacting Group 4 Pediatrics Clinic.");
                        this.nCur = State.WELCOMING;
                    }
                    break;
                case State.VIEW_GET_REFERENCE:
                    this.aApp.referenceId = sInMessage;
                    bool found = this.aApp.searchByReferenceID();
                    if(found == false)
                    {
                        sMessage.add("You have entered incorrect reference ID for your appointment.\n Please choose from below:\n1. if you wish to re-enter your reference ID \n2. if you wish to book a new appointment.");
                        this.nCur = State.VIEW_OPTIONS;
                    }
                    else
                    {
                        //display information
                       sMessage.add(this.aApp.viewAppointmentInfoByID() +"\n.Thank you for contacting Group 4 Pediatrics Clinic.");
                        this.nCur = State.WELCOMING;
                    }
                    
                    break;
                case State.VIEW_OPTIONS:
                    if(int.TryParse(sInMessage, out int viewOption))
                    {
                        if(viewOption == 1)
                        {
                            sMessage.add("Enter reference ID for your appointment to view details");
                            this.nCur = State.VIEW_GET_REFERENCE;
                        }
                        else if(viewOption == 2)
                        {
                            sMessage.add("Enter patient's name to book new appointment");
                            this.nCur = State.BOOK_PATIENT_NAME;
                        }
                        else
                        {
                            sMessage.add("You have chosen incorrect option, please choose from available options 1 or 2.");
                        }
                    }
                    break;
                case State.CANCEL_OPTION:
                    if(int.TryParse(sInMessage, out int opt))
                        {
                            switch (opt)
                            {
                                case 1:
                                    sMessage.add("Enter the REFERENCE ID for your appointment");
                                    this.nCur = State.CANCEL_BYREFERENCE;
                                    break;
                                case 2:
                                    sMessage.add("Enter Patient's Name for your appointment");
                                    this.nCur = State.CANCEL_BYNAME;
                                    break;
                                default:
                                    sMessage.add("You have chosen incorrect option");
                                    sMessage.add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                                    break;
                            }
                        }
                        else
                        {
                            sMessage.add("You have chosen incorrect option");
                            sMessage.add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                        }
                    break;
                case State.CANCEL_BYNAME:
                    this.aApp.patientName = sInMessage;
                    bool found = this.aApp.searchAppointmentByName();
                    if(found == false)
                    {
                        sMessage.add("We could not find any record of upcoming appointment for patient "+this.aApp.referenceId);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.add(this.aApp.viewAppointmentInfoByName() +"\n.Please enter Y to cancel this appointment.");
                        this.nCur = State.CANCEL_CONFIRMATIONBYNAME;
                    }
                    break;
                case State.CANCEL_BYREFERENCE:
                    this.aApp.referenceId = sInMessage;
                    bool found = this.aApp.searchByReferenceID();
                    if(found == false)
                    {
                        sMessage.add("We could not find any record of upcoming appointment for reference ID "+this.aApp.referenceId);
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        //display information
                        sMessage.add(this.aApp.viewAppointmentInfoByID() +"\n.Please enter Y to cancel this appointment.");
                        this.nCur = State.CANCEL_CONFIRMATIONBYID;
                    }
                    break;
                case State.CANCEL_CONFIRMATIONBYID:
                    if(sInMessage.equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        this.aApp.deleteAppointmentInfoByID();
                        sMessage.add("Your appointment has been successfully cancelled.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    else
                    {
                        sMessage.add("You have chosen to not cancel your appointment "+this.aApp.referenceId+" with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    this.nCur = State.WELCOMING;
                    break;
                case State.CANCEL_CONFIRMATIONBYNAME:
                    if(sInMessage.equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        this.aApp.deleteAppointmentInfoByName();
                        sMessage.add("Your appointment has been successfully cancelled.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    else
                    {
                        sMessage.add("You have chosen to not cancel your appointment "+this.aApp.patientName+" with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
                    }
                    this.nCur = State.WELCOMING;
                    break;
                case State.BOOK_PATIENT_NAME:
                    this.aApp.patientName = sInMessage;
                    bool found=this.aApp.searchAppointmentByName();
                     if(found == true)
                    {
                        //display information
                        sMessage.add("You already have an upcoming appointment");
                        sMessage.add(this.aApp.viewAppointmentInfoByName());
                        sMessage.add("Enter 1 to reschedule the existing appointment\nEnter 2 to keep the appointment as it is and exit.");
                        this.nCur = State.EXISTING_APP_OPTION;
                    }
                    else
                    {
                        //check if appointment already exists for this patient
                        sMessage.add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                        this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                    }
                    break;
                case State.EXISTING_APP_OPTION:
                    if(int.TryParse(sInMessage, out int opt))
                    {
                        switch (opt)
                        {
                            case 1:
                                sMessage.add("Choose option to select your preferred day from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.RESCHEDULE_DAY;
                                break;
                            case 2:
                                sMessage.add("Your appointment is scheduled as before.\nThank you for contacting Group 4 Pediatrics Clinic!");
                                this.nCur = State.WELCOMING;
                                break;
                            default:
                                sMessage.add("You have chosen incorrect option");
                                sMessage.add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                                break;
                        }
                    }
                    else
                    {
                        sMessage.add("You have chosen incorrect option");
                        sMessage.add("Choose from below to cancel your upcoming appointment:\n1. If you have reference ID.\n2. If you do not have reference ID and cancel with Patient Name.");
                    }
                    break;
                case State.BOOK_PATIENT_AGE_CHOICE:
                    if(int.TryParse(sInMessage, out ageOption))
                    {
                        switch (ageOption)
                        {
                            case 1:
                                sMessage.add("Enter age in months from 1 to 11.");
                                this.nCur = State.BOOK_PATIENT_AGE;
                                break;
                            case 2:
                                sMessage.add("Enter age in days from 1 to 29.");
                                this.nCur = State.BOOK_PATIENT_AGE;
                                break;
                            case 3:
                                sMessage.add("Enter age in years from 1 to 18.");
                                this.nCur = State.BOOK_PATIENT_AGE;
                                break;
                            default:
                                sMessage.add("You have entered incorrect input option. \n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");  
                                break;
                        }
                    }
                    else
                    {
                      sMessage.add("You have entered incorrect input option. \n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");  
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
                                    this.aApp.age = sInMessage + " month(s)";
                                    this.aApp.savePatientInfo();
                                    this.nCur = State.BOOK_REASON;
                                }
                                else if(age >= 12)
                                {
                                    sMessage.add("You have entered age > or = 12 months for this category.");
                                    sMessage.add("Please enter 3 to choose correct category for age from 1 year to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                else
                                {
                                    sMessage.add("You have entered incorrect age for this category.");
                                    sMessage.add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                break;
                            case 2:
                                if(age <= 29 && age >= 1)
                                {
                                    this.aApp.age = sInMessage + " day(s)";
                                    this.aApp.savePatientInfo();
                                    this.nCur = State.BOOK_REASON;
                                }
                                else if(age > 29)
                                {
                                    sMessage.add("You have entered age > 29 days for this category.");
                                    sMessage.add("Please enter 1 to choose correct category for age from 1 month to 1 year.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                else
                                {
                                    sMessage.add("You have entered incorrect age for this category.");
                                    sMessage.add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                break;
                            case 3:
                                if(age <= 18 && age >=1)
                                {
                                    this.aApp.age = sInMessage + " year(s)";
                                    this.aApp.savePatientInfo();
                                    this.nCur = State.BOOK_REASON;
                                }
                                else if(age > 18)
                                {
                                    sMessage.add("Sorry, the pediatrics clinic only deals with patients under the age of 18 years.\n Thank you for contacting Group 4 Pediatrics Clinic.")
                                }
                                else
                                {
                                    sMessage.add("You have entered incorrect age for this category.");
                                    sMessage.add("Please choose from below to enter Patient's Age.\n1. If Patient's age is between 1 month and 1 year. \n2. If Patient is less than 1 month old.\n3. If Patient's age is between 1 to 18 years.");
                                    this.nCur = State.BOOK_PATIENT_AGE_CHOICE;
                                }
                                break;
                        }
                    }
                    else
                    {
                        sMessage.add("You have entered incorrect input for age.\n Please enter numeric value only for age.");  
                    }
                    break;  
                case State.BOOK_REASON:
                    this.aApp.reason = sInMessage;
                    //display options to user for working days
                    sMessage.add("Choose option to select your preferred day from below for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                    this.nCur = State.BOOK_DAY;
                    break; 
                case State.BOOK_DAY:
                    if(int.TryParse(sInMessage, out int dayOption))
                    {
                        this.aApp.appDay = sInMessage;
                        if(dayOption == 1 || dayOption == 2 || dayOption == 3 || dayOption == 4 || dayOption == 5 || dayOption == 6)
                        {
                            //fetch dates from DB based on days selected
                            availableDates = searchAvailableDay(this.aApp.appDay);
                            sMessage.add("Please choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);
                            this.nCur = State.BOOK_DATE;
                        }
                        else
                        {
                            sMessage.add("You have entered incorrect choice for days, please choose between 1 to 6.");
                        }
                    }
                    else
                    {
                        sMessage.add("You have entered incorrect choice for days, please choose between 1 to 6.");
                    }
                    break;  
                case State.BOOK_DATE:
                    if(int.TryParse(sInMessage, out optionVal))
                    {
                        switch(optionVal)
                        {
                            case 1:
                                this.aApp.appDate = availableDates[0];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.BOOK_TIME;
                                break;
                            case 2:
                                this.aApp.appDate = availableDates[1];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.BOOK_TIME;
                                break;
                            case 3:
                                this.aApp.appDate = availableDates[2];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.BOOK_TIME;
                                break;
                            case 4:
                                this.aApp.appDate = availableDates[3];
                                sMessage.add("Please choose from the below mentioned available slots:\n1.9AM - 11AM\n2.12PM-2PM\n3.3PM-5PM");
                                this.nCur = State.BOOK_TIME;
                                break;
                            default:
                                sMessage.add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                                break;
                        } 
                    }
                    else
                    {
                        sMessage.add("You have chosen incorrect option for appointment date.\nPlease choose your availability for below dates for appointment:\n1. "+availableDates[0]+"\n2. "+availableDates[1]+"\n3. "+availableDates[2]+"\n4. "+availableDates[3]);  
                    }
                    break;
                case State.BOOK_TIME:
                    if(int.TryParse(sInMessage, out int timeChoice ))
                    {
                        if(timeChoice == 1 || timeChoice == 2 || timeChoice == 3 )
                        {
                            //check DB for available slots between user's selection
                            availableTimeSlot = aApp.searchAvailableTime(timeChoice);
                            if(availableTimeSlot[0].equals("false", StringComparison.OrdinalIgnoreCase))
                            {
                                sMessage.add("Sorry, no available slots for your chosen time slot.\nPlease choose another day for appointment:\n1.Monday\n2.Tuesday\n3.Wednesday\n4.Thursday\n5.Friday\n6.Saturday");
                                this.nCur = State.BOOK_DAY;
                            }
                            else
                            {
                                this.aApp.appTime = availableTimeSlot[0];
                                sMessage.add("Please enter Y to confirm the below details to finalise your booking:");
                                sMessage.add("Patient Name: "+this.aApp.patientName+"\nAge: "+this.aApp.age+"\nAppointment Date: "+this.aApp.appDate+"\nAppointment Time: "+this.aApp.appTime+"\nReason: "+this.aApp.reason);
                                this.nCur = State.BOOK_CONFIRMATION;
                            }
                        }
                        else
                        {
                            sMessage.add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                        }
                    }
                    else
                    {
                       sMessage.add("You have entered incorrect choice for time slots, please choose between 1, 2, or 3.");
                    }
                    break;
                case State.BOOK_CONFIRMATION:
                    if(sInMessage.equals("Y",StringComparison.OrdinalIgnoreCase))
                    {
                        //insert appointment details in DB
                        this.aApp.saveAppointmentInfo();
                        sMessage.add("Your appointment has been successfully booked.\nThank you for contacting Group 4 Pediatrics Clinic!");
                        this.nCur = State.WELCOMING;
                    }
                    else
                    {
                        sMessage.add("You have chosen to not book an appointment with us.\nThank you for contacting Group 4 Pediatrics Clinic!");
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
