/* Autore: Francesco Di Lena
 * Classe: 4F
 * Consegna: Progettare un sistema di gestione del Fantacalcio.
 */

//Direttive using standard.
using System; //Spazio dei nomi che permette l'uso dei metodi più comuni.
using System.IO; //Spazio dei nomi che permette la gestione dei file in generale.
using System.Collections.Generic; //Spazio dei nomi che permette la gestione delle liste.
//Direttive using aggiuntive (NuGet).
using Newtonsoft.Json; //Spazio dei nomi che permette la gestione dei file json.


namespace Fantacalcio
{
    //Classi che compongono il programma.

    class Programma //La classe Programma è la prima ad essere richiamata dal programma al suo avvio e contiene la visualizzazione a schermo di testo, oltre alle richieste di inserimento rivolte all'utente. 
    {
        //Variabili

        //Variabili necessarie per contenere i percorsi dei file e della loro cartella che possono essere solamente lette (readonly).
        static private readonly string[] percorsiIO = new string[] { @"C:\Programma Gestionale Fantacalcio\fanta-allenatori.json", @"C:\Programma Gestionale Fantacalcio\fanta-calciatori.json", @"C:\Programma Gestionale Fantacalcio\schieramenti.json" };
        static private readonly string percorsoCartella = @"C:\Programma Gestionale Fantacalcio";

        //Liste necessarie per contenere ogni dato del fanta-torneo.
        static private List<FantaAllenatore> FantaAllenatori = new List<FantaAllenatore>();
        static private List<FantaCalciatore> FantaCalciatori = new List<FantaCalciatore>();
        static private List<(int, int)> CodiciSchieramenti = new List<(int, int)>();
        static private List<int> Indici = new List<int>();

        //Metodi

        //Metodo principale, il primo che viene richiamato dal programma, che stabilisce quali metodi invocare in base al fatto che si tratti di un avvio comune oppure il primo. 
        static private void Main()
        { 
            bool chiusuraProgramma;
            bool[] verificaFile = new bool[2];
            Console.Title = "Programma Gestionale del Fantacalcio";
            do
            {
                OperazioniFile operazioniFile = new OperazioniFile();
                Console.Clear();
                for (int i = 0; i < 2; i++)
                {
                    verificaFile[i] = operazioniFile.VerificaEsistenzaFile(percorsiIO[i]);
                }
                if (verificaFile[0] == false || verificaFile[1] == false)
                {
                    VisualizzaIntestazione();
                    operazioniFile.CreaDirectory(percorsoCartella);
                    PrimoAvvio();
                }
                for (int i = 0; i < 2; i++)
                {
                    string contenutoInput = operazioniFile.LeggiFile(percorsiIO[i]);
                    if (i == 0)
                    {
                        FantaAllenatori = JsonConvert.DeserializeObject<List<FantaAllenatore>>(contenutoInput);
                    }
                    else
                    {
                        FantaCalciatori = JsonConvert.DeserializeObject<List<FantaCalciatore>>(contenutoInput);
                    }
                }
                VisualizzaIntestazione();
                chiusuraProgramma = AvvioComune();
                if (chiusuraProgramma == false)
                {
                    Console.WriteLine("\nPer tornare alla schermata iniziale premere un tasto qualsiasi...");
                    Console.ReadKey();
                }
            } while (chiusuraProgramma == false);
            Console.WriteLine("\n\nPer uscire dal programma premi un tasto qualsiasi...");
            Console.ReadKey();
            Environment.Exit(0);

        }

        //Metodo che permette la visualizzazione dell'intestazione "Programma gestionale del Fantacalcio" in ogni schermata.
        static private void VisualizzaIntestazione() 
        {
            SetResetColori(true);
            Console.WriteLine("                                       _________          ________                                 " +
                              "\n                     Il               |    _____|        |    ____|                                " +
                              "\n                 programma            |   |___           |   |                                     " +
                              "\n                gestionale            |    ___|          |   |                                     " +
                              "\n                    del               |   |              |   |___                                  " +
                              "\n                                      |___|     anta     |_______| alcio                           " +
                              "\n                                                                                                   \n");
            SetResetColori(false);
        }

