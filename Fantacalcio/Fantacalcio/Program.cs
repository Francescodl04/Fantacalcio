/* Autore: Francesco Di Lena
 * Classe: 4F
 * Consegna: Progettare un sistema di gestione del Fantacalcio.
 */

//Direttive using standard.
using System; //Spazio dei nomi che permette l'uso dei metodi più comuni.
using System.Threading.Tasks; //Spazio dei nomi che permette la posticipazione delle istruzioni.
using System.IO; //Spazio dei nomi che permette la gestione dei file in generale.
using System.Collections.Generic; //Spazio dei nomi che permette la gestione delle liste.
//Direttive using aggiuntive (NuGet).
using Newtonsoft.Json; //Spazio dei nomi che permette la gestione dei file json.


namespace Fantacalcio
{
    //Classi che compongono il programma.

    class Programma //La classe Programma è la prima ad essere richiamata dal programma al suo avvio e contiene la visualizzazione a schermo di testo, oltre alle richieste di inserimento rivolte all'utente. 
    {
        static FantaAllenatore FantaAllenatore;
        static FantaCalciatore FantaCalciatore;
        static string ruolo, torneo;
        static string[] percorsiIO = new string[] { @"C:\Programma Gestionale Fantacalcio\fanta-allenatori.json", @"C:\Programma Gestionale Fantacalcio\fanta-calciatori.json", @"C:\Programma Gestionale Fantacalcio\schieramenti.json" };
        static string percorsoCartella = @"C:\Programma Gestionale Fantacalcio";
        static List<FantaAllenatore> FantaAllenatori = new List<FantaAllenatore>();
        static List<FantaCalciatore> FantaCalciatori = new List<FantaCalciatore>();
        static List<int> indici = new List<int>();

        static void Main()
        {
            bool chiusuraProgramma;
            bool[] verificaFile = new bool[2];
            Console.Title = "Programma Gestionale del Fantacalcio";
            do
            {
                Console.Clear();
                for (int i = 0; i < 2; i++)
                {
                    OperazioniFile operazioniFile = new OperazioniFile();
                    verificaFile[i] = operazioniFile.VerificaEsistenzaFile(percorsiIO[i]);
                }
                VisualizzaIntestazione();
                if (verificaFile[0] == false || verificaFile[1] == false)
                {
                    var fantacalcio = FantaAllenatore;
                    OperazioniFile operazioniFile = new OperazioniFile();
                    operazioniFile.CreaDirectory(percorsoCartella);
                    PrimoAvvio();
                }
                for (int i = 0; i < 2; i++)
                {
                    OperazioniFile operazioniFile = new OperazioniFile();
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
            torneo = Console.ReadLine();
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

        static private void SchermataInserimentoRose()
        {
            bool verifica = false;
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
                    Console.WriteLine($"\n{FantaAllenatori[i].nome}, inserisci il nome, il cognome, la squadra, il numero di maglia, la quotazione iniziale\ne il punteggio in classifica (separati da una virgola) del tuo {j + 1}° fanta-giocatore ({ruolo}):");
                    SetResetColori(false);
                    do
                    {
                        string informazioniInserite = Console.ReadLine();
                        caratteristicheInserite = informazioniInserite.Trim(' ').Split(',');
                        if (caratteristicheInserite.Length != 6)
                        {
                            Console.WriteLine("\nNon hai inserito abbastanza caratteristiche, quindi reinserisci il fanta-calciatore:");
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
                    FantaCalciatore = new FantaCalciatore(caratteristicheInserite[0], caratteristicheInserite[1], caratteristicheInserite[2], ruolo, int.Parse(caratteristicheInserite[3]), int.Parse(caratteristicheInserite[4]), int.Parse(caratteristicheInserite[5]), 0, i);
                    FantaCalciatori = FantaCalciatore.AggiungiFantaCalciatore(FantaCalciatori);
                }
            }
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                Console.WriteLine($"Questo è l'elenco del fanta-calciatori di {FantaAllenatori[i].nome}:");
                for (int j = 0; j < FantaCalciatori.Count / FantaAllenatori.Count; j++)
                {
                    Console.WriteLine($"{j + 1}) {string.Join(' ', FantaCalciatori[i])}");
                }
                if (i != FantaCalciatori.Count - 1)
                {
                    Console.WriteLine("\nPer passare alla rosa del prossimo fanta-allenatore, premi un tasto qualsiasi...");
                    Console.ReadKey();
                }
            }
            Console.WriteLine("\nSei sicuro di voler salvare le rose dei Fanta-Allenatori? (inserisci \"S\" per salvare, oppure \"N\" per rifare l'inserimento delle rose dei fanta-calciatori)");
            string confermaSalvataggio = Console.ReadLine().ToUpper();
            switch (confermaSalvataggio)
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
        }

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
                              "2) Visualizza/crea lo schieramento in campo attuale\n" +
                              "3) Aggiorna le statistiche dei fanta-calciatori\n" +
                              $"4) Visualizza la classifica parziale del torneo di fanta-{torneo}\n" +
                              "5) Ripristina le impostazioni iniziali\n" +
                              "6) Esci dal programma");
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
                        SchermataSchieramentoCampoFantaCalciatori();
                        break;
                    case ('3'):
                        SchermataAggiornamentoStatisticheFantaCalciatori();
                        break;
                    case ('4'):
                        SchermataVisualizzazioneClassifiche();
                        break;
                    case ('5'):
                        chiusuraProgramma = SchermataCancellazioneDati();
                        break;
                    case ('6'):
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

        static private void SchermataRicercaFantaCalciatori()
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                        Funzionalità di ricerca dei fanta-calciatori                       ");
            Ricerca();
        }

