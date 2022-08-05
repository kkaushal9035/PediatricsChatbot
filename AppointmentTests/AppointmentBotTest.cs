using System;
using System.IO;
using Xunit;
using AppointmentBot;
using System.Collections.Generic;

namespace AppointmentBot.tests
{
    [Collection("Sequential")]
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
            oSession.OnMessage("hello");
            oSession.OnMessage("1");
            oSession.OnMessage(patientName);
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("Flu");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            String sInput = oSession.OnMessage("Y")[0];
            Assert.True(sInput.Contains("Your appointment has been successfully booked"));
            Assert.True(sInput.Contains(patientName));
            return patientName;
        }
        
        [Fact]
        public void TestChatBotInitiation()
        {
            Session oSession = new Session("1234567890");
            String sInput = oSession.OnMessage("hello")[0];
            Assert.True(sInput.Contains("Welcome"));
        }

        [Fact]
        public void TestOptionsToInitiate()
        {
            Session oSession = new Session("1234567890");
            String sInput = oSession.OnMessage("hello")[1];
            Assert.True(sInput.Contains("Please choose"));
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
            oSession.OnMessage("hello");
            oSession.OnMessage("1");
            oSession.OnMessage(patientName2);
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("Flu");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            String sInput = oSession.OnMessage("N")[0];
            Assert.True(sInput.Contains("You have chosen to not book an appointment"));
        }

        [Fact]
        public void BookNewAppt_ForPatientWithExistingAppt()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            oSession.OnMessage("hello");
            oSession.OnMessage("1");
            List<string> sInput = new List<string>();
            sInput.AddRange(oSession.OnMessage(patientName));
            Assert.True(sInput[0].Contains("You already have an upcoming appointment"));
            Assert.True(sInput[1].Contains("Patient Name: "+patientName));
            Assert.True(sInput[2].Contains("Enter 1 to reschedule"));
            String sInput2 = oSession.OnMessage("2")[0];
            Assert.True(sInput2.Contains("Your appointment is scheduled as before."));
        }

        [Fact]
        public void BookNewAppt_ForPatientWithExistingAppt_Reschedule()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            oSession.OnMessage("hello");
            oSession.OnMessage("1");
            List<string> sInput2 = new List<string>();
            sInput2.AddRange(oSession.OnMessage(patientName));
            Assert.True(sInput2[0].Contains("You already have an upcoming appointment"));
            Assert.True(sInput2[1].Contains("Patient Name: "+patientName));
            Assert.True(sInput2[2].Contains("Enter 1 to reschedule"));
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            List<string> sInput3 = new List<string>();
            sInput3.AddRange(oSession.OnMessage("Y"));
            Assert.True(sInput3[0].Contains(patientName));
            Assert.True(sInput3[1].Contains("Your appointment has been successfully rescheduled"));
        }
       
       [Fact]
        public void ViewExistingAppt_ByName()
        {
            patientName = apptCreation();
            Session oSession = new Session("1234567890");
            oSession.OnMessage("hello");
            oSession.OnMessage("2"); 
            oSession.OnMessage("2");
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
            oSession.OnMessage("hello");
            oSession.OnMessage("2"); 
            oSession.OnMessage("1");
            String sInput = oSession.OnMessage(referenceid)[0];
            Assert.True(sInput.Contains("Patient Name: "+patientName));
            Assert.True(sInput.Contains("Reference ID: "+referenceid));
        }

    }
}