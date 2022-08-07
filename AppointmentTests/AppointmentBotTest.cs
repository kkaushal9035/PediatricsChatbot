using System;
using System.IO;
using Xunit;
using AppointmentBot;
using System.Collections.Generic;

namespace AppointmentBot.tests
{
    public class AppointmentBotTests
    {   
        public string patientName;
        Random rnd = new Random();

        public int randomNo()
        {
            int num = rnd.Next();
            return num;
        }

        public string apptCreation()
        {
            Session oSession = new Session("1234567890");
            patientName = "Patient"+randomNo();
            //initiate chat
            oSession.OnMessage("hello");
            //choose 1 for booking new appointment
            oSession.OnMessage("1");
            //enter name of the patient for appointment
            oSession.OnMessage(patientName);
            //choose the category patient falls in based on their age in months, years, or days.
            oSession.OnMessage("1");
            //enter age of the patient
            oSession.OnMessage("1");
            //enter reason for appointment
            oSession.OnMessage("Flu");
            //choose day of week for appointment
            oSession.OnMessage("1");
            //choose date for appointment from the options 
            oSession.OnMessage("1");
            //choose time slot for appointment
            oSession.OnMessage("1");
            //enter Y to confirm booking
            String sInput = oSession.OnMessage("Y")[0];
            Assert.True(sInput.Contains("Your appointment has been successfully booked"));
            Assert.True(sInput.Contains(patientName));
            return patientName;
        }
        
        [Fact]
        public void TestChatBotInitiation()
        {
            Session oSession = new Session("1234567890");
            //initiate chat with bot
            String sInput = oSession.OnMessage("hello")[0];
            Assert.True(sInput.Contains("Welcome"));
            AppointmentBot.Appointment appt = new AppointmentBot.Appointment();
            appt.deleteAllDataFromSqlLite();
        }

        [Fact]
        public void TestOptionsToInitiate()
        {
            Session oSession = new Session("1234567890");
            String sInput = oSession.OnMessage("hello")[1];
            Assert.True(sInput.Contains("Please choose"));
        }

        [Fact]
        public void TestInitiationWithIncorrectOptions()
        {
            Session oSession = new Session("1234567890");
            String sInput = oSession.OnMessage("hello")[1];
            //test for incorrect option entered for managing appointment
            sInput = oSession.OnMessage("Reschedule")[0];
            Assert.True(sInput.Contains("You have chosen incorrect option, please choose from available options 1, 2, 3, or 4."));
        }
        
        [Fact]
        public void BookNewAppt_NewPatient()
        {
            patientName = apptCreation();
        }

        [Fact]
        public void BookNewAppt_UnconfirmBooking()
        {
            Session oSession = new Session("1234567890");
            String patientName2 = "Patient"+randomNo();
            //initiate chat
            oSession.OnMessage("hello");
            //choose 1 for booking new appointment
            oSession.OnMessage("1");
            //enter name of the patient for appointment
            oSession.OnMessage(patientName2);
            //choose the category patient falls in based on their age in months, years, or days.
            oSession.OnMessage("1");
            //enter age of the patient
            oSession.OnMessage("1");
            //enter reason for appointment
            oSession.OnMessage("Flu");
            //choose day of week for appointment
            oSession.OnMessage("1");
            //choose date for appointment from the options 
            oSession.OnMessage("1");
            //choose time slot for appointment
            oSession.OnMessage("1");
            //enter anything other than 'Y' or 'y' to unconfirm appointment
            String sInput = oSession.OnMessage("N")[0];
            Assert.True(sInput.Contains("You have chosen to not book an appointment"));
        }

        [Fact]
        public void BookNewAppt_ForPatientWithExistingAppt()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //enter 1 to book new appointment
            oSession.OnMessage("1");
            List<string> sInput = new List<string>();
            //enter patient name with existing upcoming appointment
            sInput.AddRange(oSession.OnMessage(patientName));
            Assert.True(sInput[0].Contains("You already have an upcoming appointment"));
            Assert.True(sInput[1].Contains("Patient Name: "+patientName));
            Assert.True(sInput[2].Contains("Enter 1 to reschedule"));
            //select 2 to keep the existing upcoming appointment schedule
            String sInput2 = oSession.OnMessage("2")[0];
            Assert.True(sInput2.Contains("Your appointment is scheduled as before."));
        }

