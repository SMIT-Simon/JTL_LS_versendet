using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExternDLL
{
    static class Program
    {
        static HttpListener listener;
        //const string AuthKey = "GJJHF-787865-23883-HUZT"; // Definiere den Authentifizierungsschlüssel

        // Variablen für die Datenbankverbindung und Authentifizierungsschlüssel
        private static string Server;
        private static string Datenbank;
        private static string Benutzer;
        private static string Passwort;
        private static string AuthKey;

        [STAThread]
        static void Main(string[] args)
        {
            LoadConfig();
            StartServer();

        }

        static void LoadConfig()
        {
            var configText = File.ReadAllText("config.json");
            dynamic config = JsonConvert.DeserializeObject(configText);
            Server = config.Server;
            Datenbank = config.Datenbank;
            Benutzer = config.Benutzer;
            Passwort = config.Passwort;
            AuthKey = config.AuthKey;
        }

        static void StartServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Listening for connections on http://localhost:5000/");

            Logger logger = new Logger(@"C:\log\");

            while (true)
            {
                HandleIncomingConnections(logger).GetAwaiter().GetResult();
            }
        }

        static async Task HandleIncomingConnections(Logger logger)
        {
            int para1 = 0;
            int para2 = 0;
            int para3 = 0;
            int para4 = 0;
            string para5 = "";
            string para6 = "";
            string para7 = "";
            string para8 = "";
            string authKey = "";
            int aktion = 0;

            HttpListenerContext ctx = await listener.GetContextAsync();

            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            string responseString = "";
            byte[] responseData;

            if (req.HttpMethod == "POST")
            {
                string postData;
                using (Stream body = req.InputStream)
                using (StreamReader reader = new StreamReader(body, req.ContentEncoding))
                {
                    postData = reader.ReadToEnd();
                    string[] parameters = postData.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string parameter in parameters)
                    {
                        string[] keyValue = parameter.Split('=');
                        if (keyValue.Length == 2)
                        {
                            string key = keyValue[0];
                            string value = keyValue[1];

                            switch (key)
                            {
                                case "para1": int.TryParse(value, out para1); break;
                                case "para2": int.TryParse(value, out para2); break;
                                case "para3": int.TryParse(value, out para3); break;
                                case "para4": int.TryParse(value, out para4); break;
                                case "para5": para5 = value; break;
                                case "para6": para6 = value; break;
                                case "para7": para7 = value; break;
                                case "para8": para8 = value; break;
                                case "key": authKey = value; break;
                                case "aktion": int.TryParse(value, out aktion); break;
                            }
                        }
                    }
                }

                if (authKey != AuthKey)
                {
                    responseString = "Ungültiger Authentifizierungsschlüssel";
                }
                else
                {
                    Worker worker = new Worker();


                    switch (aktion)
                    {
                        case 1:
                            int eventID = 3; // Event-ID für "Lieferschein versendet"
                            worker.JTL_WorkflowLieferschein(Server, Datenbank, Benutzer, Passwort, para1, para2, eventID);
                            break;
                        case 2:
                            WorkerVersand workerVersand = new WorkerVersand();
                            // Erstelle eine Liste von Versandinformationen basierend auf den Parametern 
                            List<Versandinformation> versandinfos = new List<Versandinformation>();
                            // Füge Versandinformationen hinzu.
                            versandinfos.Add(new Versandinformation { Id = para5, Versanddatum = DateTime.Now, TrackingId = para6, VersandInfo = para7 });
                            workerVersand.VersanddatenImport(Server, Datenbank, Benutzer, Passwort, para1, versandinfos);
                            break;
                            // Weitere Fälle ...
                    }

                    responseString = $"Aktion: {aktion}, Para1: {para1}, Para2: {para2}, Para3: {para3}, Para4: {para4}, Para5: {para5}, Para6: {para6}, Para7: {para7}, Para8: {para8}";

                    logger.LogRequest(req.HttpMethod, postData);
                }
            }

            responseData = Encoding.UTF8.GetBytes(responseString);
            resp.ContentType = "text/plain";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = responseData.LongLength;

            await resp.OutputStream.WriteAsync(responseData, 0, responseData.Length);
            resp.Close();
        }
    }
}