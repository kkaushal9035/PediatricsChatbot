using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilio;
using Twilio.TwiML;
using AppointmentBot;


namespace AppointmentClass.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        private static Dictionary<string, Session>? aOrders = null;

        public ActionResult OnPost()
        {
            string sFrom = Request.Form["From"];
            string sBody = Request.Form["Body"];
            var oMessage = new Twilio.TwiML.MessagingResponse();
            if (aOrders == null)
            {
                aOrders = new Dictionary<string, Session>();
            }
            if (!aOrders.ContainsKey(sFrom))
            {
                aOrders[sFrom] = new Session(sFrom);
            }
            List<String> aMessages = aOrders[sFrom].OnMessage(sBody);
            aMessages.ForEach(delegate(String sMessage){
                oMessage.Message(sMessage);
            });
            return Content(oMessage.ToString(), "application/xml");
        }

    }
}