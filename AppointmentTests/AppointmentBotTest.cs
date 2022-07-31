using System;
using System.IO;
using Xunit;
using AppointmentBot;

namespace AppointmentBot.tests
{
    public class AppointmentBotTests
    {   
        Random rnd = new Random();
        public int randomNo()
        {
            int num = rnd.Next();
            return num;
        }

        string patientname;

        [Fact]
        public void TestChatBotInitiation()
        {
            Session oSession = new Session("TestChatBotInitiation");
            String sInput = oSession.OnMessage("hello")[0];
            Assert.True(sInput.Contains("Welcome"));
        }
        [Fact]
        public void TestOptionsToInitiate()
        {
            Session oSession = new Session("TestChatBotInitiation");
            String sInput = oSession.OnMessage("hello")[1];
            Assert.True(sInput.Contains("Please choose"));
        }
        
        [Fact]
        public void BookNewAppt()
        {
            Session oSession = new Session("BookNewAppt");
            patientname = "Patient"+randomNo();
            oSession.OnMessage("hello");
            oSession.OnMessage("1");
            oSession.OnMessage(patientname);
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("Flu");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            oSession.OnMessage("1");
            String sInput = oSession.OnMessage("Y")[0];
            Assert.True(sInput.Contains("Your appointment has been successfully booked"));
            Assert.True(sInput.Contains(patientname));
        }

        
        // [Fact]
        // public void TestShawarama()
        // {
        //     Session oSession = new Session("12345");
        //     String sInput = oSession.OnMessage("hello")[0];
        //     Assert.True(sInput.ToLower().Contains("shawarama"));
        // }
        // [Fact]
        // public void TestSize()
        // {
        //     Session oSession = new Session("12345");
        //     String sInput = oSession.OnMessage("hello")[1];
        //     Assert.True(sInput.ToLower().Contains("size"));
        // }
        // [Fact]
        // public void TestLarge()
        // {
        //     Session oSession = new Session("12345");
        //     oSession.OnMessage("hello");
        //     String sInput = oSession.OnMessage("large")[0];
        //     Assert.True(sInput.ToLower().Contains("protein"));
        //     Assert.True(sInput.ToLower().Contains("large"));
        // }
        // [Fact]
        // public void TestChicken()
        // {
        //     string sPath = DB.GetConnectionString();
        //     Session oSession = new Session("12345");
        //     oSession.OnMessage("hello");
        //     oSession.OnMessage("large");
        //     String sInput = oSession.OnMessage("chicken")[0];
        //     Assert.True(sInput.ToLower().Contains("toppings"));
        //     Assert.True(sInput.ToLower().Contains("large"));
        //     Assert.True(sInput.ToLower().Contains("chicken"));
        // }
    }
}