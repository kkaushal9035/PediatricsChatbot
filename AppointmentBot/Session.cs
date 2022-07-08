using System;

namespace OrderBot
{
    public class Session
    {
        private enum State
        {
            WELCOMING, SIZE, PROTEIN
        }

        private State nCur = State.WELCOMING;
        private Order oOrder;

        public Session(string sPhone){
            this.oOrder = new Order();
            this.oOrder.Phone = sPhone;
        }

        public String OnMessage(String sInMessage)
        {
            String sMessage = "Welcome to Rich's Shawarama! What size would you like?";
            switch (this.nCur)
            {
                case State.WELCOMING:
                    this.nCur = State.SIZE;
                    break;
                case State.SIZE:
                    this.oOrder.Size = sInMessage;
                    this.oOrder.Save();
                    sMessage = "What protein would you like on this  " + this.oOrder.Size + " Shawarama?";
                    this.nCur = State.PROTEIN;
                    break;
                case State.PROTEIN:
                    string sProtein = sInMessage;
                    sMessage = "What toppings would you like on this  " + this.oOrder.Size + " " + sProtein + " Shawarama?";
                    break;
                    

            }
            System.Diagnostics.Debug.WriteLine(sMessage);
            return sMessage;
        }

    }
}
