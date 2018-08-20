System-Partnertag 2018 - Entwickeln der ersten eigenen Erweiterung
==================================================================

Anforderungen an die Entwicklungsumgebung
-----------------------------------------

- Lokale PROFFIX Installation mit SQL Server
    - Lizenzierte Zeitverwaltung

- Lokale PROFFIX REST API Version 2.5
    - Lizenzierte Zeitverwaltung
    - Version 2.6 (über PROFFIX v4.0.1016 Beta) oder [Entwicklerversion](http://pxnews.proffix.net/PROFFIX/pxNewsletter.asmx/R?Value=ZgfPgeMI1sxddzvIMrmViFuFO2dDWKvKD68EjlZkhxY53-kiakjBryBo2Ntmlq5M8AlLTQr9OjLEBHr7iMb5GEYmvJqGJPK2_lf_fdQMZmhqo3dvEQCWcBIF4Vv-E4-qaMamkUpmB-nGLEMXdQn3_wCv-RDpCNpk_iOav66Cqj14f-IKQ0FO5_jNqreel9V2Ersw-TAsD6ryXX4MLsTqtg==) der PROFFIX REST API
        - Entwicklerversion: Diese ZIP-Datei in das PROFFIX Data Verzeichnis legen und die PROFFIX REST API Konfiguration starten, um das Update zu durchzuführen.
- Visual Studio 2017 (Windows, Edition egal)

Entwicklungsumgebung in Betrieb nehmen
--------------------------------------
Nach erfolgreicher Installation von Visual Studio, inkl. SQL-Server, sowie PROFFIX und der PROFFIX REST API sind folgende Schritte nötig:

1. Visual Studio Template für die Entwicklung einer PROFFIX REST API Erweiterung herunterladen und installieren: https://github.com/PROFFIX-NET/RestApiErweiterungenTemplate

2. Erweiterung installieren, welche alle Aufrufe an die lokale Entwicklungsumgebung weiterleitet (also auf http://localhost:5000): https://github.com/PROFFIX-NET/SystemPartnerTag2018Fortgeschritten/raw/master/RestApiDevelopmentExtension.zip

Vorgehen Entwicklung der ersten eigenen Erweiterung
---------------------------------------------------
Für diejenigen für die [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/web-api/) Neuland ist, befindet sich im Branch "loesung" dieses Repositories das fertige Projekt.

1. Neues Projekt mit dem Visual Studio Template erstellen

2. Models mit den angegebenen Eigenschaften erstellen:
    - Attendance: `MitarbeiterName:string, Eingestempelt:boolean, Dauer:string`
    - Mitarbeiter: `MitarbeiterNr:int, Name:string`
    - Stempel: `Eingestempelt:boolean, EinstempelnZeitpunkt:string`

3. Herstellername in `extension.json` anpassen

4. `AttendanceListController` implementieren
    - Auf der Route `/AttendanceList` muss ein Array des Typs `Attendance` zurückgegeben werden
    - Im Template ist ein einfacher HTTP-Client mitgeliefert, der über Constructor Injection im Controller verwendet werden kann (`IPxRestApiClient`)
