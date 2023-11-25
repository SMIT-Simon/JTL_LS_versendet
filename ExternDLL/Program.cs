using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExternDLL
{
    public class MyService : ServiceBase
    {
        public MyService()
        {
            this.ServiceName = "JTL_SMIT_externDLL";
        }

        static void Main(string[] args)
        {
            // Überprüfen, ob das Programm als Dienst oder als Konsolenanwendung laufen soll
            if (Environment.UserInteractive)
            {
                // Konsolenmodus
                RunAsConsole(args);
            }
            else
            {
                // Dienstmodus
                ServiceBase.Run(new MyService());
            }
        }

        private static void RunAsConsole(string[] args)
        {
            Console.WriteLine("Starting in console mode...");
            Program.LoadConfig();
            Program.StartServer();

            Console.WriteLine("Press Enter to stop...");

            while (Program.Running)
            {
                // Überprüfen, ob eine Eingabe vorliegt
                if (Console.In.Peek() != -1)
                {
                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit") // Benutzer kann "exit" eingeben, um den Dienst zu stoppen
                    {
                        Program.Running = false;
                    }
                }

                Thread.Sleep(100); // Kleine Verzögerung, um CPU-Nutzung zu reduzieren
            }

            Program.StopServer();
            Console.WriteLine("Stopped.");
        }


        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            Program.LoadConfig();
            Program.StartServer();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Program.StopServer();
        }
    }

    static class Program
    {
        static HttpListener listener;
        public static bool Running { get; set; }

        private static string Server;
        private static string Datenbank;
        private static string Benutzer;
        private static string Passwort;
        private static string AuthKey;

        public static void LoadConfig()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                var configText = File.ReadAllText(configPath);
                dynamic config = JsonConvert.DeserializeObject(configText);


            Server = config.Server;
            Datenbank = config.Datenbank;
            Benutzer = config.Benutzer;
            Passwort = config.Passwort;
            AuthKey = config.AuthKey;
            }
            catch (Exception ex)
            {
                // Log the error and exit the application or handle it appropriately
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                Environment.Exit(1);
            }

        }

        public static void StartServer()
        {
            Running = true;
            try
            {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Listening for connections on http://localhost:5000/");
            Logger logger = new Logger(@"C:\log\");
                Task.Run(() =>
                {
                    while (listener.IsListening)
                    {
                        HandleIncomingConnections(logger).GetAwaiter().GetResult();
                    }
                });

            }
            catch (Exception ex)
            {
                // Log the error and exit the application or handle it appropriately
                Console.WriteLine($"Error starting server: {ex.Message}");
                Environment.Exit(1);
            }


        }

        public static void StopServer()
        {

            Running = false;
            if (listener != null && listener.IsListening)
            {
                listener.Stop();
                listener.Close();
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