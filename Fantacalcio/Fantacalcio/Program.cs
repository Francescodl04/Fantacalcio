/* Alunno: Francesco Di Lena
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
        static private readonly string[] percorsiFile = new string[] { @"C:\Programma Gestionale Fantacalcio\fanta-allenatori.json", @"C:\Programma Gestionale Fantacalcio\fanta-calciatori.json", @"C:\Programma Gestionale Fantacalcio\schieramenti.json" };
        static private readonly string percorsoCartella = @"C:\Programma Gestionale Fantacalcio";

        //Liste necessarie per contenere ogni dato del fanta-torneo.
        static private List<FantaAllenatore> FantaAllenatori = new List<FantaAllenatore>(); //Lista che conterrà gli attri dei fanta-allenatori.
        static private List<FantaCalciatore> FantaCalciatori = new List<FantaCalciatore>(); //Lista che conterrà i dati dei fanta-allenatori.
        static private List<(int, int)> CodiciSchieramenti = new List<(int, int)>(); //Lista che conterrà i dati dei fanta-allenatori.
        static private List<int> Indici = new List<int>(); //Lista che conterrà i dati dei fanta-allenatori.

        //Metodi

        // Metodo principale, il primo che viene richiamato dal programma, che stabilisce quali metodi invocare in base al fatto che si tratti di un avvio comune oppure il primo. 
        static private void Main()
        {
            bool chiusuraProgramma; //Variabile che definisce quando il programma deve essere chiuso o meno.
            bool[] verificaFile = new bool[2]; //Matrice che contiene il valore booleano di esistenza o meno dei primi due file di salvataggio (fanta-allenatori.json e fanta-calciatori.json).
            Console.Title = "Programma Gestionale del Fantacalcio"; //Viene definito il titolo della finestra in cui il programma verrà eseguito.
            //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
            do
            {
                OperazioniFile operazioniFile = new OperazioniFile(); //Viene istanziata la classe OperazioniFile.
                Console.Clear(); //Elimina il contenuto della console.
                //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                for (int i = 0; i < 2; i++) 
                {
                    verificaFile[i] = operazioniFile.VerificaEsistenzaFile(percorsiFile[i]); //Inserisce il valore restituito dal metodo VerificaEsistenzaFile in ogni "casella" della matrice.
                }
                if (verificaFile[0] == false || verificaFile[1] == false) //Se almeno uno dei file non esiste, allora vengono eseguite le seguenti istruzioni.
                {
                    VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
                    string messaggioOutput = operazioniFile.CreaCartella(percorsoCartella); //Il valore restituito dal metodo CreaCartella viene assegnato alla variabile.
                    if (messaggioOutput.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                    {
                        Console.WriteLine(messaggioOutput); //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        break; //Il ciclo si interrompe.
                    }
                    else
                    {
                        //Se non ci sono state eccezioni si procede con l'inserimento dei dati, contenuto nel metodo PrimoAvvio; se all'interno di quest'ultimo si verificano errori oppure l'utente lo decide, allora permette di uscire dal programma.
                        if (PrimoAvvio() == true) 
                        {
                            break;
                        }
                    }
                }
                else
                {
                    //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                    for (int i = 0; i < 2; i++)
                    {
                        string contenutoInput = operazioniFile.LeggiFile(percorsiFile[i]); //Inserisce il valore restituito dal metodo LeggiFile() all'interno della stringa: questo valore può essere il testo letto dal file oppure un messaggio di errore.
                        if (contenutoInput.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        {
                            Console.WriteLine(contenutoInput); //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                            break; //Il ciclo si interrompe.
                        }
                        else
                        {
                            //Se non ci sono state eccezioni, allora si può lavorare con il testo dei due file.
                            //Si effettua la deserializzazione del file, che divide i vari attributi presenti nei due file e vengono inseriti ordinatamente all'interno della rispettive liste.
                            if (i == 0)
                            {
                                FantaAllenatori = JsonConvert.DeserializeObject<List<FantaAllenatore>>(contenutoInput);
                            }
                            else
                            {
                                FantaCalciatori = JsonConvert.DeserializeObject<List<FantaCalciatore>>(contenutoInput);
                            }
                        }
                    }
                }
                VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
                //Si fa riferimento al metodo AvvioComune che permette la visualizzazione del menu principale e il valore che restituisce viene usato per decidere se è il momento o meno di interrompere il ciclo do-while per permettere la chiusura del programma.
                chiusuraProgramma = AvvioComune(); 
                if (chiusuraProgramma == false) //Viene visualizzato al termine di una funzionalità per avvisare l'utente del ritorno alla schermata iniziale.
                {
                    Console.WriteLine("\nPer tornare alla schermata iniziale premere un tasto qualsiasi...");
                    Console.ReadKey(); //Metodo che permette di acquisire la pressione di un tasto qualsiasi da parte dell'utente.
                }
            } while (chiusuraProgramma == false);
            Console.WriteLine("\n\nPer uscire dal programma premi un tasto qualsiasi...");
            Console.ReadKey(); //Metodo che permette di acquisire la pressione di un tasto qualsiasi da parte dell'utente.
            Environment.Exit(0); //Metodo che permette di uscire dal programma.

        }

        //Metodo che permette la visualizzazione dell'intestazione "Programma gestionale del Fantacalcio" in ogni schermata.
        static private void VisualizzaIntestazione() 
        {
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            //Il metodo seguente effettua la visualizzazione vera e propria dell'intestazione.
            Console.WriteLine("                                       _________          ________                                 " +
                              "\n                     Il               |    _____|        |    ____|                                " +
                              "\n                 programma            |   |___           |   |                                     " +
                              "\n                gestionale            |    ___|          |   |                                     " +
                              "\n                    del               |   |              |   |___                                  " +
                              "\n                                      |___|     anta     |_______| alcio                           " +
                              "\n                                                                                                   \n");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
        }

        //Metodo richiamato dal Main solamente quando il programma viene avviato per la prima volta. Contiene le visualizzazioni video e le prime richieste di inserimento per l'utente. 
        static public bool PrimoAvvio()
        {
            bool ripetiCiclo; //Variabile necessaria per definire se continuare o meno l'iterazione dei cicli do-while.
            Console.WriteLine("                      Benvenuto nel programma gestionale del fantacalcio.                      \n"
                    + "       Questo è il primo avvio del programma, così, per giocare devi inserire tutti i dati        \n" +
                    "       che riguardano il torneo; il programma ti guiderà nell'inserimento e ogni volta che ti       \n" +
                    "               farà delle richieste dovrai inserire un comando seguito da un INVIO.                 \n");
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("Inserisci il torneo reale di riferimento per il gioco:");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
            string fantaTorneo = Console.ReadLine(); //Viene acquisito il nome del fanta-torneo.
            int numeroFantaAllenatori; //Variabile necessaria per contenere il numero di fanta-allenatori che partecipano al gioco.
            //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
            do
            {
                SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                Console.WriteLine("\nInserisci il numero dei fanta-allenatori che parteciperanno al gioco:");
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                ripetiCiclo = int.TryParse(Console.ReadLine(), out numeroFantaAllenatori); //Questo metodo permette di verificare se è possibile o meno convertire una variabile stringa in input: restituisce true in caso positivo, altrimenti restituisce false.
                if (ripetiCiclo == false || numeroFantaAllenatori < 2 || numeroFantaAllenatori > 10) //Se il precedente metodo restituisce false oppure il numero inserito dall'utente è minore di 2 o maggiore di 10, allora avvisa l'utente con un messaggio.
                {
                    Console.WriteLine("\nDevi inserire un numero compreso tra 2 e 10 (inclusi). Riprova...");
                    ripetiCiclo = false;
                }
            }
            while (ripetiCiclo == false);
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < numeroFantaAllenatori; i++)
            {
                SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                Console.WriteLine($"\nInserisci il nome del {i + 1}° giocatore:");
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                string nomeFantaAllenatore = Console.ReadLine(); //Viene acquisito il nome del fanta-allenatore che si sta inserendo.
                int budgetDisponibile; //Variabile necessaria per contenere il budget disponibile di un fanta-allenatore.
                //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
                do
                {
                    SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                    Console.WriteLine($"\nInserisci il budget di {nomeFantaAllenatore} (in Fantamilioni):"); 
                    SetResetColori(false); //Riporta i colori alle proprietà standard.
                    ripetiCiclo = int.TryParse(Console.ReadLine(), out budgetDisponibile); //Questo metodo permette di verificare se è possibile o meno convertire una variabile stringa in input: restituisce true in caso positivo, altrimenti restituisce false.
                    if (ripetiCiclo == false) //Se il precedente metodo restituisce false, allora avvisa l'utente con un messaggio.
                    {
                        Console.WriteLine("\nDevi inserire un numero. Riprova...");
                    }
                }
                while (ripetiCiclo == false);
                FantaAllenatore FantaAllenatore = new FantaAllenatore(nomeFantaAllenatore, i, budgetDisponibile); //Viene istanziata la classe FantaAllenatore e vengono passati gli attributi acquisiti dall'inserimento dell'utente.
                FantaAllenatori = FantaAllenatore.AggiungiFantaAllenatore(FantaAllenatori); //Si fa riferimento al metodo AggiungiFantaAllenatore per eseguire l'inserimento del fanta-allenatore nella lista di FantaCalciatori. Il metodo restituisce quindi la lista aggiornata.
            }
            Console.WriteLine("\nBene, ora è necessario inserire le rose di ognuno dei giocatori. Se premi INVIO,\nla console verrà ripulita e si passerà alla schermata che permetterà di far questo.");
            Console.ReadKey(); //Metodo che permette di acquisire la pressione di un tasto qualsiasi da parte dell'utente.
            Console.Clear(); //Elimina il contenuto della console.
            return SchermataInserimentoRose(); //Si fa riferimento a questo metodo per la visualizzazione della schermata di inserimento delle rose di fanta-calciatori.
        }

        //Metodo che viene richiamato solo da PrimoAvvio e che contiene la visualizzazione video delle richieste che permettono all'utente l'inserimento delle rose dei fanta-allenatori.
        static private bool SchermataInserimentoRose() 
        {
            bool ripetiCiclo = false, chiusuraProgramma = false; //La prima variabile è necessaria per definire se continuare o meno l'iterazione dei cicli do-while, l'altra per determinare se consentire o meno all'utente l'uscita dal programma.
            string ruolo; //Variabile che conterrà il ruolo deciso automaticamente dal programma per un fanta-calciatore.
            string[] caratteristicheInserite; //Matrice che conterrà caratteristiche di un fanta-calciatore.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            Console.WriteLine("\nBene, ora che sono stati inseriti i dati principali dei fanta-allenatori, è necessario  " +
                "\nche venga inserito ognuno dei nomi dei fanta-calciatori che sono stati aggiudicati durante l'asta.");
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                //Inizia il primo ciclo for interno, che permette l'esecuzione dell'inserimento di ogni singolo fanta-calciatore.
                for (int j = 0; j < 25; j++)
                {
                    //In base al numero di fanta-calciatore al quale si è arrivati, ne viene scelto automaticamente il ruolo.
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
                    SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                    Console.WriteLine($"\n{FantaAllenatori[i].OttieniNome()}, inserisci il nome, il cognome, la squadra, il numero di maglia e la quotazione iniziale\n(separati da una virgola) del tuo {j + 1}° fanta-giocatore ({ruolo}):");
                    SetResetColori(false); //Riporta i colori alle proprietà standard.
                    //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
                    do
                    {
                        string informazioniInserite = Console.ReadLine(); //Le caratteristiche di un fanta-calciatore vengono lette e inserite nella variabile.
                        //Nel seguente metodo i effettua una eliminazione degli spazi in eccesso e una divisione della singola stringa in più stringhe (in base al carattere virgola) che vengono poi inserite all'interno della matrice unidimensionale caratteristicheInserite.
                        caratteristicheInserite = informazioniInserite.Trim(' ').Split(','); 
                        if (caratteristicheInserite.Length != 5) //Viene effettuato un controllo sul numero delle caratteristiche inserite: se non sono sufficienti, allora avvisa l'utente con un messaggio.
                        {
                            Console.WriteLine("\nNon hai inserito il giusto numero caratteristiche, quindi reinserisci il fanta-calciatore:");
                            ripetiCiclo = false;
                        }
                        else
                        {
                            for (int k = 3; k < caratteristicheInserite.Length; k++) //Con questo secondo ciclo for interno si effettua un controllo sulle caratteristiche numeriche del fanta-calciatore.
                            {
                                ripetiCiclo = int.TryParse(caratteristicheInserite[k], out int verificaInteger); //Questo metodo permette di verificare se è possibile o meno convertire una variabile stringa in input: restituisce true in caso positivo, altrimenti restituisce false.
                                if ((ripetiCiclo == false || verificaInteger < 0) && k == caratteristicheInserite.Length - 1) //Nel caso in cui ci siano degli errori di inserimento (uno o più quindi) viene visualizzato un messaggio di errore solamente alla fine del ciclo di controllo.
                                {
                                    Console.WriteLine($"\nIn alcune caratteristiche non hai inserito un numero maggiore\no uguale a zero, quindi reinserisci il fanta-calciatore:");
                                    break; //Il ciclo viene interrotto.
                                }
                            }
                        }
                    }
                    while (ripetiCiclo == false);
                    //La classe FantaCalciatore viene successivamente istanziata, passando al metodo costruttore le caratteristiche del fanta-calciatore.
                    FantaCalciatore fantaCalciatore = new FantaCalciatore(caratteristicheInserite[0], caratteristicheInserite[1], caratteristicheInserite[2], ruolo, int.Parse(caratteristicheInserite[3]), int.Parse(caratteristicheInserite[4]), 0, 0, i); 
                    FantaCalciatori = fantaCalciatore.AggiungiFantaCalciatore(FantaCalciatori); //Si effettua l'aggiunta del fanta-calciatore nella lista FantaCalciatori attraverso questo metodo, che poi restituisce la lista agiornata.
                }
            }
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                //Viene effettuata la visualizzazione della rosa di un fanta-allenatore.
                Console.WriteLine($"\nQuesto è l'elenco del fanta-calciatori di {FantaAllenatori[i].OttieniNome()}:");
                for (int j = 0; j < (FantaCalciatori.Count / FantaAllenatori.Count); j++) 
                {
                    Console.WriteLine($"{j + 1}) {FantaCalciatori[j * (i + 1)].ToString()}");
                }
                if (i != FantaAllenatori.Count - 1) //Solo nel caso in cui ci sia ancora almeno una rosa di un fanta-allenatore da visionare, allora visualizza il seguente messaggio.
                {
                    Console.WriteLine("\nPer passare alla rosa del prossimo fanta-allenatore, premi un tasto qualsiasi...");
                    Console.ReadKey(); //Metodo che permette di acquisire la pressione di un tasto qualsiasi da parte dell'utente.
                }
            }
            Console.WriteLine("\nSei sicuro di voler salvare le rose dei Fanta-Allenatori? (inserisci \"S\" per salvare, oppure \"N\" per rifare l'inserimento delle rose dei fanta-calciatori)");
            switch (RispostaSiNo()) //In base al valore che assume la stringa ritornata dal metodo, si possono presentare diversi casi.
            {
                case ("S"): 
                    Console.WriteLine("\nSalvataggio in corso...");
                    //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                    for (int i = 0; i < 2; i++)
                    {
                        string output;
                        //Viene effettuata la serializzazione di una lista, cioè viene resa pronta per la scrittura in un file JSON.
                        if (i == 0)
                        {
                            output = JsonConvert.SerializeObject(FantaAllenatori);
                        }
                        else
                        {
                            output = JsonConvert.SerializeObject(FantaCalciatori);
                        }
                        OperazioniFile operazioniFile = new OperazioniFile(); //Viene istanziata la classe OperazioniFile.
                        string descrizioneErrore = operazioniFile.ScriviFile(percorsiFile[i], output); //Si prova a effettuare la scrittura del file indicato grazie a questo metodo, che poi ritorna un messaggio che indica se ci sono state eccezioni o meno.
                        if (descrizioneErrore.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        {
                            Console.WriteLine(descrizioneErrore); //Viene visualizzato il messaggio di errore.
                            chiusuraProgramma = true; //Viene consentito di uscire dal programma.
                            break; //Il ciclo viene interrotto.
                        }
                        else if (i == 1)
                        {
                            Console.WriteLine(operazioniFile.ToString()); //Viene visualizzato un messaggio che indica il completamento corretto delle operazioni di salvataggio.
                        }
                    }
                    break;
                case ("N"):
                    chiusuraProgramma = true; //Viene consentito di uscire dal programma.
                    break;
            }
            Console.Clear(); //Elimina il contenuto della console.
            return chiusuraProgramma;
        }

        //Metodo richiamato quando i dati iniziali sono stati configurati. Permette la visualizzazione di un menu di scelta tra le diverse funzionalità del programma.
        static private bool AvvioComune()  
        {
            bool chiusuraProgramma = false; //Variabile usata per determinare se consentire o meno all'utente l'uscita dal programma.
            bool ripetiCiclo; //Variabile necessaria per definire se continuare o meno l'iterazione dei cicli do-while.

            //Serie di visualizzazioni a schermo

            Console.WriteLine($"                        Bentornato nel Fantacalcio. Oggi è il {DateTime.Now:dd/MM/yyyy}.\n" + //Nella stringa è contenuto il metodo DateTime.Now consente di ottenere la data corrente secondo lo schema giorno/mese/anno.
                              " Scegli una delle seguenti funzioni inserendo il corrispondente valore numerico per iniziare... ");
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("\n                                    =========================                                      ");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
            Console.WriteLine("\n1) Esegui la ricerca dei fanta-calciatori\n" +
                              "2) Visualizza i dati dei fanta-allenatori\n" +
                              "3) Visualizza/crea lo schieramento in campo attuale\n" +
                              "4) Aggiorna le statistiche dei fanta-calciatori\n" +
                              $"5) Visualizza la classifica parziale del torneo di fanta-torneo\n" +
                              "6) Ripristina le impostazioni iniziali\n" +
                              "7) Esci dal programma");
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("\n                                    =========================                                      ");
            
            //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
            do
            {
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                char scelta = Console.ReadKey(false).KeyChar; //Alla variabile scelta viene assegnato il valore ottenuto dalla lettura di un input di tastiera. In questo caso non è necessario la pressione di invio, visto che la conferma dell'inserimento avviene in modo automatico.
                ripetiCiclo = false;
                switch (scelta) //In base al valore che assume la variabile scelta, si possono presentare diversi casi.
                {
                    case ('1'):
                        SchermataRicercaFantaCalciatori(); //Si fa riferimento a questo metodo per la visualizzazione della schermata di ricerca dei fanta-calciatori.
                        break;
                    case ('2'):
                        SchermataVisualizzazioneFantaAllenatori(); //Si fa riferimento a questo metodo per la visualizzazione della schermata dei fanta-allenatori
                        break;
                    case ('3'):
                        SchermataSchieramentoCampoFantaCalciatori(); //Si fa riferimento a questo metodo per la visualizzazione della schermata di visualizzione/creazione degli schieramenti in campo dei fanta-calciatori.
                        break;
                    case ('4'):
                        SchermataAggiornamentoStatisticheFantaCalciatori(); //Si fa riferimento a questo metodo per la visualizzazione della schermata di aggiornamento delle statistiche dei fanta-calciatori.
                        break;
                    case ('5'):
                        SchermataVisualizzazioneClassifiche(); //Si fa riferimento a questo metodo per la visualizzazione della schermata di visualizzazione della classifica parziale.
                        break;
                    case ('6'):
                        chiusuraProgramma = SchermataCancellazioneDati(); //Si fa riferimento a questo metodo per la visualizzazione della schermata di ripristino dei dati.
                        break;
                    case ('7'):
                        chiusuraProgramma = true; 
                        break;
                    default: //Nel caso in cui gli altri casi non sono verificati, quando cioè non si inserisce un numero oppure si inserisce un numero non compreso tra quelli che sono associati ad una funzionalità, ripetiCiclo è posto uguale a true, in modo che il ciclo possa esse 
                        Console.WriteLine("\nNon hai inserito un valore presente in lista, quindi riprova...");
                        ripetiCiclo = true;
                        break;
                }
            }
            while (ripetiCiclo == true);
            return chiusuraProgramma;
        }

        //Metodo che permette di visualizzare la prima parte della schermata di ricerca dei fanta-calciatori. 
        static private void SchermataRicercaFantaCalciatori() 
        {
            Console.Clear(); //Elimina il contenuto della console.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            Console.WriteLine("                        Funzionalità di ricerca dei fanta-calciatori                       ");
            Ricerca(); //Si fa riferimento a questo metodo per eseguire la serie di richieste e visualizzazione a video che implicano la ricerca.
        }

        //Metodo che permette di visualizzare i dati sui fanta-allenatori.
        static public void SchermataVisualizzazioneFantaAllenatori()  
        {
            Console.Clear(); //Elimina il contenuto della console.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            Console.WriteLine("                   Funzionalità di visualizzazione dei fanta-allenatori                   ");
            Console.WriteLine("\n   Di seguito verranno visualizzati i nomi e il budget disponibile di ogni fanta-allenatore.   \n");
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("                                    =========================                                      \n");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < FantaAllenatori.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {FantaAllenatori[i].ToString()}");
            }
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("\n                                    =========================                                      ");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
        }

        //Metodo che permette di visualizzare la prima parte della schermata di ricerca dei fanta-calciatori e poi l'inserimento di ogni giocatore in uno schieramento
        static private void SchermataSchieramentoCampoFantaCalciatori() 
        {
            Console.Clear(); //Elimina il contenuto della console.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            Console.WriteLine("                 Funzionalità di schieramento in campo dei fanta-calciatori                 ");
            OperazioniFile operazioniFile = new OperazioniFile(); //Viene istanziata la classe OperazioniFile.
            bool verificaFile = operazioniFile.VerificaEsistenzaFile(percorsiFile[2]); //Viene effettuato il controllo di esistenza del file che contiene i codici di schieramento.
            bool ripetiCiclo = false; //Variabile necessaria per definire se continuare o meno l'iterazione dei cicli do-while.
            if (verificaFile == true) //Se gli schieramenti esistono, allora vengono visualizzati a schermo.
            {
                Console.WriteLine("\nOra verrà visualizzato lo schieramento di ogni fanta-allenatore");
                //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                for (int i = 0; i < FantaAllenatori.Count; i++)
                {
                    SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                    Console.WriteLine($"\nQuesto è lo schieramento di {FantaAllenatori[i].OttieniNome()}:");
                    SetResetColori(false); //Riporta i colori alle proprietà standard.
                    if (CodiciSchieramenti == null) //Se la lista è vuota, allora viene popolata.
                    {
                        string contenutoInput = operazioniFile.LeggiFile(percorsiFile[2]); //Avviene la lettura del file specificato.
                        if (contenutoInput.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        {
                            Console.WriteLine(contenutoInput); //Viene visualizzato il messaggio di errore.
                            break; //Il ciclo si interrompe.
                        }
                        else
                        {
                            //Si effettua la deserializzazione del file, che divide i vari attributi presenti nei due file e vengono inseriti ordinatamente all'interno della rispettive liste.
                            CodiciSchieramenti = JsonConvert.DeserializeObject<List<(int, int)>>(contenutoInput);
                        }
                    }
                    //Ciclo for interno.
                    for (int j = 0; j < CodiciSchieramenti.Count / (i + 1); j++) //Viene visualizzato lo schieramento.
                    {
                        Console.WriteLine($"{j + 1}) {FantaCalciatori[CodiciSchieramenti[j * (i + 1)].Item2].ToString()}");
                    }
                    Console.WriteLine("\nDesideri mantenere questi schieramenti, oppure vuoi cancellarli? (Inserisci \"S\" se SI, altrimenti inserisci \"N\")");
                    if (RispostaSiNo() == "N")
                    {
                        Console.WriteLine("\nSalvataggio in corso...");
                        string messaggioOutput = operazioniFile.EliminaFile(percorsiFile[2]);
                        if (messaggioOutput.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        {
                            Console.WriteLine(messaggioOutput); //Viene visualizzato il messaggio di errore.
                            break; //Il ciclo si interrompe.
                        }
                        else
                        {
                            Console.WriteLine(operazioniFile.ToString()); //Viene visualizzato il messaggio di salvataggio effettuato con successo.
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("\nGli schieramenti di ogni fanta-allenatore non sono stati ancora impostati, quindi, attraverso la\n     funzione di ricerca, puoi selezionare scegliere un giocatore per volta inserendo il\n                          valore numerico che gli corrisponde.");
                for (int i = 0; i < FantaAllenatori.Count; i++)
                {
                    //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                    for (int j = 0; j < 18; j++)
                    {
                        SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                        Console.WriteLine($"\nScegli il {j + 1}o fanta-calciatore da schierare per {FantaAllenatori[i].OttieniNome()}:");
                        SetResetColori(false); //Riporta i colori alle proprietà standard.
                        bool aggiuntaGiocatore = Ricerca(); //Viene invocato il metodo di ricerca.
                        if (aggiuntaGiocatore == true) //Se sono stati trovati dei risultati nella ricerca, allora chiede all'utente di scegliere il fanta-giocatore che vuole schierare.
                        {
                            Console.WriteLine("\nOra inserisci il giocatore che desideri inserire nel tuo schieramento:");
                            int scelta; //Identifica la scelta dell'utente.
                            //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
                            do
                            {
                                scelta = int.Parse(Console.ReadLine()) - 1;
                                if (scelta < 0 || scelta > Indici.Count) //Avviene la verifica dell'inserimento di un fanta-calciatore che è stato effettivamente trovato dalla ricerca.
                                {
                                    Console.WriteLine("\nNon hai inserito un valore accettabile. Riprova...");
                                    ripetiCiclo = false;
                                }
                                else
                                {
                                    CodiciSchieramenti.Add((i, j)); //Metodo che aggiunge una posizione all'interno della lista CodiciSchieramenti.
                                    ripetiCiclo = true;
                                }

                            } while (ripetiCiclo == false);
                        }
                        else
                        {
                            Console.WriteLine($"\nVuoi riprovare a ricercare di nuovo il {j + 1}o fanta-calciatore da schierare, oppure no?");
                            if (RispostaSiNo() == "S")
                            {
                                j--;
                            }
                            else
                            {
                                ripetiCiclo = false;
                                break;
                            }
                        }
                    }
                    if (ripetiCiclo == false)
                    {
                        break;
                    }
                }
                if (ripetiCiclo == true) 
                {
                    Console.WriteLine("\nGli schieramenti sono stati inseriti correttamenti. Desideri salvarli? (inserisci \"S\" se SI, altrimenti \"N\" se NO)");
                    if (RispostaSiNo() == "S")
                    {
                        Console.WriteLine("\nSalvataggio in corso...");
                        //Viene effettuata la serializzazione della lista, la quale viene inserita in una stringa pronta per essere scritta su file.
                        string output = JsonConvert.SerializeObject(CodiciSchieramenti);
                        string messaggioOutput = operazioniFile.ScriviFile(percorsiFile[2], output); //Avviene la scrittura su file.
                        if (messaggioOutput.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        {
                            Console.WriteLine(messaggioOutput); //Viene mostrato all'utente il messaggio di errore.
                        }
                        else
                        {
                            Console.WriteLine(operazioniFile.ToString()); //Viene mostrato un messaggio di salvataggio effettuato correttamente.
                        }
                    }
                }
            }
        } 

        //Metodo che permette di effettuare l'inserimento e verificare la risposta di un utente nel caso in cui si richieda che sia "SI" o "NO".
        static private string RispostaSiNo()
        {
            string scelta; //Variabile che contiene la scelta inserita dall'utente.
            bool ripetiCiclo; //Variabile necessaria per definire se continuare o meno l'iterazione dei cicli do-while.
            //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
            do
            {
                scelta = Console.ReadLine().ToUpper(); //Effettua la lettura dell'inserimento da tastiera dell'utente e lo rende automaticamente maiuscolo.
                if (scelta != "S" && scelta != "N") //Se l'utente inserisce un valore diverso da "S" e "N", allora lo avvisa con un messaggio, e poi il ciclo viene ripetuto.
                {
                    Console.WriteLine("\nNon hai inserito un valore accettabile. Riprova...");
                    ripetiCiclo = true;
                }
                else
                {
                    ripetiCiclo = false;
                }

            } while (ripetiCiclo == true);
            return scelta;
        }

        //Metodo che permette di visualizzare le richieste all'utente che porteranno all'aggiornamento delle statistiche dei fanta-calciatori.
        static private void SchermataAggiornamentoStatisticheFantaCalciatori() 
        {
            string[] testoDaVisualizzare = new string[] { "la quotazione attuale", "il punteggio ottenuto nell'ultima partita" }; //Testo che sarà visualizzato nelle richieste di inserimento delle nuove caratteristiche.
            bool ripetiCiclo = false; //Variabile necessaria per definire se continuare o meno l'iterazione dei cicli do-while.
            Console.Clear(); //Elimina il contenuto della console.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            Console.WriteLine("            Funzionalità di aggiornamento delle statistiche dei fanta-calciatori            ");
            Console.WriteLine("\n     Ricerca un fanta-calciatore e cambiane le caratteristiche per aggiornare il suo\n                              punteggio in classifica");
            //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
            do
            {
                if (Ricerca() == true) //Solo se la ricerca ha dato uno o più risultati, allora viene richiesto all'utente di inserire il giocatore al quale vuole aggiornare le caratteristiche e successivamente chiede le più recenti caratteristiche.
                {
                    int sceltaFantaCalciatore; //Identifica la scelta di un fanta-calciatore da parte dell'utente.
                    //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
                    do
                    {
                        Console.WriteLine("\nInserisci il numero del giocatore a cui desideri aggiornare le statistiche:");
                        ripetiCiclo = int.TryParse(Console.ReadLine(), out sceltaFantaCalciatore); //Questo metodo permette di verificare se è possibile o meno convertire una variabile stringa in input: restituisce true in caso positivo, altrimenti restituisce false.
                        sceltaFantaCalciatore--; //Viene decrementato perché gli indici informatici partono sempre da 0.
                        if (sceltaFantaCalciatore < 0 || sceltaFantaCalciatore > Indici.Count || ripetiCiclo == false)  //Viene effettuato il controllo che sia stato inserito un numero che corrisponda effettivamente a uno dei fanta-calciatori trovati dalla ricerca.
                        {
                            Console.WriteLine("\nNon hai inserito un valore accettabile. Riprova...");
                            ripetiCiclo = false;
                        }

                    } while (ripetiCiclo == false);
                    //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                    for (int i = 0; i < testoDaVisualizzare.Length; i++)
                    {
                        SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                        Console.WriteLine($"\nInserisci {testoDaVisualizzare[i]} del fanta-calciatore che hai scelto:");
                        SetResetColori(false); //Riporta i colori alle proprietà standard.
                        int datoAggiornato;
                        //Qui inizia un ciclo do-while, in cui le istruzioni contenute all'interno vengono eseguite almeno una volta; se la condizione finale è soddisfatta, allora il ciclo continua e le istruzioni interne vengono ripetute, altrimenti si ferma.
                        do
                        {
                            ripetiCiclo = int.TryParse(Console.ReadLine(), out datoAggiornato); //Questo metodo permette di verificare se è possibile o meno convertire una variabile stringa in input: restituisce true in caso positivo, altrimenti restituisce false.
                            if (ripetiCiclo == false || (datoAggiornato < 0 && i == 0)) //Viene controllato che vengano inseriti numeri maggiori o uguali a zero.
                            {
                                Console.WriteLine("\nDevi inserire un numero maggiore o uguale a zero per questo campo. Riprova...");
                                ripetiCiclo = false;
                            }

                        } while (ripetiCiclo == false);
                        if (i == 0) //Viene effettuata l'aggiornamento della quotazione attuale del fanta-calciatore selezionato.
                        {
                            FantaCalciatori[Indici[sceltaFantaCalciatore]].AggiornaQuotazioneAttuale(datoAggiornato);
                        }
                        else //Viene effettuata l'aggiornamento del punteggio del fanta-calciatore selezionato.
                        {
                            FantaCalciatori[Indici[sceltaFantaCalciatore]].AggiornaPunteggio(datoAggiornato);
                        }
                    }
                    //Viene visualizzato il fanta-calciatore con i dati aggiornati e viene chiesto all'utente se desidera salvarlo o meno.
                    Console.WriteLine("\nEcco il fanta-calciatore che hai scelto con i nuovi dati inseriti:");
                    Console.WriteLine(FantaCalciatori[Indici[sceltaFantaCalciatore]].ToString());
                    Console.WriteLine("\nVuoi salvarlo? (inserisci \"S\" se SI, altrimenti \"N\" se NO)");
                    if (RispostaSiNo() == "S")
                    {
                        Console.WriteLine("\nSalvataggio in corso...");
                        //Viene effettuata la serializzazione della lista, in modo che quest'ultima possa essere pronta per la scrittura su file.
                        string output = JsonConvert.SerializeObject(FantaCalciatori);
                        OperazioniFile operazioniFile = new OperazioniFile(); //Viene istanziata la classe OperazioniFile.
                        string descrizioneErrore = operazioniFile.ScriviFile(percorsiFile[1], output); //Viene effettuata la scrittura su file.
                        if (descrizioneErrore.StartsWith("ERRORE:")) //Se si è generata un'eccezione, quindi la striga inizia per "ERRORE:", allora vengono eseguite le seguenti istruzioni.
                        {
                            Console.WriteLine(descrizioneErrore); //Viene visualizzato il messaggio di errore.
                            break;
                        }
                        else
                        {
                            Console.WriteLine(operazioniFile.ToString()); //Viene visualizzato il messaggio di salvataggio effettuato correttamente.
                        }
                    }
                }
                //Viene chiesto all'utente se desidera nuovamente cambiare le caratteristiche a un altro giocatore oppure se vuole tornare alla schermata iniziale.
                SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                Console.WriteLine("\nVuoi riprovare a cercare un giocatore, oppure vuoi tornare alla schermata iniziale?\n(inserisci \"S\" se SI, altrimenti \"N\" se NO)");
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                if (RispostaSiNo() == "N")
                {
                    ripetiCiclo = true;
                }

            } while (ripetiCiclo == false);
        }

        //Metodo che permette di visualizzare le richieste dei filtri di ricerca, ma anche i risultati della ricerca. 
        static private bool Ricerca()
        {
            string[] testoDaVisualizzare = new string[] { "il nome", "il cognome", "la squadra", "il ruolo", "il numero di maglia", "la quotazione iniziale", "la quotazione attuale", "il punteggio in classifica" }; //Contiene il testo da visualizzare durante la richiesta di filtri di ricerca.
            string[] giocatoreRicercato = new string[] { "", "", "", "", "0", "0", "0", "0" }; //Conterrà le caratteristiche inserite dall'utente durante la ricerca.
            int contatoreFiltri = 0; //Conta i filtri inseriti dall'utente nella ricerca.
            bool risultati; //Indica con true che la ricerca ha prodotto almeno un risultato, altrimenti contiene false.
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < testoDaVisualizzare.Length; i++)
            {
                SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                Console.WriteLine($"\nInserisci {testoDaVisualizzare[i]} del fanta-calciatore che vuoi cercare:");
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                giocatoreRicercato[i] = Console.ReadLine(); //Viene effettuata la lettura del filtro.
                if (giocatoreRicercato[i] == "") //Se non si inserisce nulla, allora la richiesta dei filtri viene terminata.
                {
                    if (i > 3) //Nel caso in cui il filtro che viene saltato richieda un'informazione numerica, allora viene reimpostato il valore della stringa "0" in modo che non sorgano problemi nella conversione stringa-integer.
                    {
                        giocatoreRicercato[i] = "0";
                    }
                    break; //Il ciclo viene terminato.
                }
                else
                {
                    if (i > 3) //Nel caso in cui il filtro che viene saltato richieda un'informazione numerica, allora viene effettuato un controllo preventivo.
                    {
                        //Si prova a fare la conversione stringa-integer e il controllo che il numero inserito sia maggiore di zero: se ci sono degli errori, allora l'utente viene avvisato e si procede a fargli compiere nuovamente l'inserimento.
                        if (int.TryParse(giocatoreRicercato[i], out int verificaInteger) == false || verificaInteger < 0) 
                        {
                            Console.WriteLine("\nDevi inserire un numero maggiore di zero. Riprova...");
                            i--;
                        }
                    }
                    else //Se non ci sono errori, si conta un filtro inserito in più.
                    {
                        contatoreFiltri++;
                    }
                }
            }
            //Viene istanziata la classe FantaCalciatore e viene passato al metodo costruttore i filtri di ricerca inseriti dall'utente riguardo al fanta-calciatore.
            FantaCalciatore fantaCalciatore = new FantaCalciatore(giocatoreRicercato[0], giocatoreRicercato[1], giocatoreRicercato[2], giocatoreRicercato[3], int.Parse(giocatoreRicercato[4]), int.Parse(giocatoreRicercato[5]), int.Parse(giocatoreRicercato[6]), int.Parse(giocatoreRicercato[7]), 0);
            Indici = fantaCalciatore.RicercaFantaCalciatore(FantaCalciatori, contatoreFiltri); //Gli indici degli elementi che corrispondono ai filtri di ricerca ritornati dal metodo RicercaFantaCalciatore vengono inseriti nella lista Indici.
            //Visualizzazione a schermo dei risultati di ricerca.
            if (Indici.Count > 0)
            {
                SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                if (Indici.Count == 1)
                {
                    Console.WriteLine("\nLa ricerca ha prodotto un risultato...\n");
                }
                else
                {
                    Console.WriteLine($"\nLa ricerca ha prodotto {Indici.Count} risultati...\n");
                }
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                Console.WriteLine("Di seguito verrà riportato l'elenco dei risultati con nome, cognome, squadra, ruolo, numero di maglia, quotazione iniziale, quotazione attuale, punteggio in classifica e proprietario del fanta-calciatore\n");
                risultati = true;
            }
            else
            {
                SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
                Console.WriteLine("\nLa ricerca non ha prodotto risultati...");
                SetResetColori(false); //Riporta i colori alle proprietà standard.
                risultati = false; 
            } 
            int multipliDiciassete = 1; //Contatore necessario successivamente per evitare di visualizare troppi risultati (in questo caso più dei multipli di diciassette) nello stesso momento, rendendo più agile la lettura da parte dell'utente.
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < Indici.Count; i++) //Visualizzazione a schermo dei fanta-calciatori trovati dalla ricerca.
            {
                Console.WriteLine($"{i + 1}) {FantaCalciatori[Indici[i]].ToString()} {FantaAllenatori[FantaCalciatori[Indici[i]].OttieniCodiceRosa()].OttieniNome()}");
                if (i == 17 * multipliDiciassete)
                {
                    multipliDiciassete++;
                    Console.WriteLine("\nPremi invio per visualizzare ulteriori risultati...\n");
                    Console.ReadKey(); //Metodo che permette di acquisire la pressione di un tasto qualsiasi da parte dell'utente.
                }
                else if (i == FantaCalciatori.Count - 1)
                {
                    Console.WriteLine("\n ===================  Fine dei risultati  =================== ");
                }
            }
            return risultati;
        }

        //Metodo che consente la visualizzazione della classifica dei fanta-calciatori.
        static private void SchermataVisualizzazioneClassifiche()
        {
            Console.Clear(); //Elimina il contenuto della console.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            Console.WriteLine("            Funzionalità di visualizzazione della classifica dei fanta-calciatori            ");
            Console.WriteLine("\n  Di seguito verrà visualizzata la classifica ordinata in modo crescente in base al punteggio.  ");
            FantaCalciatore fantaCalciatore = new FantaCalciatore("", "", "", "", 0, 0, 0, 0, 0); //Viene istanziata la classe FantaCalciatore.
            List<FantaCalciatore> FantaCalciatoriOrdinati = fantaCalciatore.OrdinaFantaCalciatori(FantaCalciatori); //Avviene l'ordinamento della lista di fanta-calciatori in base al loro punteggio attraverso il metodo OrdinaFantaCalciatori; il valore restituito viene inserito all'interno di una nuova lista.
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("\n                                    =========================                                      \n");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
            int multipliNove = 1; //Contatore necessario successivamente per evitare di visualizare troppi risultati (in questo caso più dei multipli di nove) nello stesso momento, rendendo più agile la lettura da parte dell'utente.
            //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
            for (int i = 0; i < FantaCalciatoriOrdinati.Count; i++) //Viene effettuata la visualizzazione della classifica.
            {
                Console.WriteLine($"{i + 1}) {FantaCalciatoriOrdinati[i].ToString()}");
                if (i == 9 * multipliNove && i != FantaCalciatoriOrdinati.Count - 1)
                {
                    multipliNove++;
                    Console.WriteLine("\nPremi invio per visualizzare ulteriori risultati...");
                    Console.ReadKey(); //Metodo che permette di acquisire la pressione di un tasto qualsiasi da parte dell'utente.
                }
            }
        }

        //Metodo che visualizza le richieste a video che poi porteranno o meno alla cancellazione dei dati del fanta-torneo. Restituisce un valore 
        static private bool SchermataCancellazioneDati() 
        {
            bool chiusuraProgramma = false; //Variabile usata per determinare se consentire o meno all'utente l'uscita dal programma.
            Console.Clear(); //Elimina il contenuto della console.
            VisualizzaIntestazione(); //Viene visualizzata a schermo l'intestazione "Programma gestionale del Fantacalcio".
            //Serie di visualizzazioni a schermo.
            Console.WriteLine("                 Funzionalità di cancellazione dei dati del fanta-torneo                 ");
            Console.WriteLine("\n          Grazie a questa funzionalità sei in grado di eliminare qualsiasi dato\n                  del fanta-torneo e inserire delle informazioni nuove.");
            SetResetColori(true); //Cambia i colori secondo le proprietà del metodo.
            Console.WriteLine("\nSei sicuro di voler eliminare tutti i dati inseriti del fanta-torneo?\n(inserisci \"S\" se SI, altrimenti inserisci \"N\" se NO)");
            SetResetColori(false); //Riporta i colori alle proprietà standard.
            if (RispostaSiNo() == "S") //Se l'utente decide di salvare, allora vengono eseguite le seguenti istruzioni.
            {
                Console.WriteLine("\nSalvataggio delle modifiche in corso..."); //Visualizzazione a schermo.
                //Inizia un ciclo for, in cui un ciclo si ripete fino a quando una variabile non assume un determinato valore. La variabile ad ogni ciclo viene aumentata.
                for (int i = 0; i < percorsiFile.Length; i++)
                {
                    OperazioniFile operazioniFile = new OperazioniFile(); //Viene istanziata la classe OperazioniFile.
                    string messaggioOutput = operazioniFile.EliminaFile(percorsiFile[i]); //Si richiama il metodo EliminaFile per effettuare l'eliminazione del file indicato da percorsiFile[i]. 
                    if (messaggioOutput.StartsWith("ERRORE:")) //Se si è verificata un'eccezione, quindi il messaggio in output inizia per "ERRORE:" allora avviene la visualizzazione a schermo di tale errore.
                    {
                        Console.WriteLine(messaggioOutput);
                        break; //Il ciclo viene interrotto.
                    }
                    if (i == 2)
                    {
                        Console.WriteLine(operazioniFile.ToString()); //Viene visualizzato il messaggio di salvataggio eseguito correttamente.
                    }
                }
                //Attraverso il metodo List.Clear() si effettua l'eliminazione di ogni elemento presente nella lista.
                FantaAllenatori.Clear();
                FantaCalciatori.Clear();
                Indici.Clear();
                CodiciSchieramenti.Clear();
                chiusuraProgramma = true;
            }
            return chiusuraProgramma;
        }

        //Metodo che permette di cambiare i colori dei caratteri e dello sfondo, oppure di farli tornare alle proprietà standard in base al valore boolenano fornito come argomento. 
        static public void SetResetColori(bool setReset)
        {
            if (setReset == true)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen; //Metodo che definisce il colore dello sfondo.
                Console.ForegroundColor = ConsoleColor.White; //Metodo che definisce il colore del testo.
            }
            else
            {
                Console.ResetColor(); //Metodo che riporta i colori alle proprietà standard.
            }
        }

    }

    class OperazioniFile //La classe OperazioniFile contiene i metodi necessari a compiere le operazioni su file e a gestirne le eccezioni. 
    {
        //Metodi

        //Metodo costruttore.
        public OperazioniFile()
        {

        }

        //Metodo che permette di stabilire se un file esiste o meno, restituendo rispettivamente true o false.
        public bool VerificaEsistenzaFile(string percorsoIO) //Il metodo accetta come argomento il percorso del file.
        {
            return File.Exists(percorsoIO);
        }

        //Metodo che permette di eseguire la creazione di una cartella (directory).
        public string CreaCartella(string percorsoIO) //Il metodo accetta come argomento il percorso del file e restituisce un messaggio per indicare se ci sono state delle eccezioni o meno.
        {
            string messaggioOutput = "NESSUN ERRORE";
            try //Prova a eseguire le istruzioni.
            {
                Directory.CreateDirectory(percorsoIO); //Metodo che esegue la creazione di una directory al percorso specificato.
            }
            catch (UnauthorizedAccessException) //Rileva l'eccezione che non autorizza l'accesso al file.
            {
                messaggioOutput = GestisciErrori(0);
            }
            catch (IOException) //Rileva l'eccezione che indica un errore di IO.
            {
                messaggioOutput = GestisciErrori(1);
            }
            return messaggioOutput;
        }

        //Metodo che permette di eseguire la lettura di un file.
        public string LeggiFile(string percorsoIO) //Il metodo accetta come argomento il percorso del file e restituisce il contenuto del file letto oppute messaggio per indicare se ci sono state delle eccezioni.
        {
            string contenutoInput;
            try //Prova a eseguire le istruzioni.
            {
                contenutoInput = File.ReadAllText(percorsoIO); //Metodo che esegue la lettura di tutto il testo di un file al percorso specificato e restituisce la stringa del testo letto.
            }
            catch (UnauthorizedAccessException) //Rileva l'eccezione che non autorizza l'accesso al file.
            {
                contenutoInput = GestisciErrori(0);
            }
            catch (IOException) //Rileva l'eccezione che indica un errore di IO.
            {
                contenutoInput = GestisciErrori(1);
            }
            return contenutoInput;
        }

        //Metodo che permette di eseguire la scrittura di un file.
        public string ScriviFile(string percorsoIO, string contenutoOutput) //Il metodo accetta come argomenti il percorso del file e il contenuto che dovrà essere scritto su file e restituisce un messaggio per indicare se ci sono state delle eccezioni o meno.
        {
            string messaggioOutput = "NESSUN ERRORE";
            try //Prova a eseguire le istruzioni.
            {
                File.WriteAllText(percorsoIO, contenutoOutput); //Metodo che esegue la scrittura di un file al percorso specificato (se il percorso non esiste, allora crea il file) e inserisce l'intero contenuto di una stringa.
            } 
            catch (UnauthorizedAccessException) //Rileva l'eccezione che non autorizza l'accesso al file.
            {
                messaggioOutput = GestisciErrori(0);
            }
            catch (IOException) //Rileva l'eccezione che indica un errore di IO.
            {
                messaggioOutput = GestisciErrori(1);
            }
            return messaggioOutput;
        }

        //Metodo che permette di eseguire la cancellazione di un file.
        public string EliminaFile(string percorsoIO) //Il metodo accetta come argomento il percorso del file e restituisce un messaggio per indicare se ci sono state delle eccezioni o meno.
        {
            string messaggioOutput = "NESSUN ERRORE";
            try //Prova a eseguire le istruzioni.
            {
                File.Delete(percorsoIO); //Metodo che esegue l'eliminazione del file al percorso specificato.
            }
            catch (UnauthorizedAccessException) //Rileva l'eccezione che non autorizza l'accesso al file.
            {
                messaggioOutput = GestisciErrori(0);
            }
            catch (IOException) //Rileva l'eccezione che indica un errore di IO.
            {
                messaggioOutput = GestisciErrori(1);
            }
            return messaggioOutput;
        }

        //Metodo che permette di gestire le eccezioni derivanti dai metodi soprastanti.
        public string GestisciErrori(int codiceErrore) //Il metodo accetta come argomento il codice dell'errore che si è verificato nei metodi.
        {
            string descrizioneErrore = "";
            switch (codiceErrore) //In base al valore che assume la variabile, si possono presentare diversi casi.
            {
                case (0):
                    descrizioneErrore = "\nERRORE: Si è verificato un errore nell'accesso al file (accesso non autorizzato).\nSi prega di riavviare il programma e, eventualmente, se non si risolve il problema, contattare l'amministratore di sistema.";
                    break;
                case (1):
                    descrizioneErrore = "\nERRORE: Si è verificato un errore nell'accesso al file (errore di IO).\nSi prega di riavviare il programma e, eventualmente, se non si risolve il problema, contattare l'amministratore di sistema.";
                    break;
            }
            return descrizioneErrore;
        }

        //Metodo che sovrascrive il metodo ToString() e ritorna un messaggio che segnala all'utente che il salvataggio delle modifiche è stato compiuto con successo.
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
            if (punteggioClassifica < 0) //Se risulta un punteggio minore di zero, che non può esistere, allora il punteggio viene posto uguale a zero.
            {
                punteggioClassifica = 0;
            }
            return punteggioClassifica;
        }

        //Metodo che sovrascrive il metodo ToString() e ritorna gli attributi del fanta-calciatore concatenati e divisi da uno spazio fra di loro grazie al metodo string.Join(). 
        public override string ToString()
        {
            return string.Join(' ', nome, cognome, squadra, ruolo, numeroMaglia, quotazioneIniziale, quotazioneAttuale, punteggioClassifica);
        }
    }
}
