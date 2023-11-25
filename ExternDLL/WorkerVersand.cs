using JTLwawiExtern;
using System.Collections.Generic;

namespace ExternDLL
{
    public class WorkerVersand
    {
        CJTLwawiExtern _wawiExtern = new CJTLwawiExtern();

        public void VersanddatenImport(string server, string datenbank, string benutzer, string passwort, int kBenutzer, IList<Versandinformation> versandinformationen)
        {
            var versanddatenImporter = this._wawiExtern.VersanddatenImporter(server, datenbank, benutzer, passwort, kBenutzer);

            foreach (var versandinformation in versandinformationen)
            {
                versanddatenImporter.Add(versandinformation.Id, versandinformation.Versanddatum, versandinformation.TrackingId, versandinformation.VersandInfo);
            }

            versanddatenImporter.Apply();
        }
    }
}