        static private void SchermataSchieramentoCampoFantaCalciatori()
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                 Funzionalità di schieramento in campo dei fanta-calciatori                 ");
            OperazioniFile operazioniFile = new OperazioniFile();
            bool verificaFile = operazioniFile.VerificaEsistenzaFile(percorsiIO[2]);
            bool verifica = false;
            int[,] codiciSchieramenti = new int[FantaAllenatori.Count, 18];
            if (verificaFile == true)
            {
                Console.WriteLine("\nOra verrà visualizzato lo schieramento di ogni fanta-allenatore");
            }
            else
            {
                Console.WriteLine("\nGli schieramenti di ogni fanta-allenatore non sono stati ancora impostati.");
                for (int i = 0; i < FantaAllenatori.Count; i++)
                {
                    codiciSchieramenti[i, 0] = i;
                    for (int j = 0; j < 18; j++)
                    {
                        SetResetColori(true);
                        Console.WriteLine($"\nScegli il {j + 1}o fanta-calciatore da schierare per {FantaAllenatori[i].nome}:");
                        SetResetColori(false);
                        Ricerca();
                        Console.WriteLine("\nOra inserisci il giocatore che desideri inserire nel tuo schieramento:");
                        int scelta;
                        do
                        {
                            scelta = int.Parse(Console.ReadLine()) - 1;
                            if (scelta < 0 || scelta > indici.Count)
                            {
                                Console.WriteLine("\nNon hai inserito un valore accettabile. Riprova...");
                                verifica = false;
                            }
                            else
                            {
                                codiciSchieramenti[i, 1] = scelta;
                                verifica = true;
                            }

                        } while (verifica == false);
                    }
                }
            }
            
        }

