# Mein C# HTTP Server Projekt

## Überblick
Dieses Projekt implementiert einen einfachen HTTP-Server in C#, der POST-Anfragen entgegennimmt. Die Anfragen enthalten Daten zu einem Benutzer (`User`) und einem Lieferschein (`Lieferschein`). Mithilfe der `JTLwawiExtern.dll` werden spezifische Aktionen in der JTL-Wawi Software ausgeführt, insbesondere Workflows für den übergebenen Lieferschein. Zusätzlich werden die Anfragen in einem Log-Verzeichnis (`C:\\log`) protokolliert.

## Komponenten
Das Projekt besteht aus mehreren Schlüsselkomponenten:
- `Program.cs`: Hauptteil des Servers, der das HTTP-Listening und die Anfrageverarbeitung handhabt.
- `Logger.cs`: Verantwortlich für das Protokollieren der eingehenden Anfragen.
- `ValidateAssembly.cs`: Überprüft die Integrität und Kompatibilität der verwendeten Assemblys.
- `Worker.cs`: Enthält die Logik für die Interaktion mit der JTL-Wawi Software.

## Installation
Um das Projekt zu verwenden, müssen Sie folgende Schritte ausführen:
1. Stellen Sie sicher, dass die JTL-Wawi-Software auf Ihrem System installiert ist und dass `JTLwawiExtern.dll` korrekt integriert ist.
2. Klone das Repository in deinen lokalen Speicher.
3. Öffne das Projekt in deiner bevorzugten C#-Entwicklungsumgebung (z.B. Visual Studio).
4. Führe das Projekt aus, um den Server zu starten.
5. Alternativ kannst du das Programm auch veröffentlichen (publish) und als Dienst auf deinem System installieren.

### Als Dienst installieren
Um das Programm als Dienst zu installieren, führe folgende Schritte aus:
- Veröffentliche das Programm in einem geeigneten Verzeichnis.
- Öffne die Eingabeaufforderung als Administrator.
- Führe den Befehl aus: `sc create [Dienstname] binPath= "[Pfad zum veröffentlichten Programm]"`. Ersetze `[Dienstname]` mit einem Namen für den Dienst und `[Pfad zum veröffentlichten Programm]` mit dem Pfad zur ausführbaren Datei deines Programms.
- Starte den Dienst mit `sc start [Dienstname]`.

### Dienst deinstallieren
Um den Dienst zu deinstallieren, folge diesen Schritten:
- Stoppe den Dienst zuerst mit dem Befehl `sc stop [Dienstname]`.
- Deinstalliere den Dienst mit `sc delete [Dienstname]`.
- Ersetze `[Dienstname]` mit dem Namen des zu deinstallierenden Dienstes.

## Verwendung
Nach dem Start des Servers kann er POST-Anfragen an `http://localhost:5000/` entgegennehmen. Die Anfragen sollten die Parameter `kUser` und `kLieferschein` enthalten.

## Protokollierung
Jede Anfrage wird im Verzeichnis `C:\\log` mit einem Zeitstempel, der HTTP-Methode und den Parametern der Anfrage protokolliert.

## Wichtig
Beachten Sie die Sicherheit und Datenschutzbestimmungen, besonders wenn Sie persönliche Daten oder sensible Informationen über den Server verarbeiten.

## Beitrag
Feedback und Beiträge sind immer willkommen. Erstellen Sie gerne ein Issue oder einen Pull-Request, wenn Sie Verbesserungen oder Vorschläge haben.

## Lizenz
[MIT](https://choosealicense.com/licenses/mit/)
