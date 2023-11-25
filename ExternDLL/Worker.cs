using JTLwawiExtern;
using System;
using System.Windows.Forms;

namespace ExternDLL
{
    public class Worker
    {
        private readonly CJTLwawiExtern _wawiExtern;

        public Worker()
        {
            this._wawiExtern = new CJTLwawiExtern();
        }

        public string GetXmlFilePath()
        {
            string file = "";
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Title = @"XML auswählen";
                openFileDialog.DefaultExt = ".xml";
                openFileDialog.Filter = @"XML-Datei|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    file = openFileDialog.FileName;
                }
            }
            return file;
        }

        private bool HasConnection(string server, string datenbank, string benutzer, string passwort)
        {
            string fehler = "";
            if (this._wawiExtern.JTL_TesteDatenbankverbindung(server, datenbank, benutzer, passwort, out fehler) == 1)
                return true;
            else
            {
                MessageBox.Show(string.Format("Fehler beim Verbinden {0}", fehler));
                return false;
            }
        }

        public void Import(string server, string datenbank, string benutzer, string passwort, int kBenutzer, string filePath)
        {
            if (HasConnection(server, datenbank, benutzer, passwort) && filePath.Length != 0)
            {
                string cKRechnungen;
                this._wawiExtern.JTL_OrderXmlImport(server, datenbank, benutzer, passwort, kBenutzer, filePath, out cKRechnungen);
                MessageBox.Show(cKRechnungen);
            }
        }

        public void WorkFlow(string server, string datenbank, string benutzer, string passwort, int kBenutzer, int key, int id)
        {
            if (HasConnection(server, datenbank, benutzer, passwort))
            {
                this._wawiExtern.JTL_WorkflowAuftrag(server, datenbank, benutzer, passwort, kBenutzer, key, id);
            }
        }

        public void JTL_WorkflowLieferschein(string server, string datenbank, string benutzer, string passwort, int kUser, int kLieferschein, int eventID)
        {
            if (HasConnection(server, datenbank, benutzer, passwort))
            {
                this._wawiExtern.JTL_WorkflowLieferschein(server, datenbank, benutzer, passwort, kUser, kLieferschein, eventID);
            }
        }



        public void TestConnection(string server, string datenbank, string benutzer, string passwort)
        {
            if (HasConnection(server, datenbank, benutzer, passwort))
            {
                MessageBox.Show("Verbindung erfolgreich");
            }
        }
    }
}