        static private void SchermataAggiornamentoStatisticheFantaCalciatori()
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("            Funzionalità di aggiornamento delle statistiche dei fanta-calciatori            ");
        }

        static private void Ricerca()
        {
            string[] testoDaVisualizzare = new string[] { "il nome", "il cognome", "la squadra", "il ruolo", "il numero di maglia", "la quotazione iniziale", "la quotazione attuale", "il punteggio in classifica" };
            string[] giocatoreRicercato = new string[] { "", "", "", "", "0", "0", "0", "0" };
            int contatoreFiltri = 0;
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
            FantaCalciatore = new FantaCalciatore(giocatoreRicercato[0], giocatoreRicercato[1], giocatoreRicercato[2], giocatoreRicercato[3], int.Parse(giocatoreRicercato[4]), int.Parse(giocatoreRicercato[5]), int.Parse(giocatoreRicercato[6]), int.Parse(giocatoreRicercato[7]), 0);
            indici = FantaCalciatore.RicercaFantaCalciatore(FantaCalciatori, contatoreFiltri);
            if (indici.Count > 0)
            {
                SetResetColori(true);
                if (indici.Count == 1)
                {
                    Console.WriteLine("\nLa ricerca ha prodotto un risultato...\n");
                }
                else
                {
                    Console.WriteLine($"\nLa ricerca ha prodotto {indici.Count} risultati...\n");
                }
                SetResetColori(false);
                Console.WriteLine("Di seguito verrà riportato l'elenco dei risultati con nome, cognome, squadra, ruolo, numero di maglia, quotazione iniziale, quotazione attuale, punteggio in classifica e proprietario del fanta-calciatore\n");
            }
            else
            {
                SetResetColori(true);
                Console.WriteLine("\nLa ricerca non ha prodotto risultati...");
                SetResetColori(false);
            }
            int multipliDiciassete = 1;
            for (int i = 0; i < indici.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {FantaCalciatori[indici[i]].nome} {FantaCalciatori[indici[i]].cognome} {FantaCalciatori[indici[i]].squadra} {FantaCalciatori[indici[i]].ruolo} {FantaCalciatori[indici[i]].numeroMaglia} {FantaCalciatori[indici[i]].quotazioneIniziale} {FantaCalciatori[indici[i]].quotazioneAttuale} {FantaCalciatori[indici[i]].punteggioClassifica} {FantaAllenatori[FantaCalciatori[indici[i]].codiceRosa].nome}");
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
        }

        static private void SchermataVisualizzazioneClassifiche()
        {
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("            Funzionalità di visualizzazione della classifica dei fanta-calciatori            ");
        }

        static private bool SchermataCancellazioneDati()
        {
            bool verifica = false;
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
            }
            return chiusuraProgramma;
        }

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
        public OperazioniFile()
        {
            
        }
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
            File.Delete(percorsoIO);
        }
        public void GestisciErrori(int codiceErrore)
        {
            
        }
        public override string ToString()
        {
            return "\nLe modifiche sono state salvate con successo";
        }
    }

    class FantaAllenatore //La classe FantaAllenatore contiene gli attributi e i metodi che permettono operazioni per ogni fanta-allenatore. 
    {
        public string nome { get; set; }
        public int codiceRosa { get; set; }
        public int budgetDisponibile { get; set; }

        public FantaAllenatore(string nome, int codiceRosa, int budgetDisponibile)
        {
            this.nome = nome;
            this.codiceRosa = codiceRosa;
            this.budgetDisponibile = budgetDisponibile;
        }

        public List<FantaAllenatore> AggiungiFantaAllenatore(List<FantaAllenatore> FantaAllenatori)
        {
            FantaAllenatori.Add(new FantaAllenatore(nome, codiceRosa, budgetDisponibile)
            {
                nome = nome,
                codiceRosa = codiceRosa,
                budgetDisponibile = budgetDisponibile
            });
            return FantaAllenatori;
        }

        public override string ToString()
        {
            return "";
        }

    }

    class FantaCalciatore //La classe FantaCalciatore contiene gli attributi e i metodi che permettono operazioni per ogni fanta-calciatore. 
    {
        public string nome { get; set; }
        public string cognome { get; set; }
        public string squadra { get; set; }
        public string ruolo { get; set; }
        public int numeroMaglia { get; set; }
        public int quotazioneIniziale { get; set; }
        public int quotazioneAttuale { get; set; }
        public int punteggioClassifica { get; set; }
        public int codiceRosa { get; set; }

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

        public List<FantaCalciatore> AggiungiFantaCalciatore(List<FantaCalciatore> FantaCalciatori)
        {
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

        public List<int> RicercaFantaCalciatore(List<FantaCalciatore> FantaCalciatori, int contatoreFiltri)
        {
            List<FantaCalciatore> FantaCalciatoriTrovati = FantaCalciatori;
            List<int> indici = new List<int>();
            for (int i = 0; i < contatoreFiltri; i++)
            {
                var elementi = FantaCalciatoriTrovati.FindAll(CreaPredicato(i));
                if (i == contatoreFiltri - 1)
                {
                    foreach (var elemento in elementi)
                    {
                        indici.Add(FantaCalciatoriTrovati.IndexOf(elemento));
                    }
                }
            }
            return indici;
        }

        Predicate<FantaCalciatore> CreaPredicato(int i)
        {
            Predicate<FantaCalciatore> predicato = null;
            switch (i)
            {
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
        public void AggiornaStatisticheFantaCalciatore()
        {

        }

        public void OrdinaFantaCalciatori()
        {

        }

        public override string ToString()
        {
            return $"{nome} {cognome} {squadra} {ruolo} {numeroMaglia} {quotazioneIniziale} {quotazioneAttuale} {punteggioClassifica}";
        }
    }
}
