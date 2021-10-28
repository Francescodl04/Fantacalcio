using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Fantacalcio
{
    class Programma
    {
        string comandoInserito;

        static void Main(string[] args)
        {

        }

        public void SchermataInserimentoRose()
        {

        }

        public void SchermataSchieramentoCampoFantaCalciatori()
        {

        }

        public void SchermataVisualizzazioneFantaCalciatori()
        {

        }

        public void SchermataAggiornamentoStatisticheFantaCalciatori()
        {

        }

        public void SchermataVisualizzazioneClassifiche()
        {

        }

        public void SchermataCancellazioneDati()
        {

        }

        public void SchermataComandi()
        {

        }

        public void UscitaProgramma()
        {
            Environment.Exit(0);
        }
    }

    class Files
    {
        string inputOutput, percorsoIO;
        public Files(string inputOutput, string percorsoIO)
        {
            this.inputOutput = inputOutput;
            this.percorsoIO = percorsoIO;
        }
        public void VerificaEsistenzaFile()
        {
            
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
        public override string ToString()
        {
            return base.ToString();
        }
    }

    class FantaAllenatore
    {
        string torneo, nome;
        int codiceRosa, budgetDisponibile;
        List<string> FantaCalciatori = new List<string>();
        List<int> RoseFantaCalciatori = new List<int>();
        List<int> SchieramentiFantaCalciatori = new List<int>();

        public FantaAllenatore(string nome, int codiceRosa, int budgetDisponibile)
        {
            this.nome = nome;
            this.codiceRosa = codiceRosa;
            this.budgetDisponibile = budgetDisponibile;
        }

        public void AggiungiFantaAllenatore()
        {

        }

        public void AggiungiRosa()
        {

        }

        public void AggiungiSchieramento()
        {

        }

    }

    class FantaCalciatore
    {
        string nome, cognome, squadra, ruolo, numeroMaglia;
        int quotazioneIniziale, quotazioneAttuale, punteggioClassifica;
        List<string> FantaCalciatori = new List<string>();

        public FantaCalciatore(string nome, string cognome, string squadra, string ruolo, string numeroMaglia, int quotazioneIniziale, int quotazioneAttuale, int punteggioClassifica)
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
