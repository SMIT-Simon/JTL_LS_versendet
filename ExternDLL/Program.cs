using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternDLL;

namespace ExternDLL
{
    static class Program
    {
        static HttpListener listener;

        [STAThread]
        static void Main(string[] args)
        {
            StartServer();
        }

        static void StartServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Listening for connections on http://localhost:5000/");

            // Logger initialisieren
            Logger logger = new Logger(@"C:\log\");

            // Endlosschleife für das kontinuierliche Hören auf Anfragen
            while (true)
            {
                HandleIncomingConnections(logger).GetAwaiter().GetResult();
            }
        }

        static async Task HandleIncomingConnections(Logger logger)
        {
            int kUser = 0; // Vorab initialisieren
            int kLieferschein = 0; // Vorab initialisieren

            HttpListenerContext ctx = await listener.GetContextAsync();

            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            if (req.HttpMethod == "POST")
            {
                string postData; // Deklaration nur im inneren Bereich
                using (Stream body = req.InputStream)
                using (StreamReader reader = new StreamReader(body, req.ContentEncoding))
                {
                    postData = reader.ReadToEnd(); // Keine erneute Deklaration hier
                    string[] parameters = postData.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string parameter in parameters)
                    {
                        string[] keyValue = parameter.Split('=');
                        if (keyValue.Length == 2)
                        {
                            string key = keyValue[0];
                            string value = keyValue[1];

                            if (key == "kUser")
                            {
                                int.TryParse(value, out kUser);
                            }
                            else if (key == "kLieferschein")
                            {
                                int.TryParse(value, out kLieferschein);
                            }
                        }
                    }
                }

                // Hier werden die Parameter an JTL_WorkflowLieferschein übergeben
                Worker worker = new Worker();
                string server = "localhost\\JTLWAWI";
                string datenbank = "eazybusiness";
                string benutzer = "sa";
                string passwort = "sa04jT14";
                int eventID = 3; // Event-ID für "Lieferschein versendet"

                worker.JTL_WorkflowLieferschein(server, datenbank, benutzer, passwort, kUser, kLieferschein, eventID);

                ValidateAssembly validateAssembly = new ValidateAssembly();
                if (!validateAssembly.IsValid)
                    return;

                // Anfrage protokollieren
                string requestMethod = req.HttpMethod;
                string requestParameters = postData;
                logger.LogRequest(requestMethod, requestParameters);
            }

            string responseString = $"KUser: {kUser}, KLieferschein: {kLieferschein}";
            byte[] responseData = Encoding.UTF8.GetBytes(responseString);
            resp.ContentType = "text/plain";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = responseData.LongLength;

            await resp.OutputStream.WriteAsync(responseData, 0, responseData.Length);
            resp.Close();
        }

    }
}
