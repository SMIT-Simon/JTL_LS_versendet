# Mein C# HTTP Server Projekt

## Überblick
Dieses Projekt implementiert einen einfachen HTTP-Server in C#, der POST-Anfragen entgegennimmt. Die Anfragen können eine Vielzahl von Parametern enthalten, die je nach ausgeführter Aktion variieren. Der Server ist so konzipiert, dass er flexibel auf verschiedene Parameter reagieren und unterschiedliche Funktionen ausführen kann, je nachdem, welche Aktion angefordert wird. Ein Authentifizierungsschlüssel (`key`) wird verwendet, um sicherzustellen, dass nicht autorisierter Code nicht ausgeführt wird. Mithilfe der `JTLwawiExtern.dll` werden spezifische Aktionen in der JTL-Wawi Software ausgeführt, basierend auf den übergebenen Parametern.

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
Nach dem Start des Servers kann er POST-Anfragen an `http://localhost:5000/` entgegennehmen. Die Anfragen können eine Vielzahl von Parametern enthalten (`para1`, `para2`, `para3`, `para4`, `para5`, `para6`, `para7`, `para8`), deren Bedeutung und Verwendung von der angeforderten Aktion (`aktion`) abhängt. Der `key`-Parameter ist für die Authentifizierung erforderlich.

### Beispielhafte `curl`-Anfrage
Du kannst `curl` verwenden, um eine POST-Anfrage an den Server zu senden. Hier ist ein Beispielbefehl:

##Für einen Lieferschein-Workflow:
- curl -X POST http://localhost:5000/ -d "para1=123&para2=456&para3=789&para4=1011&para5=wert5&para6=wert6&para7=wert7&para8=wert8&key=GJJHF-787865-23883-HUZT&aktion=1"
  
#### Ersetze die Werte entsprechend den Anforderungen deiner Anwendung.

##Für den Versanddatenimport:
- curl -X POST http://localhost:5000/ -d "para5=AU-202311-10000-001&para6=Trackingcode123456&para7=irgendwelcheVersandInfos&key=GJJHF-787865-23883-HUZT&aktion=2"
  
#### Ersetze die Werte entsprechend den Anforderungen deiner Anwendung.


## Protokollierung
Jede Anfrage wird im Verzeichnis `C:\\log` mit einem Zeitstempel, der HTTP-Methode und den Parametern der Anfrage protokolliert.

## Wichtig
Beachten Sie die Sicherheit und Datenschutzbestimmungen, besonders wenn Sie persönliche Daten oder sensible Informationen über den Server verarbeiten.

## Beitrag
Feedback und Beiträge sind immer willkommen. Erstellen Sie gerne ein Issue oder einen Pull-Request, wenn Sie Verbesserungen oder Vorschläge haben.

## Lizenz
[MIT](https://choosealicense.com/licenses/mit/)