        [Fact]
        public void BookNewAppt_ForPatientWithExistingAppt_Reschedule()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //enter 1 to book appointment
            oSession.OnMessage("1");
            List<string> sInput2 = new List<string>();
            //enter patient name with already existing upcoming appointment
            sInput2.AddRange(oSession.OnMessage(patientName));
            Assert.True(sInput2[0].Contains("You already have an upcoming appointment"));
            Assert.True(sInput2[1].Contains("Patient Name: "+patientName));
            Assert.True(sInput2[2].Contains("Enter 1 to reschedule"));
            //enter 1 to reschedule the existing appointment
            oSession.OnMessage("1");
            //choose day from options to reschedule
            oSession.OnMessage("1");
            //choose date from options to reschedule
            oSession.OnMessage("1");
            //choose time slot
            oSession.OnMessage("1");
            List<string> sInput3 = new List<string>();
            //enter 'Y' or 'y' to confirm rescheduling 
            sInput3.AddRange(oSession.OnMessage("Y"));
            Assert.True(sInput3[0].Contains(patientName));
            Assert.True(sInput3[1].Contains("Your appointment has been successfully rescheduled"));
        }
   
        [Fact]
        public void BookNewAppt_WithIncorrectOptionForAgeChoice()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 1 to book an appointment
            oSession.OnMessage("1");
            //enter patient name
            oSession.OnMessage(patientName);
            //choosing incorrect age choice
            string sInput = oSession.OnMessage("7 years")[0];
            Assert.True(sInput.Contains("You have entered incorrect input option"));
        }

        [Fact]
        public void BookNewAppt_WithIncorrectOptionForAge()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 1 to book appointment
            oSession.OnMessage("1");
            //enter patient name
            oSession.OnMessage(patientName); 
            //choose age choice based on year, months or days
            oSession.OnMessage("2");
            //choosing incorrect age choice
            string sInput = oSession.OnMessage("2 months")[0];
            Assert.True(sInput.Contains("You have entered incorrect input for age"));
        }

        [Fact]
        public void BookNewAppt_WithIncorrectOptionForDay()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //enter 1 to book appointment
            oSession.OnMessage("1");
            ///enter patient name
            oSession.OnMessage(patientName);
            //choose age choice based on days, month, year
            oSession.OnMessage("2");
            //enter age
            oSession.OnMessage("2");
            //enter reason for appointment
            oSession.OnMessage("Flu");
            //enter incorrect choice for day of week for appointment
            string sInput = oSession.OnMessage("-4")[0];
            Assert.True(sInput.Contains("You have entered incorrect choice for days, please choose between 1 to 6"));
        }

        [Fact]
        public void BookNewAppt_WithIncorrectOptionForDate()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
            //initate chat
            oSession.OnMessage("hello");
             //enter 1 to book appointment
            oSession.OnMessage("1");
            ///enter patient name
            oSession.OnMessage(patientName);
            //choose age choice based on days, month, year
            oSession.OnMessage("2");
            //enter age
            oSession.OnMessage("2");
            //enter reason for appointment
            oSession.OnMessage("Flu");
            //choose day of week for appointment
            oSession.OnMessage("2");
            //enter incorrect option for date of appointment
            string sInput = oSession.OnMessage("-4")[0];
            Assert.True(sInput.Contains("You have chosen incorrect option for appointment date"));
        }

        [Fact]
        public void BookNewAppt_WithIncorrectOptionForTime()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
            //initate chat
            oSession.OnMessage("hello");
            //enter 1 to book appointment
            oSession.OnMessage("1");
            ///enter patient name
            oSession.OnMessage(patientName);
            //choose age choice based on days, month, year
            oSession.OnMessage("2");
            //enter age
            oSession.OnMessage("2");
            //enter reason for appointment
            oSession.OnMessage("Flu");
            //choose day of week for appointment
            oSession.OnMessage("2");
            //choose date for appointment
            oSession.OnMessage("1");
            //choose incorrect option for time slot
            string sInput = oSession.OnMessage("5")[0];
            Assert.True(sInput.Contains("You have entered incorrect choice for time slots"));
        }

        [Fact]
        public void ViewExistingAppt_ByName()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 2 to view appointment info
            oSession.OnMessage("2"); 
            //choose option to enter patient name to search appointment details
            oSession.OnMessage("2");
            //enter patient name
            String sInput = oSession.OnMessage(patientName)[0];
            Assert.True(sInput.Contains("Patient Name: "+patientName));
            Assert.True(sInput.Contains("Thank you"));
        }

        [Fact]
        public void ViewExistingAppt_ByReference()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            Appointment appt = new AppointmentBot.Appointment();
            string referenceid = appt.getReferenceID(patientName, "1234567890");
            //initiate chat 
            oSession.OnMessage("hello");
            //choose 2 to view appointment info
            oSession.OnMessage("2"); 
            //choose 1 to enter reference id for searching appointment details
            oSession.OnMessage("1");
            //enter reference id for existing appointment
            String sInput = oSession.OnMessage(referenceid)[0];
            Assert.True(sInput.Contains("Patient Name: "+patientName));
            Assert.True(sInput.Contains("Reference ID: "+referenceid));
        }

        [Fact]
        public void ViewNonExistingAppt()
        {
            int referenceid = randomNo();
            Session oSession = new Session("1234567890");
            //intiate chat
            oSession.OnMessage("hello");
            //choose 2 to view appointment info
            oSession.OnMessage("2"); 
            //choose 1 to enter reference id to search appointment details
            oSession.OnMessage("1");
            //enter incorrect reference id
            String sInput = oSession.OnMessage(referenceid+"")[0];
            Assert.True(sInput.Contains("You have entered incorrect reference ID for your appointment"));
        }

        [Fact]
        public void ViewNonExistingPatient()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
             //intiate chat
            oSession.OnMessage("hello");
            //choose 2 to view appointment info
            oSession.OnMessage("2"); 
            //choose 2 to enter patient name to search appointment details
            oSession.OnMessage("2");
            //enter non-existing patient name
            String sInput = oSession.OnMessage(patientName)[0];
            Assert.True(sInput.Contains("We could not find any upcoming appointment for this patient from this phone number"));
        }

        [Fact]
        public void View_WithIncorrectOptions()
        {
            patientName = "Patient" + randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 2 to view appointment info
            oSession.OnMessage("2"); 
            //enter incorrect option to view appointment
            String sInput = oSession.OnMessage("Reference")[0];
            Assert.True(sInput.Contains("You have chosen incorrect option"));
        }

        [Fact]
        public void CancelExistingAppt_ByReference()
        {
            //create new appointment
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            Appointment appt = new AppointmentBot.Appointment();
            //fetch reference id for previously created appointment
            string referenceid = appt.getReferenceID(patientName, "1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 4 to cancel appointment
            oSession.OnMessage("4"); 
            //choose 1 to enter reference id to cancel appointment
            oSession.OnMessage("1");
            //enter reference id fetched previously
            String sInput = oSession.OnMessage(referenceid)[0];
            Assert.True(sInput.Contains("Reference ID: "+referenceid));
            //enter 'y' or 'Y' to confirm cancellation
            sInput = oSession.OnMessage("Y")[0];
            Assert.True(sInput.Contains("Your appointment has been successfully cancelled"));
        }

        [Fact]
        public void CancelExistingAppt_ByName()
        {
            //create new appointment
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 4 to cancel appointment
            oSession.OnMessage("4"); 
            //choose 2 to enter patient name for cancelling
            oSession.OnMessage("2");
            //enter patient name with existing appointment
            String sInput = oSession.OnMessage(patientName)[0];
            Assert.True(sInput.Contains("Patient Name: "+patientName));
            //enter 'y' or 'Y' to confirm cancellation
            sInput = oSession.OnMessage("Y")[0];
            Assert.True(sInput.Contains("Your appointment has been successfully cancelled"));
        }

        [Fact]
        public void UnconfirmCancellation_ByReference()
        {
            //create new appointment
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            Appointment appt = new AppointmentBot.Appointment();
            //fetch reference id for the previously created appointment
            string referenceid = appt.getReferenceID(patientName, "1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 4 to cancel appointment
            oSession.OnMessage("4"); 
            //choose 1 to enter reference id to cancel appointment
            oSession.OnMessage("1");
            //enter reference id
            String sInput = oSession.OnMessage(referenceid)[0];
            Assert.True(sInput.Contains("Reference ID: "+referenceid));
            //enter anything other than 'y' or 'Y' to unconfirm cancelling
            sInput = oSession.OnMessage("N")[0];
            Assert.True(sInput.Contains("You have chosen to not cancel your appointment"));
        }

        [Fact]
        public void CancelNonExistentAppt_ByName()
        {
            //generate random patient name that does not exist in patient database of clinic
            patientName = "Patient"+randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 4 to cancel appointment
            oSession.OnMessage("4"); 
            //choose 2 to enter patient name for cancelling
            oSession.OnMessage("2");
            //enter patient name generated in first step
            String sInput = oSession.OnMessage(patientName)[0];
            Assert.True(sInput.Contains("We could not find any record of upcoming appointment for patient"));
        }

        [Fact]
        public void CancelNonExistentAppt_ByReference()
        {
            int referenceid = randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 4 to cancel appointment
            oSession.OnMessage("4"); 
            //choose 1 to enter reference id for cancelling
            oSession.OnMessage("1");
            //enter non-existent reference id to cancel
            String sInput = oSession.OnMessage(referenceid+"")[0];
            Assert.True(sInput.Contains("We could not find any record of upcoming appointment for reference ID"));
        }

        [Fact]
        public void Cancel_WithIncorrectOption()
        {
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 4 to cancel appointment
            oSession.OnMessage("4"); 
            //choose incorrect option by entering anything other than 1 or 2
            String sInput =  oSession.OnMessage("ABC")[0];
            Assert.True(sInput.Contains("You have chosen incorrect option"));
        }

        [Fact]
        public void RescheduleExistingAppt_ByReference()
        {
            //create new appointment
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //fetch reference id for the newly created appointment to use ahead
            Appointment appt = new AppointmentBot.Appointment();
            string referenceid = appt.getReferenceID(patientName, "1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 1 to enter reference id for rescheduling
            oSession.OnMessage("1");
            //enter reference id fetched previously
            String sInput = oSession.OnMessage(referenceid)[1];
            Assert.True(sInput.Contains("Reference ID: "+referenceid));
            //choose day of week
            oSession.OnMessage("2");
            //choose date
            oSession.OnMessage("2");
            //choose time slot
            oSession.OnMessage("2");
            //enter 'Y' or 'y' to confirm rescheduling
            sInput = oSession.OnMessage("Y")[1];
            Assert.True(sInput.Contains("Your appointment has been successfully rescheduled"));
        }

        [Fact]
        public void RescheduleExistingAppt_ByName()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 2 to enter patient name
            oSession.OnMessage("2");
            //enter patient name
            String sInput = oSession.OnMessage(patientName)[1];
            Assert.True(sInput.Contains("Patient Name: "+patientName));
            //choose week of the day 
            oSession.OnMessage("2");
            //choose date
            oSession.OnMessage("2");
            //choose time slot
            oSession.OnMessage("2");
            //enter 'y' or 'Y' to confirm rescheduling
            sInput = oSession.OnMessage("Y")[1];
            Assert.True(sInput.Contains("Your appointment has been successfully rescheduled"));
        }

        [Fact]
        public void RescheduleForNonExistingAppt()
        {
            int referenceid = randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 1 to enter reference id for rescheduling
            oSession.OnMessage("1");
            //enter incorrect reference id
            String sInput = oSession.OnMessage(referenceid+"")[0];
            Assert.True(sInput.Contains("We could not find any record of upcoming appointment to reschedule for reference ID"));
        }

        [Fact]
        public void RescheduleForNonExistingPatient()
        {
            patientName ="Patient"+ randomNo();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 2 to enter patient name for rescheduling
            oSession.OnMessage("2");
            //enter patient name that does not have any appointment
            String sInput = oSession.OnMessage(patientName)[0];
            Assert.True(sInput.Contains("We could not find any record of upcoming appointment to reschedule for patient"));
        }

        [Fact]
        public void Reschedule_WithIncorrectOption()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //enter incorrect option for rescheduling
            String sInput = oSession.OnMessage("-1")[0];
            Assert.True(sInput.Contains("You have chosen incorrect option"));
        }

        [Fact]
        public void Reschedule_WithIncorrectOptionForDay()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 2 to enter patient name
            oSession.OnMessage("2");
            String sInput = oSession.OnMessage(patientName)[1];
            Assert.True(sInput.Contains("Patient Name: "+patientName));
            //choose incorrect option for week day
            sInput = oSession.OnMessage("Monday")[0];
            Assert.True(sInput.Contains("You have entered incorrect choice for days, please choose between 1 to 6."));
        }

        [Fact]
        public void Reschedule_WithIncorrectOptionForDate()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 2 to enter patient name to reschedule
            oSession.OnMessage("2");
            //enter patient name
            oSession.OnMessage(patientName);
            //choose day of week
            oSession.OnMessage("1");
            //choose incorrect option for date
            string sInput = oSession.OnMessage("Today")[0];
            Assert.True(sInput.Contains("You have chosen incorrect option for appointment date"));
        }

        [Fact]
        public void Reschedule_WithIncorrectOptionForTime()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            //initiate chat
            oSession.OnMessage("hello");
            //choose 3 to reschedule existing appointment
            oSession.OnMessage("3"); 
            //choose 2 to enter patient name for rescheduling
            oSession.OnMessage("2");
            //enter patient name 
            oSession.OnMessage(patientName);
            //choose day of week to reschedule
            oSession.OnMessage("1");
            //enter date to reschedule
            oSession.OnMessage("2");
            //enter incorrect value for time slot
            string sInput = oSession.OnMessage("Now")[0];
            Assert.True(sInput.Contains("You have entered incorrect choice for time slots, please choose between 1, 2, or 3."));
        }

        
        
    }
}