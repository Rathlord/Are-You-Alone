using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {

    // Many  game variables will be stored here

    [SerializeField] int spoons = 3;
    enum Screen { Tutorial, Gameplay, Night, NightStats, Moving, Gameover, RelationshipsScreen, Flirt, FlirtLina, FlirtJenna, FlirtAlex, FlirtPina, FlirtSammy, FlirtWinry, Date, AskOut };
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
    int earnings = 0;
    int giftVar = 0;
    int askOutVar = 0;

    // Special value, daily random number, impacts many things

    [SerializeField] int dailyRand;

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
    bool flirtWinry = false;

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
    [SerializeField] int workAbscence = 0;
    [SerializeField] int schoolAbscence = 0;
    [SerializeField] int friendAttitude = 50;
    [SerializeField] int onlineAttitude = 50;
    [SerializeField] int parentsAttitude = 30;
    bool friendToday;
    bool onlineToday;
    bool flirtToday;
    bool dateToday;

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

    // Winry vars

    int winryDifficulty = 150;
    int winryAttitude = 0;
    bool winryLove = false;
    int winryJokeMod = 3;
    int winryGiftMod = 1;
    int winrySweetMod = 3;

    // Use this for initialization
    void Start()
    {
        Terminal.WriteLine("Are You Alone?");
        AddSpace();
        Tutorial();
        currentHouse = House.Parents;
        dailyRand = UnityEngine.Random.Range(0, 101);
        currentJob = Job.None;
        currentScreen = Screen.Tutorial;
        yesterdayStress = stress;
        yesterdayPride = pride;
        yesterdayHappiness = happiness;
        employedYesterday = false;
        unknownWomen.Add("Lina");
        unknownWomen.Add("Jenna");
        unknownWomen.Add("Alex");
        unknownWomen.Add("Pina");
        unknownWomen.Add("Sammy");
        unknownWomen.Add("Winry");
    }
		
	// TODO ENSURE ABSENCES ARE HANDLED WITH NEW JOBS
    // TODO LIMIT MORE ACTIONS WITH STATS
    // TODO IMPLEMENT "IN LOVE"
    // TODO IMPLEMENT RANDOM MESSAGES FOR POST-DAY SCREEN
    // TODO IMPLEMENT RANDOM DAILY EVENTS
    // TODO EASE UP LONELINESS MAYBE?
    // TODO IMPLEMENT STAT METERS
    // TODO CAP STATS AT -100/100 & penalties/buffs for thems
    // TODO STOP FLIRTING WITH SAME GIRL TWICE IN A DAY
    // TODO PRIDE IS TOO EASY
	// TODO CHECK EVICTIONS
	// TODO SCORE DISPLAY
	// TODO WINS/LOSSES
	// TODO PARENTS ATTITUDE CHECKS  (parents now lose all attitude if you quit school while living with them)
	// TODO SHOW ACTIONS ON MAIN?
	// TODO CHECK IF PLAYER ACTUALLY WANTS NEW JOB WHEN CURRENT JOB = TRUE
	// TODO DID I IMPLEMENT PARENTS ATTITUDE AFTER DROPPING OUT?
	// TODO ALLOW VOLUNTARY QUITTING SCHOOL/WORK
	// TODO MAKE JEALOUS COME BEFORE MISSING, DUH

	//    void Event1()
	//    {
	//        Terminal.WriteLine("You run into your friend at the coffee shop. He says he's sorry for what he did and wants to be friends again.");
	//        Terminal.WriteLine("Enter '1' to be friends again, or '2' to walk away from him");
	//        //input manager handles the input
	//        answer = Input;
	//        if (answer = 1)
	//       {
	//           happiness = (happiness + 5);
	//           stress = (stress - 5);
	//           friendAttitude = 0;
	//        }
    //        else
	//       {
	//            happiness = (happiness - 3);
    //            stress = (stress + 2);
	//        }
	//    }

    void WomenList()
    {
        foreach (string women in knownWomen)
        {
            Terminal.WriteLine(women);
        }
    }

    void MainScreen()
    {
        currentScreen = Screen.Gameplay;
        RefreshScreen();
        Terminal.WriteLine("Enter an action!                (You can enter 'actions' to see a list)");
        AddSpace();
        Terminal.WriteLine("These are the people you know:");      //TODO add list of known women
        AddSpace();
        WomenList();
        AddSpace();
        if (schoolDay == true && school == true && schooledToday == false)
        {
            Terminal.WriteLine("Today is a school day. You should go to school");
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
            Invoke("Night", 5f);
        }
    }

    // Morning Cleanup
    void Morning()
    {
        RefreshScreen();
        Terminal.WriteLine("You wake up.");
        turn++;
        spoons = 3;
        CheckDays();
        dailyRand = UnityEngine.Random.Range(0, 101);
        Attitudes();
        Invoke("MainScreen", 3f);
        yesterdayStress = stress;
        yesterdayHappiness = happiness;
        yesterdayPride = pride;
        flirted = false;
        flirtLina = false;
        flirtJenna = false;
        flirtAlex = false;
        flirtPina = false;
        flirtSammy = false;
        flirtWinry = false;
        if (employed == true)
        {
            employedYesterday = true;
        }
        else
        {
            employedYesterday = false;
        }
    }

    void Night()
    {
        currentScreen = Screen.Night;
        RefreshScreen();
        totalExpenses = (dailyExpenses + housingExpenses);
        AddSpace();
        Terminal.WriteLine("Your expense for the day is -" + totalExpenses + "$");
        money = (money - totalExpenses);
        if (employed == true && workDay == true && workedToday == true)
        {
            Terminal.WriteLine("You earned " + earnings + "$ today.");
            money = (money + earnings);
        }
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
        int picker = UnityEngine.Random.Range(0, 4);
        string[] happinessGreat = { "You feel ecstatic", "You fall asleep with a big grin", "You feel like partying!", "Today was an awesome day" };
        string[] happinessGood = { "You feel happy", "This was a good day", "You fall asleep with a slight smile", "You're feeling good" };
        string[] happinessBad = { "Today wasn't a great day", "You're not feeling very well", "You feel sadder", "Things aren't going so well" };
        string[] happinessTerrible = { "Life doesn't feel worth living", "You're having dark thoughts", "You feel awful", "Why bother sleeping..." };
        string[] stressGreat = { "You feel like you can do anything!", "You're ready to tackle life", "You feel studious", "You're carefree!" };
        string[] stressGood = { "You feel chill", "You feel relaxed", "You feel like going out and doing something", "You're not worried about life" };
        string[] stressBad = { "You feel kind of stressed", "You're having trouble concentrating", "You're under a lot of pressure", "You don't feel like doing much" };
        string[] stressTerrible = { "You're done with life's shit", "Why bother with anything...", "You don't wanna get out of bed", "You're buckling under the pressure" };
        string[] prideGreat = { "You're proud of yourself", "You think you have a lot to offer", "You know you're the best", "You're all you need" };
        string[] prideGood = { "You think you're doing a good job", "You're confident in yourself", "You think you're pretty cool", "You feel strong" };
        string[] prideBad = { "You know you're not doing your best", "You wish you felt more fulfilled", "What do you have to offer?", "You don't feel like you contribute to society" };
        string[] prideTerrible = { "You're useless", "You feel worthless", "You don't feel like you've accomplished anything at all", "You feel like a waste of breath" };
        HappinessCheck(picker, happinessGreat, happinessGood, happinessBad, happinessTerrible);
        StressChecker(picker, stressGreat, stressGood, stressBad, stressTerrible);
        PrideCheck(picker, prideGreat, prideGood, prideBad, prideTerrible);
        AddSpace();
        ImprovementCheck();
        AddSpace();
        Abscences();
        Terminal.WriteLine("Write 'next' to continue to a new day.");
    }

    void Tutorial()
    {
        currentScreen = Screen.Tutorial;
        RefreshScreen();
        Terminal.WriteLine("Play the game by entering commands on the keyboard.");
        Terminal.WriteLine("You can type 'home' from almost any screen to return to the main screen.");
        AddSpace();
        Terminal.WriteLine("You can perform three actions per day. These are called spoons.");
        Terminal.WriteLine("Spoons are tracked at the top right of your screen.");
        AddSpace();
        Terminal.WriteLine("On the main screen you can type 'actions' to list what you can do.");
        Terminal.WriteLine("Most actions can only be performed once per day.");
        AddSpace();
        Terminal.WriteLine("You can learn about most actions by typing them with an exclamation mark.");
        Terminal.WriteLine("For instance, type '!home' to learn about the home command.");
        AddSpace();
        Terminal.WriteLine("Type 'next' for the next page.");
    }

    void TutorialTwo()
    {
        currentScreen = Screen.Gameplay;
        RefreshScreen();
        Terminal.WriteLine("You can 'Win' the game by becoming happy or giving yourself a good future.");
        Terminal.WriteLine("You will lose the game if you become too sad or otherwise can't continue.");
        Terminal.WriteLine("If your stress, pride, or happiness get too low you won't be able to do things.");
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
        if (money > 100)
        {
            Terminal.WriteLine("You feel rich! You spend some of your money. It makes you feel good.");
            happiness = (happiness + 15);
            stress = (stress - 5);
            pride = (pride + 10);
            money = (money - 25);
        }
        else if (money > 20)
        {
            Terminal.WriteLine("You feel financially stable.");
            happiness = (happiness + 5);
            stress = (stress - 2);
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
            Terminal.WriteLine("You spent some time with your friend today.");
        }
        if (onlineToday == true)
        {
            Terminal.WriteLine("You spent some time online today.");
        }
        if (friendToday == false && onlineToday == false)
        {
            Terminal.WriteLine("You didn't talk to any friends today. You miss them.");
            loneliness = (loneliness - 4);
        }
        if (flirtToday == false && dateToday == false)
        {
            Terminal.WriteLine("You miss talking to someone special.");
            loneliness = (loneliness - 7);
        }
        if (loneliness < 0)
        {
            Terminal.WriteLine("You feel disconnected and lonely.");
            happiness = (happiness - 10);
            stress = (stress + 1);
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
            Terminal.WriteLine("You rent your own place. It's not much, but at least you're independent");
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
            Terminal.WriteLine("They've kicked you out! You're homeless!");
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
        if (currentHouse == House.Rent && money < 1)
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
            Terminal.WriteLine("You feel happier than before");
        }
        else if (happiness == yesterdayHappiness)
        {
            Terminal.WriteLine("You feel about the same happiness as before");
        }
        else if (happiness < yesterdayHappiness)
        {
            Terminal.WriteLine("You feel sadder than before");
        }
        if (stress > yesterdayStress)
        {
            Terminal.WriteLine("You feel less stressed than before");
        }
        else if (stress == yesterdayStress)
        {
            Terminal.WriteLine("Your stress level feels constant");
        }
        else if (stress < yesterdayStress)
        {
            Terminal.WriteLine("You feel more stressed out");
        }
        if (pride > yesterdayPride)
        {
            Terminal.WriteLine("You feel more proud of yourself than before");
        }
        else if (pride == yesterdayPride)
        {
            Terminal.WriteLine("Your thoughts about yourself haven't changed");
        }
        else if (pride < yesterdayPride)
        {
            Terminal.WriteLine("You feel less proud than before");
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
            loneliness = (loneliness - 5);
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
        GirlfriendAttitude();
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
            if (girlfriendName == "Winry")
            {
                Terminal.WriteLine(WinryStatus());
            }
        }
    }

    string WinryStatus()
    {
        string statusMessage;
        if (winryAttitude > 75 && dailyRand > 50)
        {
            statusMessage = "Winry tells you she's in love with you. It makes you very happy";
            happiness = (happiness + 5);
            pride = (pride + 2);
            stress = (stress + 2);
            loneliness = (loneliness + 3);
        }
        else if (winryAttitude > 75)
        {
            statusMessage = "Winry invites you to stay the night with her, and you both have a great time.";
            happiness = (happiness + 2);
            stress = (stress + 3);
            loneliness = (loneliness + 2);
        }
        else if (winryAttitude > 0 && dailyRand > 50)
        {
            statusMessage = "Winry calls to say goodnight before you fall asleep.";
            happiness = (happiness + 1);
            loneliness = (loneliness + 1);
        }
        else if (winryAttitude > 0)
        {
            statusMessage = "Winry texts you to say goodnight.";
        }
        else if (dailyRand > 50)
        {
            statusMessage = "Winry isn't talking to you tonight.";
        }
        else
        {
            statusMessage = "Winry breaks up with you. You're heartbroken.";
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
            girlfriendName = "none";
        }
        return statusMessage;
    }

    void RelationshipAttitudes()
    {
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
            linaAttitude = (linaAttitude - 1);
        }
		if (knownWomen.Contains("Alex") == true && flirted == true && dailyRand > 80 && flirtAlex == false)
		{
			Terminal.WriteLine("Alex is jealous you spent time with someone else today.");
			linaAttitude = (linaAttitude - 4);
		}
        else if (knownWomen.Contains("Alex") == true && flirtAlex == false)
        {
            Terminal.WriteLine("You didn't spend time with Alex today. She misses you some.");
            linaAttitude = (linaAttitude - 1);
        }
		if (knownWomen.Contains("Pina") == true && flirted == true && flirtPina == false)
		{
			Terminal.WriteLine("Pina is very jealous you spent time with someone else today.");
			linaAttitude = (linaAttitude - 8);
		}
        else if (knownWomen.Contains("Pina") == true && flirtPina == false)
        {
            Terminal.WriteLine("You didn't spend time with Pina today. She misses you.");
            linaAttitude = (linaAttitude - 2);
        }
        if (knownWomen.Contains("Winry") == true && flirtWinry == false)
        {
            Terminal.WriteLine("You didn't spend time with Winry today. She misses you.");
            linaAttitude = (linaAttitude - 1);
        }
    }

    void Abscences() // Checks if the player showed up to work/school and fires them if they've missed too much
    {
        if (workedToday == false && workDay == true && employedYesterday == true)
        {
            workAbscence++;
        }
        if (schooledToday == false && schoolDay == false)
        {
            schoolAbscence++;
        }
        AddSpace();
        workedToday = false;
        schooledToday = false;
        if (workAbscence > 5)
        {
            Terminal.WriteLine("You get fired for not showing up to work!");
            AddSpace();
            currentJob = Job.None;
            employed = false;
			workAbscence = 0;
        }
        if (schoolAbscence > 5)
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

    void OnUserInput(string input)
    {
        if (input == "quit") // Exit the game
        {
            Application.Quit();
        }
        if (input == "home" && (currentScreen == Screen.Gameplay || currentScreen == Screen.Flirt))
        {
            RefreshScreen();
            MainScreen();
        }
        if (input == "hiddenstats") // Dev command to see all stats
        {
            Terminal.WriteLine("stress" + stress + " happiness" + happiness + " pride" + pride);
        }
        else if ((input == "actions" || input == "action") && currentScreen == Screen.Gameplay) // List available actions
        {
            ActionList();
        }
        else if (currentScreen == Screen.Tutorial) // Pass username/go to main screen
        {
            if (input == "next")
            {
                TutorialTwo();
            }
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
        else if (currentScreen == Screen.FlirtWinry)
        {
            WinryFlirt(input);
        }
        else if (currentScreen == Screen.Gameplay && spoons > 0) // Pass info to input manager
        {
            InputManager(input);
        }
        else if (currentScreen == Screen.Gameplay && spoons <= 0) // Stop user from doing actions if out of spoons
        {
            RefreshScreen();
            Terminal.WriteLine("Out of spoons.");
            Invoke("MainScreen", 1f);
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
            throw new NotImplementedException(); // Handle weird screen cases
        }
    }     // Pass user input to other methods via this

    void ActionList() //List available actions to player TODO show all the time at mainscreen
    {
        RefreshScreen();
        Terminal.WriteLine("You can do the following:");
        AddSpace();
        Terminal.WriteLine("hobby");
        Terminal.WriteLine("move");
        if (onlineToday == false)
        {
            Terminal.WriteLine("online");
        }
        else
        {
            Terminal.WriteLine("*Already went online today*");
        }
        if (friendToday == false)
        {
            Terminal.WriteLine("friend");
        }
        else
        {
            Terminal.WriteLine("*Already interacted with your friend today*");
        }
        Terminal.WriteLine("jobsearch");
        if (school == true && schoolDay == true && schooledToday == false)
        {
            Terminal.WriteLine("school");
        }
        else if (school == false)
        {
            Terminal.WriteLine("*You dropped out of school*");
        }
        else if (schooledToday == true && schoolDay == true)
        {
            Terminal.WriteLine("*You're done with school for today*");
        }
        else
        {
            Terminal.WriteLine("*No school today*");
        }
        if (knownWomen.Count > 0)
        {
            Terminal.WriteLine("flirt");
        }
        else
        {
            Terminal.WriteLine("*You don't know anyone to flirt with*");
        }
        if ((girlfriend == true || knownWomen.Count > 0) && money > 3)
        {
            Terminal.WriteLine("date");
        }
        else if (money < 4)
        {
            Terminal.WriteLine("*You can't afford to go on a date*");
        }
        else
        {
            Terminal.WriteLine("*You don't know anyone to take on a date*");
        }
        if (employed == true  && workedToday == false && workDay == true)
        {
            Terminal.WriteLine("work");
        }
        else if (employed == false)
        {
            Terminal.WriteLine("*You don't have a job*");
        }
        else if (workedToday == true && workDay == true)
        {
            Terminal.WriteLine("*You already worked today*");
        }
        else
        {
            Terminal.WriteLine("*You're not scheduled to work today*");
        }
        AddSpace();  
    }

    void InputManager(string input)
    {
        if (input == "move")
        {
            MoveWhere();
        }
        if (input == "hobby")
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
        if (input == "jobsearch")
        {
            JobSearch();
            spoons--;
        }
        if (input == "school" && school == true && schoolDay == true && schooledToday == false)
        {
            School();
            spoons--;
        }
        if (input == "flirt" && knownWomen.Count > 0)
        {
            Flirt();
            spoons--;
        }
        if (input == "date" && ((girlfriend == true || knownWomen.Count > 0) && money > 0))
        {
            Date();
            spoons--;
        }
        if (input == "work" && employed == true && workedToday == false && workDay == true)
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

    void WomanInfo(string input)
    {
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
        if (input == "!Winry" && knownWomen.Contains("Winry"))
        {
            Terminal.WriteLine("Winry is smart, bold, and honest.");
            Terminal.WriteLine("She is outgoing and somewhat nerdy.");
            Terminal.WriteLine("She has wavy blonde hair and is a little short.");
            Terminal.WriteLine("She is attractive and decisive.");
            Terminal.WriteLine("She's good at cooking and singing.");
            Terminal.WriteLine("She likes having fun debates and listening to music.");
        }
    }

    void Tooltips(string input)
    {
        if (input == "!home")
        {
            Terminal.WriteLine("Return to the main menu");
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

    void Work() // Work your job. TODO: Double shifts at some maybe?
    {
        RefreshScreen();
        Terminal.WriteLine("You go to work...");
        loneliness  = (loneliness + 1);
        workedToday = true;
        if (currentJob == Job.DepartmentStore)
        {
            if (dailyRand == 1)
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
            if (dailyRand == 1)
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
            if (dailyRand == 1)
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
            if (dailyRand == 1)
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
        if (knownWomen.Contains("Winry") == true)
        {
            Terminal.WriteLine("Winry");
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
        if (dateAttitude > 50)
            {
                dateDecline = "She says she doesn't feel up for it tonight.";
            }
        if (dateAttitude > 25)
            {
                dateDecline = "She doesn't feel like she knows you well enough";
            }
        if (dateAttitude > 0)
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
        if (knownWomen.Contains("Winry") == true && input == "Winry")
        {
            RefreshScreen();
            dateAttitude = winryAttitude;
            if (dailyRand + winryAttitude >= (winryDifficulty / 2))
            {
                Terminal.WriteLine(DateRandom());
                flirted = true;
                flirtWinry = true;
                if (dailyRand < 76)
                {
                    winryAttitude = (winryAttitude + 10);
                    loneliness = (loneliness + 10);
                    happiness = (happiness + 9);
                    stress = (stress + 3);
                    pride = (pride + 5);
                }
                else
                {
                    winryAttitude = (winryAttitude - 1);
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
        if (knownWomen.Contains("Winry") == true)
        {
            Terminal.WriteLine("Winry");
        }
        AddSpace();
        Terminal.WriteLine("Chose someone or enter 'home' to go back.");
    }

    void FlirtInput(string input)
    {
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
        if (knownWomen.Contains("Winry") == true && input == "Winry")
        {
            RefreshScreen();
            Terminal.WriteLine("How would you like to flirt with Winry?");
            Terminal.WriteLine("You can:    gift   joke     sweet    relationship");
            WinryFlirt(input);
            happiness = (happiness + 5);
            stress = (stress + 5);
            pride = (pride + 5);
            loneliness = (loneliness + 8);
            flirtWinry = true;
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
        if (dailyRand > 50)
        {
            randomLine = "You surprise her with a boquet of her favorite flowers. She loves them!";
            giftVar = 3;
        }
        if (dailyRand > 25)
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
        if (dailyRand > 50)
        {
            randomLine = "You tell her you think she's beautiful. She blushes.";
        }
        if (dailyRand > 25)
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
        if (dailyRand > 50)
        {
            randomLine = "You tell a joke off a popsicle stick. It's stupid but funny.";
        }
        if (dailyRand > 25)
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
        if (dailyRand > 50)
        {
            randomLine = "You bring her a boquet of her favorite flowers and ask her out.";
        }
        if (dailyRand > 25)
        {
            randomLine = "You're hanging out having a good time, so you just ask her out on the spot.";
        }
        else
        {
            randomLine = "You swing by her place and ask her out.";
        }
        return randomLine;
    }

    void WinryFlirt(string input)
    {
        AddSpace();
        flirtToday = true;
        currentScreen = Screen.FlirtWinry;
        if (input == "gift")
        {
            Terminal.WriteLine(RandomGift());
            winryAttitude = (winryAttitude + winryGiftMod + giftVar);
        }
        else if (input == "joke")
        {
            Terminal.WriteLine(RandomJoke());
            winryAttitude = (winryAttitude + winryJokeMod);
        }
        else if (input == "sweet")
        {
            Terminal.WriteLine(RandomSweet());
            winryAttitude = (winryAttitude + winrySweetMod);
        }
        else if (input == "relationship")
        {
			if (girlfriend == true) 
			{
				Terminal.WriteLine ("You already have a girlfriend! You'd have to break up with her first!");
				return;
			}
            Terminal.WriteLine(RandomAskOut());
            if (dailyRand + winryAttitude >= winryDifficulty)
            {
                Terminal.WriteLine("She says she would love to be your girlfriend and wraps you in a huge hug.");
                girlfriend = true;
                girlfriendName = "Winry";
                Invoke("MainScreen", 3f);
                winryAttitude = (winryAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says you're sweet, but she doesn't think she knows you well enough quite yet.");
                Invoke("MainScreen", 3f);
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
                Invoke("MainScreen", 3f);
                sammyAttitude = (sammyAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She shrugs and tells you she's fine with your current relationship.");
                Invoke("MainScreen", 3f);
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
                Invoke("MainScreen", 3f);
                pinaAttitude = (pinaAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She asks who would like you, and doesn't talk to you anymore that day.");
                Invoke("MainScreen", 3f);
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
                Invoke("MainScreen", 3f);
                alexAttitude = (alexAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says she doesn't really know what she's looking for right now, but that she's flattered.");
                Invoke("MainScreen", 3f);
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
                Invoke("MainScreen", 3f);
                jennaAttitude = (jennaAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says she's not quite ready for that yet.");
                Invoke("MainScreen", 3f);
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
                Invoke("MainScreen", 3f);
                linaAttitude = (linaAttitude + 10);
            }
            else
            {
                Terminal.WriteLine("She says she's just not happy enough right now for that.");
                Invoke("MainScreen", 3f);
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
            Terminal.WriteLine("You get a phone call, but it's a wrong number, but the girl on the other end talks to you for a while.");
            Terminal.WriteLine("She seems bored, and tells you to call her sometime!");
        }
        if (tempName == "Winry")
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
        if (dailyRand > 85 && dailyRand < 90)
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

    void JobSearch()
    {
        RefreshScreen();
        dailyRand = UnityEngine.Random.Range(0, 101);
        Terminal.WriteLine("You search for a job...");
        if (dailyRand == 100)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Paint Store! You're on the schedule soon!");
            happiness = (happiness + 10);
            employed = true;
            currentJob = Job.PaintStore;
        }
        if (dailyRand > 90 && dailyRand <= 93)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Department Store! You're on the schedule soon!");
            happiness = (happiness + 5);
            employed = true;
            currentJob = Job.DepartmentStore;
        }
        if (dailyRand > 96 && dailyRand <= 98)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Lumber Yard! You're on the schedule soon!");
            happiness = (happiness + 7);
            employed = true;
            currentJob = Job.LumberYard;
        }
        if (dailyRand >93 && dailyRand <= 95)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Pizza Place! You're on the schedule soon!");
            happiness = (happiness + 7);
            employed = true;
            currentJob = Job.PizzaPlace;
        }
        if (dailyRand == 96)
        {
            AddSpace();
            Terminal.WriteLine("You get a job as a Traveling Salesman! You're on the schedule soon!");
            happiness = (happiness + 10);
            employed = true;
            currentJob = Job.TravelingSales;
        }
        if (dailyRand == 99)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Game Company! You're on the schedule soon!");
            happiness = (happiness + 15);
            employed = true;
            currentJob = Job.GameCompany;
        }
        else if (dailyRand <= 90)
        {
            AddSpace();
            Terminal.WriteLine("You don't have any luck, though...");
            happiness--;
            stress--;
            pride++;
        }
        AddSpace();
        Terminal.WriteLine("Continue to 'home' or 'actions' from here.");
    } // TODO Ask if player wants to take new job

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
        else if (dailyRand == 99)
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
        dailyRand = UnityEngine.Random.Range(0, 101);
        RefreshScreen();
        loneliness = (loneliness - 1);
        if (dailyRand > 90)
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
        else if(dailyRand > 0)
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
        Terminal.WriteLine("Are You Alone?            Current Money: " + money +"$           Current Spoons: " + spoons);
        Terminal.WriteLine("           Type 'home' to get back to the home screen at any time.  ");
        Terminal.WriteLine("------------------------------------------------------------------------");
        AddSpace();
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
    void Update () {
        if (Input.GetKey(KeyCode.Escape)) // Exit game on Escape
        {
            Application.Quit();
        }
		if (happiness < -100) // Game over or save if determination
        {
            if (determination > 0)
            {
                determination--;
                happiness = (happiness + 10);
            }
            else
            {
                RefreshScreen();
                Terminal.WriteLine("You can't keep going.");
                Terminal.WriteLine("Game Over!");
                AddSpace();
                Terminal.WriteLine("Type 'quit' to exit game");
                currentScreen = Screen.Gameover;
            }
        }
        if (pride >= 100) // Grant determination
        {
            pride = (pride - 25);
            determination++;
            AddSpace();
            Terminal.WriteLine("You are filled with Determination!");
            AddSpace();
        }
	}
}