        //Metodo richiamato dal Main solamente quando il programma viene avviato per la prima volta. Contiene le visualizzazioni video e le prime richieste di inserimento per l'utente. 
        static public void PrimoAvvio()
        {
            bool verifica;
            Console.WriteLine("                      Benvenuto nel programma gestionale del fantacalcio.                      \n"
                    + "       Questo è il primo avvio del programma, così, per giocare devi inserire tutti i dati        \n" +
                    "       che riguardano il torneo; il programma ti guiderà nell'inserimento e ogni volta che ti       \n" +
                    "               farà delle richieste dovrai inserire un comando seguito da un INVIO.                 \n");
            SetResetColori(true);
            Console.WriteLine("Inserisci il torneo reale di riferimento per il gioco:");
            SetResetColori(false);
            string torneo = Console.ReadLine();
            int numeroFantaAllenatori;
            do
            {
                SetResetColori(true);
                Console.WriteLine("\nInserisci il numero dei fanta-allenatori che parteciperanno al gioco:");
                SetResetColori(false);
                verifica = int.TryParse(Console.ReadLine(), out numeroFantaAllenatori);
                if (verifica == false || numeroFantaAllenatori < 2 || numeroFantaAllenatori > 10)
                {
                    Console.WriteLine("\nDevi inserire un numero compreso tra 2 e 10 (inclusi). Riprova...");
                }
            }
            while (verifica == false || numeroFantaAllenatori < 2 || numeroFantaAllenatori > 10);
            for (int i = 0; i < numeroFantaAllenatori; i++)
            {
                SetResetColori(true);
                Console.WriteLine($"\nInserisci il nome del {i + 1}° giocatore:");
                SetResetColori(false);
                string nomeFantaAllenatore = Console.ReadLine();
                int budgetDisponibile;
                do
                {
                    SetResetColori(true);
                    Console.WriteLine($"\nInserisci il budget di {nomeFantaAllenatore} (in Fantamilioni):");
                    SetResetColori(false);
                    verifica = int.TryParse(Console.ReadLine(), out budgetDisponibile);
                    if (verifica == false)
                    {
                        Console.WriteLine("\nDevi inserire un numero. Riprova...");
                    }
                }
                while (verifica == false);
                FantaAllenatore FantaAllenatore = new FantaAllenatore(nomeFantaAllenatore, i, budgetDisponibile);
                FantaAllenatori = FantaAllenatore.AggiungiFantaAllenatore(FantaAllenatori);
            }
            Console.WriteLine("\nBene, ora è necessario inserire le rose di ognuno dei giocatori. Se premi INVIO,\nla console verrà ripulita e si passerà alla schermata che permetterà di far questo.");
            Console.ReadKey();
            Console.Clear();
            SchermataInserimentoRose();
        }

