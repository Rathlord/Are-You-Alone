using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {

    // Many  game variables will be stored here

    [SerializeField] int spoons = 3;
    enum Screen { Introduction, Event, Gameplay, Night, NightStats, Moving, Gameover, RelationshipsScreen, Flirt, FlirtLina, FlirtJenna, FlirtAlex, FlirtPina, FlirtSammy, Flirtkrissi, Date, AskOut, JobQuery, RandomEvents };
    Screen currentScreen;
    [SerializeField] int turn = 0;
    [SerializeField] int money = 25;
    bool schoolDay = true;
    bool workDay = false;
    bool workedToday;
    bool schooledToday;
    int dailyExpenses = 1;
    int housingExpenses;
    int totalExpenses;
    int extraExpenses;
    int earnings = 0;
    int giftVar = 0;
    int askOutVar = 0;
    int introState = 0; //determine which intro screen is being shown
    int availableActions = 0; //gameover if available actions hits zero in actions screen
    bool spoonsDown = false;
    int spoonsDownTimer = 0;
    bool moneyDown = false;
    int moneyDownTimer = 0;
    string winText;
    int creditsTimer = 0;

    // Special value, daily random number, impacts many things

    [SerializeField] int dailyRand;
    [SerializeField] int eventRand;

    // Random Event System variables

    List<MyEvent> events;                                                                                                       // Declare a list called events that contains MyEvent object (other class)
    MyEvent currentEvent;

    // Relationship variables go here

    List<string> unknownWomen = new List<string>();
    List<string> knownWomen = new List<string>();
    int womenRand;
    string girlfriendName = "none";
    int dateAttitude;
    bool flirted = false;
    bool flirtLina = false;
    bool flirtJenna = false;
    bool flirtAlex = false;
    bool flirtPina = false;
    bool flirtSammy = false;
    bool flirtKrissi = false;
    bool flirtTooks = false;
    bool inLove = false;



    // Many character variables will be stored here

    bool school = true;
    bool girlfriend = false;
    bool employed = false;
    enum House { Parents, Friend, Rent, None };
    House currentHouse;
    enum Job { DepartmentStore, LumberYard, PaintStore, PizzaPlace, TravelingSales, GameCompany, None }
    Job currentJob;
    [SerializeField] int happiness = 50;
    [SerializeField] int stress = 50;
    [SerializeField] int pride = 50;
    [SerializeField] int determination = 0;
    [SerializeField] int loneliness = 0;
    [SerializeField] int workabsence = 0;
    [SerializeField] int schoolabsence = 0;
    [SerializeField] int friendAttitude = 50;
    [SerializeField] int onlineAttitude = 50;
    [SerializeField] int parentsAttitude = 30;
    int extraHappiness = 0;
    bool friendToday;
    bool onlineToday;
    bool flirtToday;
    bool dateToday;
    bool coffeeAddict = false;

    // Previous day stats to get end-of-day screen info

    int yesterdayStress;
    int yesterdayHappiness;
    int yesterdayPride;
    bool employedYesterday;


    // Lina variables

    int linaDifficulty = 80;
    int linaAttitude = 10;
    bool linaLove = false;
    int linaJokeMod = 5;
    int linaGiftMod = 2;
    int linaSweetMod = 4;


    // Jenna vars

    int jennaDifficulty = 100;
    int jennaAttitude = 20;
    bool jennaLove = false;
    int jennaJokeMod = 5;
    int jennaGiftMod = 3;
    int jennaSweetMod = 1;

    // Alex vars

    int alexDifficulty = 200;
    int alexAttitude = 5;
    bool alexLove = false;
    int alexJokeMod = 2;
    int alexGiftMod = -1;
    int alexSweetMod = 1;

    // Pina vars

    int pinaDifficulty = 120;
    int pinaAttitude = -10;
    bool pinaLove = false;
    int pinaJokeMod = 1;
    int pinaGiftMod = 5;
    int pinaSweetMod = 5;

    // Sammy vars

    int sammyDifficulty = 140;
    int sammyAttitude = -20;
    bool sammyLove = false;
    int sammyJokeMod = 3;
    int sammyGiftMod = 8;
    int sammySweetMod = 0;

    // krissi vars

    int krissiDifficulty = 150;
    int krissiAttitude = 0;
    bool krissiLove = false;
    int krissiJokeMod = 3;
    int krissiGiftMod = 1;
    int krissiSweetMod = 3;

    // Tooks vars

    int tooksDifficulty = 60;
    int tooksAttitude = 30;
    bool tooksLove = false;
    int tooksJokeMod = 8;
    int tooksGiftMod = 0;
    int tooksSweetMod = 6;

    // Use this for initialization
    void Start()
    {
        print("game start");
        AddSpace();
        IntroHandler("");
        currentHouse = House.Parents;
        dailyRand = UnityEngine.Random.Range(1, 101);
        currentJob = Job.None;
        currentScreen = Screen.Introduction;
        yesterdayStress = stress;
        yesterdayPride = pride;
        yesterdayHappiness = happiness;
        employedYesterday = false;
        unknownWomen.Add("Lina");
        unknownWomen.Add("Jenna");
        unknownWomen.Add("Alex");
        unknownWomen.Add("Pina");
        unknownWomen.Add("Sammy");
        unknownWomen.Add("krissi");
        InitializeEvents();
    }

    // TODO IMPLEMENT RANDOM DAILY EVENTS
    // TODO UNCOMMENT EVENT RANDOMIZATION AT FIXME IN CODE
    // TODO CREDITS
    // TODO CHECK ALL GAMEOVERS



    void OnUserInput(string input)
    {
        if (input == "Find a Friendly Bear" || input == "find a friendly bear")
        {
            knownWomen.Add("Tooks");
        }
        if (input == "exit") // Exit the game
        {
            Application.Quit();
        }
        if (input == "hiddenstats") // Dev command to see all stats
        {
            Terminal.WriteLine("stress" + stress + " happiness" + happiness + " pride" + pride);
        }
        if (input == "home" && (currentScreen == Screen.Gameplay || currentScreen == Screen.Flirt || currentScreen == Screen.Date))
        {
            RefreshScreen();
            MainScreen();
        }
        else if (input == "credits" && currentScreen == Screen.Gameover)
        {
            Credits();
        }
        else if (currentScreen == Screen.Event)
        {
            ExecuteEvent(input);
        }
        else if (currentScreen == Screen.JobQuery)
        {
            JobSearch(input);
            print("input passed to jobsearch");
        }
        else if (currentScreen == Screen.Introduction && input == "next")
        {
            IntroHandler(input);
        }
        else if ((input == "actions" || input == "action") && currentScreen == Screen.Gameplay) // List available actions
        {
            ActionList();
        }
        else if (currentScreen == Screen.Flirt)
        {
            FlirtInput(input);
        }
        else if (currentScreen == Screen.Date)
        {
            DateInput(input);
        }
        else if (currentScreen == Screen.FlirtLina)
        {
            LinaFlirt(input);
        }
        else if (currentScreen == Screen.FlirtJenna)
        {
            JennaFlirt(input);
        }
        else if (currentScreen == Screen.FlirtAlex)
        {
            AlexFlirt(input);
        }
        else if (currentScreen == Screen.FlirtPina)
        {
            PinaFlirt(input);
        }
        else if (currentScreen == Screen.FlirtSammy)
        {
            SammyFlirt(input);
        }
        else if (currentScreen == Screen.Flirtkrissi)
        {
            krissiFlirt(input);
        }
        else if (currentScreen == Screen.Gameplay && spoons > 0) // Pass info to input manager
        {
            InputManager(input);
            print("input passed into input manager from onuserinput");
        }
        else if (currentScreen == Screen.Gameplay && spoons <= 0) // Stop user from doing actions if out of spoons
        {
            AddSpace();
            Terminal.WriteLine("Out of spoons.");
            Invoke("MainScreen", 3f);
        }
        else if (currentScreen == Screen.Night)
        {
            if (input == "next")
            {
                RelationshipStats();
            }
        }
        else if (currentScreen == Screen.NightStats)
        {
            if (input == "next")
            {
                Morning();
            }
        }
        else if (currentScreen == Screen.RelationshipsScreen)
        {
            if (input == "next")
            {
                NightStats();
            }
        }
        else if (currentScreen == Screen.Moving)
        {
            if (input == "parents" && parentsAttitude > 0 && school == true)
            {
                currentHouse = House.Parents;
                Moved();
            }
            else if (input == "friend" && friendAttitude > 0)
            {
                currentHouse = House.Friend;
                Moved();
            }
            else if (input == "rent" && money > 10)
            {
                currentHouse = House.Rent;
                Moved();
            }
            else if (input == "homeless")
            {
                currentHouse = House.None;
                Moved();
            }
            else if (input == "cancel")
            {
                currentScreen = Screen.Gameplay;
                MainScreen();
            }
        }
        else
        {
            Terminal.WriteLine("Incorrect Command");
            return;
        }
    }     // Pass user input to other methods via this

    void IntroHandler(string input)
    {
        Introduction();
        if (input == "next")
        {
            introState++;
            Terminal.ClearScreen();
            Introduction();
        }
    }

    void Introduction()
    {
        if (introState == 0)
        {
            Terminal.ClearScreen();
            Terminal.WriteLine(@"
 █████╗ ██████╗ ███████╗                                           
██╔══██╗██╔══██╗██╔════╝                                           
███████║██████╔╝█████╗                                             
██╔══██║██╔══██╗██╔══╝                                             
██║  ██║██║  ██║███████╗                                           
╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝                                           
                                                                   
        ██╗   ██╗ ██████╗ ██╗   ██╗                                
        ╚██╗ ██╔╝██╔═══██╗██║   ██║                                
         ╚████╔╝ ██║   ██║██║   ██║                                
          ╚██╔╝  ██║   ██║██║   ██║                                
           ██║   ╚██████╔╝╚██████╔╝                                
           ╚═╝    ╚═════╝  ╚═════╝                                 
                                                                   
                 █████╗ ██╗      ██████╗ ███╗   ██╗███████╗██████╗ 
                ██╔══██╗██║     ██╔═══██╗████╗  ██║██╔════╝╚════██╗
                ███████║██║     ██║   ██║██╔██╗ ██║█████╗    ▄███╔╝
                ██╔══██║██║     ██║   ██║██║╚██╗██║██╔══╝    ▀▀══╝ 
                ██║  ██║███████╗╚██████╔╝██║ ╚████║███████╗  ██╗   
                ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝  ╚═╝   
                                                                    ");
            Terminal.WriteLine("Enter 'next' to continue.");
        }
        else if (introState == 1)
        {
            RefreshScreen();
            AddSpace();
            Terminal.WriteLine("Play the game by entering commands on the keyboard.");
            Terminal.WriteLine("You can type 'home' from almost any screen to return to the main screen.");
            AddSpace();
            Terminal.WriteLine("You can usually perform three actions per day. These are called spoons.");
            Terminal.WriteLine("Spoons are tracked at the top right of your screen.");
            Terminal.WriteLine("Some actions can only be performed once per day.");
            AddSpace();
            Terminal.WriteLine("You can learn about most actions by typing them with an exclamation mark.");
            Terminal.WriteLine("For instance, type '!home' at the main menu to learn about the home command.");
            AddSpace();
            Terminal.WriteLine("Type 'next' for the next page.");
            AddSpace();
        }
        else if (introState == 2)
        {
            currentScreen = Screen.Gameplay;
            RefreshScreen();
            Terminal.WriteLine("You can 'Win' the game by becoming happy or giving yourself a good future.");
            Terminal.WriteLine("You will lose the game if you become too sad or otherwise can't continue.");
            Terminal.WriteLine("If your stress, pride, or happiness get too low you won't be able to do certain things.");
            AddSpace();
            Terminal.WriteLine("Increase your happiness by doing things you enjoy.");
            Terminal.WriteLine("Decrease your stress by relaxing and doing things that aren't strenuous.");
            Terminal.WriteLine("Increase your pride by doing things you'd be proud of yourself for.");
            AddSpace();
            Terminal.WriteLine("You also need to manage your loneliness and relationships.");
            Terminal.WriteLine("Make sure not to neglect people in your life or there may be consequences.");
            AddSpace();
            Terminal.WriteLine("Enter 'home' to begin the game.");
        }
        else
        {
            return;
        }
    }

    void WomenList()
    {
        StringBuilder womenlist = new StringBuilder();
        foreach (string women in knownWomen)
        {
            womenlist.Append(women + " ");
        }
        Terminal.WriteLine(womenlist.ToString());
        if (womenlist.Length == 0)
        {
            Terminal.WriteLine("No One");
            AddSpace();
        }
    }

    void MainScreen()
    {
        currentScreen = Screen.Gameplay;
        RefreshScreen();
        Terminal.WriteLine("Enter an action!                (You can enter 'actions' to see a list)");
        AddSpace();
        Terminal.WriteLine("These are the people you know:");
        WomenList();
        AddSpace();
        if (schoolDay == true && school == true && schooledToday == false)
        {
            Terminal.WriteLine("Today is a school day. You should go to university.");
        }
        else if (schooledToday == true && schoolDay == true)
        {
            Terminal.WriteLine("You went to university today.");
        }
        if (workDay == true && employed == true && workedToday == false)
        {
            Terminal.WriteLine("Today is a work day. You should go to work today");
        }
        else if (workedToday == true && workDay == true)
        {
            Terminal.WriteLine("You worked today.");
        }
        AddSpace();                                                                            
        if (spoons == 0)
        {
            RefreshScreen();
            Terminal.WriteLine("Out of spoons. It's time for bed.");
            Invoke("Night", 1.5f);
        }
    }

    // Morning Cleanup
    void Morning()
    {
        RefreshScreen();
        Terminal.WriteLine(WakeUp());
        turn++;
        spoons = 3;
        if (coffeeAddict == true)
        {
            spoons++;
            happiness -= 2;
        }
        SpoonsDownCheck();
        MoneyDownCheck();
        CheckDays();
        dailyRand = UnityEngine.Random.Range(1, 101);
        Attitudes();
        Invoke("RNGChecker", 3f);                                            
        currentScreen = Screen.Event;
        yesterdayStress = stress;
        yesterdayHappiness = happiness;
        yesterdayPride = pride;
        flirted = false;
        flirtLina = false;
        flirtJenna = false;
        flirtAlex = false;
        flirtPina = false;
        flirtSammy = false;
        flirtKrissi = false;
        flirtTooks = false;
        if (employed == true)
        {
            employedYesterday = true;
        }
        else
        {
            employedYesterday = false;
        }
    }

    public string WakeUp()
    {
        string wakeUp;
        if (dailyRand > 95)
        {
            wakeUp = "You had horrible nightmares in which people you love were hurt. You feel terrible.";
            happiness--;
            stress--;
        }
        else if (dailyRand > 80)
        {
            wakeUp = "You couldn't sleep, tossing and turning all night. Sleep deprivation stresses you out.";
            stress = (stress - 2);
        }
        else if (dailyRand > 78)
        {
            wakeUp = "In your dreams you found someone who made you happy... but then you lost them.";
            loneliness--;
        }
        else if (dailyRand > 65)
        {
            wakeUp = "It took you hours to fall asleep. You don't feel very rested.";
            stress--;
        }
        else if (dailyRand > 50)
        {
            wakeUp = "You feel like you slept in a strange position. You feel kind of achey.";
            happiness--;
        }
        else if (dailyRand > 48)
        {
            wakeUp = "You dreamt you were fighting all night. You're not sure how it makes you feel";
            loneliness--;
            happiness--;
            stress++;
        }
        else if (dailyRand > 30)
        {
            wakeUp = "You didn't sleep much, but that's pretty normal for you these days.";
        }
        else if (dailyRand > 10)
        {
            wakeUp = "You actually got a decent night's sleep, pretty rare for you.";
            stress++;
        }
        else
        {
            wakeUp = "You sleep really well, and wake up feeling good.";
        }
        return wakeUp;
    }

    void Night()
    {
        currentScreen = Screen.Night;
        RefreshScreen();
        totalExpenses = (dailyExpenses + housingExpenses + extraExpenses);
        AddSpace();
        Terminal.WriteLine("Your expense for the day is -" + totalExpenses + "$");
        money = (money - totalExpenses);
        if (employed == true && workDay == true && workedToday == true)
        {
            Terminal.WriteLine("You earned " + earnings + "$ today.");
            money = (money + earnings);
        }
        happiness = happiness + extraHappiness;
        KickedOutCheck();
        AddSpace();
        HousingCheck();
        AddSpace();
        LonelinessCheck();
        AddSpace();
        FulfillmentCheck();
        AddSpace();
        Terminal.WriteLine("Write 'next' to proceed");
    }

    void RelationshipStats()
    {
        currentScreen = Screen.RelationshipsScreen;
        RefreshScreen();
        NightRelationships();
        AddSpace();
        GirlfriendAttitude();
        AddSpace();
        Terminal.WriteLine("Write 'next' to proceed");
    }

    void NightStats()
    {
        currentScreen = Screen.NightStats;
        RefreshScreen();
        if (schoolDay == true && schooledToday == false && school == true)
        {
            Terminal.WriteLine("You skipped school today. Hopefully there aren't any consequences...");
        }
        if (workDay == true && workedToday == false && employed == true && employedYesterday == true)
        {
            Terminal.WriteLine("You skipped work today. Better not do that too often...");
        }
        AddSpace();
        if (coffeeAddict == true)
        {
            Terminal.WriteLine("You are addicted to coffee. You can do more, but get headaches sometimes that make you less happy.");
        }
        AddSpace();
        int picker = UnityEngine.Random.Range(0, 5);
        string[] happinessGreat = { "You feel strangely hopeful.", "You fall asleep with a big grin.", "You're in a good mood despite everything.", "You fall asleep listening to music you love, dancing to the beat.", "You feel like everything will be okay" };
        string[] happinessGood = { "You feel content.", "You let yourself think about good things in the future.", "You fall asleep with a slight smile.", "You're feeling good.", "You're thankful for your current situation." };
        string[] happinessBad = { "You're in a bad mood.", "You think about things ending.", "You're upset, even though you don't know exactly why.", "You don't feel hopeful about your future.", "You go to sleep feeling upset." };
        string[] happinessTerrible = { "Life doesn't feel worth living.", "You find yourself wanting to be in pain, but not knowing why.", "You're ready to give up.", "You wonder what the world would be like without you.", "You regret your choices in life." };
        string[] stressGreat = { "You feel like you can do anything!", "You're ready to tackle life.", "You feel incredibly focused.", "For once, you have no worries.", "You feel like you could learn anything." };
        string[] stressGood = { "You feel at ease.", "You feel relaxed.", "You feel like going out and doing something.", "You're not worried about life", "Things aren't perfect, but you don't spend your time worrying about it." };
        string[] stressBad = { "Stress feels like a weight on your shoulders.", "You're having trouble concentrating on anything.", "You're under a lot of pressure", "You don't feel like doing anything.", "Your worries are making you anxious." };
        string[] stressTerrible = { "You're done with life's shit.", "Why bother with anything?", "You can barely force yourself out of bed.", "You're buckling under the pressure.", "Any task, no matter how small, feels overwhelming." };
        string[] prideGreat = { "You're proud of yourself.", "You think you have a lot to offer the world.", "You know you're the best.", "You feel self sufficient.", "You don't care what others think of you." };
        string[] prideGood = { "You think you're doing a good job.", "You're confident in yourself.", "You think you're pretty cool.", "You feel strong.", "You're not bothered by other people." };
        string[] prideBad = { "You know you're not doing your best.", "You wish you felt more fulfilled.", "What do you have to offer?", "You don't feel like you contribute to society.", "You worry what other people think of you." };
        string[] prideTerrible = { "You feel useless and worthless.", "You feel like you've fucked your entire life up.", "You don't feel like you've accomplished anything at all.", "You feel like a waste of breath.", "You hate yourself." };
        HappinessCheck(picker, happinessGreat, happinessGood, happinessBad, happinessTerrible);
        StressChecker(picker, stressGreat, stressGood, stressBad, stressTerrible);
        PrideCheck(picker, prideGreat, prideGood, prideBad, prideTerrible);
        AddSpace();
        ImprovementCheck();
        AddSpace();
        StatChecks();
        Absences();
        if (currentScreen == Screen.NightStats)
        {
            Terminal.WriteLine("Write 'next' to continue to a new day.");
        }
        else
        {
            print("Game Over");
        }

    }

    void StatChecks() // Caps stats at 100/-100 and fires events at day end if they exceed cap
    {
        if (happiness > 100)
        {
            happiness = (happiness - 25);
            determination++;
            pride = (pride + 10);
            stress = (stress + 10);
            loneliness = (loneliness + 5);
            Terminal.WriteLine("You are so happy you feel your whole life getting better.");
            Terminal.WriteLine("You are filled with Determination.");
        }
        if (happiness < -100) // Game over or save if determination
        {
            if (determination > 0)
            {
                determination--;
                happiness = (happiness + 10);
                Terminal.WriteLine("You are swallowed by dark thoughts, but your Determination helps you pull through.");
                pride = (pride - 10);
                stress = (stress - 10);
            }
            else
            {
                DepressionGameOver();
            }
        }
        if (pride >= 100) // Grant determination
        {
            pride = (pride - 25);
            determination++;
            AddSpace();
            Terminal.WriteLine("You are extremely proud of what you've done with your life.");
            Terminal.WriteLine("You are filled with Determination!");
            AddSpace();
        }
        if (pride < -100 && determination < 1)
        {
            if (determination > 0)
            {
                determination--;
                Terminal.WriteLine("The meaninglessness of your life almost overwhelms you, but your determination helps you pull through.");
                happiness = (happiness - 10);
                stress = (stress - 10);
            }
            else
            {
                ShameGameOver();
            }
        }
        if (stress > 100)
        {
            stress = (stress - 25);
            Terminal.WriteLine("You feel so carefree that your whole life just seems better.");
            happiness = (happiness + 20);
            pride = (pride + 10);
            loneliness = (loneliness + 10);
        }
        if (stress < -100)
        {
            if (determination > 0)
            {
                determination--;
                Terminal.WriteLine("You are so stressed you almost give up, but your Determination helps you pull through.");
                stress = (stress + 10);
            }
            else
            {
                StressGameOver();
            }
        }
        if (loneliness > 100)
        {
            loneliness = (loneliness - 25);
            Terminal.WriteLine("You feel overwhelmed by the support of the people who love you.");
            Terminal.WriteLine("You are filled with Determination.");
            determination++;
        }
        if (loneliness < -100)
        {
            if (determination > 0)
            {
                determination--;
                Terminal.WriteLine("You feel so utterly alone that you almost give up, but your Determination helps you pull through");
                happiness = (happiness - 10);
                stress = (stress - 10);
                pride = (pride - 10);
            }
            else
            {
                LonelinessGameOver();
            }
        }
        if (money < 1 && parentsAttitude > 0)
        {
            Terminal.WriteLine("You run out of money. Your parents give you a loan... this time.");
            money = (money + 10);
            parentsAttitude = (parentsAttitude - 10);
        }
        else if (money < 1 && friendAttitude > 0)
        {
            Terminal.WriteLine("You run out of money. Your friend gives you a small loan grudgingly.");
            money = (money + 5);
            friendAttitude = (friendAttitude - 15);
        }
        else if (money < 1)
        {
            MoneyGameOver();
        }
    }

    void MoneyGameOver()
    {
        RefreshScreen();
        Terminal.WriteLine("You can't even afford your living expenses any more. You're out of money. \n You have no one to support you. You can't keep living like this");
        AddSpace();
        Terminal.WriteLine("Game Over");
        AddSpace();
        Terminal.WriteLine("Type 'exit' to exit game.");
        currentScreen = Screen.Gameover;
    }

    void LonelinessGameOver()
    {
        RefreshScreen();
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "The entire universe feels like it's closing in on you. \n You feel so undescribably alone that you can't take it anymore.";
        }
        else if (dailyRand > 50)
        {
            randomLine = "You feel horrible. You check your phone to see if there's anyone you can talk to.  \n There just isn't anyone there, though. You are alone.  \n You just can't take it anymore.";
        }
        else if(dailyRand > 25)
        {
            randomLine = "You feel like there's no one out there who cares about you.  \n You don't even think you'll be missed when you're gone.  \n You can't take it anymore.";
        }
        else
        {
            randomLine = "You haven't spoken to anyone who cares about you in days. \n You're sure no one cares. You can't take it anymore.";
        }
        Terminal.WriteLine(randomLine);
        AddSpace();
        Terminal.WriteLine("Game Over!");
        AddSpace();
        Terminal.WriteLine("Type 'exit' to exit game");
        currentScreen = Screen.Gameover;
    }

    void StressGameOver()
    {
        RefreshScreen();
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "The pressure of your life is just too much to bear. \n You can't take it anymore.";
        }
        else if(dailyRand > 50)
        {
            randomLine = "Everyone expects so much from you, but that's never been what you wanted. \n You can't bear being a disappointment anymore.";
        }
        else if(dailyRand > 25)
        {
            randomLine = "You have so much on your plate that you can't handle it. \n You'd rather the alternative than to face all of it.";
        }
        else
        {
            randomLine = "The stress is so much that you just break down.  \n You just can't take it anymore.";
        }
        Terminal.WriteLine(randomLine);
        AddSpace();
        Terminal.WriteLine("Game Over!");
        AddSpace();
        Terminal.WriteLine("Type 'exit' to exit game");
        currentScreen = Screen.Gameover;
    }

    void ShameGameOver()
    {
        RefreshScreen();
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "You hate the person you've become and can't stand to live with yourself anymore.";
        }
        else if(dailyRand > 50)
        {
            randomLine = "You're disgusted by the life you've lived. You squandered so many opportunities. \n You can't live with it anymore.";
        }
        else if(dailyRand > 25)
        {
            randomLine = "You're so disappointed with the way your life has gone. It's just all gone wrong. \n You can't keep going like this.";
        }
        else
        {
            randomLine = "You are so ashamed of what you've accomplished that you can't bear it anymore.";
        }
        Terminal.WriteLine(randomLine);
        AddSpace();
        Terminal.WriteLine("Game Over!");
        AddSpace();
        Terminal.WriteLine("Type 'exit' to exit game");
        currentScreen = Screen.Gameover;
    }

    void DepressionGameOver()
    {
        RefreshScreen();
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "You're so tired of being sad all the time. You just have to get away from it. \n You don't have any choice. You can't take anymore.";
        }
        else if(dailyRand > 50)
        {
            randomLine = "Every day is misery. You don't see any other future for yourself. \n This kind of life isn't worth it. You can't keep going.";
        }
        else if(dailyRand > 25)
        {
            randomLine = "It feels like you go through life dragging an anchor behind you. Today it's just too heavy. \n You just can't take it anymore.";
        }
        else
        {
            randomLine = "You are angry and sad, sick and disappointed. You don't know if you let down life or it let down you. \n All you know is you're through with it.";
        }
        Terminal.WriteLine(randomLine);
        AddSpace();
        Terminal.WriteLine("Game Over!");
        AddSpace();
        Terminal.WriteLine("Type 'exit' to exit game");
        currentScreen = Screen.Gameover;
    }

    void FulfillmentCheck()
    {
        if (school == false)
        {
            Terminal.WriteLine("You dropped out of school. You still feel guilty about it.");
            pride = (pride - 2);
        }
        if (school == true)
        {
            Terminal.WriteLine("You're still going to university. You don't like it much, but you're proud of it.");
            pride = (pride + 2);
        }
        if (employed == false)
        {
            Terminal.WriteLine("You don't have a job. You feel guilty for being unemployed.");
            pride = (pride - 4);
        }
        if (employed == true)
        {
            Terminal.WriteLine("You're holding down a steady job. Go you!");
            pride = (pride + 2);
        }
        if (money >= 20)
        {
            Terminal.WriteLine("You feel financially stable.");
            happiness = (happiness + 5);
            stress = (stress + 2);
            pride = (pride + 3);
        }
        else if (money < 20)
        {
            Terminal.WriteLine("Money feels tight.");
            happiness = (happiness - 2);
            stress = (stress - 2);
            pride = (pride - 2);
        }
    }

    void LonelinessCheck()
    {
        if (friendToday == true)
        {
            Terminal.WriteLine("You made time for your friend today.");
        }
        if (onlineToday == true)
        {
            Terminal.WriteLine("You spent some time with your online friends today.");
        }
        if (friendToday == false && onlineToday == false)
        {
            Terminal.WriteLine("You didn't talk to any friends today. You miss them.");
            loneliness = (loneliness - 4);
        }
        if (flirtToday == false && dateToday == false)
        {
            Terminal.WriteLine("You miss talking to someone special.");
            loneliness = (loneliness - 4);
        }
        if (loneliness < 0)
        {
            Terminal.WriteLine("You feel disconnected and lonely.");
            happiness = (happiness - 7);
            stress = (stress - 1);
            pride = (pride - 5);
        }
    }

    void HousingCheck()
    {
        if (currentHouse == House.None)
        {
            Terminal.WriteLine("You are homeless. You feel terrible.");
            happiness = (happiness - 20);
            stress = (stress - 10);
            pride = (pride - 20);
            housingExpenses = 0;
        }
        if (currentHouse == House.Friend)
        {
            Terminal.WriteLine("You're crashing with a friend. It's cheap, but makes him unhappy.");
            happiness = (happiness + 1);
            stress = (stress - 2);
            pride = (pride - 3);
            friendAttitude = (friendAttitude - 4);
            housingExpenses = 1;
        }
        if (currentHouse == House.Parents)
        {
            Terminal.WriteLine("You're living with your parents. It's free, but not good for you.");
            happiness = (happiness - 3);
            stress = (stress - 3);
            pride = (pride - 5);
            parentsAttitude = (parentsAttitude - 2);
            housingExpenses = 0;
        }
        if (currentHouse == House.Rent)
        {
            Terminal.WriteLine("You rent your own place. It's not much, but at least you're independent.");
            happiness = (happiness + 3);
            stress = (stress - 1);
            pride = (pride + 5);
            housingExpenses = 3;
        }
    }

    void KickedOutCheck()
    {
        if (currentHouse == House.Parents && school == false)
        {
            currentHouse = House.None;
            Terminal.WriteLine("Your parents aren't okay with you dropping out of school.");
            Terminal.WriteLine("They've kicked you out. You're homeless!");
            if (parentsAttitude > 0)
            {
                parentsAttitude = 0;
            }
        }
        if (currentHouse == House.Friend && friendAttitude < 1)
        {
            currentHouse = House.None;
            Terminal.WriteLine("Your friend is sick of you. He asks you to leave.");
            Terminal.WriteLine("You're homeless!");
        }
        if (currentHouse == House.Rent && money < 4)
        {
            currentHouse = House.None;
            Terminal.WriteLine("You can't afford to pay rent. Your landlord kicks you out!");
            Terminal.WriteLine("You're homeless!");
        }
    }

    void ImprovementCheck()
    {
        if (happiness > yesterdayHappiness)
        {
            Terminal.WriteLine("You feel happier.");
        }
        else if (happiness == yesterdayHappiness)
        {
            Terminal.WriteLine("You feel about the same happiness.");
        }
        else if (happiness < yesterdayHappiness)
        {
            Terminal.WriteLine("You feel sadder.");
        }
        if (stress > yesterdayStress)
        {
            Terminal.WriteLine("You feel less stressed.");
        }
        else if (stress == yesterdayStress)
        {
            Terminal.WriteLine("Your stress level feels constant.");
        }
        else if (stress < yesterdayStress)
        {
            Terminal.WriteLine("You feel more stressed out.");
        }
        if (pride > yesterdayPride)
        {
            Terminal.WriteLine("You feel more proud of yourself.");
        }
        else if (pride == yesterdayPride)
        {
            Terminal.WriteLine("Your thoughts about yourself haven't changed.");
        }
        else if (pride < yesterdayPride)
        {
            Terminal.WriteLine("You feel less proud.");
        }
    }

    void PrideCheck(int picker, string[] prideGreat, string[] prideGood, string[] prideBad, string[] prideTerrible)
    {
        if (pride > 60)
        {
            Terminal.WriteLine(prideGreat[picker]);
        }
        else if (pride > 0)
        {
            Terminal.WriteLine(prideGood[picker]);
        }
        else if (pride > -60)
        {
            Terminal.WriteLine(prideBad[picker]);
        }
        else
        {
            Terminal.WriteLine(prideTerrible[picker]);
        }
    }

    void StressChecker(int picker, string[] stressGreat, string[] stressGood, string[] stressBad, string[] stressTerrible)
    {
        if (stress > 60)
        {
            Terminal.WriteLine(stressGreat[picker]);
        }
        else if (stress > 0)
        {
            Terminal.WriteLine(stressGood[picker]);
        }
        else if (stress > -60)
        {
            Terminal.WriteLine(stressBad[picker]);
        }
        else
        {
            Terminal.WriteLine(stressTerrible[picker]);
        }
    }

    void HappinessCheck(int picker, string[] happinessGreat, string[] happinessGood, string[] happinessBad, string[] happinessTerrible)
    {
        if (happiness > 60)
        {
            Terminal.WriteLine(happinessGreat[picker]);
        }
        else if (happiness > 0)
        {
            Terminal.WriteLine(happinessGood[picker]);
        }
        else if (happiness > -60)
        {
            Terminal.WriteLine(happinessBad[picker]);
        }
        else
        {
            Terminal.WriteLine(happinessTerrible[picker]);
        }
    }

    void Attitudes() // Does daily attitude adjustments and then resets daily encounters       
    {
        if (friendToday == true)
        {
            friendAttitude = (friendAttitude + 5);
        }
        else
        {
            friendAttitude = (friendAttitude - 3);
        }
        if (onlineToday == true)
        {
            onlineAttitude = (onlineAttitude + 3);
        }
        else
        {
            onlineAttitude = (onlineAttitude - 1);
        }
        friendToday = false;
        onlineToday = false;
    }

    void NightRelationships()
    {
        RefreshScreen();
        if (girlfriend == false)
        {
            Terminal.WriteLine("You don't have a girlfriend. You miss having someone who cares about you.");
            happiness = (happiness - 5);
            pride = (pride - 2);
            stress = (stress + 1);
            loneliness = (loneliness - 3);
        }
        else
        {
            Terminal.WriteLine("You are in a relationship with " + girlfriendName + ". That makes you happy!");
            happiness = (happiness + 3);
            pride = (pride + 3);
            stress = (stress - 1);
            loneliness = (loneliness + 8);
        }
        AddSpace();
        RelationshipAttitudes();
        AddSpace();
        LeaveLife();
        AddSpace();
    }

    void LeaveLife()
    {
        if (knownWomen.Contains("krissi") == true && krissiAttitude < -100)
        {
            knownWomen.Remove("krissi");
            Terminal.WriteLine("krissi moves on. She is no longer in your life.");
        }
        if (knownWomen.Contains("Sammy") == true && sammyAttitude < -100)
        {
            knownWomen.Remove("Sammy");
            Terminal.WriteLine("Sammy moves on. She is no longer in your life.");
        }
        if (knownWomen.Contains("Lina") == true && linaAttitude < -100)
        {
            knownWomen.Remove("Lina");
            Terminal.WriteLine("Lina moves on. She is no longer in your life.");
        }
        if (knownWomen.Contains("Jenna") == true && jennaAttitude < -100)
        {
            knownWomen.Remove("Jenna");
            Terminal.WriteLine("Jenna moves on. She is no longer in your life.");
        }
        if (knownWomen.Contains("Alex") == true && alexAttitude < -100)
        {
            knownWomen.Remove("Alex");
            Terminal.WriteLine("Alex moves on. She is no longer in your life.");
        }
        if (knownWomen.Contains("Pina") == true && pinaAttitude < -100)
        {
            knownWomen.Remove("Pina");
            Terminal.WriteLine("Pina moves on. She is no longer in your life.");
        }
        if (knownWomen.Contains("Tooks") == true && tooksAttitude < -100)
        {
            knownWomen.Remove("Tooks");
            Terminal.WriteLine("Tooks returns to the woods. He is no longer in your life.");
        }
    }

    void GirlfriendAttitude()
    {
        if (girlfriend == true)
        {
            Terminal.WriteLine("You are in a relationship with " + girlfriendName);
            if (girlfriendName == "Lina")
            {
                Terminal.WriteLine(LinaStatus());
            }
            if (girlfriendName == "Jenna")
            {
                Terminal.WriteLine(JennaStatus());
            }
            if (girlfriendName == "Alex")
            {
                Terminal.WriteLine(AlexStatus());
            }
            if (girlfriendName == "Pina")
            {
                Terminal.WriteLine(PinaStatus());
            }
            if (girlfriendName == "Sammy")
            {
                Terminal.WriteLine(SammyStatus());
            }
            if (girlfriendName == "krissi")
            {
                Terminal.WriteLine(krissiStatus());
            }
            if (girlfriendName == "Tooks")
            {
                Terminal.WriteLine(TooksStatus());
            }
        }
    }

    string TooksStatus()
    {
        string statusMessage;
        if (tooksAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Tooks nuzzles you with his snout, and you get the impression he's expressing his love.";
            happiness = (happiness + 5);
            tooksLove = true;
            inLove = true;
            pride = (pride + 2);
            stress = (stress + 2);
            loneliness = (loneliness + 3);
        }
        else if (tooksAttitude > 75)
        {
            statusMessage = "Tooks invites you to the secret bear picnic in the woods. \n You bring sandwiches. It goes rather well.";
            happiness = (happiness + 2);
            stress = (stress + 3);
            loneliness = (loneliness + 2);
        }
        else if (tooksAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Tooks somehow calls you and makes bear noises before snoring into the phone.";
            happiness = (happiness + 1);
            loneliness = (loneliness + 1);
        }
        else if (tooksAttitude > 0)
        {
            statusMessage = "You find a small piece of salmon in your pocket. It must be from Tooks.";
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Tooks refuses to share his honey with you today.";
        }
        else
        {
            statusMessage = "Tooks storms out of your apartment. Through the wall. You're heartbroken.";
            inLove = false;
            tooksLove = false;
            happiness = (happiness - 999);
            stress = (stress - 999);
            loneliness = (loneliness - 999);
            pride = (pride - 999);
            girlfriend = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    string krissiStatus()
    {
        string statusMessage;
        if (krissiAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Krissi tells you she's in love with you. It makes you very happy";
            happiness = (happiness + 5);
            krissiLove = true;
            inLove = true;
            pride = (pride + 2);
            stress = (stress + 2);
            loneliness = (loneliness + 3);
        }
        else if (krissiAttitude > 75)
        {
            statusMessage = "Krissi invites you to stay the night with her, and you both have a great time.";
            happiness = (happiness + 2);
            stress = (stress + 3);
            loneliness = (loneliness + 2);
        }
        else if (krissiAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Krissi calls to say goodnight before you fall asleep.";
            happiness = (happiness + 1);
            loneliness = (loneliness + 1);
        }
        else if (krissiAttitude > 0)
        {
            statusMessage = "Krissi texts you to say goodnight.";
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Krissi isn't talking to you tonight.";
        }
        else
        {
            statusMessage = "Krissi breaks up with you. You're heartbroken.";
            inLove = false;
            krissiLove = false;
            happiness = (happiness - 45);
            stress = (stress - 15);
            loneliness = (loneliness - 40);
            pride = (pride - 10);
            girlfriend = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    string SammyStatus()
    {
        string statusMessage;
        if (sammyAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Sammy spends the night in with you, listening to music and dancing.";
            sammyLove = true;
            inLove = true;
            happiness = (happiness + 2);
            stress = (stress + 2);
            loneliness = (loneliness + 2);
        }
        else if (sammyAttitude > 75)
        {
            statusMessage = "Sammy gets concert tickets and you both attend a show. She has a lot of fun!";
            happiness = (happiness + 1);
            stress = (stress - 1);
        }
        else if (sammyAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Sammy texts you to say goodnight";
            loneliness = (loneliness + 1);
        }
        else if (sammyAttitude > 0)
        {
            statusMessage = "You don't hear from Sammy tonight. You think she might be busy";
            loneliness = (loneliness - 1);
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Sammy is ignoring your messages.";
        }
        else
        {
            statusMessage = "Sammy breaks up with you. You're heartbroken.";
            inLove = false;
            sammyLove = false;
            happiness = (happiness - 25);
            stress = (stress - 15);
            loneliness = (loneliness - 20);
            pride = (pride - 5);
            girlfriend = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    string PinaStatus()
    {
        string statusMessage;
        if (pinaAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Pina makes you dinner and then you spent the entire night cuddling together.";
            pinaLove = true;
            inLove = true;
            happiness = (happiness + 6);
            if (stress < 0)
            {
                stress = 0;
            }
            else
            {
                stress = (stress + 5);
            }
            pride = (pride + 3);
            loneliness = (loneliness + 2);
        }
        else if (pinaAttitude > 75)
        {
            statusMessage = "Pina pulls you to bed and smiles at you suggestively.";
            happiness = (happiness + 3);
            stress = (stress + 3);
            loneliness = (loneliness + 1);
        }
        else if (pinaAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Pina comes over to your place and watches TV with you all night.";
            happiness = (happiness + 2);
            stress = (stress + 1);
            loneliness = (loneliness + 1);
        }
        else if (pinaAttitude > 0)
        {
            statusMessage = "Pina is upset with you tonight. You're not sure why, you think it'll blow over though.";
            loneliness = (loneliness - 1);
            stress = (stress - 1);
            happiness = (happiness - 1);
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Pina isn't talking to you tonight.";
        }
        else
        {
            statusMessage = "Pina leaves you. You're heartbroken.";
            inLove = false;
            pinaLove = false;
            happiness = (happiness - 35);
            stress = (stress - 25);
            loneliness = (loneliness - 30);
            pride = (pride - 20);
            girlfriend = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    string AlexStatus()
    {
        string statusMessage;
        if (alexAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Alex comes up with something fun and creative to do with her tonight.";
            alexLove = true;
            inLove = true;
            happiness = (happiness + 3);
            loneliness = (loneliness + 3);
            stress = (stress - 1);
        }
        else if (alexAttitude > 75)
        {
            statusMessage = "You and Alex have some wine and she takes you to bed with her.";
            happiness = (happiness + 1);
            loneliness = (loneliness + 1);
            stress = (stress + 5);

        }
        else if (alexAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "You go shopping with Alex to some of her favorite stores.";
            happiness = (happiness + 1);
            loneliness = (loneliness + 1);
            stress = (stress - 1);
        }
        else if (alexAttitude > 0)
        {
            statusMessage = "Alex is busy hanging out with someone else tonight.";
            loneliness = (loneliness - 2);
            stress = (stress - 2);
            happiness = (happiness - 1);
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Alex isn't talking to you tonight.";
        }
        else
        {
            statusMessage = "Alex starts seeing other people. You're heartbroken.";
            inLove = false;
            alexLove = false;
            happiness = (happiness - 45);
            stress = (stress - 15);
            loneliness = (loneliness - 40);
            pride = (pride - 10);
            girlfriend = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    string JennaStatus()
    {
        string statusMessage;
        if (jennaAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "You have pizza and binge Netflix and cuddles with Jenna all night.";
            jennaLove = true;
            inLove = true;
            happiness = (happiness + 2);
            stress = (stress + 1);
            loneliness = (loneliness + 1);
        }
        else if (jennaAttitude > 75)
        {
            statusMessage = "You hang out with Jenna and her friends for a while.";
            happiness = (happiness + 1);
            stress = (stress - 2);
            loneliness = (loneliness + 2);
        }
        else if (jennaAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Jenna sends you fun Snapchats for a while before bed.";
            happiness = (happiness + 1);
        }
        else if (jennaAttitude > 0)
        {
            statusMessage = "Jenna sends you a quick good night message.";
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Jenna isn't talking to you tonight.";
        }
        else
        {
            statusMessage = "Jenna stops talking to you. You're crushed.";
            inLove = false;
            jennaLove = false;
            happiness = (happiness - 25);
            stress = (stress - 15);
            loneliness = (loneliness - 20);
            pride = (pride - 5);
            girlfriend = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    string LinaStatus()
    {
        string statusMessage;
        if (linaAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Lina brings you tasty snacks and comes over for a hangout.";
            linaLove = true;
            inLove = true;
            happiness = (happiness + 2);
            loneliness = (loneliness + 1);
        }
        else if (linaAttitude > 75)
        {
            statusMessage = "Lina plays video games with you for hours. It's a lot of fun!";
            happiness = (happiness + 1);
            loneliness = (loneliness + 1);
        }
        else if (linaAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Lina Skype calls with you for an hour before bed.";
            loneliness = (loneliness + 1);
        }
        else if (linaAttitude > 0)
        {
            statusMessage = "Lina texts you goodnight.";
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Lina isn't talking to you tonight.";
        }
        else
        {
            statusMessage = "Lina breaks up with you. You're heartbroken.";
            happiness = (happiness - 35);
            stress = (stress - 15);
            loneliness = (loneliness - 20);
            pride = (pride - 15);
            girlfriend = false;
            inLove = false;
            linaLove = false;
            girlfriendName = "none";
        }
        return statusMessage;
    }

    void RelationshipAttitudes()
    {
        if (knownWomen.Contains("Tooks") == true && flirtTooks == false)
        {
            Terminal.WriteLine("You didn't spend time with Tooks today. He is a sad bear.");
            tooksAttitude -= 1;
        }
		if (knownWomen.Contains("Lina") == true && flirted == true && dailyRand > 70 && flirtLina == false)
		{
			Terminal.WriteLine("Lina is jealous you spent time with someone else today.");
			linaAttitude = (linaAttitude - 4);
		}
        else if (knownWomen.Contains("Lina") == true && flirtLina == false)
        {
            Terminal.WriteLine("You didn't spend time with Lina today. She misses you some.");
            linaAttitude = (linaAttitude - 1);
        }
        if (knownWomen.Contains("Jenna") == true && flirtJenna == false)
        {
            Terminal.WriteLine("You didn't spend time with Jenna today. She misses you some.");
            jennaAttitude = (jennaAttitude - 1);
        }
		if (knownWomen.Contains("Alex") == true && flirted == true && dailyRand > 80 && flirtAlex == false)
		{
			Terminal.WriteLine("Alex is jealous you spent time with someone else today.");
			alexAttitude = (alexAttitude - 4);
		}
        else if (knownWomen.Contains("Alex") == true && flirtAlex == false)
        {
            Terminal.WriteLine("You didn't spend time with Alex today. She misses you some.");
            alexAttitude = (alexAttitude - 1);
        }
		if (knownWomen.Contains("Pina") == true && flirted == true && flirtPina == false)
		{
			Terminal.WriteLine("Pina is very jealous you spent time with someone else today.");
			pinaAttitude = (pinaAttitude - 8);
		}
        else if (knownWomen.Contains("Pina") == true && flirtPina == false)
        {
            Terminal.WriteLine("You didn't spend time with Pina today. She misses you.");
            pinaAttitude = (pinaAttitude - 2);
        }
        if (knownWomen.Contains("krissi") == true && flirtKrissi == false)
        {
            Terminal.WriteLine("You didn't spend time with Krissi today. She misses you.");
            krissiAttitude = (krissiAttitude - 1);
        }
    }

    void Absences() // Checks if the player showed up to work/school and fires them if they've missed too much
    {
        if (workedToday == false && workDay == true && employedYesterday == true)
        {
            workabsence++;
        }
        if (schooledToday == false && schoolDay == true)
        {
            schoolabsence++;
        }
        AddSpace();
        workedToday = false;
        schooledToday = false;
        if (workabsence > 5)
        {
            Terminal.WriteLine("You get fired for not showing up to work!");
            AddSpace();
            currentJob = Job.None;
            employed = false;
			workabsence = 0;
        }
        if (schoolabsence > 5)
        {
            Terminal.WriteLine("You get expelled for not showing up to university");
            AddSpace();
            school = false;
        }
    }

    void CheckDays() // Check and set whether each day is a school day/work day
    {
        if (turn%2 == 0)
        {
            schoolDay = true;
        }
        else if (turn%2 == 1)
        {
            schoolDay = false;
        }
        if (turn % 2 == 1)
        {
            workDay = true;
        }
        else if (turn % 2 == 0)
        {
            workDay = false;
        }
    }

    void ActionList() //List available actions to player                       
    {
        RefreshScreen();
        Terminal.WriteLine("You can do the following:");
        AddSpace();
        if (loneliness > -50)
        {
            Terminal.WriteLine("hobby");
            availableActions++;
        }
        else
        {
            Terminal.WriteLine("*You're too lonely to pursue a hobby*");
        }
        if (stress > -50)
        {
            Terminal.WriteLine("move");
            availableActions++;
        }
        else
        {
            Terminal.WriteLine("*You're too stressed to move*");
        }
        if (onlineToday == false)
        {
            Terminal.WriteLine("online");
            availableActions++;
        }
        else
        {
            Terminal.WriteLine("*Already went online today*");
            availableActions--;
        }
        if (friendToday == false)
        {
            Terminal.WriteLine("friend");
            availableActions++;
        }
        else
        {
            Terminal.WriteLine("*Already interacted with your friend today*");
            availableActions--;
        }
        if (stress > -50)
        {
            Terminal.WriteLine("jobsearch");
            availableActions++;
        }
        else if (dailyRand > 50)
        {
            Terminal.WriteLine("*You're too stressed to look for a job today.");
        }
        if (school == true && schoolDay == true && schooledToday == false && happiness >= -50)
        {
            Terminal.WriteLine("school");
            availableActions++;
        }
        else if (school == false)
        {
            Terminal.WriteLine("*You dropped out of school*");
        }
        else if (schooledToday == true && schoolDay == true)
        {
            Terminal.WriteLine("*You're done with school for today*");
            availableActions--;
        }
        else if (schoolDay == true)
        {
            Terminal.WriteLine("*You're too depressed to go to school today*");
        }
        else
        {
            Terminal.WriteLine("*No school today*");
        }
        if (knownWomen.Count > 0 && flirted == false && happiness > -50)
        {
            Terminal.WriteLine("flirt");
            availableActions++;
        }
        else if (happiness <= -50 && knownWomen.Count > 0 && flirted == false)
        {
            Terminal.WriteLine("*You're too sad to flirt today*");
        }
        else if (flirted == true)
        {
            Terminal.WriteLine("*You've had enough flirting for the day*");
            availableActions--;
        }
        else
        {
            Terminal.WriteLine("*You don't know anyone to flirt with*");
        }
        if ((girlfriend == true || knownWomen.Count > 0) && money > 3 && happiness > -50)
        {
            Terminal.WriteLine("date");
            availableActions++;
        }
        else if (money < 4)
        {
            Terminal.WriteLine("*You can't afford to go on a date*");
        }
        else if (happiness <= -50)
        {
            Terminal.WriteLine("*You are too depressed to go on a date*");
        }
        else
        {
            Terminal.WriteLine("*You don't know anyone to take on a date*");
        }
        if (employed == true  && workedToday == false && workDay == true && stress > -50)
        {
            Terminal.WriteLine("work");
            availableActions++;
        }
        else if (employed == false)
        {
            Terminal.WriteLine("*You don't have a job*");
        }
        else if (stress <= -50)
        {
            Terminal.WriteLine("*You're too stressed out to go to work*");
        }
        else if (workedToday == true && workDay == true)
        {
            Terminal.WriteLine("*You already worked today*");
            availableActions--;
        }
        else
        {
            Terminal.WriteLine("*You're not scheduled to work today*");
        }
        if (girlfriend == true)
        {
            Terminal.WriteLine("breakup");
            availableActions++;
        }
        else
        {
            Terminal.WriteLine("*You don't have anyone to break up with*");
        }
        AddSpace();                                                                                         
        if (availableActions == 0)                                                                           
        {
            RefreshScreen();
            Terminal.WriteLine("You have no available actions.");
            Terminal.WriteLine("Life has simply become too much with you, and you give in.");
            AddSpace();
            Terminal.WriteLine("Game Over!");
            AddSpace();
            Terminal.WriteLine("Type 'exit' to exit game");
            currentScreen = Screen.Gameover;
        }
    }

    void InputManager(string input)
    {
        if (input == "move" && stress > -50)
        {
            MoveWhere();
        }
        if (input == "hobby" && loneliness > -50)
        {
            Hobby();
            spoons--;
        }
        if (input == "online" && onlineToday == false)
        {
            Online();
            spoons--;
        }
        if (input == "friend" && friendToday == false)
        {
            Friends();
            spoons--;
        }
        if (input == "jobsearch" && stress > -50)
        {
            JobSearch(input);
            print("input passed to jobsearch from inputmanager");
            spoons--;
        }
        if (input == "breakup" && girlfriend == true)
        {
            BreakUp();
            spoons--;
        }
        if (input == "school" && school == true && schoolDay == true && schooledToday == false && happiness >= -50)
        {
            School();
            spoons--;
        }
        if (input == "flirt" && knownWomen.Count > 0 && flirted == false && happiness > -50)
        {
            Flirt();
            spoons--;
        }
        if (input == "date" && ((girlfriend == true || knownWomen.Count > 0) && money > 0) && happiness > -50)
        {
            Date();
            spoons--;
        }
        if (input == "work" && employed == true && workedToday == false && workDay == true && stress > -50)
        {
            Work();
            spoons--;
        }
        if (input == "move")
        {
            MoveWhere();
        }
        Tooltips(input);
        WomanInfo(input);
    }

    void BreakUp()
    {
        Terminal.WriteLine("You break up with " + girlfriendName);
        Terminal.WriteLine("Even though it was your decision it makes you feel terrible.");
        switch (girlfriendName)
        {
            case "Krissi":
                krissiAttitude = (krissiAttitude - 50);
                break;
            case "Sammy":
                sammyAttitude = (sammyAttitude - 50);
                break;
            case "Lina":
                linaAttitude = (linaAttitude - 50);
                break;
            case "Jenna":
                jennaAttitude = (jennaAttitude - 50);
                break;
            case "Alex":
                alexAttitude = (alexAttitude - 50);
                break;
            case "Pina":
                pinaAttitude = (pinaAttitude - 50);
                break;
        }
        inLove = false;
        krissiLove = false;
        sammyLove = false;
        linaLove = false;
        jennaLove = false;
        alexLove = false;
        pinaLove = false;
        happiness = (happiness - 15);
        stress = (stress - 15);
        loneliness = (loneliness - 20);
        girlfriend = false;
        girlfriendName = "none";
    }

    void WomanInfo(string input)
    {
        if (input == "!Tooks" && knownWomen.Contains("Tooks"))
        {
            Terminal.WriteLine("Tooks is a literal bear. Like a real bear.");
            Terminal.WriteLine("He is friendly, cuddly, and quite shy.");
            Terminal.WriteLine("He has bear fur and a bear body.");
            Terminal.WriteLine("He is quite chubby, as he is a bear.");
            Terminal.WriteLine("He is good at eating, sleeping, and playing.");
            Terminal.WriteLine("He enjoys being a bear.");
        }
        if (input == "!Lina" && knownWomen.Contains("Lina"))
            {
            Terminal.WriteLine("Lina is friendly, sweet, and talented.");
            Terminal.WriteLine("She is playful and quite nerdy.");
            Terminal.WriteLine("She has long light brown hair and a friendly smile.");
            Terminal.WriteLine("She's a little chubby and she's very short.");
            Terminal.WriteLine("She loves playing music, and is amazing at guitar and flute especially.");
            Terminal.WriteLine("She likes playing video games and going out.");
        }
        if (input == "!Jenna" && knownWomen.Contains("Jenna"))
        {
            Terminal.WriteLine("Jenna is bubbly, excitable, and gorgeous.");
            Terminal.WriteLine("She likes to have fun and is pretty cool.");
            Terminal.WriteLine("She has very long red hair and pale skin.");
            Terminal.WriteLine("She's skinny but curvy and very short.");
            Terminal.WriteLine("She's good at having fun and cuddling.");
            Terminal.WriteLine("She likes hanging out and doing whatever she can to relax.");
        }
        if (input == "!Alex" && knownWomen.Contains("Alex"))
        {
            Terminal.WriteLine("Alex is intelligent, pretty, and funny.");
            Terminal.WriteLine("She's gregarious and a huge nerd.");
            Terminal.WriteLine("She has long, dark brown hair and is kind of short.");
            Terminal.WriteLine("She's very skinny and beautiful.");
            Terminal.WriteLine("She's good at writing and telling jokes");
            Terminal.WriteLine("She enjoys going out, writing, and being around lots of people.");
        }
        if (input == "!Pina" && knownWomen.Contains("Pina"))
        {
            Terminal.WriteLine("Pina is brilliant, adorable, and friendly.");
            Terminal.WriteLine("She is sometimes reserved, but very cool");
            Terminal.WriteLine("She has short very dark hair and is very short.");
            Terminal.WriteLine("She is curvy and very pretty with an enchanting voice");
            Terminal.WriteLine("She knows many languages and has traveled the world");
            Terminal.WriteLine("She enjoys great food, deep conversations, and cuddling.");
        }
        if (input == "!Sammy" && knownWomen.Contains("Sammy"))
        {
            Terminal.WriteLine("Sammy is relaxed, smart, and wild.");
            Terminal.WriteLine("She is quiet and super cool.");
            Terminal.WriteLine("She has long black hair and is rather tall.");
            Terminal.WriteLine("She is skinny, pale, and has many tattoos.");
            Terminal.WriteLine("She's good at banter and dancing.");
            Terminal.WriteLine("She loves music and meeting guys.");
        }
        if (input == "!Krissi" && knownWomen.Contains("krissi"))
        {
            Terminal.WriteLine("Krissi is smart, bold, and honest.");
            Terminal.WriteLine("She is outgoing and very nerdy.");
            Terminal.WriteLine("She has thick brown hair and is super short.");
            Terminal.WriteLine("She is attractive and decisive.");
            Terminal.WriteLine("She's good at cooking, writing code, and singing.");
            Terminal.WriteLine("She likes gaming, music, and watching TV.");
        }
    }

    void Tooltips(string input)
    {
        if (input == "!home")
        {
            Terminal.WriteLine("Use home to return to the main menu");
        }
        if (input == "!move")
        {
            Terminal.WriteLine("Find somewhere new to live");
        }
        if (input == "!hobby")
        {
            Terminal.WriteLine("Do a hobby. Usually makes happiness, stress, and/or pride better.");
        }
        if (input == "!online" && onlineToday == false)
        {
            Terminal.WriteLine("Chat with friends online. Usually makes you happy and less lonely.");
        }
        if (input == "!friend" && friendToday == false)
        {
            Terminal.WriteLine("Talk to your friend. Usually makes you happy and less lonely.");
        }
        if (input == "!jobsearch")
        {
            Terminal.WriteLine("Look for a new job. Makes you proud but it's stressful.");
        }
        if (input == "!school" && school == true)
        {
            Terminal.WriteLine("Go to university for class. Makes you unhappy and stressed, but proud.");
        }
        if (input == "!flirt" && knownWomen.Count > 0)
        {
            Terminal.WriteLine("Flirt with a girl you know. Results may vary.");
        }
        if (input == "!date" && ((girlfriend == true || knownWomen.Count > 0) && money > 0))
        {
            Terminal.WriteLine("Go on a date with a girl you know- if she's up for it.");
        }
        if (input == "!work" && employed == true && workedToday == false)
        {
            Terminal.WriteLine("Go to work. Makes you proud, but it's probably stressful.");
        }
    }

    void MoveWhere()
    {
        currentScreen = Screen.Moving;
        RefreshScreen();
        if (currentHouse == House.None)
        {
            Terminal.WriteLine("You are homeless");
            AddSpace();
            if (parentsAttitude > 0 && school == true)
            {
                Terminal.WriteLine("You could move in with your parents         (type 'parents')");
            }
            else
            {
                Terminal.WriteLine("Your parents won't let you move back");
            }
            if (friendAttitude > 0)
            {
                Terminal.WriteLine("You could move in with your friend          (type 'friend')");
            }
            else
            {
                Terminal.WriteLine("Your friend doesn't want you staying there");
            }
            if (money > 10)
            {
                Terminal.WriteLine("For a $10 deposit you could rent your own place     (type 'rent')");
                Terminal.WriteLine("It would cost $3 per day");
            }
            else
            {
                Terminal.WriteLine("You can't afford your own place");
                Terminal.WriteLine("The deposit is $10");
            }
            AddSpace();
            Terminal.WriteLine("Type 'cancel' to go back to the main screen instead");
        }
        if (currentHouse == House.Parents)
        {
            Terminal.WriteLine("You live with your parents.");
            AddSpace();
            if (friendAttitude > 0)
            {
                Terminal.WriteLine("You could move in with your friend          (type 'friend')");
            }
            else
            {
                Terminal.WriteLine("Your friend doesn't want you staying there");
            }
            if (money > 10)
            {
                Terminal.WriteLine("For a $10 deposit you could rent your own place     (type 'rent')");
                Terminal.WriteLine("It would cost $3 per day");
            }
            else
            {
                Terminal.WriteLine("You can't afford your own place");
                Terminal.WriteLine("The deposit is $10");
            }
            Terminal.WriteLine("If you have no other choice, you could be homeless      (type 'homeless')");
            AddSpace();
            Terminal.WriteLine("Type 'cancel' to go back to the main screen instead");
        }
        if (currentHouse == House.Friend)
        {
            Terminal.WriteLine("You live with your friend.");
            AddSpace();
            if (parentsAttitude > 0 && school == true)
            {
                Terminal.WriteLine("You could move in with your parents         (type 'parents')");
            }
            else
            {
                Terminal.WriteLine("Your parents won't let you move back");
            }
            if (money > 10)
            {
                Terminal.WriteLine("For a $10 deposit you could rent your own place     (type 'rent')");
                Terminal.WriteLine("It would cost $3 per day");
            }
            else
            {
                Terminal.WriteLine("You can't afford your own place");
                Terminal.WriteLine("The deposit is $10");
            }
            Terminal.WriteLine("If you have no other choice, you could be homeless      (type 'homeless')");
            AddSpace();
            Terminal.WriteLine("Type 'cancel' to go back to the main screen instead");
        }
        if (currentHouse == House.Rent)
        {
            Terminal.WriteLine("You rent your own place.");
            AddSpace();
            if (parentsAttitude > 0 && school == true)
            {
                Terminal.WriteLine("You could move in with your parents         (type 'parents')");
            }
            else
            {
                Terminal.WriteLine("Your parents won't let you move back");
            }
            if (friendAttitude > 0)
            {
                Terminal.WriteLine("You could move in with your friend          (type 'friend')");
            }
            else
            {
                Terminal.WriteLine("Your friend doesn't want you staying there");
            }
            Terminal.WriteLine("If you have no other choice, you could be homeless      (type 'homeless')");
            AddSpace();
            Terminal.WriteLine("Type 'cancel' to go back to the main screen instead");
        }
        AddSpace();
        Terminal.WriteLine("Where would you like to move?");
    }

    void Moved()
    {
        RefreshScreen();
        currentScreen = Screen.Gameplay;
        Terminal.WriteLine("You moved!");
        Terminal.WriteLine("Enter 'home' to return to the menu.");
        spoons--;
    }

    void Work() // Work your job.
    {
        RefreshScreen();
        Terminal.WriteLine("You go to work...");
        loneliness  = (loneliness + 1);
        workedToday = true;
        if (currentJob == Job.DepartmentStore)
        {
            if (dailyRand < 5)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 1);
                earnings = 4;
            }
            else if (dailyRand < 20)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                stress = (stress - 2);
                happiness = (happiness - 5);
                pride = (pride - 1);
                earnings = 4;
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                stress = (stress - 1);
                happiness = (happiness - 1);
                pride = (pride + 1);
                earnings = 4;
            }
        }
        if (currentJob == Job.LumberYard)
        {
            if (dailyRand > 10 && dailyRand < 16)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 2);
                earnings = 8;
            }
            else if (dailyRand < 10)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                stress = (stress - 2);
                happiness = (happiness - 5);
                pride = (pride - 1);
                earnings = 8;
            }
            else if (dailyRand > 90)
            {
                Terminal.WriteLine("You have a good day at work.");
                stress = (stress + 1);
                happiness = (happiness + 1);
                pride = (pride + 4);
                earnings = 8;
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                stress = (stress - 1);
                happiness = (happiness - 1);
                pride = (pride + 3);
                earnings = 8;
            }
        }
        if (currentJob == Job.PaintStore)
        {
            if (dailyRand < 5)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 5);
                earnings = 6;
            }
            else if (dailyRand < 10)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                stress = (stress - 2);
                happiness = (happiness - 3);
                pride = (pride - 1);
                earnings = 6;
            }
            else if (dailyRand > 80)
            {
                Terminal.WriteLine("You have a good day at work.");
                stress = (stress + 1);
                happiness = (happiness + 1);
                pride = (pride + 7);
                earnings = 6;
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                stress = (stress - 1);
                earnings = 6;
            }
        }
        if (currentJob == Job.PizzaPlace)
        {
            if (dailyRand < 10)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                stress = (stress - 1);
                happiness = (happiness - 5);
                pride = (pride - 1);
                earnings = 2;
            }
            else if (dailyRand > 85)
            {
                Terminal.WriteLine("You make good tips at work.");
                happiness = (happiness + 1);
                pride = (pride + 1);
                earnings = 6;
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                stress = (stress - 1);
                happiness = (happiness - 1);
                pride = (pride + 1);
                earnings = 4;
            }
        }
        if (currentJob == Job.TravelingSales)
        {
            if (dailyRand < 5)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 5);
                earnings = 12;
            }
            else if (dailyRand < 15)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                stress = (stress - 5);
                happiness = (happiness - 10);
                pride = (pride - 5);
                earnings = 12;
            }
            else if (dailyRand > 95)
            {
                Terminal.WriteLine("You have a good day at work.");
                stress = (stress + 2);
                happiness = (happiness + 3);
                pride = (pride + 5);
                earnings = 12;
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                stress = (stress - 2);
                happiness = (happiness - 3);
                pride = (pride + 3);
                earnings = 12;
            }
        }
        if (currentJob == Job.GameCompany)
        {
            if (dailyRand < 2)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                stress = (stress - 1);
                happiness = (happiness - 1);
                pride = (pride - 1);
                earnings = 2;
            }
            else if (dailyRand > 95)
            {
                Terminal.WriteLine("You get a bonus at work.");
                stress = (stress + 1);
                happiness = (happiness + 3);
                pride = (pride + 3);
                earnings = 6;
                spoons++;
                Terminal.WriteLine("You don't even feel tired from working!");
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                happiness = (happiness + 1);
                pride = (pride + 2);
                earnings = 2;
                spoons++;
                Terminal.WriteLine("You don't even feel tired from working!");
            }
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    }

    void Date() 
    {
        currentScreen = Screen.Date;
        RefreshScreen();
        Terminal.WriteLine("Who would you to ask on a date?");
        if (knownWomen.Contains("Tooks") == true)
        {
            Terminal.WriteLine("Tooks");
        }
        if (knownWomen.Contains("Lina") == true)
        {
            Terminal.WriteLine("Lina");
        }
        if (knownWomen.Contains("Jenna") == true)
        {
            Terminal.WriteLine("Jenna");
        }
        if (knownWomen.Contains("Alex") == true)
        {
            Terminal.WriteLine("Alex");
        }
        if (knownWomen.Contains("Pina") == true)
        {
            Terminal.WriteLine("Pina");
        }
        if (knownWomen.Contains("Sammy") == true)
        {
            Terminal.WriteLine("Sammy");
        }
        if (knownWomen.Contains("krissi") == true)
        {
            Terminal.WriteLine("krissi");
        }
        AddSpace();
        Terminal.WriteLine("Chose someone or enter 'home' to go back.");
    }

    public string DateDecline()
    {
        RefreshScreen();
        string dateDecline;
        if (dateAttitude > 75)
            {
                dateDecline = "She says she's sorry, but she's busy tonight. Maybe another night soon?";
            }
        else if(dateAttitude > 50)
            {
                dateDecline = "She says she doesn't feel up for it tonight.";
            }
        else if (dateAttitude > 25)
            {
                dateDecline = "She doesn't feel like she knows you well enough";
            }
        else if (dateAttitude > 0)
            {
                dateDecline = "She says no.";
            }
        else
            {
            dateDecline = "She says no way!";
            }
        Terminal.WriteLine("Enter 'home' to return.");
        return dateDecline;
    }

    public string DateRandom()
    {
        RefreshScreen();
        string randomDate;
        if (dailyRand > 75)
        {
            randomDate = "You have kind of an awkward date. You're not sure why";
        }
        else if (dailyRand > 50)
        {
            randomDate = "You go out for a nice dinner and drinks afterwords and have a really good time.";
        }
        else if (dailyRand > 25)
        {
            randomDate = "You walk around downtown looking in shops and talking all afternoon. You have a good time!";
        }
        else
        {
            randomDate = "You stay home watching TV and cuddling all night. It's so much fun!";
        }
        Terminal.WriteLine("Enter 'home' to return.");
        return randomDate;
    }

    void DateInput(string input)
    {
        if (knownWomen.Contains("Tooks") == true && input == "Tooks")
        {
            RefreshScreen();
            Terminal.WriteLine("Tooks takes you on the best bear-date of your life.");
            flirted = true;
            flirtTooks = true;
            tooksAttitude += 10;
            loneliness += 10;
            happiness += 10;
            stress += 10;
            pride += 10;
            Terminal.WriteLine("Enter 'home' to return.");
        }
        if (knownWomen.Contains("Lina") == true && input == "Lina")
        {
            RefreshScreen();
            dateAttitude = linaAttitude;
            if (dailyRand + linaAttitude >= (linaDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtLina = true;
                if (dailyRand < 76)
                {
                    linaAttitude = (linaAttitude + 10);
                    loneliness = (loneliness + 5);
                    happiness = (happiness + 5);
                    stress = (stress - 1);
                    pride = (pride + 3);
                }
                else
                {
                    linaAttitude = (linaAttitude - 1);
                    loneliness = (loneliness + 1);
                    happiness = (happiness + 1);
                    stress = (stress - 1);
                    pride = (pride - 1);
                }
            }
            else
            {
                Terminal.WriteLine(DateDecline());
                loneliness = (loneliness - 2);
                happiness = (happiness - 2);
                stress = (stress - 2);
                pride = (pride - 2);
            }
        }
        if (knownWomen.Contains("Jenna") == true && input == "Jenna")
        {
            RefreshScreen();
            dateAttitude = jennaAttitude;
            if (dailyRand + jennaAttitude >= (jennaDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtJenna = true;
                if (dailyRand < 76)
                {
                    jennaAttitude = (jennaAttitude + 10);
                    loneliness = (loneliness + 5);
                    happiness = (happiness + 7);
                    stress = (stress - 2);
                    pride = (pride + 4);
                }
                else
                {
                    jennaAttitude = (jennaAttitude - 1);
                    loneliness = (loneliness + 1);
                    happiness = (happiness + 1);
                    stress = (stress - 1);
                    pride = (pride - 1);
                }
            }
            else
            {
                Terminal.WriteLine(DateDecline());
                loneliness = (loneliness - 2);
                happiness = (happiness - 2);
                stress = (stress - 2);
                pride = (pride - 2);
            }
        }
        if (knownWomen.Contains("Alex") == true && input == "Alex")
        {
            RefreshScreen();
            dateAttitude = alexAttitude;
            if (dailyRand + alexAttitude >= (alexDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtAlex = true;
                if (dailyRand < 76)
                {
                    alexAttitude = (alexAttitude + 10);
                    loneliness = (loneliness + 5);
                    happiness = (happiness + 3);
                    stress = (stress - 3);
                }
                else
                {
                    alexAttitude = (alexAttitude - 1);
                    loneliness = (loneliness + 1);
                    happiness = (happiness + 1);
                    stress = (stress - 1);
                    pride = (pride - 1);
                }
            }
            else
            {
                Terminal.WriteLine(DateDecline());
                loneliness = (loneliness - 2);
                happiness = (happiness - 1);
                stress = (stress - 1);
                pride = (pride - 1);
            }
        }
        if (knownWomen.Contains("Pina") == true && input == "Pina")
        {
            RefreshScreen();
            dateAttitude = pinaAttitude;
            if (dailyRand + pinaAttitude >= (pinaDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtPina = true;
                if (dailyRand < 76)
                {
                    pinaAttitude = (pinaAttitude + 10);
                    loneliness = (loneliness + 8);
                    happiness = (happiness + 8);
                    stress = (stress - 2);
                    pride = (pride + 7);
                }
                else
                {
                    pinaAttitude = (pinaAttitude - 1);
                    loneliness = (loneliness + 1);
                    happiness = (happiness + 1);
                    stress = (stress - 1);
                    pride = (pride - 1);
                }
            }
            else
            {
                Terminal.WriteLine(DateDecline());
                loneliness = (loneliness - 2);
                happiness = (happiness - 2);
                stress = (stress - 2);
                pride = (pride - 2);
            }
        }
        if (knownWomen.Contains("Sammy") == true && input == "Sammy")
        {
            RefreshScreen();
            dateAttitude = sammyAttitude;
            if (dailyRand + sammyAttitude >= (sammyDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtSammy = true;
                if (dailyRand < 76)
                {
                    sammyAttitude = (sammyAttitude + 10);
                    loneliness = (loneliness + 3);
                    happiness = (happiness + 3);
                }
                else
                {
                    sammyAttitude = (sammyAttitude - 1);
                    loneliness = (loneliness + 1);
                    happiness = (happiness + 1);
                    stress = (stress - 1);
                    pride = (pride - 1);
                }
            }
            else
            {
                Terminal.WriteLine(DateDecline());
                loneliness = (loneliness - 2);
                happiness = (happiness - 2);
                stress = (stress - 2);
                pride = (pride - 2);
            }
        }
        if (knownWomen.Contains("krissi") == true && input == "krissi")
        {
            RefreshScreen();
            dateAttitude = krissiAttitude;
            if (dailyRand + krissiAttitude >= (krissiDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtKrissi = true;
                if (dailyRand < 76)
                {
                    krissiAttitude = (krissiAttitude + 10);
                    loneliness = (loneliness + 10);
                    happiness = (happiness + 9);
                    stress = (stress + 3);
                    pride = (pride + 5);
                }
                else
                {
                    krissiAttitude = (krissiAttitude - 1);
                    loneliness = (loneliness + 1);
                }
            }
            else
            {
                Terminal.WriteLine(DateDecline());
                loneliness = (loneliness - 2);
                happiness = (happiness - 1);
                stress = (stress - 1);
                pride = (pride - 1);
            }
        }
    }

    void Flirt()
    {
        currentScreen = Screen.Flirt;
        RefreshScreen();
        Terminal.WriteLine("Who would you like to flirt with?");
        if (knownWomen.Contains("Lina") == true && flirtLina == false)
        {
            Terminal.WriteLine("Lina");
        }
        if (knownWomen.Contains("Jenna") == true && flirtJenna == false)
        {
            Terminal.WriteLine("Jenna");
        }
        if (knownWomen.Contains("Alex") == true && flirtAlex == false)
        {
            Terminal.WriteLine("Alex");
        }
        if (knownWomen.Contains("Pina") == true && flirtPina == false)
        {
            Terminal.WriteLine("Pina");
        }
        if (knownWomen.Contains("Sammy") == true && flirtSammy == false)
        {
            Terminal.WriteLine("Sammy");
        }
        if (knownWomen.Contains("krissi") == true && flirtKrissi == false)
        {
            Terminal.WriteLine("krissi");
        }
        if (knownWomen.Contains("Tooks") == true && flirtTooks == false)
        {
            Terminal.WriteLine("Tooks");
        }
        AddSpace();
        Terminal.WriteLine("Chose someone or enter 'home' to go back.");
    }

    void FlirtInput(string input)
    {
        if (knownWomen.Contains("Tooks") == true && input == "Tooks")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Tooks?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            TooksFlirt(input);
            happiness = (happiness + 5);
            stress = (stress + 1);
            pride = (pride + 1);
            loneliness = (loneliness + 4);
            flirtTooks = true;
            flirted = true;
        }
        if (knownWomen.Contains("Lina") == true && input == "Lina")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Lina?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            LinaFlirt(input);
            happiness = (happiness + 5);
            stress = (stress + 1);
            pride = (pride + 1);
            loneliness = (loneliness + 4);
            flirtLina = true;
            flirted = true;
        }
        if (knownWomen.Contains("Jenna") == true && input == "Jenna")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Jenna?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            JennaFlirt(input);
            happiness = (happiness + 5);
            stress = (stress + 1);
            loneliness = (loneliness + 6);
            flirtJenna = true;
            flirted = true;
        }
        if (knownWomen.Contains("Alex") == true && input == "Alex")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Alex?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            AlexFlirt(input);
            happiness = (happiness + 7);
            stress = (stress - 2);
            pride = (pride - 3);
            loneliness = (loneliness + 6);
            flirtAlex = true;
            flirted = true;
        }
        if (knownWomen.Contains("Pina") == true && input == "Pina")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Pina?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            PinaFlirt(input);
            happiness = (happiness + 5);
            stress = (stress + 4);
            pride = (pride + 4);
            loneliness = (loneliness + 6);
            flirtPina = true;
            flirted = true;
        }
        if (knownWomen.Contains("Sammy") == true && input == "Sammy")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Sammy?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            SammyFlirt(input);
            happiness = (happiness + 1);
            stress = (stress - 1);
            pride = (pride - 1);
            loneliness = (loneliness + 2);
            flirtSammy = true;
            flirted = true;
        }
        if (knownWomen.Contains("krissi") == true && input == "krissi")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with krissi?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            krissiFlirt(input);
            happiness = (happiness + 5);
            stress = (stress + 5);
            pride = (pride + 5);
            loneliness = (loneliness + 8);
            flirtKrissi = true;
            flirted = true;
        }
    }

    public string RandomGift()
    {
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "You write her a poem. It's kinda mushy, but well written. She likes it a lot!";
            giftVar = 4;
        }
        else if (dailyRand > 50)
        {
            randomLine = "You surprise her with a boquet of her favorite flowers. She loves them!";
            giftVar = 3;
        }
        else if (dailyRand > 25)
        {
            randomLine = "You get her some jewelry you think she'd like. She thanks you profusely.";
            giftVar = 2;
        }
        else
        {
            randomLine = "You get her a box of her favorite candy. She smiles and shares it with you.";
        }
        return randomLine;
    }

    public string RandomSweet()
    {
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "You tell her you love spending time with her. She beams.";
        }
        else if (dailyRand > 50)
        {
            randomLine = "You tell her you think she's beautiful. She blushes.";
        }
        else if (dailyRand > 25)
        {
            randomLine = "You tell her she's hilarious. She makes a joke! It's funny!";
        }
        else
        {
            randomLine = "You compliment your favorite part of her personality. You can tell it means a lot to her.";
        }
        return randomLine;
    }

    public string RandomJoke()
    {
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "You trade jokes that are so funny you're both crying laughing.";
        }
        else if (dailyRand > 50)
        {
            randomLine = "You tell a joke off a popsicle stick. It's stupid but funny.";
        }
        else if (dailyRand > 25)
        {
            randomLine = "You make a joke about the situation you're in. She laughs.";
        }
        else
        {
            randomLine = "You make a terrible pun. She still laughs, though.";
        }
        return randomLine;
    }

    public string RandomAskOut()
    {
        string randomLine;
        if (dailyRand > 75)
        {
            randomLine = "You cook her a delicious dinner and ask her out afterwords.";
        }
        else if (dailyRand > 50)
        {
            randomLine = "You bring her a boquet of her favorite flowers and ask her out.";
        }
        else if (dailyRand > 25)
        {
            randomLine = "You're hanging out having a good time, so you just ask her out on the spot.";
        }
        else
        {
            randomLine = "You swing by her place and ask her out.";
        }
        return randomLine;
    }

    void TooksFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        if (input == "gift")
        {
            Terminal.WriteLine("You give Tooks some honey. He seems indifferent. But takes it.");
            tooksAttitude += tooksGiftMod;
        }
        else if (input == "joke")
        {
            Terminal.WriteLine("You tell tooks a joke: \n How do you catch fish without a fishing rod? \n 'With your bear hands' \n The universe groans in agony but Tooks is delighted.");
            tooksAttitude += tooksJokeMod;
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine("You tell Tooks he's the bestest bear ever. He gives you a bear grin.");
            tooksAttitude += tooksSweetMod;
        }
        else if (input == "relationship")
        {
            if (girlfriend == true)
            {
                Terminal.WriteLine("You have a girlfriend! You'd have to break up with her first!");
                return;
            }
            else
            {
                Terminal.WriteLine("You ask Tooks out. He wraps you in a bear hug. You think that means yes.");
                girlfriend = true;
                girlfriendName = "Tooks";
                Invoke("MainScreen", 5f);
                tooksAttitude += 10;
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void krissiFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.Flirtkrissi;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            krissiAttitude = (krissiAttitude + krissiGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            krissiAttitude = (krissiAttitude + krissiJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            krissiAttitude = (krissiAttitude + krissiSweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + krissiAttitude >= krissiDifficulty)
            {
                Terminal.WriteLine("She says she would love to be your girlfriend and wraps you in a huge hug.");
                girlfriend = true;
                girlfriendName = "krissi";
                Invoke("MainScreen", 5f);
                krissiAttitude = (krissiAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says you're sweet, but she doesn't think she knows you well enough quite yet.");
                Invoke("MainScreen", 5f);
                loneliness = (loneliness - 4);
                happiness = (happiness - 1);
                stress = (stress - 1);
                pride = (pride - 1);
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void SammyFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.FlirtSammy;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            sammyAttitude = (sammyAttitude + sammyGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            sammyAttitude = (sammyAttitude + sammyJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            sammyAttitude = (sammyAttitude + sammySweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + sammyAttitude >= sammyDifficulty)
            {
                Terminal.WriteLine("She shrugs and agrees to be your girlfriend. She kisses you, and seems a lot more interested in that.");
                girlfriend = true;
                girlfriendName = "Sammy";
                Invoke("MainScreen", 5f);
                sammyAttitude = (sammyAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She shrugs and tells you she's fine with your current relationship.");
                Invoke("MainScreen", 5f);
                loneliness = (loneliness - 4);
                happiness = (happiness - 4);
                stress = (stress - 4);
                pride = (pride - 4);
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void PinaFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.FlirtPina;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            pinaAttitude = (pinaAttitude + pinaGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            pinaAttitude = (pinaAttitude + pinaJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            pinaAttitude = (pinaAttitude + pinaSweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + pinaAttitude >= pinaDifficulty)
            {
                Terminal.WriteLine("She says she doesn't like you... but smiles and stays the night at your place.");
                girlfriend = true;
                girlfriendName = "Pina";
                Invoke("MainScreen", 5f);
                pinaAttitude = (pinaAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She asks who would like you, and doesn't talk to you anymore that day.");
                Invoke("MainScreen", 5f);
                loneliness = (loneliness - 4);
                happiness = (happiness - 4);
                stress = (stress - 4);
                pride = (pride - 4);
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void AlexFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.FlirtAlex;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            alexAttitude = (alexAttitude + alexGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            alexAttitude = (alexAttitude + alexJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            alexAttitude = (alexAttitude + alexSweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + alexAttitude >= alexDifficulty)
            {
                Terminal.WriteLine("She ignores your question, but pulls you to her bedroom and closes the door.");
                girlfriend = true;
                girlfriendName = "Alex";
                Invoke("MainScreen", 5f);
                alexAttitude = (alexAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says she doesn't really know what she's looking for right now, but that she's flattered.");
                Invoke("MainScreen", 5f);
                loneliness = (loneliness - 4);
                happiness = (happiness - 2);
                stress = (stress - 2);
                pride = (pride - 2);
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void JennaFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.FlirtJenna;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            jennaAttitude = (jennaAttitude + jennaGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            jennaAttitude = (jennaAttitude + jennaJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            jennaAttitude = (jennaAttitude + jennaSweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + jennaAttitude >= jennaDifficulty)
            {
                Terminal.WriteLine("She gives you a big smile, and you spend all night cuddling and talking together.");
                girlfriend = true;
                girlfriendName = "Jenna";
                Invoke("MainScreen", 5f);
                jennaAttitude = (jennaAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says she's not quite ready for that yet.");
                Invoke("MainScreen", 5f);
                loneliness = (loneliness - 4);
                happiness = (happiness - 4);
                stress = (stress - 4);
                pride = (pride - 4);
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void LinaFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.FlirtLina;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            linaAttitude = (linaAttitude + linaGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            linaAttitude = (linaAttitude + linaJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            linaAttitude = (linaAttitude + linaSweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + linaAttitude >= linaDifficulty)
            {
                Terminal.WriteLine("She says yes, and you spend the day gaming together. What a blast!");
                girlfriend = true;
                girlfriendName = "Lina";
                Invoke("MainScreen", 5f);
                linaAttitude = (linaAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says she's just not happy enough right now for that.");
                Invoke("MainScreen", 5f);
                loneliness = (loneliness - 4);
                happiness = (happiness - 4);
                stress = (stress - 4);
                pride = (pride - 4);
            }
        }
        else if (input == "home")
        {
            MainScreen();
        }
        AddSpace();
        Terminal.WriteLine("Enter 'home' to go to the main screen.");
    }

    void MeetGirl ()
    {
        RefreshScreen();
        if (unknownWomen.Count > 0)
        {
            womenRand = UnityEngine.Random.Range(0, unknownWomen.Count);
            Terminal.WriteLine("You meet " + (string)unknownWomen[womenRand]);
            string tempName;
            tempName = (string)unknownWomen[womenRand];
            AddSpace();
            Introductions(tempName);
            Terminal.WriteLine("To learn more about her any time enter !" + (string)unknownWomen[womenRand]);
            AddSpace();
            knownWomen.Add((string)unknownWomen[womenRand]);
            unknownWomen.Remove((string)unknownWomen[womenRand]);
            currentScreen = Screen.Gameplay;
        }
        else
            return;
    }
    
    void Introductions(string tempName)
    {
        if (tempName == "Lina")
        {
            Terminal.WriteLine("A friend introduces you to a girl named Lina. She gives you her number!");
            Terminal.WriteLine("She seems really friendly!");
        }
        if (tempName == "Jenna")
        {
            Terminal.WriteLine("You get to know Jenna at her job and really hit it off. She adds you on Snapchat!");
            Terminal.WriteLine("She seems kinda into you!");
        }
        if (tempName == "Alex")
        {
            Terminal.WriteLine("While you have a bit of downtime you peruse a dating app and trade numbers with a girl named Alex!");
            Terminal.WriteLine("She seems really interesting!");
        }
        if (tempName == "Pina")
        {
            Terminal.WriteLine("You meet a girl named Pina completely by chance. She's really sweet and gives you her number.");
            Terminal.WriteLine("She seems fascinating!");
        }
        if (tempName == "Sammy")
        {
            Terminal.WriteLine("You get a phone call, but it's a wrong number. A girl named Sammy on the other end talks to you for a while.");
            Terminal.WriteLine("She seems bored, and tells you to call her sometime!");
        }
        if (tempName == "krissi")
        {
            Terminal.WriteLine("You go out and meet a girl in your favorite aisle of the old bookstore. You hit it off immediately!");
            Terminal.WriteLine("She's charming and excited to talk to you!");
        }
    }

    void School()
    {
        RefreshScreen();
        Terminal.WriteLine("You go to university...");
        schooledToday = true;
        loneliness = (loneliness + 1);
        if (dailyRand > 83 && dailyRand < 90)
        {
            MeetGirl();
        }
        else if (dailyRand > 87)
        {
            Terminal.WriteLine("You ace a test. Nice! Didn't even study!");
            stress--;
            pride = (pride + 5);
            happiness = (happiness + 5);
        }
        else if (dailyRand > 70)
        {
            Terminal.WriteLine("You have an okay day at school.");
            stress--;
            pride = (pride + 3);
        }
        else if (dailyRand > 50)
        {
            Terminal.WriteLine("You're frustrated with the pace of school. Everything goes so slow...");
            stress = (stress - 3);
            pride = (pride + 1);
            happiness = (happiness - 2);
        }
        else if (dailyRand > 1)
        {
            Terminal.WriteLine("You hate everything to do with school. Why are you even here?");
            stress = (stress - 5);
            pride = (pride + 1);
            happiness = (happiness - 4);
        }
        else if ((dailyRand == 1) && (stress < 0))
        {
            Terminal.WriteLine("Fuck school, you quit.");
            stress = (stress + 10);
            pride = (pride - 10);
            happiness = (happiness + 5);
        }
        else if (dailyRand == 1)
        {
            Terminal.WriteLine("You daydream about quitting all through class.");
            stress = (stress + 2);
            pride = (pride - 1);
            happiness = (happiness + 1);
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    }

    void JobSearch(string input)
    {
        RefreshScreen();
        dailyRand = UnityEngine.Random.Range(1, 101);
        Terminal.WriteLine("You search for a job...");
        if (dailyRand == 100)
        {
            if (employed == false)
            {
                AddSpace();
                Terminal.WriteLine("You get a job at a Paint Store! You're on the schedule soon!");
                Terminal.WriteLine("It pays 6$ per work day.");
                happiness = (happiness + 10);
                employed = true;
                currentJob = Job.PaintStore;
            }
            else if (employed == true)
            {
                currentScreen = Screen.JobQuery;
                AddSpace();
                Terminal.WriteLine("You get offered a job at a Paint Store! Quit your curren job and take this one?");
                Terminal.WriteLine("It pays 6$ per work day.");
                Terminal.WriteLine("Enter 'yes' or 'no'.");
                if (input == "yes")
                {
                    Terminal.WriteLine("You take the job and quit your current one!");
                    happiness = (happiness + 10);
                    employed = true;
                    currentJob = Job.PaintStore;
                    currentScreen = Screen.Gameplay;
                    Invoke("MainScreen", 1f);
                }
                else if (input == "no")
                {
                    Terminal.WriteLine("You decline the job.");
                    Invoke("MainScreen", 1f);
                    currentScreen = Screen.Gameplay;
                }
            }
        }
        else if (dailyRand > 90 && dailyRand <= 93)
        {
            if (employed == false)
            {
                AddSpace();
                Terminal.WriteLine("You get a job at a Department Store! You're on the schedule soon!");
                Terminal.WriteLine("It pays 4$ per work day.");
                happiness = (happiness + 5);
                employed = true;
                currentJob = Job.DepartmentStore;
            }
            else if (employed == true)
            {
                currentScreen = Screen.JobQuery;
                AddSpace();
                Terminal.WriteLine("You get offered a job at a Department Store! Quit your curren job and take this one?");
                Terminal.WriteLine("It pays 4$ per work day.");
                Terminal.WriteLine("Enter 'yes' or 'no'.");
                if (input == "yes")
                {
                    Terminal.WriteLine("You take the job and quit your current one!");
                    happiness = (happiness + 5);
                    employed = true;
                    currentJob = Job.DepartmentStore;
                    currentScreen = Screen.Gameplay;
                    Invoke("MainScreen", 1f);
                }
                else if (input == "no")
                {
                    Terminal.WriteLine("You decline the job.");
                    Invoke("MainScreen", 1f);
                }
            }
        }
        else if (dailyRand > 96 && dailyRand <= 98)
        {
            if (employed == false)
            {
                AddSpace();
                Terminal.WriteLine("You get a job at a Lumber Yard! You're on the schedule soon!");
                Terminal.WriteLine("It pays 8$ per work day.");
                happiness = (happiness + 7);
                employed = true;
                currentJob = Job.LumberYard;
            }
            else if (employed == true)
            {
                currentScreen = Screen.JobQuery;
                AddSpace();
                Terminal.WriteLine("You get offered a job at a Lumber Yard! Quit your curren job and take this one?");
                Terminal.WriteLine("It pays 8$ per work day.");
                Terminal.WriteLine("Enter 'yes' or 'no'.");
                if (input == "yes")
                {
                    Terminal.WriteLine("You take the job and quit your current one!");
                    happiness = (happiness + 7);
                    employed = true;
                    currentJob = Job.LumberYard;
                    currentScreen = Screen.Gameplay;
                    Invoke("MainScreen", 1f);
                }
                else if (input == "no")
                {
                    Terminal.WriteLine("You decline the job.");
                    Invoke("MainScreen", 1f);
                }
            }
        }
        else if (dailyRand >93 && dailyRand <= 95)
        {
            if (employed == false)
            {
                AddSpace();
                Terminal.WriteLine("You get a job at a Pizza Place! You're on the schedule soon!");
                Terminal.WriteLine("It pays 2$ per work day usually.");
                happiness = (happiness + 7);
                employed = true;
                currentJob = Job.PizzaPlace;
            }
            else if (employed == true)
            {
                currentScreen = Screen.JobQuery;
                AddSpace();
                Terminal.WriteLine("You get offered a job at a Pizza Place! Quit your curren job and take this one?");
                Terminal.WriteLine("It pays 2$ per work day usually.");
                Terminal.WriteLine("Enter 'yes' or 'no'.");
                if (input == "yes")
                {
                    Terminal.WriteLine("You take the job and quit your current one!");
                    happiness = (happiness + 7);
                    employed = true;
                    currentJob = Job.PizzaPlace;
                    currentScreen = Screen.Gameplay;
                    Invoke("MainScreen", 1f);
                }
                else if (input == "no")
                {
                    Terminal.WriteLine("You decline the job.");
                    Invoke("MainScreen", 1f);
                }
            }
        }
        else if (dailyRand == 96)
        {
            if (employed == false)
            {
                AddSpace();
                Terminal.WriteLine("You get a job as a Traveling Salesman! You're on the schedule soon!");
                Terminal.WriteLine("It pays 12$ per work day.");
                happiness = (happiness + 10);
                employed = true;
                currentJob = Job.TravelingSales;
            }
            else if (employed == true)
            {
                currentScreen = Screen.JobQuery;
                AddSpace();
                Terminal.WriteLine("You get offered a job as a Traveling Salesman! Quit your curren job and take this one?");
                Terminal.WriteLine("It pays 12$ per work day.");
                Terminal.WriteLine("Enter 'yes' or 'no'.");
                if (input == "yes")
                {
                    Terminal.WriteLine("You take the job and quit your current one!");
                    happiness = (happiness + 10);
                    employed = true;
                    currentJob = Job.TravelingSales;
                    currentScreen = Screen.Gameplay;
                    Invoke("MainScreen", 1f);
                }
                else if (input == "no")
                {
                    Terminal.WriteLine("You decline the job.");
                    Invoke("MainScreen", 1f);
                }
            }
        }
        else if (dailyRand == 99)
        {
            if (employed == false)
            {
                AddSpace();
                Terminal.WriteLine("You get a job at a Game Company! You're on the schedule soon!");
                Terminal.WriteLine("It pays 2$ per work day.");
                happiness = (happiness + 15);
                employed = true;
                currentJob = Job.GameCompany;
            }
            else if (employed == true)
            {
                currentScreen = Screen.JobQuery;
                AddSpace();
                Terminal.WriteLine("You get offered a job at a Game Company! Quit your curren job and take this one?");
                Terminal.WriteLine("It pays 2$ per work day.");
                Terminal.WriteLine("Enter 'yes' or 'no'.");
                if (input == "yes")
                {
                    Terminal.WriteLine("You take the job and quit your current one!");
                    happiness = (happiness + 15);
                    employed = true;
                    currentJob = Job.GameCompany;
                    currentScreen = Screen.Gameplay;
                    Invoke("MainScreen", 1f);
                }
                else if (input == "no")
                {
                    Terminal.WriteLine("You decline the job.");
                    Invoke("MainScreen", 1f);
                }
            }
        }
        else if (dailyRand <= 90)
        {
            AddSpace();
            Terminal.WriteLine("You don't have any luck, though...");
            happiness--;
            stress--;
            pride++;
            currentScreen = Screen.Gameplay;
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    }

    void Friends()
    {
        friendToday = true;
        RefreshScreen();
        if (friendAttitude < 1)
        {
            Terminal.WriteLine("Your friend doesn't want to talk to you today.");
            friendAttitude++;
        }
        else if (friendAttitude + dailyRand > 190)
        {
            FriendWin();
        }
        else if (friendAttitude + dailyRand > 150)
        {
            Terminal.WriteLine("You have a lot of fun with your friend! You feel good.");
            stress = (stress + 5);
            happiness = (happiness + 15);
            pride = (pride + 7);
            loneliness = (loneliness + 2);
        }
        else if (friendAttitude + dailyRand > 100)
        {
            Terminal.WriteLine("You have a great conversation with your friend. That was nice.");
            happiness = (happiness + 10);
            pride = (pride + 5);
            loneliness = (loneliness + 2);
        }
        else if (friendAttitude + dailyRand > 75)
        {
            Terminal.WriteLine("You share silly texts with your friend. Fun.");
            happiness = (happiness + 6);
            loneliness = (loneliness + 2);
        }
        else if (friendAttitude + dailyRand > 50)
        {
            Terminal.WriteLine("You talk on the phone with your friend for a while");
            happiness = (happiness + 3);
            loneliness = (loneliness + 2);
        }
        else
        {
            Terminal.WriteLine("You argue with your friend. Bummer.");
            happiness = (happiness - 1);
            friendAttitude = (friendAttitude - 3);
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    }

    void Online()
    {
        onlineToday = true;
        RefreshScreen();
        if (onlineAttitude < 1)
        {
            Terminal.WriteLine("Your online clan doesn't want to talk to you today.");
            onlineAttitude++;
        }
        else if (dailyRand > 70 && dailyRand < 75)
        {
            MeetGirl();
        }
        else if (onlineAttitude + dailyRand > 190)
        {
            Terminal.WriteLine("You play a game with your friends, and you win all night working together. Awesome! You feel like a hero!");
                stress = (stress + 10);
                happiness = (happiness + 20);
                pride = (pride + 10);
            loneliness = (loneliness + 1);
        }
        else if (onlineAttitude + dailyRand > 150)
        {
            Terminal.WriteLine("You talk in Discord voice chat for hours. It was a blast!");
            stress = (stress + 3);
            happiness = (happiness + 10);
            pride = (pride + 3);
            loneliness = (loneliness + 1);
        }
        else if (onlineAttitude + dailyRand > 100)
        {
            Terminal.WriteLine("You live stream gaming with some of your friends. Cool.");
            happiness = (happiness + 6);
            pride = (pride + 1);
            loneliness = (loneliness + 1);
        }
        else if (onlineAttitude + dailyRand > 75)
        {
            Terminal.WriteLine("You share some memes with your clan. Haha.");
            happiness = (happiness + 3);
            loneliness = (loneliness + 1);
        }
        else if (onlineAttitude + dailyRand > 50)
        {
            Terminal.WriteLine("You chat in Discord for a while.");
            happiness = (happiness + 2);
            loneliness = (loneliness + 1);
        }
        else
        {
            Terminal.WriteLine("You argue with your friends. Damn.");
            happiness = (happiness - 1);
            onlineAttitude = (onlineAttitude - 3);
            loneliness = (loneliness - 1);
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    }

    void Hobby()  
    {
        dailyRand = UnityEngine.Random.Range(1, 101);
        RefreshScreen();
        loneliness = (loneliness - 1);
        if (dailyRand > 45 && dailyRand < 55){
            Terminal.WriteLine("You go out to the store, and meet someone interesting while you're out!");
            Invoke("MeetGirl", 3f);
        }
        else if (dailyRand > 90)
        {
            Terminal.WriteLine("You read a book. You feel edified!");
            stress = (stress + 2);
            happiness = (happiness + 10);
            pride = (pride + 5);
        }
        else if (dailyRand > 80)
        {
            Terminal.WriteLine("You peruse Wikipedia and learn something new. You feel smarter!");
            stress = (stress + 1);
            happiness = (happiness + 7);
            pride = (pride + 1);
        }
        else if (dailyRand > 70)
        {
            Terminal.WriteLine("You cook a home cooked meal. It was delicious!");
            stress = (stress + 1);
            happiness = (happiness + 6);
            pride = (pride + 4);
        }
        else if (dailyRand > 60)
        {
            Terminal.WriteLine("You go for a bike ride. Feels good to stretch for once.");
            stress = (stress + 2);
            happiness = (happiness + 3);
            pride = (pride + 4);
        }
        else if(dailyRand > 50)
        {
            Terminal.WriteLine("You make some delightful tea. Yum!");
            stress = (stress + 4);
            happiness = (happiness + 2);
            pride = (pride + 0);
        }
        else if(dailyRand > 40)
        {
            Terminal.WriteLine("You play some games. It was fun.");
            stress = (stress + 1);
            happiness = (happiness + 2);
            pride = (pride + 0);
        }
        else if(dailyRand > 30)
        {
            Terminal.WriteLine("You zonk out and watch TV for a while.");
            stress = (stress + 0);
            happiness = (happiness + 2);
            pride = (pride + 0);
        }
        else if(dailyRand > 20)
        {
            Terminal.WriteLine("You have a few drinks at home to take the edge off.");
            stress = (stress - 1);
            happiness = (happiness + 4);
            pride = (pride - 1);
        }
        else if(dailyRand > 10)
        {
            Terminal.WriteLine("You meditate. It clears your mind. You feel less stressed.");
            stress = (stress + 7);
            happiness = (happiness + 0);
            pride = (pride + 1);
        }
        else
        {
            Terminal.WriteLine("You can't think of anything to do. You're bored.");
            stress = (stress + 0);
            happiness = (happiness + 0);
            pride = (pride + 0);
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    }

    void RefreshScreen() 
    {
        Terminal.ClearScreen();
        AddSpace();
        Terminal.WriteLine("Turn: " + turn + "              Current Money: " + money +"$              Current Spoons: " + spoons);
		Terminal.WriteLine("        Hap: "+HappyMeter() +"    "+ "Strs: "+StressMeter() +"    "+ "Prd: "+PrideMeter() +"    "+ "Lnly: "+LonelyMeter());
        Terminal.WriteLine("------------------------------------------------------------------------");
        AddSpace();
    }

	public string LonelyMeter()
	{
		string lonelyMeter;
		if (loneliness > 74)
		{
			lonelyMeter = "││││";
		}
		else if (loneliness > 49)
		{
			lonelyMeter = "│││∣ ";
		}
		else if (loneliness > 24)
		{
			lonelyMeter = "│││·";
		}
		else if (loneliness > 5)
		{
			lonelyMeter = "││∣·";
		}
		else if (loneliness > -6 && loneliness < 6)
		{
			lonelyMeter = "││··";
		}
		else if (loneliness > -26)
		{
			lonelyMeter = "│∣··";
		}
		else if (loneliness > -51)
		{
			lonelyMeter = "│···";
		}
		else if (loneliness > -76)
		{
			lonelyMeter = "∣···";
		}
		else
		{
			lonelyMeter = "····";
		}
		return lonelyMeter;
	}

	public string HappyMeter()
	{
		string happyMeter;
		if (happiness > 74)
		{
			happyMeter = "││││";
		}
		else if (happiness > 49)
		{
			happyMeter = "│││∣";
		}
		else if (happiness > 24)
		{
			happyMeter = "│││·";
		}
		else if (happiness > 5)
		{
			happyMeter = "││∣·";
		}
		else if (happiness > -6 && happiness < 6)
		{
			happyMeter = "││··";
		}
		else if (happiness > -26)
		{
			happyMeter = "│∣··";
		}
		else if (happiness > -51)
		{
			happyMeter = "│···";
		}
		else if (happiness > -76)
		{
			happyMeter = "∣···";
		}
		else
		{
			happyMeter = "····";
		}
		return happyMeter;
	}

	public string StressMeter()
	{
		string stressMeter;
		if (stress > 74)
		{
			stressMeter = "││││";
		}
		else if (stress > 49)
		{
			stressMeter = "│││∣";
		}
		else if (stress > 24)
		{
			stressMeter = "│││·";
		}
		else if (stress > 5)
		{
			stressMeter = "││∣·";
		}
		else if (stress > -6 && stress < 6)
		{
			stressMeter = "││··";
		}
		else if (stress > -26)
		{
			stressMeter = "│∣··";
		}
		else if (stress > -51)
		{
			stressMeter = "│···";
		}
		else if (stress > -76)
		{
			stressMeter = "∣···";
		}
		else
		{
			stressMeter = "····";
		}
		return stressMeter;
	}

	public string PrideMeter()
	{
		string prideMeter;
		if (pride > 74)
		{
			prideMeter = "││││";
		}
		else if (pride > 49)
		{
			prideMeter = "│││∣";
		}
		else if (pride > 24)
		{
			prideMeter = "│││·";
		}
		else if (pride > 5)
		{
			prideMeter = "││∣·";
		}
		else if (pride > -6 && pride < 6)
		{
			prideMeter = "││··";
		}
		else if (pride > -26)
		{
			prideMeter = "│∣··";
		}
		else if (pride > -51)
		{
			prideMeter = "│···";
		}
		else if (pride > -76)
		{
			prideMeter = "∣···";
		}
		else
		{
			prideMeter = "····";
		}
		return prideMeter;
	}

    void AddSpace() //Quick Method for adding spaces
    {
        Terminal.WriteLine("");
    }

    void FriendWin() // Win with perfect attitude towards friend. Secret ending! Implement me plox.
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(KeyCode.Escape)) // Exit game on Escape
        {
            Application.Quit();
        }
	}

    void RNGChecker()
    {
        //FIXME eventRand = UnityEngine.Random.Range(0, 45);
        if (eventRand == 11 && school == false) //check conditional events, if false reroll for an applicable event 
        {
            RNGChecker(); // reroll
        }
        else if (eventRand == 51 && employed == false)
        {
            RNGChecker(); // reroll
        }
        else if (eventRand == 45 && employed == false)
        {
            RNGChecker(); // reroll
        }
        else if (eventRand == 80 && employed == false)
        {
            RNGChecker(); // reroll
        }
        else if (eventRand == 81 && currentHouse != House.Rent)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 82 && employed == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 83 && employed == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 84 && employed == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 85 && girlfriend == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 86 && currentHouse != House.Rent)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 87 && girlfriend == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 88 && employed == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 89 && employed == false)
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 90 && employed == false)
        {
            RNGChecker(); //reroll  
        }
        else if (eventRand == 91 && girlfriendName != "Lina")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 92 && girlfriendName != "Jenna")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 93 && girlfriendName != "Jenna")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 94 && girlfriendName != "Alex")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 95 && girlfriendName != "Pina")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 96 && girlfriendName != "Alex")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 97 && girlfriendName != "Pina")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 98 && girlfriendName != "Sammy")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 99 && girlfriendName != "Krissi")
        {
            RNGChecker(); //reroll
        }
        else if (eventRand == 100 && (school == false || turn <= 10))
        {
            RNGChecker(); //reroll
        }
        else
        {
            StartEvent(eventRand); // if no conditional clauses, just start it
        }
    }

    void InitializeEvents()
    {
        events = new List<MyEvent>();

        // To call a next event use response.setNextEvent(() => StartEvent(#))


      



        // EVENT #0
        MyEvent myEvent = new MyEvent("You find 5$ in your pocket.");                                                  
        myEvent.addLine("What do you do with it?");                                                                                     

        Response response = new Response("spend it on something fun for yourself");                                                  
        response.setTrigger("1");                   
        response.addResponseLine("You buy something cool for yourself, and feel rather giddy!");        
        response.setStatChange(+5, 0, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });                         
        myEvent.addResponse(response);               

        
        response = new Response("save it for something responsible");                                                          
        response.setTrigger("2");
        response.addResponseLine("You make the smart choice and save the money for a rainy day.");
        response.setStatChange(0, +1, +1, 0, 5);
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        
        events.Add(myEvent);

        // EVENT #1
        myEvent = new MyEvent("You rake up a huge pile of leaves in the yard.");                                               
        myEvent.addLine("What should you do with them?");

        response = new Response("jump into the giant pile and make a mess");
        response.setTrigger("1");
        response.addResponseLine("You have a surprising amount of fun being silly in the leaves. \n");
        response.addResponseLine("However, after cleaning it all up you feel exhausted and can't do as much today.");
        response.setStatChange(7, 3, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("bag them up and take them to the curb");
        response.setTrigger("2");
        response.addResponseLine("You buckle down and get the chore done, tired but glad to have it finished.");
        response.setStatChange(-1, 0, 3, 1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #2
        myEvent = new MyEvent("You and your friend get in an argument about politics and it gets a little heated.");
        myEvent.addLine("You are sure you're in the right, but during the argument you both yell some.");
        myEvent.addLine("How do you handle it?");

        response = new Response("ignore him for a while and wait for it to blow over");
        response.setTrigger("1");
        response.addResponseLine("You pretend it never happened and don't talk for a few days. \n");
        response.addResponseLine("Eventually you both forget about it.");
        response.setStatChange(-3, -5, -2, -5, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("apologize, even though you don't feel it was your fault");
        response.setTrigger("2");
        response.addResponseLine("You're still a little upset, but he is happy you apologized.");
        response.setStatChange(-5, 0, 5, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            FriendAttitudeUp();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("ask him to apologize to you");
        response.setTrigger("3");
        response.addResponseLine("He begrudgingly apologizes to you. You feel like you won. \n");
        response.addResponseLine("He doesn't seem as happy, though.");
        response.setStatChange(2, 2, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            FriendAttitudeDown();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #3
        myEvent = new MyEvent("Your car breaks down while you're driving to pick up a cheeseburger.");
        myEvent.addLine("You need to be able to get around, what do you do?");

        response = new Response("spend 5$ (if you have it) to get the car fixed");
        response.setTrigger("1");
        response.addResponseLine("You get the car to a mechanic and have the work done. \n");
        response.addResponseLine("It's expensive and came at a bad time, but at least you got it fixed.");
        response.setStatChange(0, -3, 5, 0, -5); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("bum a ride off of people until you can save up the money");
        response.setTrigger("2");
        response.addResponseLine("You slowly save up from your regular expenditure to get it fixed. \n");
        response.addResponseLine("In the mean time, you get rides from people you know. \n");
        response.addResponseLine("This is frustrating and troublesome, not to mention embarassing.");
        response.setStatChange(-7, -5, -3, 1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #4
        myEvent = new MyEvent("You go on vacation to the beach with your family.");
        myEvent.addLine("How do you spend the trip?");

        response = new Response("read books on the beach and relax");
        response.setTrigger("1");
        response.addResponseLine("You have a nice time reading, and the stress just melts away.");
        response.setStatChange(3, 15, -1, -3, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("spend the time with your family, cooking and talking with them");
        response.setTrigger("2");
        response.addResponseLine("You reconnect with your family and have a wonderful holiday.");
        response.setStatChange(5, 3, 3, 5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            ParentsAttitudeIncrease();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("go meet people at the beach and have fun");
        response.setTrigger("3");
        response.addResponseLine("You meet some fun people- no one really meaningful, but it's a good time.");
        response.setStatChange(3, 3, 0, 5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #5
        myEvent = new MyEvent("You fall down the staircase and feel pretty shaken up.");
        myEvent.addLine("What do you do?");

        response = new Response("go to the hospital, even though it'll be expensive, and make sure you're okay");
        response.setTrigger("1");
        response.addResponseLine("As expected it's expensive, but you get checked out. Luckily you're not badly injured.");
        response.setStatChange(-1, -1, -1, 0, -3); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("hobble into bed and rest for a bit");
        response.setTrigger("2");
        response.addResponseLine("You fall asleep in pain. When you wake up you feel like you may have cracked a rib. \n");
        response.addResponseLine("You aren't able to get around well or do as much for a while.");
        response.setStatChange(-3, -4, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDown();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #6
        myEvent = new MyEvent("You get a new game you've been waiting a long time for.");
        myEvent.addLine("How much should you play?");

        response = new Response("save it for later- you've got stuff to do");
        response.setTrigger("1");
        response.addResponseLine("You were looking forward to it, but what's a bit longer of waiting?\n");
        response.addResponseLine("You hold off and save your time to get stuff done.");
        response.setStatChange(-1, 0, 3, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("play for a little while");
        response.setTrigger("2");
        response.addResponseLine("You enjoy running from and fighting zombies for a while.");
        response.setStatChange(3, 2, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("binge it all day");
        response.setTrigger("3");
        response.addResponseLine("You throw responsibility to the wind and spend all day playing.\n");
        response.addResponseLine("You become a master of the Kentucky apocalypse with your handy fireaxe.");
        response.setStatChange(7, 3, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);

        events.Add(myEvent);

        // EVENT #7
        myEvent = new MyEvent("You feel suddenly depressed and decided to spend money to make yourself feel better.");
        myEvent.addLine("What do you buy?");

        response = new Response("buy some video games off Steam you've had your eye on");
        response.setTrigger("1");
        response.addResponseLine("You particularly enjoy playing one roguelike.\n");
        response.addResponseLine("It's a great time, but you die to a troll named Bill many times.");
        response.setStatChange(5, 3, 0, 0, -3); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("purchase some delicious (although not entirely health) foods");
        response.setTrigger("2");
        response.addResponseLine("You make an awesome meal, surprising even yourself with how tasty it is.");
        response.setStatChange(3, 0, 5, 0, -1); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

       
        
        // EVENT #8
        myEvent = new MyEvent("You get unwittingly dragged out to a party with a friend. They disappear on you, though.");                                                 
        myEvent.addLine("What do you do after they walk away from you?");                                                                                     

        response = new Response("get trashed and do your best to mingle");                                                  
        response.setTrigger("1");                   
        response.addResponseLine("You drink... a lot. You're not sure if it was worth it. You meet someone kind of fun, though.");        
        response.setStatChange(-2, 0, -3, 2, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            DelayedMeetGirl();
            return -1;
        });                         
        myEvent.addResponse(response);               

        
        response = new Response("just leave");                                                          
        response.setTrigger("2");
        response.addResponseLine("You walk right out the door. You've got better things to do than get ditched.");
        response.setStatChange(-1, 0, 2, -5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("try to find your friend again");
        response.setTrigger("3");
        response.addResponseLine("You walk around awkwardly searching for your friend, but get claustrophobic.");
        response.addResponseLine("Before too long you decide to just give up and leave.");
        response.setStatChange(-4, -4, -2, -5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #9
        myEvent = new MyEvent("You decide to play an online game of Dungeons and Dragons with your friends.");
        myEvent.addLine("Do you volunteer to be the Dungeon Master, or just play a character?");

        response = new Response("be the Dungeon Master");
        response.setTrigger("1");
        response.addResponseLine("You create a wild and fun adventure for your party of friends!");
        response.addResponseLine("It's a little stressful, but everyone seems to have a blast.");
        response.setStatChange(3, -2, 4, 5, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("play a cool Elf Wizard");
        response.setTrigger("2");
        response.addResponseLine("You're of tremendous service to your party, if a bit haughty.");
        response.addResponseLine("You have a great time playing the game.");
        response.setStatChange(3, 2, 0, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #10
        myEvent = new MyEvent("A nearby cat shelter has a show at a store that you're shopping in.");
        myEvent.addLine("There are lots of cats needing homes and money for supplies, what do you do?");

        response = new Response("adopt one of the cats");
        response.setTrigger("1");
        response.addResponseLine("You adopt a beautiful cat named Winry who brings you lots of happiness.");
        response.addResponseLine("It costs you a little bit of money to adopt her and ongoing costs to take care of her, though.");
        response.setStatChange(5, 5, 5, 3, -4); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            DailyHappinessBoost();
            ExtraExpense();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("donate a little money to the shelter");
        response.setTrigger("2");
        response.addResponseLine("You drop some money into the shelter's collection. You feel a little sad to leave the cats behind,");
        response.addResponseLine("but you also feel proud for helping the shelter out a little.");
        response.setStatChange(1, 0, 3, -1, -1); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("look at the cats for a while and then leave");
        response.setTrigger("3");
        response.addResponseLine("You stick your fingers in the cages and look at the cats for a while, then go about your day.");
        response.setStatChange(1, 0, -3, -3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #11

        myEvent = new MyEvent("Your professor finds a cheat sheet near your chair and accuses you of cheating.");
        myEvent.addLine("How do you respond to the accusation?");

        response = new Response("confess to cheating [LIE]");
        response.setTrigger("1");
        response.addResponseLine("You have no idea why, but you admitted to cheating even though you didn't do it.");
        response.addResponseLine("You get kicked out of the university.");
        response.setStatChange(-10, 0, -10, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            KickedFromSchool();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("confess to cheating [HONEST]");
        response.setTrigger("2");
        response.addResponseLine("The stress was just too much and you cheated. You confess.");
        response.addResponseLine("You get kicked out of the university.");
        response.setStatChange(-10, 3, -10, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            KickedFromSchool();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("explain your innocence [LIE]");
        response.setTrigger("3");
        response.addResponseLine("The cheat sheet was yours, but you attempt to lie your way out of it.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            LieGamble();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("explain your innocence [HONEST]");
        response.setTrigger("4");
        response.addResponseLine("The cheat sheet belonged to someone else, which you explain.");
        response.addResponseLine("You feel glad you didn't get wrongfully blamed.");
        response.setStatChange(2, 1, 2, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #12

        myEvent = new MyEvent("On a whim you decide to cook up a big meal for yourself!");
        myEvent.addLine("What kind of food do you make?");

        response = new Response("make something healthy. It costs a bit extra, but it's good for you!");
        response.setTrigger("1");
        response.addResponseLine("You make a healthy, tasty meal that you're proud of!");
        response.setStatChange(3, 1, 4, 0, -1); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("make something unhealthy and impressive.");
        response.setTrigger("2");
        response.addResponseLine("You make an incredibly delicious, rich meal. Incredible!");
        response.setStatChange(5, 2, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);



        // EVENT #13

        myEvent = new MyEvent("You're doing some laundry when suddenly your dryer stops working.");
        myEvent.addLine("How do you move forward?");

        response = new Response("get the dryer fixed for $5");
        response.setTrigger("1");
        response.addResponseLine("You get the dryer fixed.");
        response.setStatChange(-3, -4, 2, 0, -5); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("take your laundry to a friend's house for a while");
        response.setTrigger("2");
        response.addResponseLine("You cart your laundry around. What a pain.");
        response.setStatChange(-4, -3, -2, 1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #14

        myEvent = new MyEvent("You get a phonecall that a close family member has died.");
        myEvent.addLine("You've been given the date for the funeral.");

        response = new Response("take some time away and go to the funeral");
        response.setTrigger("1");
        response.addResponseLine("The service is beautiful and while it was a hard time,");
        response.addResponseLine("seeing your family helped you through it.");
        response.setStatChange(-4, -3, 0, -3, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            ParentsAttitudeIncrease();
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("stay home and skip the funeral");
        response.setTrigger("2");
        response.addResponseLine("You have a challenging time dealing with the loss on your own.");
        response.addResponseLine("Your family is disappointed you didn't make it for the funderl.");
        response.setStatChange(-6, -5, 1, -5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            ParentsAttitudeDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);



        // EVENT #15

        myEvent = new MyEvent("You find a Mexican restaurant you've never been to before.");
        myEvent.addLine("Do you go by?");

        response = new Response("give it a try");
        response.setTrigger("1");
        response.addResponseLine("The food is incredible and you enjoy yourself thoroughly.");
        response.setStatChange(2, 1, 1, 1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("pass it by");
        response.setTrigger("2");
        response.addResponseLine("You'll never know what you missed out on.");
        response.setStatChange(-1, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #16

        myEvent = new MyEvent("One of your old ex-girlfriends calls you.");
        myEvent.addLine("How do you handle the situation?");

        response = new Response("ignore the call completely");
        response.setTrigger("1");
        response.addResponseLine("The phone doesn't ring again and you're left not knowing what she wanted.");
        response.setStatChange(-2, -2, 2, -2, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("tell her you don't want to talk");
        response.setTrigger("2");
        response.addResponseLine("She doesn't say much before hanging up.");
        response.setStatChange(-2, -2, -2, -3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("talk to her for a little while");
        response.setTrigger("3");
        response.addResponseLine("You fight for over and hour, and remember clearly why you broke up.");
        response.setStatChange(-4, -4, -3, -5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #17

        myEvent = new MyEvent("You think you may have an allergy; your stomach has been bothering you for a while.");
        myEvent.addLine("What do you do about it?");

        response = new Response("ignore it and hope you get better");
        response.setTrigger("1");
        response.addResponseLine("You have considerable discomfort that won't go away..");
        response.setStatChange(-8, -2, 1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("go by the doctor's office [$2]");
        response.setTrigger("2");
        response.addResponseLine("It turns out you have a wheat allergy. Now that you know, you can live more comfortably.");
        response.setStatChange(-1, 1, 0, 0, -2); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);



        // EVENT #18

        myEvent = new MyEvent("You find a wallet on the street with some money in it.");
        myEvent.addLine("What do you do with the wallet?");

        response = new Response("turn it in to the police");
        response.setTrigger("1");
        response.addResponseLine("The police take the wallet and thank you. You don't hear back about it.");
        response.setStatChange(2, 0, 5, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("throw the wallet out and keep the money");
        response.setTrigger("2");
        response.addResponseLine("You feel a little guilty for keeping the wallet but hey- money's money.");
        response.setStatChange(-4, 1, -5, 0, 5); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);



        // EVENT #19

        myEvent = new MyEvent("While drinking at a bar, a man pushes you from behind. You spill your drink.");
        myEvent.addLine("How do you react?");

        response = new Response("completely ignore it and order a new drink");
        response.setTrigger("1");
        response.addResponseLine("You're a little upset about the lost drink, but you move on.");
        response.setStatChange(-1, -1, -3, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("punch the man square in the face");
        response.setTrigger("2");
        response.addResponseLine("You the man sprawls over backwards onto a table, and a brawl ensues.");
        response.addResponseLine("Somehow, you escape further injury than a mildly bruised hand, though.");
        response.setStatChange(2, 3, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("turn and swear at the man who shoved you");
        response.setTrigger("3");
        response.addResponseLine("The man listens to you berate him for a few moments before taking a swing at you.");
        response.addResponseLine("Your head crashes into the bar and you fall down. Ouch. You take a cab home.");
        response.addResponseLine("You are injured.");
        response.setStatChange(-5, 0, -3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDown();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("turn and say 'excuse me?' to the man");
        response.setTrigger("4");
        response.addResponseLine("The man apologizes- turns out it was an accident. He buys you a round and hands you some money");
        response.addResponseLine("to make up for spilling your drink. You hang out with him for a while and have a good time.");
        response.setStatChange(2, 1, 1, 5, 2); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);



        // EVENT #20

        myEvent = new MyEvent("A homeless man approaches you downtown and asks for money");
        myEvent.addLine("What do you do?");

        response = new Response("give him some change");
        response.setTrigger("1");
        response.addResponseLine("He thanks you and walks away.");
        response.setStatChange(2, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("be generous [$2]");
        response.setTrigger("2");
        response.addResponseLine("The man thanks you profusely and smiles happily.");
        response.addResponseLine("You feel lucky today!");
        response.setStatChange(7, 0, 4, 0, -2); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            Lucky();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);



        // EVENT #21

        myEvent = new MyEvent("You enter a radio competition and win a year's supply of jam!");
        myEvent.addLine("Awesome!?");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("What a lucky break!");
        response.setStatChange(2, 1, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #22

        myEvent = new MyEvent("You get a large bill in the mail you weren't expecting.");
        myEvent.addLine("How do you take care of it?");

        response = new Response("pay it up front [$3]");
        response.setTrigger("1");
        response.addResponseLine("You pay the bill. Frustrating, but good to have it done.");
        response.setStatChange(-3, -3, 2, 0, -3); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("pay it over the next few weeks");
        response.setTrigger("2");
        response.addResponseLine("You pay $1 per turn for the next 5 turns.");
        response.addResponseLine("You are stressed about having it over your head.");
        response.setStatChange(-4, -5, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MoneyDown();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #23

        myEvent = new MyEvent("You receive an unexpected check in the mail.");
        myEvent.addLine("Awesome!");

        response = new Response("deposit the check");
        response.setTrigger("1");
        response.addResponseLine("You get some extra money. Fantastic!");
        response.setStatChange(3, 3, 0, 0, 5); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #24

        myEvent = new MyEvent("A tree falls and the power goes out for a day.");
        myEvent.addLine("What do you do with your day?");

        response = new Response("stay home and read by the window");
        response.setTrigger("1");
        response.addResponseLine("You enjoy hanging out at home and having a chill day reading.");
        response.setStatChange(2, 5, 1, 1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("go for a walk downtown");
        response.setTrigger("2");
        response.addResponseLine("Walking around downtown is fun, and you meet a nice girl at a coffee shop!");
        response.setStatChange(3, 1, 0, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            DelayedMeetGirl();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #25

        myEvent = new MyEvent("You go downtown with friends for the evening.");
        myEvent.addLine("How do you spend the night?");

        response = new Response("have some drinks and take an Uber home [$1]");
        response.setTrigger("1");
        response.addResponseLine("You have a surprisingly good time- you feel great.");
        response.setStatChange(3, 3, 0, 3, -1); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("stay sober and drive yourself home later");
        response.setTrigger("2");
        response.addResponseLine("You were a little bored, and reminded that you don't much like hanging out in groups.");
        response.setStatChange(0, -1, 1, -1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #26

        myEvent = new MyEvent("You decide to start a YouTube channel and get it set up.");
        myEvent.addLine("What do you upload first?");

        response = new Response("post a video of a cat dancing");
        response.setTrigger("1");
        response.addResponseLine("You become briefly internet famous as your cat video hits the front page.");
        response.addResponseLine("You are surprised to get some add revenue, but nothing really comes of it beyond that.");
        response.setStatChange(3, 0, 0, 0, 2); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("post a video of you playing your favorite game");
        response.setTrigger("2");
        response.addResponseLine("You don't get many views, but you really enjoy doing it.");
        response.setStatChange(4, 0, 4, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #27

        myEvent = new MyEvent("You join an acting troupe and put on a play");
        myEvent.addLine("How does the play go?");

        response = new Response("break a leg");
        response.setTrigger("1");
        response.addResponseLine("You break a leg... literally. Ouch.");
        response.addResponseLine("The troupe covers your medical bills luckily, but it still hurts.");
        response.setStatChange(-2, -2, 1, 3, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("break a leg");
        response.setTrigger("2");
        response.addResponseLine("You break a leg... figuratively. You do great, and have a fun time doing it!");
        response.setStatChange(3, -2, 3, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #

        myEvent = new MyEvent("Your attic gets bats. They're a protected species, so you can't get an exterminator.");
        myEvent.addLine("What do you do about them?");

        response = new Response("get rid of them yourself");
        response.setTrigger("1");
        response.addResponseLine("You try to get rid of the bats, but one of them bites you.");
        response.addResponseLine("You have to go pay for a rabies shot.");
        response.setStatChange(-3, -2, 1, 0, -1); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("leave them alone");
        response.setTrigger("2");
        response.addResponseLine("They're actually kinda cute, so you leave them alone other than taking some pictures.");
        response.setStatChange(1, 1, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #29

        myEvent = new MyEvent("You go out to the movies with your friend for the evening.");
        myEvent.addLine("It's your turn to pick the movie, what do you go see?");

        response = new Response("that new superhero film");
        response.setTrigger("1");
        response.addResponseLine("You both enjoy yourselves immensely and have a fun argument about which character is stronger.");
        response.setStatChange(3, 3, 0, 3, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            FriendAttitudeUp();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("a funny comedy");
        response.setTrigger("2");
        response.addResponseLine("The movie is hilarious and you both have a great time.");
        response.setStatChange(2, 4, 0, 2, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("a documentary");
        response.setTrigger("3");
        response.addResponseLine("The movie is interesting, but you friend hates it.");
        response.addResponseLine("On the way out you meet a cute girl, though.");
        response.setStatChange(1, 1, 1, 1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            DelayedMeetGirl();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #30

        myEvent = new MyEvent("A long forgotten friend reaches out to you needing money.");
        myEvent.addLine("How do you respond?");

        response = new Response("rebuke them for asking for money when they're not in your life");
        response.setTrigger("1");
        response.addResponseLine("They hang up obviously hurt. You don't hear from them again.");
        response.setStatChange(-5, 0, -2, -2, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("tell them you can't afford it, but still chat with them for a while");
        response.setTrigger("2");
        response.addResponseLine("They seem to enjoy talking to you, but you can tell they're going through a rough time.");
        response.addResponseLine("You don't hear from them any time soon again.");
        response.setStatChange(1, 0, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("give them the money [$2]");
        response.setTrigger("3");
        response.addResponseLine("They seem really relieved, and you have a great conversation and reconnect with them.");
        response.addResponseLine("You hang out with them soon after, and not only do you have a blast, but they also pay you back right away!");
        response.setStatChange(7, 4, 5, 10, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #31

        myEvent = new MyEvent("You get a phonecall from a telemarketer while sitting around watching TV.");
        myEvent.addLine("How do you handle the call?");

        response = new Response("hang up immediately");
        response.setTrigger("1");
        response.addResponseLine("You hang up as soon as you get the call. Those calls are so annoying.");
        response.setStatChange(-1, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("listen to the sales pitch");
        response.setTrigger("2");
        response.addResponseLine("You listen to the sales pitch and it turns out to be hilarious. The guy doesn't even know what he's selling,");
        response.addResponseLine("and he continually stumbles on the pitch and eventually just gives up and hangs up on his own.");
        response.addResponseLine("You stiffle a laugh the whole time and are crying from holding it in by the time he hangs up. What fun!");
        response.setStatChange(5, 3, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("mess with the telemarketer");
        response.setTrigger("3");
        response.addResponseLine("You make fun of the telemarketer for a while, trying to keep him on the line.");
        response.addResponseLine("He eventually gets annoyed and hangs up, but you feel good for wasting his time like he tried to waste yours.");
        response.setStatChange(1, 1, 2, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #32

        myEvent = new MyEvent("A powerline gets knocked over and sets your lawn on fire.");
        myEvent.addLine("How do you react?");

        response = new Response("run try to put it out with the hose");
        response.setTrigger("1");
        response.addResponseLine("You get shocked... and burned. Neither too bad, but that was a terrible idea!");
        response.setStatChange(-5, -3, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("call the fire department");
        response.setTrigger("2");
        response.addResponseLine("The fire department and the power company handle it for you quickly and efficiently.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #33

        myEvent = new MyEvent("You plan out a camping trip with your friend.");
        myEvent.addLine("However, it pours raining the day before and doesn't seem to be letting up. What do you do?");

        response = new Response("brave the weather and go anyways");
        response.setTrigger("1");
        response.addResponseLine("You slog through the mud and try to have fun anyway.");
        response.addResponseLine("It doesn't turn out to be very fun, but you're proud of sticking it out.");
        response.setStatChange(-3, 0, 1, 2, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("stay home and do some co-op gaming");
        response.setTrigger("2");
        response.addResponseLine("It pours raining all weekend, but you have fun with you friend from the comfort of home.");
        response.setStatChange(3, 1, 0, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            FriendAttitudeUp();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #34

        myEvent = new MyEvent("A vicious stray cat starts chasing you around.");
        myEvent.addLine("How do you get away?");

        response = new Response("climb up a tree");
        response.setTrigger("1");
        response.addResponseLine("A firetruck happens to drive by as you're climbing up the tree, and they can't resist taking a picture.");
        response.addResponseLine("The humiliating picture makes it into the paper, looking as if the fireman is saving you from the tree while the cat waits below.");
        response.setStatChange(-2, 1, -5, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("make a break for it");
        response.setTrigger("2");
        response.addResponseLine("You get scratched up a little, but you make it away.");
        response.setStatChange(-1, 0, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #35

        myEvent = new MyEvent("While climbing out of your car, you drop your phone and it smashes on the curb.");
        myEvent.addLine("What do you do for a phone?");

        response = new Response("try to get it repaired [$1]");
        response.setTrigger("1");
        response.addResponseLine("You take it to get fixed, and while the fix the screen it sometimes shuts off while you use it. How annoying.");
        response.addResponseLine("You meet a nice girl at the phone repair shop, though.");
        response.setStatChange(-2, -2, -2, 0, -1); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            DelayedMeetGirl();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("buy a new phone [$3]");
        response.setTrigger("2");
        response.addResponseLine("You buy a fancy new phone. It's expensive, but super cool!");
        response.setStatChange(2, -1, 3, 0, -3); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("use your old flip phone");
        response.setTrigger("3");
        response.addResponseLine("It's kind of embarassing, but your old flip phone does the job.");
        response.setStatChange(-1, 0, -4, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #36

        myEvent = new MyEvent("You get a horrific migraine on a day when you need to get work done.");
        myEvent.addLine("What do you do?");

        response = new Response("skip the work and rest");
        response.setTrigger("1");
        response.addResponseLine("You immediately go lie down and take medicine, and feel better fairly soon.");
        response.addResponseLine("You feel well enough by that evening to get caught up with the work.");
        response.setStatChange(-2, -1, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("try to power through");
        response.setTrigger("2");
        response.addResponseLine("You try to power through, but end up just causing yourself more pain and doing shoddy work.");
        response.addResponseLine("You can't get as much done this turn.");
        response.setStatChange(-4, -2, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #37

        myEvent = new MyEvent("You decide to grow some fresh veggies to cook with.");
        myEvent.addLine("What do you plant?");

        response = new Response("plant peppers");
        response.setTrigger("1");
        response.addResponseLine("They grow really well, and you have delicious spicy peppers for all kinds of recipes.");
        response.setStatChange(4, 1, 2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("plant strawberries");
        response.setTrigger("3");
        response.addResponseLine("You plant strawberries, but they never grow. Darn.");
        response.setStatChange(-1, 0, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("plant tomatoes");
        response.setTrigger("2");
        response.addResponseLine("The tomatoes grow really well, and you have enough to share with friends!");
        response.setStatChange(1, 1, 3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #38

        myEvent = new MyEvent("Heavy rainfall causes flooding in your town when you need to go out.");
        myEvent.addLine("What do you do?");

        response = new Response("drive anyway");
        response.setTrigger("1");
        response.addResponseLine("You try to drive but quickly get in too deep. You have to turn around and go home.");
        response.setStatChange(-1, -2, 3, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("stay home");
        response.setTrigger("2");
        response.addResponseLine("You stay home. You don't get your work done, and feel frustrated.");
        response.setStatChange(-2, -4, -3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #39

        myEvent = new MyEvent("A game you enjoy asks you to be one of their forum moderators.");
        myEvent.addLine("Do you accept?");

        response = new Response("do it");
        response.setTrigger("1");
        response.addResponseLine("It is stressful and unrewarding work, yet you feel it could lead to good things later.");
        response.setStatChange(-1, -5, 5, 3, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("decline");
        response.setTrigger("2");
        response.addResponseLine("You decide to turn them down. It sounds stressful.");
        response.setStatChange(0, 0, -3, -1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #40

        myEvent = new MyEvent("You find a gray hair!");
        myEvent.addLine("You feel old.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You're a little sad, and contemplate your mortality.");
        response.setStatChange(-1, -1, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #41

        myEvent = new MyEvent("A great song comes on your radio while you're showering.");
        myEvent.addLine("Sing along?");

        response = new Response("stay quiet");
        response.setTrigger("1");
        response.addResponseLine("You listen to the music but stay quiet. As you're about to grab your towel, you slip and fall. OUCH.");
        response.addResponseLine("You can't do as much this turn due to your injury.");
        response.setStatChange(-3, -2, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("belt it out");
        response.setTrigger("2");
        response.addResponseLine("You sing it loud, but unfortunately you slip while dancing a little and fall. OUCH.");
        response.addResponseLine("You can't do as much this turn due to your injury.");
        response.setStatChange(0, -1, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #42

        myEvent = new MyEvent("Someone knocks on your door while you're watching TV.");
        myEvent.addLine("What do you do?");

        response = new Response("answer the door");
        response.setTrigger("1");
        response.addResponseLine("You open the door, and there's a beautiful young woman on your doorstep.");
        response.addResponseLine("She just moved in next door, and wanted to introduce herself.");
        response.setStatChange(2, 1, 0, 2, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            DelayedMeetGirl();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("look through the peephole");
        response.setTrigger("2");
        response.addResponseLine("There's a lovely young woman out there!");
        response.addResponseLine("You scramble to open the door, but accidentally smash it into your foot.");
        response.addResponseLine("She's a new neighbor, but you're too concerned with your smashed foot to pay much attention.");
        response.setStatChange(-2, -1, -3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);

        response = new Response("ignore it");
        response.setTrigger("2");
        response.addResponseLine("The knock only comes once, so it's easy to ignore.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #43

        myEvent = new MyEvent("You witness an armed robbery at a gas station.");
        myEvent.addLine("What do you do?");

        response = new Response("try to stop the robber");
        response.setTrigger("1");
        response.addResponseLine("You sneak down an aisle and try to sneak up on the man.");
        response.addResponseLine("As you come around the corner, you charge at him. However, he notices you quickly.");
        response.addResponseLine("He turns to you, pulls up a pistol, and fires.");
        response.addResponseLine("You fall to the ground, completely stunned.");
        response.addResponseLine("As the world goes black, you feel disappointed but also strangely content.");
        response.addResponseLine("While you didn't manage to stop him or fulfill your goals, you feel proud that you tried to do right.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            Invoke("GameOver", 5f);
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("do nothing; leave");
        response.setTrigger("2");
        response.addResponseLine("You quietly sneak out the back of the building and feel lucky to be alive.");
        response.addResponseLine("As you run away outside, you hear a single gunshot.");
        response.setStatChange(-2, -5, -5, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("call the police");
        response.setTrigger("2");
        response.addResponseLine("You quietly sneak out the back of the building and call the police.");
        response.addResponseLine("A car happened to be in the area and pulls up only moments later.");
        response.addResponseLine("The robber is apprehended in front of you, and the business owner thanks you profusely.");
        response.addResponseLine("Apparently the robber was about to shoot him. He gives you $5 as thanks.");
        response.setStatChange(2, -2, 4, 1, 5); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #44

        myEvent = new MyEvent("You get an awful cold and a fever");
        myEvent.addLine("You don't feel like doing anything except staying home.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You can't do as much this turn due to your sickness.");
        response.setStatChange(-1, -3, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #45

        myEvent = new MyEvent("Coffee at work is no longer free!");
        myEvent.addLine("What do you do about this frustrating turn of events?");

        response = new Response("stop drinking coffee");
        response.setTrigger("1");
        response.addResponseLine("Losing caffeine costs you many headaches, but you feel better not being addicted.");
        response.setStatChange(-4, -3, 2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            NoAddiction();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("bring coffee from home [$1]");
        response.setTrigger("2");
        response.addResponseLine("You bring coffee from home. Costs a little, but it's worth it.");
        response.setStatChange(0, 0, 0, 0, -1); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            CoffeeAddiction();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #46

        myEvent = new MyEvent("Your sister calls and asks if you want to go in together on a gift for a family member.");
        myEvent.addLine("What do you say?");

        response = new Response("pitch in half for a great gift[$2]");
        response.setTrigger("1");
        response.addResponseLine("You buy an awesome gift. Feels good to give!");
        response.setStatChange(3, 0, 1, 1, -2); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("decide to get separate gifts");
        response.setTrigger("2");
        response.addResponseLine("Your sister seems a bit disappointed, and the gift was okay but not great. [$1]");
        response.setStatChange(1, 0, 2, 0, -1); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("skp the gift all together");
        response.setTrigger("3");
        response.addResponseLine("You don't get a gift for the family member. You feel bad.");
        response.setStatChange(-3, 0, -2, -1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #47

        myEvent = new MyEvent("While cooking dinner a pot of sauce gets jarred off the stove.");
        myEvent.addLine("How do you react?");

        response = new Response("let the pot fall");
        response.setTrigger("1");
        response.addResponseLine("You don't try to catch the pot and jump back. It hits the floor.");
        response.addResponseLine("Sauce splatters on your leg and burns you. Ow!");
        response.setStatChange(-2, 0, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("try to catch the pot");
        response.setTrigger("2");
        response.addResponseLine("As the pot slips, you dextrously reach out and grab the handle.");
        response.addResponseLine("You manage to snag it without spilling a drop. Dinner is saved!");
        response.setStatChange(2, 1, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #48

        myEvent = new MyEvent("You feel a rare cleaning mood coming on.");
        myEvent.addLine("Do you do it?");

        response = new Response("give in to the mood and clean all day");
        response.setTrigger("1");
        response.addResponseLine("Before you know it you've spent an entire day doing chores.");
        response.addResponseLine("It feels great to have everything done, but you can't do as much this turn.");
        response.setStatChange(3, 2, 10, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("resist and spend the day doing normal activities");
        response.setTrigger("2");
        response.addResponseLine("The mood goes away.");
        response.setStatChange(0, 0, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #49

        myEvent = new MyEvent("Your eye begins to twitch uncontrollably.");
        myEvent.addLine("It lasts for 3 days and then goes away.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("It bothers you the whole time.");
        response.setStatChange(-1, -2, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #50

        myEvent = new MyEvent("While walking near home you see a stray dog. It looks cute!");
        myEvent.addLine("What do you do?");

        response = new Response("pet it");
        response.setTrigger("1");
        response.addResponseLine("You go to pet it, but it looks surprised and scared and bites your hand before running off. OUCH!");
        response.setStatChange(-2, 0, 0, -1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("let it sniff your hand");
        response.setTrigger("2");
        response.addResponseLine("It sniffs your hand cautiously before nuzzling you. You pet it for a while before it runs off. Awww.");
        response.setStatChange(3, 3, 1, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #51

        myEvent = new MyEvent("You get in an argument with a coworker over something small.");
        myEvent.addLine("What do you do about it?");

        response = new Response("escalate it to your boss");
        response.setTrigger("1");
        response.addResponseLine("You complain to your boss, but he just seems annoyed.");
        response.setStatChange(-2, -3, 1, -1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("let it slide");
        response.setTrigger("2");
        response.addResponseLine("You ignore it and soon make up with your co-worker.");
        response.setStatChange(-1, -1, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #52

        myEvent = new MyEvent("You feel strangely motivated today. You can accomplish  more!");
        myEvent.addLine("");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("Spoons increased by 1!");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsIncrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #53

        myEvent = new MyEvent("As you're laying in bed, you come up with a great idea for a novel.");
        myEvent.addLine("You really feel like you should write it- what do you do?");

        response = new Response("get up immediately and start writing");
        response.setTrigger("1");
        response.addResponseLine("You wake up and write several chapters before eventually passing out in the middle of the night.");
        response.addResponseLine("You're not sure if you'll ever finish it, but you're happy to have gotten a lot of ideas on paper.");
        response.addResponseLine("Unfortunately, you're exhausted and can't do as much now.");
        response.setStatChange(7, 0, 4, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("jot down the idea and go to bed");
        response.setTrigger("2");
        response.addResponseLine("You get the basic outline of the idea down and get your rest.");
        response.addResponseLine("Looking at it the next day it doesn't make quite as much sense.");
        response.addResponseLine("You stick it in the closet and promise yourself you'll get back to it.");
        response.setStatChange(2, 0, 1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("roll over and go back to sleep");
        response.setTrigger("3");
        response.addResponseLine("You decide to worry about it in the morning. You completely forget, though.");
        response.setStatChange(0, 0, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #54

        myEvent = new MyEvent("You stub your toe on the table while walking by.");
        myEvent.addLine("Your toenail breaks and there's some blood.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("It hurts like hell but eventually gets better.");
        response.setStatChange(-3, -1, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #55

        myEvent = new MyEvent("You noticed your gutters were clogged last time it rained.");
        myEvent.addLine("How do you take care of them?");

        response = new Response("clean them yourself");
        response.setTrigger("1");
        response.addResponseLine("You manage to clean them out, but it's exhausted. You can't do as much today.");
        response.setStatChange(-1, 0, 2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("hire someone to do it [$1]");
        response.setTrigger("2");
        response.addResponseLine("They do a great job and you're happy to have it done.");
        response.setStatChange(2, 1, 1, 0, -1); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("skip it, they'll be fine");
        response.setTrigger("3");
        response.addResponseLine("The next time it rains the gutters flood water into your roof.");
        response.addResponseLine("It causes considerable damage. You're forced to spend $5 to have everything fixed, whether you can afford it or not.");
        response.setStatChange(-4, -6, -2, 0, -5); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #56

        myEvent = new MyEvent("A big dog runs up to you with its owner while you're outside.");
        myEvent.addLine("What do you do?");

        response = new Response("run away");
        response.setTrigger("1");
        response.addResponseLine("You bravely scarper off as the friendly dog tries to lick you.");
        response.setStatChange(0, 0, -1, -1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("shout at the woman to control her dog");
        response.setTrigger("2");
        response.addResponseLine("She leaves with her dog after calling you an asshole.");
        response.setStatChange(-2, 0, 1, -1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("pet the dog");
        response.setTrigger("3");
        response.addResponseLine("The dog gives you a big slobbery kiss. You also hit it off with the dogs owner.");
        response.addResponseLine("She gives you her number!");
        response.setStatChange(4, 2, 1, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            DelayedMeetGirl();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #57

        myEvent = new MyEvent("While in the middle of something, you idly daydream for a few minutes.");
        myEvent.addLine("You find yourself wondering what other lives you could be leading.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("Nobody really notices you spacing out, and you get back to what you're doing.");
        response.setStatChange(1, 1, 0, -1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #58

        myEvent = new MyEvent("You suddenly get a hankering for waffles for no reason.");
        myEvent.addLine("What do you do about it?");

        response = new Response("go out for waffles [$1]");
        response.setTrigger("1");
        response.addResponseLine("You go out and get yourself a big plate of waffles, and bacon and coffee to boot.");
        response.addResponseLine("You find yourself enjoying the simple pleasure of good food, and have a better day for it.");
        response.setStatChange(3, 2, 0, 0, -1); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("ignore it");
        response.setTrigger("2");
        response.addResponseLine("You consider making waffles, but remember you don't have a waffle iron. Oh well.");
        response.setStatChange(-1, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #59

        myEvent = new MyEvent("You get a nasty bug bite while walking outside.");
        myEvent.addLine("Your ankle swells up horribly and aches.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("It feels sore and painful from the bite for the next few days, but doesn't slow you down too much.");
        response.setStatChange(-2, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #60

        myEvent = new MyEvent("You've been drinking a whole lot of coffee lately and are feeling kind of funny.");
        myEvent.addLine("Do you do anything about it?");

        response = new Response("ignore it; keep drinking");
        response.setTrigger("1");
        response.addResponseLine("Your heart feels kind of funny, but you also have tons of energy.");
        response.addResponseLine("You can do more today!");
        response.setStatChange(1, -1, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsIncrease();
            CoffeeAddiction();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("cut back on the coffee a bit");
        response.setTrigger("2");
        response.addResponseLine("You feel normal again.");
        response.setStatChange(-1, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("swear off coffee entirely");
        response.setTrigger("3");
        response.addResponseLine("You feel healthier generally speaking, but for the next while you drag as you get over a caffeine addiction.");
        response.addResponseLine("You can't do as much for a while, but are very proud of yourself for quitting.");
        response.setStatChange(-3, -1, 8, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            NoAddiction();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #61

        myEvent = new MyEvent("As you're gaming one day, your computer developes a serious fault and won't restart.");
        myEvent.addLine("What do you do about it?");

        response = new Response("diagnose and fix it [$2]");
        response.setTrigger("1");
        response.addResponseLine("It takes a while, but you're able to find the faulty part and replace it.");
        response.addResponseLine("While not having a computer is a bit frustrating, you find you actually enjoy the work.");
        response.setStatChange(3, -1, 5, 0, -2); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("replace it with a better one [$6]");
        response.setTrigger("2");
        response.addResponseLine("You shell out for parts for an awesome new gaming rig and put it together.");
        response.addResponseLine("It's top of the line and you love it.");
        response.setStatChange(5, -1, 2, 0, -6); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("use your old laptop");
        response.setTrigger("3");
        response.addResponseLine("Using your laptop is frustrating and you can't play many of the games you love.");
        response.addResponseLine("It also keeps you from doing some hobbies you enjoy. At least you saved some money, though.");
        response.setStatChange(-8, -2, -4, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #62

        myEvent = new MyEvent("You find a book you've wanted for ages at the thrift store for only a few cents.");
        myEvent.addLine("You snatch it up happily.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You put it proudly on your shelf and read it when you get the chance.");
        response.setStatChange(2, 0, 2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #63

        myEvent = new MyEvent("You drink a little too much one night.");
        myEvent.addLine("You wake up with a heck of a hangover. Whoops.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("Your head hurts all day and you can't get as much done.");
        response.setStatChange(-2, 1, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #64

        myEvent = new MyEvent("It snows completely out of season, surprising everyone.");
        myEvent.addLine("What do you do with the snow day?");

        response = new Response("go out and play");
        response.setTrigger("1");
        response.addResponseLine("You have a blast out in the snow, enjoying a much needed break.");
        response.setStatChange(2, 5, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("stay in and get some chores done");
        response.setTrigger("2");
        response.addResponseLine("You get a lot done, and also enjoy watching the snow fall outside.");
        response.setStatChange(1, 1, 3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #65

        myEvent = new MyEvent("You feel tired from a long day and want to crash and watch some TV.");
        myEvent.addLine("What show do you watch?");

        response = new Response("watch some sci-fi");
        response.setTrigger("1");
        response.addResponseLine("The show strikes you strangely; you wish you were doing more with your life like the characters in the show.");
        response.setStatChange(-2, -3, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("watch a comedy");
        response.setTrigger("2");
        response.addResponseLine("The show strikes you strangely; you wish your life was more like a comedy and could believe everything would turn out okay.");
        response.setStatChange(-5, 0, 0, -2, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("watch a fantasy");
        response.setTrigger("2");
        response.addResponseLine("The show strikes you strangely; you secretly yearn for adventure like in the show and feel unfulfilled by your life.");
        response.setStatChange(-2, -1, -5, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #66

        myEvent = new MyEvent("A neighbor knocks on your door to make sure you're okay.");
        myEvent.addLine("They say they haven't seen you in weeks and were worried about you.");

        response = new Response("tell them you're alright");
        response.setTrigger("1");
        response.addResponseLine("You feel a little embarassed for not getting out more.");
        response.setStatChange(0, 0, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("tell them you've been out of town");
        response.setTrigger("2");
        response.addResponseLine("You feel a little embarassed for not getting out more. You feel even more embarassed you felt the need to lie about it.");
        response.setStatChange(0, 0, -4, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #67

        myEvent = new MyEvent("While at a restaurant you've never been to before you have the best key lime pie of your life.");
        myEvent.addLine("");

        response = new Response("enjoy it");
        response.setTrigger("1");
        response.addResponseLine("It was delicious and you think about it for the rest of the week.");
        response.setStatChange(3, 2, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #68

        myEvent = new MyEvent("Someone leaves a tasty looking casserole on your doorstep.");
        myEvent.addLine("There's a nice note on it, but you're not sure who it's from.");

        response = new Response("eat it for dinner");
        response.setTrigger("1");
        response.addResponseLine("Eating dinner off your doorstep feels a bit weird, but it's tasty nonetheless.");
        response.setStatChange(3, -1, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("toss it out");
        response.setTrigger("2");
        response.addResponseLine("Eating food you found on your doorstep is just too weird. You put it in the trash.");
        response.setStatChange(0, 1, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #69

        myEvent = new MyEvent("You oversleep your alarm... by a lot.");
        myEvent.addLine("An entire morning is lost.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You don't have as much time to do stuff, but you do feel well rested.");
        response.setStatChange(2, -1, -1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #70

        myEvent = new MyEvent("You cut yourself shaving.");
        myEvent.addLine("Ouch!");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("The cut stings and bothers you a little all day.");
        response.setStatChange(-1, -1, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #71

        myEvent = new MyEvent("As you're walking to your car a hornet flies near you.");
        myEvent.addLine("How do you escape?");

        response = new Response("run to the car!");
        response.setTrigger("1");
        response.addResponseLine("You successfully evade the hornet, but as you run to your car you step on a nail.");
        response.addResponseLine("It goes right into your foot straight through your shoe. You have to go to the hospital.");
        response.addResponseLine("They clean the wound and give you a tetanus shot. It's expensive [$2].");
        response.setStatChange(-4, -1, 0, 0, -2); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("walk calmly to the ca!");
        response.setTrigger("2");
        response.addResponseLine("You get away from the hornet, but right before you get into the car you step on a nail.");
        response.addResponseLine("Luckily you notice it before it pokes through your shoe. You pull it off your shoe and throw it away.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #72

        myEvent = new MyEvent("Something piques your interest online...");
        myEvent.addLine("There's a very informative Wikipedia article about it.");

        response = new Response("read the article and move on");
        response.setTrigger("1");
        response.addResponseLine("You check out the article and learn a bit and then go on with your day.");
        response.setStatChange(1, -1, 1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("click through to a couple of other pages to learn more");
        response.setTrigger("2");
        response.addResponseLine("You end up educating yourself on an entirely new subject... but you spent hours doing it!");
        response.addResponseLine("You can't do as much today.");
        response.setStatChange(2, -2, 3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            SpoonsDecrease();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #73

        myEvent = new MyEvent("Someone sends you a link to TVTropes.");
        myEvent.addLine("You click on it and get sucked into TVTropes for hours.");

        response = new Response("enjoy it");
        response.setTrigger("1");
        response.addResponseLine("You have lots of fun reading TVTropes, but you waste a lot of time and can't do as much.");
        response.setStatChange(2, 4, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("resist");
        response.setTrigger("2");
        response.addResponseLine("You try to stop clicking links, but you just can't.");
        response.addResponseLine("You waste tons of time on it anwyay, but it's pretty fun.");
        response.setStatChange(4, 2, -2, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #74

        myEvent = new MyEvent("You get stuck on a difficult part of a game.");
        myEvent.addLine("What do you do?");

        response = new Response("take a break and then come back");
        response.setTrigger("1");
        response.addResponseLine("You grab a drink and check Reddit for a while before coming back to it.");
        response.addResponseLine("When you get back, you find it much easier and beat it within a few attempts. Yes!");
        response.setStatChange(2, 0, 1, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("try to push through");
        response.setTrigger("2");
        response.addResponseLine("You struggle for many grueling hours, but eventually manage to beat it.");
        response.addResponseLine("While it was frustrating and challenging, you're exceedingly proud of having won.");
        response.setStatChange(3, -2, 5, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("go do something else");
        response.setTrigger("3");
        response.addResponseLine("You just give up and walk away. Oh well.");
        response.setStatChange(0, 1, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #75

        myEvent = new MyEvent("You pick up a new competitive PVP game and decide to get ranked.");
        myEvent.addLine("What kind of strategy do you employ?");

        response = new Response("focus on your skill");
        response.setTrigger("1");
        response.addResponseLine("You try your hardest, and while you feel you played well, your team mates got in the way.");
        response.addResponseLine("Sometimes you didn't perform as well as you'd hope, either. You get ranked in bronze.");
        response.addResponseLine("You feel frustrated, embarrassed, and discouraged.");
        response.setStatChange(-4, -2, -5, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("focus on strategy");
        response.setTrigger("2");
        response.addResponseLine("You focus on playing strategically, outthinking your opponents and playing around their weaknesses and your strengths.");
        response.addResponseLine("You do very well over all and get placed into platinum. Awesome!");
        response.setStatChange(3, 1, 5, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("focus on team play");
        response.setTrigger("3");
        response.addResponseLine("You try to focus on supporting your team mates, but they're very inconsistent.");
        response.addResponseLine("Even though you get some good matches, over all you lose more.");
        response.addResponseLine("You're placed near the bottom of silver... oh well.");
        response.setStatChange(1, -1, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("group up with your friends");
        response.setTrigger("4");
        response.addResponseLine("While you feel your friends might be holding you back a bit, you have a really good time.");
        response.addResponseLine("You rank near the top of silver... not great, but at least you had fun.");
        response.setStatChange(3, 1, -1, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #76

        myEvent = new MyEvent("You need to swap some parts out in your computer.");
        myEvent.addLine("You spend the afternoon working at it.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You find the work strangely soothing and relaxing.");
        response.addResponseLine("As you work and listen to some good music, you feel your cares fade away some.");
        response.setStatChange(0, -4, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #77

        myEvent = new MyEvent("A piece of software stops working on your computer and you get stumped fixing it.");
        myEvent.addLine("How do you resolve it?");

        response = new Response("look for help online");
        response.setTrigger("1");
        response.addResponseLine("No one online has the exact answer, but you eventually find enough clues to solve it.");
        response.addResponseLine("You're happy to have it working again.");
        response.setStatChange(1, 1, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("look at how its made for answers");
        response.setTrigger("2");
        response.addResponseLine("It takes you hours of pulling it apart, but you eventually find the solution and fix it on your own.");
        response.addResponseLine("You're very proud of the hard work paying off without anyone's help even though it was frustrating.");
        response.setStatChange(1, -2, 4, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #78

        myEvent = new MyEvent("A distant relative dies and leaves you a spooky old house.");
        myEvent.addLine("It unfortunately isn't in good enough shape to live in. What do you do with it?");

        response = new Response("sell it");
        response.setTrigger("1");
        response.addResponseLine("You sell it and make a good chunk of money.");
        response.addResponseLine("Sometimes you find yourself feeling uneasy about the decision, almost like someone is watching and judging....");
        response.setStatChange(0, 0, 0, 1, 20); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("keep it");
        response.setTrigger("2");
        response.addResponseLine("You keep the house in your name even though you don't live there.");
        response.addResponseLine("You feel good about having it in the family. Maybe you'll do something with it someday.");
        response.setStatChange(1, 0, 2, -1, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #79

        myEvent = new MyEvent("A taco place opens next to where you're living.");
        myEvent.addLine("You try it out and it's great.");

        response = new Response("eat there every night for a week");
        response.setTrigger("1");
        response.addResponseLine("The food is SO GOOD but you get a little tired of it and take a break.");
        response.setStatChange(3, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("eat there once in a while");
        response.setTrigger("2");
        response.addResponseLine("You eat there on special occassions and it's always great.");
        response.setStatChange(3, 1, 2, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #80

        myEvent = new MyEvent("Coffee at work is now free!");
        myEvent.addLine("Do you partake?");

        response = new Response("start drinking coffee every morning");
        response.setTrigger("1");
        response.addResponseLine("You become addicted to coffee. Once in a while it gives you migraines.");
        response.addResponseLine("However, you can now get more done each day.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            CoffeeAddiction();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("keep your usual routine");
        response.setTrigger("2");
        response.addResponseLine("You are sometimes envious of your coworker's coffee, but you don't need it.");
        response.setStatChange(0, 0, 2, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #81

        myEvent = new MyEvent("Your landlord calls to say she's coming over to look at something and needs to get into your apartment.");
        myEvent.addLine("Your place is kinda messy and you feel stressed about her seeing it; how do you react?");

        response = new Response("frantically clean");
        response.setTrigger("1");
        response.addResponseLine("You spend hours cleaning everything. She then calls to say nevermind, she can't make it.");
        response.addResponseLine("You're frustrated, but at least you cleaned everything. It cost you a lot of time, though.");
        response.setStatChange(-3, -3, 3, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("leave it messy");
        response.setTrigger("2");
        response.addResponseLine("She comes over briefly and shoots you a judgey look about the mess, but says nothing about it.");
        response.setStatChange(0, -1, -3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #82

        myEvent = new MyEvent("The road you take to work is closed without any notice.");
        myEvent.addLine("You're almost certainly going to be late.");

        response = new Response("call and let the boss know");
        response.setTrigger("1");
        response.addResponseLine("Your boss sounds pissed, and you do indeed show up late. Shit.");
        response.setStatChange(-2, -4, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("speed the whole way on the alternate route");
        response.setTrigger("2");
        response.addResponseLine("You make it just in the nick of time. Awesome!");
        response.setStatChange(0, 0, 2, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #83

        myEvent = new MyEvent("Your boss quits and you get a new boss at work.");
        myEvent.addLine("How do you approach them?");

        response = new Response("find them immediately and try to make friends");
        response.setTrigger("1");
        response.addResponseLine("They don't seem to particularly care about talking to you, and brush you off. Oh well.");
        response.setStatChange(-1, -3, 0, -1, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("do your job as always");
        response.setTrigger("2");
        response.addResponseLine("You notice the boss looking over your work, and they seem impressed. Excellent!");
        response.setStatChange(1, 2, 4, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #84

        myEvent = new MyEvent("You get assigned a project way outside your job description at work, and you're not sure you have time for it.");
        myEvent.addLine("What do you do?");

        response = new Response("do it anyway as best you can");
        response.setTrigger("1");
        response.addResponseLine("Work becomes much more stressful as you try to keep up with the workload.");
        response.addResponseLine("Eventually you get it done, though, and you get a bonus when you finish. [$4]");
        response.setStatChange(-4, -8, 2, 0, 4); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("tell your boss you can't do it");
        response.setTrigger("2");
        response.addResponseLine("To your dismay, your boss immediately fires you and says they'll find someone else who can do the work.");
        response.addResponseLine("Nothing you say matters. You lose your job.");
        response.setStatChange(-5, -5, -3, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            Fired();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("ask if someone else can help out");
        response.setTrigger("2");
        response.addResponseLine("You get told in no uncertain terms that if you don't do it you'll be fired.");
        response.addResponseLine("It's hell to find time for it, but you eventually finish the project.");
        response.setStatChange(-5, -9, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #85

        myEvent = new MyEvent("You get a call saying your girlfriend has been injured in a horrible accident.");
        myEvent.addLine("You rush to the hospital immediately.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("She'll be okay, but is badly injured. You stay by her side until she's better for days.");
        response.addResponseLine("The hospital bill is expensive [-$4].");
        response.setStatChange(-7, -5, 0, 3, -4); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        // EVENT #86

        myEvent = new MyEvent("A sinkhole opens up under your apartment. You have to move out in a hurry.");
        myEvent.addLine("");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You're homeless until you can find a new place.");
        response.setStatChange(-3, -2, -2, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            Homeless();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #87

        myEvent = new MyEvent("You get into an argument with your girlfriend. You know you're right, but she won't admit it.");
        myEvent.addLine("How do you handle it?");

        response = new Response("put your foot down. You won't apologize for something that's not your fault.");
        response.setTrigger("1");
        response.addResponseLine("It turns into a nasty fight that lasts all night. You eventually make up, but your relationship is damaged.");
        response.setStatChange(-10, -5, 3, -3, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("give in. You still don't feel you were wrong, but it's not worth fighting more over.");
        response.setTrigger("2");
        response.addResponseLine("While you still feel very frustrated about the fight, you make up and spend the night cuddling after apologizing to each other.");
        response.setStatChange(-2, -3, -5, 3, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            RelationshipHurt();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #88

        myEvent = new MyEvent("You go to a work Christmas party with their coworkes and spouses.");
        myEvent.addLine("What do you do?");

        response = new Response("sit quietly and leave early");
        response.setTrigger("1");
        response.addResponseLine("You get some free food and booze and then take off.");
        response.addResponseLine("Not the best night, but not the worst.");
        response.addResponseLine("Your boss leaves you a voicemail later, but when you call him back he says nevermind.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("mingle and talk to people");
        response.setTrigger("2");
        response.addResponseLine("You're a little stressed out talking to so many people, but you have a decent time.");
        response.addResponseLine("At the end of the night your boss gives you a Christmas bonus, though- nice! [$4]");
        response.setStatChange(3, -1, 2, 4, 4); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #89

        myEvent = new MyEvent("Your boss pulls you aside and thanks you for your hard work lately.");
        myEvent.addLine("He hands you an envelope and walks away.");

        response = new Response("open it");
        response.setTrigger("1");
        response.addResponseLine("Inside is a check- a bonus for your work- and a small note saying thanks again. Awesome! [$5]");
        response.setStatChange(3, 3, 9, 1, 5); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("toss it in the trash");
        response.setTrigger("2");
        response.addResponseLine("It was probably just a card; at least your boss said some nice stuff, though.");
        response.setStatChange(2, 2, 5, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #90

        myEvent = new MyEvent("You get pulled aside at work at the end of the day.");
        myEvent.addLine("Your boss explains that due to budget reasons you're laid off effective immediately.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You're immediately stressed out and devastated... time to look for a new job.");
        response.setStatChange(-5, -10, -3, -2, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            Fired();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #91

        myEvent = new MyEvent("Lina's phone buzzes while she's in the bathroom and you idly look at the notification.");
        myEvent.addLine("It's her ex-boyfriend texting her... about having fun meeting up last night, when she was supposed to be at work.");
        myEvent.addLine("After confronting her about it, she confesses that she's been cheating on you.");

        response = new Response("break up with her");
        response.setTrigger("1");
        response.addResponseLine("You break up, and while it's not a horrible breakup you feel absolutely worthless and used, like trash.");
        response.addResponseLine("You reset your entire life. Everything feels bleaker and empty now.");
        response.setStatChange(-15, -10, -20, -20, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LoseLina();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("try to fix things");
        response.setTrigger("2");
        response.addResponseLine("You try to fix things with her, but she has no interest and you break up anyway.");
        response.addResponseLine("You feel absolutely worthless and used, like trash.");
        response.addResponseLine("You reset your entire life. Everything feels bleaker and empty now.");
        response.setStatChange(-15, -10, -25, -25, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LoseLina();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #92

        myEvent = new MyEvent("You and Jenna decide to leave it all behind and move around the country together.");
        myEvent.addLine("You pack everything into a vehicle and get ready to go.");

        response = new Response("do it");
        response.setTrigger("1");
        response.addResponseLine("You get in the car, and only look back for a moment.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            GameWin();
            JennaWin();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("back out");
        response.setTrigger("2");
        response.addResponseLine("Even though everything is packed, you can't handle the unknown in front of you.");
        response.addResponseLine("Jenna, however, decides to go on without you. She leaves your life for good.");
        response.setStatChange(-10, -5, -5, -15, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LoseJenna();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #93

        myEvent = new MyEvent("You come home one day and find Jenna lying on the floor unconscious.");
        myEvent.addLine("You try to wake her up, but she won't wake. You call an ambulance.");
        myEvent.addLine("They declare her dead on scene, and find an empty bottle of pills near her.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You knew she was depressed, but were never ready for this.");
        response.addResponseLine("You'd been doing everything you could to help her, and she'd been seeing a counselor, but apparently it wasn't enough.");
        response.setStatChange(-50, -30, -40, -40, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LoseJenna();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #94

        myEvent = new MyEvent("You and Alex come up with a great idea for a book and eventually get around to writing it.");
        myEvent.addLine("Once you finish it, she suggests sending it into a publisher. Do you do it?");

        response = new Response("yes, send it in!");
        response.setTrigger("1");
        response.addResponseLine("You get a letter back from the publishers saying they loved it!");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            AlexWin();
            GameWin();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("no, it's probably not good enough!");
        response.setTrigger("2");
        response.addResponseLine("She calms down from the excitement a bit and agrees that you're right, it's probably not good enough. Still, you wrote something cool and feel good about it.");
        response.setStatChange(4, 2, 5, 5, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #95

        myEvent = new MyEvent("You and Pina start fighting almost daily. The fights are long and emotional and pointless.");
        myEvent.addLine("Eventually it just becomes too much for both of you, and you have no choice but to split ways.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("She leaves, and while it's very emotional you know deep down it's for the best.");
        response.addResponseLine("Unfortunately, that doesn't really stop it from hurting.");
        response.setStatChange(-10, -5, -5, -10, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LosePina();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #96

        myEvent = new MyEvent("One day Alex doesn't answer your calls or texts, so you swing by her place.");
        myEvent.addLine("You knock, but there's no answer, and when you look in the window the entire apartment is empty.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You have no idea where she went, but she disappears from your life entirely. You never hear from her again.");
        response.addResponseLine("You are, of course, devastated.");
        response.setStatChange(-20, -15, -20, -20, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LoseAlex();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #97

        myEvent = new MyEvent("Pina sits you down over dinner and tells you she's leaving to travel the world.");
        myEvent.addLine("She invites you to come with her. How do you respond?");

        response = new Response("do it");
        response.setTrigger("1");
        response.addResponseLine("You pack light and get on a plane with her bound for Thailand.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            GameWin();
            PinaWin();
            return -1;
        });
        myEvent.addResponse(response);


        response = new Response("stay behind");
        response.setTrigger("2");
        response.addResponseLine("You chose to stay behind, and soon she leaves. You try to keep up with each other long distance, but it's hard.");
        response.addResponseLine("Before long you just drift apart. She is busy and meets new people on her travels, and you have to move on.");
        response.setStatChange(-10, -10, -20, -20, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LosePina();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #98

        myEvent = new MyEvent("You always knew Sammy did drugs, but her addiction becomes so bad that she can no longer work or sustain her life.");
        myEvent.addLine("You try to help her as much as you can, but she pushes you away and eventually you just stop trying to help.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You still see her around town sometimes- you suspect she's homeless and she looks sick, but she never talks to you.");
        response.setStatChange(-10, -15, -7, -10, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            BreakUpEvent();
            LoseSammy();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #99

        myEvent = new MyEvent("You surprise Krissi one night by asking her to marry you. You'd talked about it before, but you wanted to make sure it was a surprise.");
        myEvent.addLine("She looks elated and says yes immediately.");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You get married soon at a cool burnt out old church you once visited.");
        response.addResponseLine("Friends and family show up from all over, and it's a wonderful event.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            GameWin();
            KrissiWin();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);


        // EVENT #100

        myEvent = new MyEvent("You graduate from university today!");
        myEvent.addLine("");

        response = new Response("continue");
        response.setTrigger("1");
        response.addResponseLine("You're filled with pride at your achievement, as are your family and friends.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            GameWin();
            SchoolWin();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);

        /* EXAMPLE EVENT
        * 

        // EVENT #

        myEvent = new MyEvent("Scenario");                                                 
        myEvent.addLine("Question?");                                                                                     

        response = new Response("do thing number 1");                                                  
        response.setTrigger("1");                   
        response.addResponseLine("Outcome text.");        
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money                              
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });                         
        myEvent.addResponse(response);               


        response = new Response("do thing number 2");                                                          
        response.setTrigger("2");
        response.addResponseLine("Outcome text.");
        response.setStatChange(0, 0, 0, 0, 0); // Happiness, stress, pride, loneliness, money
        response.setNextEvent(() =>
        {
            MainScreen();
            return -1;
        });
        myEvent.addResponse(response);


        events.Add(myEvent);
        */
    }

    //EVENT OUTCOMES



    void SchoolWin()
    {
        winText = "After graduating, you realize you can do almost anything with your life. \n You don't know what your future will hold, but you realize you're not beholden to anyone for it. \n You stop trying to find someone so hard and just focus on making yourself happy. \n While the future is unknown, you are filled with hope and determination for it.";
    }

    void JennaWin()
    {
        winText = "While life on the road is challenging, you and Jenna manage to make it work. \n You spend the rest of your life shouldering most of the responsibilities for the two of you, but at least you have a good companion on the road. \n You find yourself wondering sometimes what else life could have been, but your busy life keeps you from wondering too much. \n Mostly, you're just happy you have someone by your side.";
    }

    void KrissiWin()
    {
        winText = "While you don't do anything extraordinary or especially meaningful with your lives, you remain happy together. \n Every day is an adventure with each other. \n You chose not to have children, but are great for your nieces and nephews. \n Every challenge you tackle, you tackle together, and while life isn't easy it is always worth it to be with each other, right until the end.";
    }

    void AlexWin()
    {
        winText = "With the money from your book being published, you manage to make ends meet and make a life together with Alex. \n While you remain very enamoured with her, you always feel a little more like business partners than a real relationship. \n Still, you make good money and always have a good time with her, and manage to live out your life in relative comfort and happiness.";
    }

    void PinaWin()
    {
        winText = "The two of you start in Thailand visiting her family, but eventually travel all over the world. \n You find yourself as in love with travel as Pina is with you. \n The two of you travel for many years before settling down in a foreign country. \n You do well for yourselves and provide a foster home for traveling young people. \n You don't know if you're happy, but you are satisfied with your life and what you did with it.";
    }

    void LoseJenna()
    {
        knownWomen.Remove("Jenna");
    }

    void LoseLina()
    {
        knownWomen.Remove("Lina");
    }

    void LoseAlex()
    {
        knownWomen.Remove("Alex");
    }

    void LosePina()
    {
        knownWomen.Remove("Pina");
    }

    void LoseSammy()
    {
        knownWomen.Remove("Sammy");
    }

    void BreakUpEvent()
    {
        girlfriend = false;
        girlfriendName = "none";
        MainScreen();
    }

    void RelationshipHurt()
    {
        if (girlfriendName == "Lina")
        {
            linaAttitude -= 10;
        }
        if (girlfriendName == "Jenna")
        {
            jennaAttitude -= 10;
        }
        if (girlfriendName == "Alex")
        {
            alexAttitude -= 10;
        }
        if (girlfriendName == "Pina")
        {
            pinaAttitude -= 10;
        }
        if (girlfriendName == "Sammy")
        {
            sammyAttitude -= 10;
        }
        if (girlfriendName == "Krissi")
        {
            krissiAttitude -= 10;
        }
        MainScreen();
    }

    void Homeless()
    {
        currentHouse = House.None;
        MainScreen();
    }

    void Fired()
    {
        currentJob = Job.None;
        MainScreen();
    }

    void NoAddiction()
    {
        coffeeAddict = false;
        MainScreen();
    }

    void CoffeeAddiction()
    {
        coffeeAddict = true;
        MainScreen();
    }

    void Lucky()
    {
        dailyRand = 100;
        MainScreen();
    }

    void SpoonsIncrease()
    {
        spoons++;
        MainScreen();
    }

    void LieGamble()
    {
        RefreshScreen();
        if (dailyRand > 50)
        {
            Terminal.WriteLine("The university doesn't believe your lie. You are kicked out!");
            Invoke("KickedFromSchool", 3f);
            happiness = happiness - 10;
            stress = stress + 3;
            pride = pride - 15;
        }
        else 
        {
            Terminal.WriteLine("The university believes your lie. You got away with it!");
            happiness++;
            stress++;
            pride = pride - 7;
            Invoke("MainScreen", 3f);
        }
    }

    void KickedFromSchool()
    {
        school = false;
        MainScreen();
    }

    void DailyHappinessBoost()
    {
        extraHappiness++;
    }

    void ExtraExpense()
    {
        extraExpenses++;
        MainScreen();
    }

    void DelayedMeetGirl()
    {
        Invoke("MeetGirl", 3f);
    }

    void SpoonsDownCheck()
    {
        if (spoonsDown == true)
        {
            AddSpace();
            Terminal.WriteLine("Your injury prevents you from accomplishing as much today.");
            spoons -= 1;
            spoonsDownTimer -= 1;
            if (spoonsDownTimer <= 0)
            {
                spoonsDown = false;
            }
        }
    }

    void MoneyDownCheck()
    {
        if (moneyDown == true)
        {
            AddSpace();
            Terminal.WriteLine("You make an extra payment today.");
            money--;
            moneyDownTimer -= 1;
            if (moneyDownTimer <= 0)
            {
                moneyDown = false;
            }
        }
    }

    void MoneyDown()
    {
        moneyDown = true;
        moneyDownTimer = 4;
        MainScreen();
        money--;
    }

    void SpoonsDown()
    {
        spoonsDown = true;
        spoonsDownTimer = 4;
        MainScreen();
        spoons--;
    }

    void ParentsAttitudeIncrease()
    {
        parentsAttitude += 10;
        MainScreen();
    }

    void ParentsAttitudeDecrease()
    {
        parentsAttitude -= 10;
        MainScreen();
    }

    void SpoonsDecrease()
    {
        spoons -= 1;
        MainScreen();
    }

    void FriendAttitudeUp()
    {
        friendAttitude += 5;
        MainScreen();
    }

    void FriendAttitudeDown()
    {
        friendAttitude -= 5;
        MainScreen();
    }

    void GameOver()
    {
        RefreshScreen();
        Terminal.WriteLine("Game Over!");
        AddSpace();
        Terminal.WriteLine("Type 'exit' to exit game");
        currentScreen = Screen.Gameover;
    }

    void GameWin()
    {
        currentScreen = Screen.Gameover;
        RefreshScreen();
        Terminal.WriteLine("You have finished the game!");
        AddSpace();
        Terminal.WriteLine(winText);
        AddSpace();
        Terminal.WriteLine("Type 'credits' to view the game credits and end the game.");
    }

    void Credits()
    {
        Terminal.ClearScreen();
        while (creditsTimer < 30)
        {
            InvokeRepeating("AddSpace", 1f, 2f);
            creditsTimer++;
        }
        Terminal.WriteLine("Thanks for playing ARE YOU ALONE? !"); //FIXME ADD THIS STUFF TO A TIMER AND ADD THE REST
        Terminal.WriteLine("I hope you enjoyed the game!");
    }






    int StartEvent(int eventId)
    {
        if (eventId < 0)
        {
            return eventId;
        }
        RefreshScreen();
        currentEvent = events[eventId];
        Terminal.WriteLine(currentEvent.question + "\n");

        foreach (Response response in currentEvent.responses)
        {
            Terminal.WriteLine("Press " + response.trigger + " to " + response.name);
        }
        return eventId;
    }

    void ExecuteEvent(string input)
    {
        foreach (Response response in currentEvent.responses)
        {
            if (input == response.trigger)
            {
                ExecuteResponse(response);
                return;
            }
        }
        Terminal.WriteLine("Invalid Entry.");
    }

    void ExecuteResponse(Response response)
    {
        updateStats(response);
        Terminal.ClearScreen();
        RefreshScreen(); // Keep screen format correct
        Terminal.WriteLine(response.response);
        PrintResponses(response);
    }

    void PrintResponses(Response response)
    {
        StartCoroutine(CallStartEvent(response));
    }
    IEnumerator CallStartEvent(Response response)
    {
        yield return new WaitForSeconds(4f);
        StartEvent(response.nextEvent());
    }

    void updateStats(Response response)
    {
        happiness += response.happinessChange;
        stress += response.stressChange;
        pride += response.prideChange;
        loneliness += response.lonelinessChange;
        money += response.moneyChange;
    }






}






