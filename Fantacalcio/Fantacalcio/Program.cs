/* Autore: Francesco Di Lena
 * Classe: 4F
 * Consegna: Progettare un sistema di gestione del Fantacalcio.
 */


using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;


namespace Fantacalcio
{
    class Programma
    {
        static bool verifica;
        static FantaAllenatore FantaAllenatore;
        static FantaCalciatore FantaCalciatore;
        static string nome, cognome, squadra, ruolo, torneo;
        static string[] percorsiIO = new string[] { @"C:\Programma Gestionale Fantacalcio\fanta-allenatori.json", @"C:\Programma Gestionale Fantacalcio\fanta-calciatori.json" };
        static int numeroMaglia, quotazioneIniziale, quotazioneAttuale, punteggioClassifica;
        static List<FantaAllenatore> FantaAllenatori = new List<FantaAllenatore>();
        static List<FantaCalciatore> FantaCalciatori = new List<FantaCalciatore>();

        static void Main(string[] args)
        {
            Console.Title = "Programma Gestionale del Fantacalcio";
            for (int i = 0; i < 2; i++)
            {
                OperazioniFile operazioniFile = new OperazioniFile(percorsiIO[i], "");
                verifica = operazioniFile.VerificaEsistenzaFile();
            }
            VisualizzaIntestazione();
            if (verifica == false)
            {
                PrimoAvvio();
                AvvioComune();
            }
            else
            {
                AvvioComune();
            }
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

        static private void AvvioComune()
        {
            Console.WriteLine($"                        Bentornato nel Fantacalcio. Oggi è il {DateTime.Now.ToString("dd/MM/yyyy")}.\n" +
                             $"                   Il tuo ultimo accesso risale alle ore 11:24 del 07/11/2021.             \n" +
                              " Scegli una delle seguenti funzioni inserendo il corrispondente valore numerico per iniziare... ");
            SetResetColori(true);
            Console.WriteLine("\n                                    =========================                                      ");
            SetResetColori(false);
            Console.WriteLine("\n1) Esegui la ricerca dei fanta-calciatori\n" +
                              "2) Visualizza/crea lo schieramento in campo attuale\n" +
                              "3) Aggiorna le statistiche dei fanta-calciatori\n" +
                              $"4) Visualizza la classifica parziale del torneo di fanta-{torneo}\n" +
                              "5) Ripristina le impostazioni iniziali\n" +
                              "6) Leggi i comandi\n" +
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
                        SchermataSchieramentoCampoFantaCalciatori();
                        break;
                    case ('3'):
                        SchermataAggiornamentoStatisticheFantaCalciatori();
                        break;
                    case ('4'):
                        SchermataVisualizzazioneClassifiche();
                        break;
                    case ('5'):
                        SchermataCancellazioneDati();
                        break;
                    case ('6'):
                        SchermataComandi();
                        break;
                    case ('7'):
                        break;
                    default:
                        Console.WriteLine("\nNon hai inserito un valore presente in lista, quindi riprova...");
                        verifica = false;
                        break;
                }
            }
            while (verifica == false);
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
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.ResetColor();
            }
        }

        static private void SchermataInserimentoRose()
        {
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
                        ruolo = "portiere";
                    }
                    else if (j >= 3 && j < 11)
                    {
                        ruolo = "difensore";
                    }
                    else if (j >= 11 && j < 19)
                    {
                        ruolo = "centrocampista";
                    }
                    else
                    {
                        ruolo = "attaccante";
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
                            output = JsonSerializer.Serialize(FantaAllenatori);
                        }
                        else
                        {
                            output = JsonSerializer.Serialize(FantaCalciatori);
                        }
                        OperazioniFile operazioniFile = new OperazioniFile(percorsiIO[i], output);
                        operazioniFile.ScriviFile();
                    }
                    break;
                case ("N"):
                    break;
            }
        }

        static private void SchermataRicercaFantaCalciatori()
        {
            string[] testoDaVisualizzare = new string[] { "il nome", "il cognome", "la squadra", "il ruolo", "il numero di maglia", "la quotazione iniziale", "la quotazione attuale", "il punteggio in classifica"};
            string[] giocatoreRicercato = new string[] { "", "", "", "", "0", "0", "0", "0" };
            Console.Clear();
            VisualizzaIntestazione();
            Console.WriteLine("                        Funzionalità di ricerca dei fanta-calciatori                       ");
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
            List<FantaCalciatore> FantaCalciatoriTrovati = FantaCalciatore.RicercaFantaCalciatore(FantaCalciatori, contatoreFiltri);
            SetResetColori(true);
            if (FantaCalciatoriTrovati.Count == 0) 
            {
                Console.WriteLine("\nLa ricerca non ha prodotto risultati...");
            }
            else if (FantaCalciatoriTrovati.Count == 1)
            {
                Console.WriteLine("\nLa ricerca ha prodotto un risultato...");
            }
            else
            {
                Console.WriteLine($"\nLa ricerca ha prodotto {FantaCalciatoriTrovati.Count} risultati...");
            }
            SetResetColori(false);
        }

        static private void SchermataSchieramentoCampoFantaCalciatori()
        {

        }

        static private void SchermataAggiornamentoStatisticheFantaCalciatori()
        {

        }

        static private void SchermataVisualizzazioneClassifiche()
        {

        }

        static private void SchermataCancellazioneDati()
        {

        }

        static private void SchermataComandi()
        {

        }
    }

    class OperazioniFile
    {
        string percorsoIO, contenutoOutput;
        public OperazioniFile(string percorsoIO, string contenutoOutput)
        {
            this.percorsoIO = percorsoIO;
            this.contenutoOutput = contenutoOutput;
        }
        public bool VerificaEsistenzaFile()
        {
            return File.Exists(percorsoIO);
        }
        public void CreaFile()
        {
            try
            {
                File.Create(percorsoIO);
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
        public void LeggiFile()
        {
            try
            {
                string contenutoInput = File.ReadAllText(percorsoIO);
            }
            catch(UnauthorizedAccessException)
            {
                GestisciErrori(0);
            }
            catch (IOException)
            {
                GestisciErrori(1);
            }
        }
        public void ScriviFile()
        {
            try
            {
                File.WriteAllText(percorsoIO, contenutoOutput);
            }
            catch(UnauthorizedAccessException)
            {
                GestisciErrori(0);
            }
            catch (IOException)
            {
                GestisciErrori(1);
            }
        }
        public void EliminaFile()
        {
            File.Delete(percorsoIO);
        }
        public void GestisciErrori(int codiceErrore)
        {
            
        }
    }

    class FantaAllenatore
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

        public void AggiungiRosa()
        {

        }

        public void AggiungiSchieramento()
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

    }

    class FantaCalciatore
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

        public List<FantaCalciatore> RicercaFantaCalciatore(List<FantaCalciatore> FantaCalciatori, int contatoreFiltri)
        {
            List<FantaCalciatore> FantaCalciatoriTrovati = FantaCalciatori;
            for (int i = 0; i < contatoreFiltri; i++)
            {
                FantaCalciatoriTrovati = FantaCalciatoriTrovati.FindAll(CreaPredicato(i));
            }
            return FantaCalciatoriTrovati;
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
            return base.ToString();
        }
    }
}