        //Metodo che viene richiamato solo da PrimoAvvio e che contiene la visualizzazione video delle richieste che permettono all'utente l'inserimento delle rose dei fanta-allenatori.
        static private void SchermataInserimentoRose() 
        {
            bool verifica = false;
            string ruolo;
            string[] caratteristicheInserite;
            VisualizzaIntestazione();
            Console.WriteLine("\nBene, ora che sono stati inseriti i dati principali dei fanta-allenatori, è necessario  " +
                "\nche venga inserito ognuno dei nomi dei fanta-calciatori che sono stati aggiudicati durante l'asta.");
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (j < 3)
                    {
                        ruolo = "Portiere";
                    }
                    else if (j >= 3 && j < 11)
                    {
                        ruolo = "Difensore";
                    }
                    else if (j >= 11 && j < 19)
                    {
                        ruolo = "Centrocampista";
                    }
                    else
                    {
                        ruolo = "Attaccante";
                    }
                    SetResetColori(true);
                    Console.WriteLine($"\n{FantaAllenatori[i].OttieniNome()}, inserisci il nome, il cognome, la squadra, il numero di maglia e la quotazione iniziale\n(separati da una virgola) del tuo {j + 1}° fanta-giocatore ({ruolo}):");
                    SetResetColori(false);
                    do
                    {
                        string informazioniInserite = Console.ReadLine();
                        caratteristicheInserite = informazioniInserite.Trim(' ').Split(',');
                        if (caratteristicheInserite.Length != 5)
                        {
                            Console.WriteLine("\nNon hai inserito il giusto numero caratteristiche, quindi reinserisci il fanta-calciatore:");
                            verifica = false;
                        }
                        else
                        {
                            for (int k = 3; k < caratteristicheInserite.Length; k++)
                            {
                                verifica = int.TryParse(caratteristicheInserite[k], out int verificaInteger);
                                if ((verifica == false || verificaInteger < 0) && k == caratteristicheInserite.Length - 1)
                                {
                                    Console.WriteLine($"\nIn alcune caratteristiche non hai inserito un numero maggiore\no uguale a zero, quindi reinserisci il fanta-calciatore:");
                                    break;
                                }
                            }
                        }
                    }
                    while (verifica == false);
                    FantaCalciatore fantaCalciatore = new FantaCalciatore(caratteristicheInserite[0], caratteristicheInserite[1], caratteristicheInserite[2], ruolo, int.Parse(caratteristicheInserite[3]), int.Parse(caratteristicheInserite[4]), 0, 0, i);
                    FantaCalciatori = fantaCalciatore.AggiungiFantaCalciatore(FantaCalciatori);
                }
            }
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                Console.WriteLine($"\nQuesto è l'elenco del fanta-calciatori di {FantaAllenatori[i].OttieniNome()}:");
                for (int j = 0; j < FantaCalciatori.Count / FantaAllenatori.Count; j++)
                {
                    Console.WriteLine($"{j + 1}) {string.Join(' ', FantaCalciatori[j])}");
                }
                if (i != FantaAllenatori.Count - 1)
                {
                    Console.WriteLine("\nPer passare alla rosa del prossimo fanta-allenatore, premi un tasto qualsiasi...");
                    Console.ReadKey();
                }
            }
            Console.WriteLine("\nSei sicuro di voler salvare le rose dei Fanta-Allenatori? (inserisci \"S\" per salvare, oppure \"N\" per rifare l'inserimento delle rose dei fanta-calciatori)");
            switch (RispostaSalvataggio())
            {
                case ("S"):
                    Console.WriteLine("\nSalvataggio in corso...");
                    for (int i = 0; i < 2; i++)
                    {
                        string output;
                        if (i == 0)
                        {
                            output = JsonConvert.SerializeObject(FantaAllenatori);
                        }
                        else
                        {
                            output = JsonConvert.SerializeObject(FantaCalciatori);
                        }
                        OperazioniFile operazioniFile = new OperazioniFile();
                        operazioniFile.ScriviFile(percorsiIO[i], output);
                    }
                    break;
                case ("N"):
                    break;
            }
            Console.Clear();
        }

        //Metodo richiamato quando i dati iniziali sono stati configurati. Permette la visualizzazione di un menu di scelta tra le diverse funzionalità del programma.
        static private bool AvvioComune()  
        {
            bool chiusuraProgramma = false;
            bool verifica;
            Console.WriteLine($"                        Bentornato nel Fantacalcio. Oggi è il {DateTime.Now.ToString("dd/MM/yyyy")}.\n" +
                              " Scegli una delle seguenti funzioni inserendo il corrispondente valore numerico per iniziare... ");
            SetResetColori(true);
            Console.WriteLine("\n                                    =========================                                      ");
            SetResetColori(false);
            Console.WriteLine("\n1) Esegui la ricerca dei fanta-calciatori\n" +
                              "2) Visualizza i dati dei fanta-allenatori\n" +
                              "3) Visualizza/crea lo schieramento in campo attuale\n" +
                              "4) Aggiorna le statistiche dei fanta-calciatori\n" +
                              $"5) Visualizza la classifica parziale del torneo di fanta-torneo\n" +
                              "6) Ripristina le impostazioni iniziali\n" +
                              "7) Esci dal programma");
            SetResetColori(true);
            Console.WriteLine("\n                                    =========================                                      ");
            do
            {
                SetResetColori(false);
                char scelta = Console.ReadKey(false).KeyChar;
                verifica = true;
                switch (scelta)
                {
                    case ('1'):
                        SchermataRicercaFantaCalciatori();
                        break;
                    case ('2'):
                        SchermataVisualizzazioneFantaAllenatori();
                        break;
                    case ('3'):
                        SchermataSchieramentoCampoFantaCalciatori();
                        break;
                    case ('4'):
                        SchermataAggiornamentoStatisticheFantaCalciatori();
                        break;
                    case ('5'):
                        SchermataVisualizzazioneClassifiche();
                        break;
                    case ('6'):
                        chiusuraProgramma = SchermataCancellazioneDati();
                        break;
                    case ('7'):
                        chiusuraProgramma = true;
                        break;
                    default:
                        Console.WriteLine("\nNon hai inserito un valore presente in lista, quindi riprova...");
                        verifica = false;
                        break;
                }
            }
            while (verifica == false);
            return chiusuraProgramma;
        }

        //Metodo che permette di visualizzare la prima parte della schermata di ricerca dei fanta-calciatori. 
        static private void SchermataRicercaFantaCalciatori() 
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                        Funzionalità di ricerca dei fanta-calciatori                       ");
            Ricerca();
        }

        //Metodo che permette di visualizzare i dati sui fanta-allenatori.
        static public void SchermataVisualizzazioneFantaAllenatori()  
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                   Funzionalità di visualizzazione dei fanta-allenatori                   ");
            Console.WriteLine("\n   Di seguito verranno visualizzati i nomi e il budget disponibile di ogni fanta-allenatore.   \n");
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {FantaAllenatori[i].ToString()}");
            }
        }

        //Metodo che permette di visualizzare la prima parte della schermata di ricerca dei fanta-calciatori e poi l'inserimento di ogni giocatore in uno schieramento
        static private void SchermataSchieramentoCampoFantaCalciatori() 
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                 Funzionalità di schieramento in campo dei fanta-calciatori                 ");
            OperazioniFile operazioniFile = new OperazioniFile();
            bool verificaFile = operazioniFile.VerificaEsistenzaFile(percorsiIO[2]);
            bool verifica = false;
            if (verificaFile == true)
            {
                Console.WriteLine("\nOra verrà visualizzato lo schieramento di ogni fanta-allenatore");
                for (int i = 0; i < FantaAllenatori.Count; i++)
                {
                    SetResetColori(true);
                    Console.WriteLine($"\nQuesto è lo schieramento di {FantaAllenatori[i].OttieniNome()}:");
                    SetResetColori(false);
                    for (int j = 0; j < CodiciSchieramenti.Count / (i + 1); j++)
                    {
                        Console.WriteLine($"{j + 1}) {FantaCalciatori[CodiciSchieramenti[j * (i + 1)].Item2].ToString()}");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nGli schieramenti di ogni fanta-allenatore non sono stati ancora impostati, quindi, attraverso la\n     funzione di ricerca, puoi selezionare scegliere un giocatore per volta inserendo il\n                          valore numerico che gli corrisponde.");
                for (int i = 0; i < FantaAllenatori.Count; i++)
                {
                    for (int j = 0; j < 18; j++)
                    {
                        SetResetColori(true);
                        Console.WriteLine($"\nScegli il {j + 1}o fanta-calciatore da schierare per {FantaAllenatori[i].OttieniNome()}:");
                        SetResetColori(false);
                        bool aggiuntaGiocatore = Ricerca();
                        if (aggiuntaGiocatore == true)
                        {
                            Console.WriteLine("\nOra inserisci il giocatore che desideri inserire nel tuo schieramento:");
                            int scelta;
                            do
                            {
                                scelta = int.Parse(Console.ReadLine()) - 1;
                                if (scelta < 0 || scelta > Indici.Count)
                                {
                                    Console.WriteLine("\nNon hai inserito un valore accettabile. Riprova...");
                                    verifica = false;
                                }
                                else
                                {
                                    CodiciSchieramenti.Add((i, j));
                                    verifica = true;
                                }

                            } while (verifica == false);
                        }
                        else
                        {
                            Console.WriteLine($"\nVuoi riprovare a ricercare di nuovo il {j + 1}o fanta-calciatore da schierare, oppure no?");
                            if (RispostaSalvataggio() == "S")
                            {
                                j--;
                            }
                            else
                            {
                                verifica = false;
                                break;
                            }
                        }
                    }
                    if (verifica == false)
                    {
                        break;
                    }
                }
            }
            Console.WriteLine("\nGli schieramenti sono stati inseriti correttamenti. Desideri salvarli? (inserisci \"S\" se SI, altrimenti \"N\" se NO)");
            if (RispostaSalvataggio() == "S")
            {
                Console.WriteLine("\nSalvataggio in corso...");
                string output = JsonConvert.SerializeObject(CodiciSchieramenti);
                operazioniFile.ScriviFile(percorsiIO[2], output);
            }
        }

        //Metodo che permette di verificare la risposta di un utente in caso di una richiesta di salvataggio.
        static private string RispostaSalvataggio()
        {
            string scelta;
            bool verifica;
            do
            {
                scelta = Console.ReadLine().ToUpper();
                if (scelta != "S" && scelta != "N")
                {
                    Console.WriteLine("\nNon hai inserito un valore accettabile. Riprova...");
                    verifica = false;
                }
                else
                {
                    verifica = true;
                }

            } while (verifica == false);
            return scelta;
        }

        //Metodo che permette di visualizzare le richieste all'utente che porteranno all'aggiornamento delle statistiche dei fanta-calciatori.
        static private void SchermataAggiornamentoStatisticheFantaCalciatori() 
        {
            string[] testoDaVisualizzare = new string[] { "la quotazione attuale", "il punteggio ottenuto nell'ultima partita" };
            bool verifica;
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("            Funzionalità di aggiornamento delle statistiche dei fanta-calciatori            ");
            Console.WriteLine("\n     Ricerca un fanta-calciatore e cambiane le caratteristiche per aggiornare il suo\n                              punteggio in classifica");
            Ricerca();
            for (int i = 0; i < testoDaVisualizzare.Length; i++)
            {
                Console.WriteLine($"\nInserisci {testoDaVisualizzare[i]} del fanta-calciatore che hai scelto:");
                do
                {
                    verifica = int.TryParse(Console.ReadLine(), out int datoAggiornato);
                    if (verifica == false || datoAggiornato < 0)
                    {
                        Console.WriteLine("\nDevi inserire un numero maggiore o uguale a zero per questo campo. Riprova...");
                        verifica = false;
                    }

                } while (verifica == false);
            }
        }

        //Metodo che permette di visualizzare le richieste dei filtri di ricerca, ma anche i risultati della ricerca. 
        static private bool Ricerca()
        {
            string[] testoDaVisualizzare = new string[] { "il nome", "il cognome", "la squadra", "il ruolo", "il numero di maglia", "la quotazione iniziale", "la quotazione attuale", "il punteggio in classifica" };
            string[] giocatoreRicercato = new string[] { "", "", "", "", "0", "0", "0", "0" };
            int contatoreFiltri = 0;
            bool valoreRitornato;
            for (int i = 0; i < testoDaVisualizzare.Length; i++)
            {
                SetResetColori(true);
                Console.WriteLine($"\nInserisci {testoDaVisualizzare[i]} del fanta-calciatore che vuoi cercare:");
                SetResetColori(false);
                giocatoreRicercato[i] = Console.ReadLine();
                if (giocatoreRicercato[i] == "")
                {
                    break;
                }
                else
                {
                    contatoreFiltri++;
                }
            }
            FantaCalciatore fantaCalciatore = new FantaCalciatore(giocatoreRicercato[0], giocatoreRicercato[1], giocatoreRicercato[2], giocatoreRicercato[3], int.Parse(giocatoreRicercato[4]), int.Parse(giocatoreRicercato[5]), int.Parse(giocatoreRicercato[6]), int.Parse(giocatoreRicercato[7]), 0);
            Indici = fantaCalciatore.RicercaFantaCalciatore(FantaCalciatori, contatoreFiltri);
            if (Indici.Count > 0)
            {
                SetResetColori(true);
                if (Indici.Count == 1)
                {
                    Console.WriteLine("\nLa ricerca ha prodotto un risultato...\n");
                }
                else
                {
                    Console.WriteLine($"\nLa ricerca ha prodotto {Indici.Count} risultati...\n");
                }
                SetResetColori(false);
                Console.WriteLine("Di seguito verrà riportato l'elenco dei risultati con nome, cognome, squadra, ruolo, numero di maglia, quotazione iniziale, quotazione attuale, punteggio in classifica e proprietario del fanta-calciatore\n");
                valoreRitornato = true;
            }
            else
            {
                SetResetColori(true);
                Console.WriteLine("\nLa ricerca non ha prodotto risultati...");
                SetResetColori(false);
                valoreRitornato = false;
            }
            int multipliDiciassete = 1;
            for (int i = 0; i < Indici.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {FantaCalciatori[Indici[i]].ToString()} {FantaAllenatori[FantaCalciatori[Indici[i]].OttieniCodiceRosa()].OttieniNome()}");
                if (i == 17 * multipliDiciassete)
                {
                    multipliDiciassete++;
                    Console.WriteLine("\nPremi invio per visualizzare ulteriori risultati...\n");
                    Console.ReadKey();
                }
                else if (i == FantaCalciatori.Count - 1)
                {
                    Console.WriteLine("\n ===================  Fine dei risultati  =================== ");
                }
            }
            return valoreRitornato;
        }

        //Metodo che consente la visualizzazione della classifica dei fanta-calciatori.
        static private void SchermataVisualizzazioneClassifiche()
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("            Funzionalità di visualizzazione della classifica dei fanta-calciatori            ");
            Console.WriteLine("\n  Di seguito verrà visualizzata la classifica ordinata in modo crescente in base al punteggio.  ");
            FantaCalciatore fantaCalciatore = new FantaCalciatore("", "", "", "", 0, 0, 0, 0, 0);
            List<FantaCalciatore> FantaCalciatoriOrdinati = fantaCalciatore.OrdinaFantaCalciatori(FantaCalciatori);
            SetResetColori(true);
            Console.WriteLine("\n                                    =========================                                      \n");
            SetResetColori(false);
            int multipliNove = 1;
            for (int i = 0; i < FantaCalciatoriOrdinati.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {FantaCalciatoriOrdinati[i].ToString()}");
                if (i == 9 * multipliNove && i != FantaCalciatoriOrdinati.Count - 1)
                {
                    multipliNove++;
                    Console.WriteLine("\nPremi invio per visualizzare ulteriori risultati...");
                    Console.ReadKey();
                }
            }
        }

        //Metodo che visualizza le richieste a video che poi porteranno o meno alla cancellazione dei dati del fanta-torneo.
        static private bool SchermataCancellazioneDati() 
        {
            bool verifica;
            bool chiusuraProgramma = false;
            string scelta;
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                 Funzionalità di cancellazione dei dati del fanta-torneo                 ");
            Console.WriteLine("\n          Grazie a questa funzionalità sei in grado di eliminare qualsiasi dato\n                  del fanta-torneo e inserire delle informazioni nuove.");
            SetResetColori(true);
            Console.WriteLine("\nSei sicuro di voler eliminare tutti i dati inseriti del fanta-torneo?\n(inserisci \"S\" se SI, altrimenti inserisci \"N\" se NO)");
            SetResetColori(false);
            do
            {
                scelta = Console.ReadLine().ToUpper();
                if (scelta != "S" && scelta != "N")
                {
                    Console.WriteLine("\nNon hai inserito un valore corretto. Riprova...");
                    verifica = false;
                }
                else
                {
                    verifica = true;
                }

            } while (verifica == false);
            Console.WriteLine("\nSalvataggio delle modifiche in corso...");
            if (scelta == "S")
            {
                for (int i = 0; i < percorsiIO.Length; i++)
                {
                    OperazioniFile operazioniFile = new OperazioniFile();
                    operazioniFile.EliminaFile(percorsiIO[i]);
                    if (i == 2)
                    {
                        Console.WriteLine(operazioniFile.ToString());
                    }
                }
                FantaAllenatori.Clear();
                FantaCalciatori.Clear();
                Indici.Clear();
                CodiciSchieramenti.Clear();
            }
            return chiusuraProgramma;
        }

        //Metodo che permette di cambiare i colori dei caratteri e dello sfondo, oppure di farli tornare alle proprietà standard. 
        static public void SetResetColori(bool controllo)
        {
            if (controllo == true)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ResetColor();
            }
        }

    }

    class OperazioniFile //La classe OperazioniFile contiene i metodi necessari a compiere le operazioni su file. 
    {
        public bool VerificaEsistenzaFile(string percorsoIO)
        {
            return File.Exists(percorsoIO);
        }

        public void CreaDirectory(string percorsoIO)
        {
            try
            {
                Directory.CreateDirectory(percorsoIO);
            }
            catch(UnauthorizedAccessException)
            {
                GestisciErrori(0);
            }
            catch(IOException)
            {
                GestisciErrori(1);
            }
        }

        public string LeggiFile(string percorsoIO)
        {
            string contenutoInput = "";
            try
            {
                contenutoInput = File.ReadAllText(percorsoIO);
            }
            catch (UnauthorizedAccessException)
            {
                GestisciErrori(0);
            }
            catch (IOException)
            {
                GestisciErrori(1);
            }
            return contenutoInput;
        }
        public void ScriviFile(string percorsoIO, string contenutoOutput)
        {
            try
            {
                File.WriteAllText(percorsoIO, contenutoOutput);
            }
            catch (UnauthorizedAccessException)
            {
                GestisciErrori(0);
            }
            catch (IOException)
            {
                GestisciErrori(1);
            }
            
        }
        public void EliminaFile(string percorsoIO)
        {
            try
            {
                File.Delete(percorsoIO);
            }
            catch(UnauthorizedAccessException)
            {
                GestisciErrori(0);
            }
            catch(IOException)
            {
                GestisciErrori(1);
            }
        }
        public string GestisciErrori(int codiceErrore)
        {
            string descrizioneErrore = "";
            switch(codiceErrore)
            {
                case (0):
                    descrizioneErrore = "Si è verificato un errore nell'accesso al file (accesso non autorizzato). Si prega di riavviare il programma e, eventualmente, se non si risolve il problema, contattare l'amministratore di sistema.";
                    break;
                case (1):
                    descrizioneErrore = "Si è verificato un errore nell'accesso al file (errore di IO). Si prega di riavviare il programma e, eventualmente, se non si risolve il problema, contattare l'amministratore di sistema.";
                    break;
            }
            return descrizioneErrore;
        }
        public override string ToString()
        {
            return "\nLe modifiche sono state salvate con successo";
        }
    }

    [Serializable] //Attributo che indica che la classe sottostante può essere oggetto di serializzazione per l'inserimento degli attributi della classe in un file JSON.
    class FantaAllenatore //La classe FantaAllenatore contiene gli attributi e i metodi che permettono operazioni per ogni fanta-allenatore. 
    {
        //Attributi

        [JsonProperty] //Attributo che indica che l'attributo sottostante pò far parte delle proprietà di un file JSON; è importante perché senza questo speciale attributo non si sarebbe in grado di inserire il contenuto dell'attributo sottostante in un file JSON.
        private string nome { get; set; } //Attibuto della classe che contiene il nome del fanta-allenatore.
        [JsonProperty]
        private int codiceRosa { get; set; } //Attibuto della classe che contiene il codice della rosa del fanta-allenatore.
        [JsonProperty]
        private int budgetDisponibile { get; set; } //Attibuto della classe che contiene il budget disponibile del fanta-allenatore.

        //Metodi

        //Metodo costruttore: vengono passati al metodo tutti gli attributi di un fanta-calciatore all'istanziazione della classe e i loro valori vengono assegnati agli attributi della classe corrente.
        public FantaAllenatore(string nome, int codiceRosa, int budgetDisponibile)
        {
            this.nome = nome;
            this.codiceRosa = codiceRosa;
            this.budgetDisponibile = budgetDisponibile;
        }

        //Metodo che permette l'aggiunta di un fanta-allenatore alla lista dei fanta-allenatori.
        public List<FantaAllenatore> AggiungiFantaAllenatore(List<FantaAllenatore> FantaAllenatori) //Il metodo accetta come argomento la lista FantaAllenatori e la restituisce con il nuovo fanta-allenatore aggiunto. 
        {
            //Il metodo List.Add() permette di creare una nuova posizione all'interno della lista e di aggiungervi i nuovi dati (in questo caso quelli di un fanta-allenatore, i cui dati corrispondono agli attributi della classe FantaAllenatore).
            FantaAllenatori.Add(new FantaAllenatore(nome, codiceRosa, budgetDisponibile)
            {
                nome = nome,
                codiceRosa = codiceRosa,
                budgetDisponibile = budgetDisponibile
            });
            return FantaAllenatori;
        }

        //Metodo che permette di accedere all'attributo nome e di restituirlo.
        public string OttieniNome()
        {
            return nome;
        }

        //Metodo che sovrascrive il metodo ToString() e ritorna gli attributi del fanta-allenatore concatenati e divisi da uno spazio fra di loro grazie al metodo string.Join(). 
        public override string ToString()
        {
            return $"{string.Join(' ', nome, budgetDisponibile)} Fantamilioni";
        }
    }

    [Serializable] //Attributo che indica che la classe sottostante può essere oggetto di serializzazione per l'inserimento degli attributi della classe in un file JSON.
    class FantaCalciatore //La classe FantaCalciatore contiene gli attributi e i metodi che permettono operazioni per ogni fanta-calciatore. 
    {
        //Attributi

        [JsonProperty] //Attributo che indica che l'attributo sottostante pò far parte delle proprietà di un file JSON; è importante perché senza questo speciale attributo non si sarebbe in grado di inserire il contenuto dell'attributo sottostante in un file JSON.
        private string nome { get; set; } //Attibuto della classe che contiene il nome del fanta-calciatore.

        [JsonProperty] 
        private string cognome { get; set; } //Attibuto della classe che contiene il cognome del fanta-calciatore.

        [JsonProperty]
        private string squadra { get; set; } //Attibuto della classe che contiene il nome della squadra del fanta-calciatore.

        [JsonProperty]
        private string ruolo { get; set; } //Attibuto della classe che contiene il ruolo del fanta-calciatore nella squadra.

        [JsonProperty]
        private int numeroMaglia { get; set; } //Attibuto della classe che contiene il numero di maglia del fanta-calciatore.

        [JsonProperty]
        private int quotazioneIniziale { get; set; } //Attibuto della classe che contiene la quotazione iniziale, cioè a inizio del fanta-torneo, del fanta-calciatore.

        [JsonProperty]
        private int quotazioneAttuale { get; set; } //Attibuto della classe che contiene la quotazione attuale, cioè l'ultima, del fanta-calciatore.

        [JsonProperty]
        private int punteggioClassifica { get; set; } //Attibuto della classe che contiene l'attuale punteggio in classifica del fanta-calciatore.

        [JsonProperty]
        private int codiceRosa { get; set; } //Attibuto della classe che contiene il codice del fanta-allenatore a cui appartiene il fanta-calciatore.

        //Metodi

        //Metodo costruttore: vengono passati al metodo tutti gli attributi di un fanta-calciatore all'istanziazione della classe e i loro valori vengono assegnati agli attributi della classe corrente.
        public FantaCalciatore(string nome, string cognome, string squadra, string ruolo, int numeroMaglia, int quotazioneIniziale, int quotazioneAttuale, int punteggioClassifica, int codiceRosa)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.squadra = squadra;
            this.ruolo = ruolo;
            this.numeroMaglia = numeroMaglia;
            this.quotazioneIniziale = quotazioneIniziale;
            this.quotazioneAttuale = quotazioneAttuale;
            this.punteggioClassifica = punteggioClassifica;
            this.codiceRosa = codiceRosa;
        }

        //Metodo che permette l'aggiunta di un fanta-calciatore alla lista dei fanta-calciatori.
        public List<FantaCalciatore> AggiungiFantaCalciatore(List<FantaCalciatore> FantaCalciatori) //Il metodo accetta come argomento la lista FantaCalciatori e la restituisce con il nuovo fanta-calciatore aggiunto.
        {
            //Il metodo List.Add() permette di creare una nuova posizione all'interno della lista e di aggiungervi i nuovi dati (in questo caso quelli di un fanta-calciatore, i cui dati corrispondono agli attributi della classe FantaCalciatore).
            FantaCalciatori.Add(new FantaCalciatore(nome, cognome, squadra, ruolo, numeroMaglia, quotazioneIniziale, quotazioneAttuale, punteggioClassifica, codiceRosa)
            {
                nome = nome,
                cognome = cognome,
                squadra = squadra,
                ruolo = ruolo,
                numeroMaglia = numeroMaglia,
                quotazioneIniziale = quotazioneIniziale,
                quotazioneAttuale = quotazioneAttuale,
                punteggioClassifica = punteggioClassifica,
                codiceRosa = codiceRosa

            });
            return FantaCalciatori;
        }

        //Metodo che permette di eseguire la ricerca del fanta-calciatore all'interno della lista dei fanta-calciatori.
        public List<int> RicercaFantaCalciatore(List<FantaCalciatore> FantaCalciatori, int contatoreFiltri) //Il metodo accetta come argomenti la lista FantaCalciatori e il numero di filtri inseriti dall'utente e la restituisce con una nuova lista (Indici) con gli indici degli elementi ricercati nella lista FantaCalciatori.
        {
            List<int> Indici = new List<int>(); //Viene inizializzata una lista di tipo int per contenere gli indici dei fanta-calciatori trovati.
            for (int i = 0; i < contatoreFiltri; i++)
            {
                //Il metodo List.FindAll() consente di trovare tutte le occorrenze all'interno di una lista basandosi su oggetti di tipo Predicate definiti dal metodo DefinisciPredicato.
                var elementi = FantaCalciatori.FindAll(DefinisciPredicato(i));
                if (i == contatoreFiltri - 1) //Solo alla fine della verifica dei filtri si eseguono le seguenti istruzioni.
                {
                    foreach (var elemento in elementi) //Per ogni elemento trovato in elementi, si aggiunge l'indice nella lista FantaCalciatori corrispondente all'elemento nella lista Indici.
                    {
                        Indici.Add(FantaCalciatori.IndexOf(elemento)); //Il metodo List.IndexOf() permette di individuare l'indice di un elemento all'interno di una lista.
                    }
                }
            }
            return Indici;
        }

        //Metodo di tipo Predicate<T> che definisce un set di criteri e determina se l'oggetto specificato soddisfa tali criteri; è utilizzato per definire il predicato da utilizzare metodo di ricerca.
        private Predicate<FantaCalciatore> DefinisciPredicato(int i) //Il metodo accetta come argomento un indice e ritorna un predicato in base al valore dell'indice.
        {
            Predicate<FantaCalciatore> predicato = null; //Viene inizializzato il predicato con il valore null.
            switch (i) //In base al valore di i, si possono verificare diversi casi.
            {
                //Ognuno dei seguenti casi viene applicato nel caso in cui ci si trovi, nella ricerca, ad analizzare l'attributo nome, cognome, squadra, ruolo, ecc... 
                //Per il confronto di stringhe viene usato il metodo string.Contains(), mentre per il valori interi si usa semplicemente un operatore di uguaglianza.
                case (0):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.nome.Contains(nome);
                    break;
                case (1):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.cognome.Contains(cognome);
                    break;
                case (2):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.squadra.Contains(squadra);
                    break;
                case (3):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.ruolo.Contains(ruolo);
                    break;
                case (4):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.numeroMaglia == numeroMaglia;
                    break;
                case (5):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.quotazioneIniziale == quotazioneIniziale;
                    break;
                case (6):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.quotazioneAttuale == quotazioneAttuale;
                    break;
                case (7):
                    predicato = FantaCalciatoriTrovati => FantaCalciatoriTrovati.punteggioClassifica == punteggioClassifica;
                    break;
            }
            return predicato;
        }

        //Metodo che permette di eseguire l'ordinamento della lista dei fanta-calciatori in base al punteggio ottenuto.
        public List<FantaCalciatore> OrdinaFantaCalciatori(List<FantaCalciatore> FantaCalciatori) //Il metodo accetta come argomento la lista FantaCalciatori e ritorna la stessa lista ordinata.
        {
            FantaCalciatori.Sort(ComparaPunteggi); //Il metodo List.Sort(Comparison<T>) permette di eseguire l'ordinamento usando un oggetto di tipo Comparison definito dal metodo ComparaPunteggi.
            return FantaCalciatori;
        }

        //Metodo di tipo Comparison<T> che permette di confrontare due fanta-calciatori e determinare quale dei due ha maggiore punteggio in classifica.
        private int ComparaPunteggi(FantaCalciatore calciatore1, FantaCalciatore calciatore2) //Il metodo accetta come argomenti due istanze di FantaCalciatore e ritorna il valore restituito dal metodo string.CompareTo().
        {
            return calciatore2.punteggioClassifica.CompareTo(calciatore1.punteggioClassifica); //Il metodo string.CompareTo() è necessario nel caso per determinare se una stringa si trovi prima o dopo nell'ordinamenti rispetto ad un'altra stringa.
        }

        //Metodo che permette di accedere all'attributo codiceRosa e che restituisce il valore di questo attributo.
        public int OttieniCodiceRosa()
        {
            return codiceRosa;
        }

        //Metodo che permette di accedere all'attributo quotazioneAttuale e aggiornarne il valore.
        public void AggiornaQuotazioneAttuale(int quotazioneAttuale)
        {
            this.quotazioneAttuale = quotazioneAttuale;
        }

        //Metodo che permette di accedere all'attributo punteggioClassifica e aggiornarne il valore, sommando il punteggio già esistente a quello ottenuto nell'ultima partita.
        public int AggiornaPunteggio(int punteggioPartita)
        {
            punteggioClassifica += punteggioPartita;
            return punteggioClassifica;
        }

        //Metodo che sovrascrive il metodo ToString() e ritorna gli attributi del fanta-calciatore concatenati e divisi da uno spazio fra di loro grazie al metodo string.Join(). 
        public override string ToString()
        {
            return string.Join(' ', nome, cognome, squadra, ruolo, numeroMaglia, quotazioneIniziale, quotazioneAttuale, punteggioClassifica);
        }
    }
}
