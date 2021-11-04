using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace Fantacalcio
{
    class Programma
    {
        static bool verifica;
        static FantaAllenatore FantaAllenatore;
        static FantaCalciatore FantaCalciatore;
        static string nome, cognome, squadra, ruolo;
        static int numeroMaglia, quotazioneIniziale, quotazioneAttuale, punteggioClassifica;

        static void Main(string[] args)
        {
            Files Files = new Files();
            verifica = Files.VerificaEsistenzaFile(@"");
            SetResetColori(true);
            Console.WriteLine("                          PROGRAMMA GESTIONALE DEL FANTACALCIO                          ");
            SetResetColori(false);
            if (verifica == false)
            {
                PrimoAvvio();
            }
            else
            {
                AvvioComune();
            }

        }
        static public void PrimoAvvio()
        {
            Console.WriteLine("                  Benvenuto nel programma gestionale del fantacalcio.                  "
                    + "\n\nQuesto è il primo avvio del programma, così, per giocare devi inserire tutti i dati " +
                    "\nche riguardano il torneo; il programma ti guiderà nell'inserimento e ogni volta che ti " +
                    "\nfarà delle richieste dovrai inserire un comando seguito da un INVIO.");
            SetResetColori(true);
            Console.WriteLine("\nInserisci il torneo reale di riferimento per il gioco:                                  ");
            SetResetColori(false);
            string torneo = Console.ReadLine();
            int numeroFantaAllenatori;
            do
            {
                SetResetColori(true);
                Console.WriteLine("\nInserisci il numero dei fanta-allenatori che parteciperanno al gioco:                   ");
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
                Console.WriteLine($"\nInserisci il nome del {i + 1}° giocatore:                                 ");
                SetResetColori(false);
                string nomeFantaAllenatore = Console.ReadLine();
                int budgetDisponibile;
                do
                {
                    SetResetColori(true);
                    Console.WriteLine($"\nInserisci il budget del {nomeFantaAllenatore} (in Fantamilioni):                   ");
                    SetResetColori(false);
                    verifica = int.TryParse(Console.ReadLine(), out budgetDisponibile);
                    if (verifica == false)
                    {
                        Console.WriteLine("\nDevi inserire un numero. Riprova...");
                    }
                }
                while (verifica == false);
                FantaAllenatore = new FantaAllenatore();
                FantaAllenatore.AggiungiFantaAllenatore(nome, i, budgetDisponibile);
                Console.WriteLine(FantaAllenatore.FantaAllenatori.Count);
            }

            Console.WriteLine("\nBene, ora è necessario inserire le rose di ognuno dei giocatori. Se premi INVIO,\nla console verrà ripulita e si passerà alla schermata che permetterà di far questo.");
            Console.ReadKey();
            Console.Clear();
            SchermataInserimentoRose();
        }

        static private void AvvioComune()
        {
            Console.WriteLine("                  Benvenuto nel programma gestionale del fantacalcio.                  ");
            SetResetColori(true);
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
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        static private void SchermataInserimentoRose()
        {
            string[] caratteristicaRichiesta = new string[] { "il nome", "il cognome", "la squadra", "il numero di maglia", "la quotazione iniziale", "il punteggio in classifica" };
            string[] caratteristicheInserite = new string[7];
            SetResetColori(true);
            Console.WriteLine("                          PROGRAMMA GESTIONALE DEL FANTACALCIO                          ");
            SetResetColori(false);
            Console.WriteLine("\nBene, ora che sono stati inseriti i dati principali dei fanta-allenatori, è necessario  " +
                "\nche venga inserito ognuno dei nomi dei fanta-calciatori che sono stati aggiudicati durante l'asta.");
            for (int i = 0; i < FantaAllenatore.FantaAllenatori.Count; i++)
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

                    for (int k = 0; k < 7; k++)
                    {
                        SetResetColori(true);
                        Console.WriteLine($"\n\n{FantaAllenatore.FantaAllenatori[1].nome}, inserisci {caratteristicaRichiesta[k]} del tuo {j + 1}° fanta-giocatore ({ruolo}):");
                        SetResetColori(false);
                        if (k < 3)
                        {
                            caratteristicheInserite[k] = Console.ReadLine();
                        }
                        else
                        {
                            do
                            {
                                verifica = int.TryParse(Console.ReadLine(), out int tmp);
                                if (verifica == false)
                                {
                                    Console.WriteLine($"Per questa caratteristica devi inserire un numero, quindi reinserisci {caratteristicaRichiesta[k]}:");
                                }
                            }
                            while (verifica == false);
                        }

                    }
                    FantaCalciatore = new FantaCalciatore(caratteristicaRichiesta[0], caratteristicaRichiesta[1], caratteristicaRichiesta[2], caratteristicaRichiesta[3], int.Parse(caratteristicaRichiesta[4]), int.Parse(caratteristicaRichiesta[5]), 0, int.Parse(caratteristicaRichiesta[6]));
                }
            }
            SetResetColori(false);
        }

        static private void SchermataSchieramentoCampoFantaCalciatori()
        {

        }

        static private void SchermataVisualizzazioneFantaCalciatori()
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

        static private void UscitaProgramma()
        {
            Environment.Exit(0);
        }
    }

    class Files
    {
        public Files()
        {

        }
        public bool VerificaEsistenzaFile(string percorsoIO)
        {
            return File.Exists(percorsoIO);
        }
        public void CreaFile()
        {

        }
        public void LeggiFile()
        {
            
        }
        public void ScriviFile()
        {

        }
        public void GestisciErrori()
        {

        }
    }

    class FantaAllenatore
    {
        public string nome { get; set; }
        public int codiceRosa { get; set; }
        public int budgetDisponibile { get; set; }

        public List<FantaAllenatore> FantaAllenatori = new List<FantaAllenatore>();

        public FantaAllenatore()
        {
            /*this.nome = nome;
            this.codiceRosa = codiceRosa;
            this.budgetDisponibile = budgetDisponibile;*/
        }

        public void AggiungiFantaAllenatore(string nome, int codiceRosa, int budgetDisponibile)
        {
            //FantaAllenatori.Insert(codiceRosa, new FantaAllenatore() { nome = this.nome, codiceRosa = this.codiceRosa, budgetDisponibile=this.budgetDisponibile });
        }

        public void AggiungiRosa()
        {

        }

        public void AggiungiSchieramento()
        {

        }

        public override string ToString()
        {
            return string.Join(',', FantaAllenatori).Trim('(').Trim(')');
        }

    }

    class FantaCalciatore
    {
        string nome, cognome, squadra, ruolo;
        int numeroMaglia, quotazioneIniziale, quotazioneAttuale, punteggioClassifica;
        List<string> FantaCalciatori = new List<string>();

        public FantaCalciatore(string nome, string cognome, string squadra, string ruolo, int numeroMaglia, int quotazioneIniziale, int quotazioneAttuale, int punteggioClassifica)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.squadra = squadra;
            this.ruolo = ruolo;
            this.numeroMaglia = numeroMaglia;
            this.quotazioneIniziale = quotazioneIniziale;
            this.quotazioneAttuale = quotazioneAttuale;
            this.punteggioClassifica = punteggioClassifica;
        }
        
        public void AggiungiFantaCalciatore()
        {

        }

        public void RicercaFantaCalciatore()
        {

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
