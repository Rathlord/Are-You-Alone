using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {

    // Many  game variables will be stored here

    [SerializeField] int spoons = 3;
    [SerializeField] string character = "default";
    enum Screen { Name, Gameplay, Night, Gameover };
    Screen currentScreen;
    [SerializeField] int turn = 0;
    [SerializeField] int money = 25;
    bool schoolDay = false;
    bool workDay = false;
    bool workedToday;
    bool schooledToday;

    // Special value, daily random number, impacts many things

    [SerializeField] int dailyRand;

    // Many character variables will be stored here

    bool school = true;
    bool girlfriend = false;
    bool employed = false;
    int knownGirls = 1;
    enum House { Parents, Friend, Rent, None };
    House currentHouse;
    enum Job { DepartmentStore, LumberYard, PaintStore, PizzaPlace, TravelingSales, GameCompany, None }
    Job currentJob;
    [SerializeField] int focus = 50;
    [SerializeField] int happiness = 50;
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

    int yesterdayFocus;
    int yesterdayHappiness;
    int yesterdayPride;

    // List of girls to know

    bool Lina = true;
    bool Jenna = false;
    bool Alex = false;
    bool Pina = false;
    bool Sammy = false;
    bool Winry = false;

    // Use this for initialization
    void Start()
    {
        Terminal.WriteLine("Are You Alone?");
        AddSpace();
        DisplayIntro();
        currentHouse = House.Parents;
        dailyRand = UnityEngine.Random.Range(0, 101);
    }

    void MainScreen()
    {
        currentScreen = Screen.Gameplay;
        RefreshScreen();
        if (schoolDay == true && school == true)
        {
            Terminal.WriteLine("Today is a school day. You should go to school");
        }
        else if (schooledToday == true)
        {
            Terminal.WriteLine("You went to university today.");
        }
        if (workDay == true && employed == true)
        {
            Terminal.WriteLine("Today is a work day. You should go to work today");
        }
        else if (workedToday == true)
        {
            Terminal.WriteLine("You worked today.");
        }
        AddSpace();
        Terminal.WriteLine("These are the people you know:");
        GirlList();
        AddSpace();
        Terminal.WriteLine("Enter an action!");
        Terminal.WriteLine("(You can enter 'actions' to see a list)");
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
        Abscences();
        Attitudes();
        Invoke("MainScreen", 3f);
        yesterdayFocus = focus;
        yesterdayHappiness = happiness;
        yesterdayPride = pride;
    }

    void Night()
    {
        currentScreen = Screen.Night;
        RefreshScreen();
        AddSpace();
        KickedOutCheck();
        AddSpace();
        HousingCheck();
        AddSpace();
        LonelinessCheck();
        AddSpace();
        FulfillmentCheck();
        AddSpace();
        int picker = UnityEngine.Random.Range(0, 4);
        string[] happinessGreat = { "You feel ecstatic", "You fall asleep with a big grin", "You feel like partying!", "Today was an awesome day" };
        string[] happinessGood = { "You feel happier", "This was a good day", "You fall asleep with a slight smile", "You're feeling good" };
        string[] happinessBad = { "Today wasn't a great day", "You're not feeling very well", "You feel sadder", "Things aren't going so well" };
        string[] happinessTerrible = { "Life doesn't feel worth living", "You're having dark thoughts", "You feel awful", "Why bother sleeping..." };
        string[] focusGreat = { "You feel like you can do anything!", "You're ready to tackle life", "You feel studious", "You have a great work ethic right now" };
        string[] focusGood = { "You feel focused", "Your concentration feels good", "You feel like going out and doing something", "Your attention span feels good" };
        string[] focusBad = { "You feel kind of scattered", "You're having trouble concentrating", "You think there's something you needed to do... but you forgot", "You don't feel like doing much" };
        string[] focusTerrible = { "You're done with life's shit", "Why bother with anything...", "You don't wanna get out of bed", "You feel antisocial" };
        string[] prideGreat = { "You're proud of yourself", "You think you have a lot to offer", "You know you're the best", "You're all you need" };
        string[] prideGood = { "You think you're doing a good job", "You're confident in yourself", "You think you're pretty cool", "You feel strong" };
        string[] prideBad = { "You know you're not doing your best", "You wish you felt more fulfilled", "What do you have to offer?", "You don't feel like you contribute to society" };
        string[] prideTerrible = { "You're useless", "You feel worthless", "You don't feel like you've accomplished anything at all", "You feel like a waste of breath" };
        HappinessCheck(picker, happinessGreat, happinessGood, happinessBad, happinessTerrible);
        FocusCheck(picker, focusGreat, focusGood, focusBad, focusTerrible);
        PrideCheck(picker, prideGreat, prideGood, prideBad, prideTerrible);
        AddSpace();
        ImprovementCheck();
        Terminal.WriteLine("Write 'next' to continue to a new day.");
    }

    void FulfillmentCheck() // Job Employment School Money
    {
        if (school == false)
        {
            Terminal.WriteLine("You dropped out of school. You still feel guilty about it.");
            pride = (pride - 2);
        }
        if (school == true)
        {
            Terminal.WriteLine("You're still going to university. You don't like it much, but you're glad you're doing it.");
        }
        if (employed == false)
        {
            Terminal.WriteLine("You don't have a job. You feel guilty for being unemployed");
            pride = (pride - 4);
        }
        if (employed == true)
        {
            Terminal.WriteLine("You're holding down a steady job. Go you!");
        }
        if (money > 100)
        {
            Terminal.WriteLine("You feel rich! You spend some of your money on something for yourself. It makes you feel good.");
            happiness = (happiness + 15);
            focus = (focus - 5);
            pride = (pride + 10);
            money = (money - 25);
        }
        else if (money > 20)
        {
            Terminal.WriteLine("You feel financially stable.");
            happiness = (happiness + 5);
            focus = (focus - 2);
            pride = (pride + 3);
        }
        else if (money < 20)
        {
            Terminal.WriteLine("Money feels tight.");
            happiness = (happiness - 2);
            focus = (focus - 2);
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
            loneliness = (loneliness - 10);
        }
        if (flirtToday == false && dateToday == false)
        {
            Terminal.WriteLine("You miss talking to someone special.");
            loneliness = (loneliness - 10);
        }
        if (loneliness < 0)
        {
            Terminal.WriteLine("You feel disconnected and lonely.");
            happiness = (happiness - 10);
            focus = (focus + 1);
            pride = (pride - 5);
        }
    }

    void HousingCheck()
    {
        if (currentHouse == House.None)
        {
            Terminal.WriteLine("You are homeless. You feel terrible.");
            happiness = (happiness - 20);
            focus = (focus - 10);
            pride = (pride - 20);
        }
        if (currentHouse == House.Friend)
        {
            Terminal.WriteLine("You're crashing with a friend. It's free, but he probably won't like it long term.");
            happiness = (happiness + 1);
            focus = (focus - 2);
            pride = (pride - 3);
            friendAttitude = (friendAttitude - 4);
        }
        if (currentHouse == House.Parents)
        {
            Terminal.WriteLine("You're living with your parents.It's free, but no one is real happy with the arrangement.");
            happiness = (happiness - 3);
            focus = (focus - 3);
            pride = (pride - 5);
            parentsAttitude = (parentsAttitude - 2);
        }
        if (currentHouse == House.Rent)
        {
            Terminal.WriteLine("You rent your own place. It's not much, but at least you're independent");
            happiness = (happiness + 3);
            focus = (focus - 1);
            pride = (pride + 5);
        }
    }

    void KickedOutCheck()
    {
        if (currentHouse == House.Parents && school == false)
        {
            currentHouse = House.None;
            Terminal.WriteLine("Your parents aren't okay with you dropping out of school.");
            Terminal.WriteLine("They've kicked you out! You're homeless!");
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
        if (focus > yesterdayFocus)
        {
            Terminal.WriteLine("You feel more focused than before");
        }
        else if (focus == yesterdayFocus)
        {
            Terminal.WriteLine("You have about the same concentration as before");
        }
        else if (focus < yesterdayFocus)
        {
            Terminal.WriteLine("You feel less motivated than before");
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

    void FocusCheck(int picker, string[] focusGreat, string[] focusGood, string[] focusBad, string[] focusTerrible)
    {
        if (focus > 60)
        {
            Terminal.WriteLine(focusGreat[picker]);
        }
        else if (focus > 0)
        {
            Terminal.WriteLine(focusGood[picker]);
        }
        else if (focus > -60)
        {
            Terminal.WriteLine(focusBad[picker]);
        }
        else
        {
            Terminal.WriteLine(focusTerrible[picker]);
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

    void Abscences() // Checks if the player showed up to work/school and fires them if they've missed too much
    {
        if (workedToday == false && workDay == true)
        {
            workAbscence++;
        }
        if (schooledToday == false && schoolDay == false)
        {
            schoolAbscence++;
        }
        AddSpace();
        if (workAbscence > 5)
        {
            Terminal.WriteLine("You get fired for not showing up to work!");
            AddSpace();
            currentJob = Job.None;
            employed = false;
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
        if (input == "home" && currentScreen == Screen.Gameplay)
        {
            MainScreen();
        }
        if (input == "hiddenstats") // Dev command to see all stats
        {
            Terminal.WriteLine("focus" + focus + " happiness" + happiness + " pride" + pride);
        }
        else if (input == "actions" && currentScreen == Screen.Gameplay) // List available actions
        {
            ActionList();
        }
        else if (currentScreen == Screen.Name) // Pass username/go to main screen
        {
            if (input == "yes")
            {
                MainScreen();
            }
            else
            EnterName(input);
        }
        else if (currentScreen == Screen.Gameplay) // Pass info to input manager
        {
            InputManager(input);
        }
        else if (currentScreen == Screen.Night)
        {
            if (input == "next")
            {
                Morning();
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
        if (onlineToday == false)
        {
            Terminal.WriteLine("online");
        }
        if (friendToday == false)
        {
            Terminal.WriteLine("friend");
        }
        Terminal.WriteLine("jobsearch");
        if (school == true)
        {
            Terminal.WriteLine("school");
        }
        if (knownGirls > 0)
        {
            Terminal.WriteLine("flirt");
        }
        if ((girlfriend == true || knownGirls > 0) && money > 0)
        {
            Terminal.WriteLine("date");
        }
        if (employed == true  && workedToday == false)
        {
            Terminal.WriteLine("work");
        }
        AddSpace();  
    }

    void InputManager(string input)
    {
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
        if (input == "school" && school == true)
        {
            School();
            spoons--;
        }
        if (input == "flirt" && knownGirls > 0)
        {
            Flirt();
            spoons--;
        }
        if (input == "date" && ((girlfriend == true || knownGirls > 0) && money > 0))
        {
            Date();
            spoons--;
        }
        if (input == "work" && employed == true && workedToday == false)
        {
            Work();
            spoons--;
        }
    }

    void Work() // Work your job. TODO: Double shifts at some maybe?
    {
        RefreshScreen();
        Terminal.WriteLine("You go to work...");
        workedToday = true;
        if (currentJob == Job.DepartmentStore)
        {
            if (dailyRand == 100)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 1);
                money = (money + 2);
            }
            else if (dailyRand < 20)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                focus = (focus - 2);
                happiness = (happiness - 5);
                pride = (pride - 1);
                money = (money + 2);
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                focus = (focus - 1);
                happiness = (happiness - 1);
                pride = (pride + 1);
                money = (money + 2);
            }
        }
        if (currentJob == Job.LumberYard)
        {
            if (dailyRand == 100)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 2);
                money = (money + 4);
            }
            else if (dailyRand < 10)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                focus = (focus - 2);
                happiness = (happiness - 5);
                pride = (pride - 1);
                money = (money + 4);
            }
            else if (dailyRand > 90)
            {
                Terminal.WriteLine("You have a good day at work.");
                focus = (focus + 1);
                happiness = (happiness + 1);
                pride = (pride + 4);
                money = (money + 4);
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                focus = (focus - 1);
                happiness = (happiness - 1);
                pride = (pride + 3);
                money = (money + 4);
            }
        }
        if (currentJob == Job.PaintStore)
        {
            if (dailyRand == 100)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 5);
                money = (money + 3);
            }
            else if (dailyRand < 10)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                focus = (focus - 2);
                happiness = (happiness - 3);
                pride = (pride - 1);
                money = (money + 3);
            }
            else if (dailyRand > 80)
            {
                Terminal.WriteLine("You have a good day at work.");
                focus = (focus + 1);
                happiness = (happiness + 1);
                pride = (pride + 7);
                money = (money + 3);
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                focus = (focus - 1);
                pride = (pride + 3);
            }
        }
        if (currentJob == Job.PizzaPlace)
        {
            if (dailyRand < 10)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                focus = (focus - 1);
                happiness = (happiness - 5);
                pride = (pride - 1);
                money = (money + 1);
            }
            else if (dailyRand > 85)
            {
                Terminal.WriteLine("You make good tips at work.");
                happiness = (happiness + 1);
                pride = (pride + 1);
                money = (money + 3);
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                focus = (focus - 1);
                happiness = (happiness - 1);
                pride = (pride + 1);
                money = (money + 1);
            }
        }
        if (currentJob == Job.TravelingSales)
        {
            if (dailyRand == 100)
            {
                MeetGirl();
                happiness = (happiness + 3);
                pride = (pride + 5);
                money = (money + 6);
            }
            else if (dailyRand < 15)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                focus = (focus - 5);
                happiness = (happiness - 10);
                pride = (pride - 5);
                money = (money + 6);
            }
            else if (dailyRand > 95)
            {
                Terminal.WriteLine("You have a good day at work.");
                focus = (focus + 2);
                happiness = (happiness + 3);
                pride = (pride + 5);
                money = (money + 7);
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                focus = (focus - 2);
                happiness = (happiness - 3);
                pride = (pride + 3);
                money = (money + 6);
            }
        }
        if (currentJob == Job.GameCompany)
        {
            if (dailyRand < 2)
            {
                Terminal.WriteLine("You have a terrible day at work.");
                focus = (focus - 1);
                happiness = (happiness - 1);
                pride = (pride - 1);
                money = (money + 1);
            }
            else if (dailyRand > 95)
            {
                Terminal.WriteLine("You get a bonus at work.");
                focus = (focus + 1);
                happiness = (happiness + 3);
                pride = (pride + 3);
                money = (money + 3);
                spoons++;
                Terminal.WriteLine("You don't even feel tired from working!");
            }
            else
            {
                Terminal.WriteLine("You finish work.");
                happiness = (happiness + 1);
                pride = (pride + 2);
                money = (money + 1);
                spoons++;
                Terminal.WriteLine("You don't even feel tired from working!");
            }
        }
    }

    void Date() //TODO IMPLEMENT ME
    {
        throw new NotImplementedException();
    }

    void Flirt() //TODO IMPLEMENT ME
    {
        throw new NotImplementedException();
    }

    void School()
    {
        RefreshScreen();
        Terminal.WriteLine("You go to university...");
        schooledToday = true;
        if (dailyRand > 95)
        {
            MeetGirl();
        }
        else if (dailyRand > 87)
        {
            Terminal.WriteLine("You ace a test. Nice! Didn't even study!");
            focus--;
            pride = (pride + 5);
            happiness = (happiness + 5);
        }
        else if (dailyRand > 70)
        {
            Terminal.WriteLine("You have an okay day at school.");
            focus--;
            pride = (pride + 3);
        }
        else if (dailyRand > 50)
        {
            Terminal.WriteLine("You're frustrated with the pace of school. Everything goes so slow...");
            focus = (focus - 3);
            pride = (pride + 1);
            happiness = (happiness - 2);
        }
        else if (dailyRand > 1)
        {
            Terminal.WriteLine("You hate everything to do with school. Why are you even here?");
            focus = (focus - 5);
            pride = (pride + 1);
            happiness = (happiness - 4);
        }
        else if ((dailyRand == 1) && (focus < 0))
        {
            Terminal.WriteLine("Fuck school, you quit.");
            focus = (focus + 10);
            pride = (pride - 10);
            happiness = (happiness + 5);
        }
        else if (dailyRand == 1)
        {
            Terminal.WriteLine("You daydream about quitting all through class.");
            focus = (focus + 2);
            pride = (pride - 1);
            happiness = (happiness + 1);
        }
    }

    void JobSearch()
    {
        RefreshScreen();
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
            focus--;
            pride++;
        }
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
            focus = (focus + 5);
            happiness = (happiness + 15);
            pride = (pride + 7);
        }
        else if (friendAttitude + dailyRand > 100)
        {
            Terminal.WriteLine("You have a great conversation with your friend. That was nice.");
            happiness = (happiness + 10);
            pride = (pride + 5);
        }
        else if (friendAttitude + dailyRand > 75)
        {
            Terminal.WriteLine("You share silly texts with your friend. Fun.");
            happiness = (happiness + 6);
        }
        else if (friendAttitude + dailyRand > 50)
        {
            Terminal.WriteLine("You talk on the phone with your friend for a while");
            happiness = (happiness + 3);
        }
        else
        {
            Terminal.WriteLine("You argue with your friend. Bummer.");
            happiness = (happiness - 1);
            friendAttitude = (friendAttitude - 3);
        }
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
                focus = (focus + 10);
                happiness = (happiness + 20);
                pride = (pride + 10);
        }
        else if (onlineAttitude + dailyRand > 150)
        {
            Terminal.WriteLine("You talk in Discord voice chat for hours. It was a blast!");
            focus = (focus + 3);
            happiness = (happiness + 10);
            pride = (pride + 3);
        }
        else if (onlineAttitude + dailyRand > 100)
        {
            Terminal.WriteLine("You live stream gaming with some of your friends. Cool.");
            happiness = (happiness + 6);
            pride = (pride + 1);
        }
        else if (onlineAttitude + dailyRand > 75)
        {
            Terminal.WriteLine("You share some memes with your clan. Haha.");
            happiness = (happiness + 3);
        }
        else if (onlineAttitude + dailyRand > 50)
        {
            Terminal.WriteLine("You chat in Discord for a while.");
            happiness = (happiness + 2);
        }
        else
        {
            Terminal.WriteLine("You argue with your friends. Damn.");
            happiness = (happiness - 1);
            onlineAttitude = (onlineAttitude - 3);
        }
    }

    void Hobby()  // Define hobbies
    {
        dailyRand = UnityEngine.Random.Range(0, 101);
        RefreshScreen();
        if (dailyRand > 90)
        {
            Terminal.WriteLine("You read a book. You feel edified!");
            focus = (focus + 2);
            happiness = (happiness + 10);
            pride = (pride + 5);
        }
        else if (dailyRand > 80)
        {
            Terminal.WriteLine("You peruse Wikipedia and learn something new. You feel smarter!");
            focus = (focus + 1);
            happiness = (happiness + 7);
            pride = (pride + 1);
        }
        else if (dailyRand > 70)
        {
            Terminal.WriteLine("You cook a home cooked meal. It was delicious!");
            focus = (focus + 1);
            happiness = (happiness + 6);
            pride = (pride + 4);
        }
        else if (dailyRand > 60)
        {
            Terminal.WriteLine("You go for a bike ride. Feels good to stretch for once.");
            focus = (focus + 2);
            happiness = (happiness + 3);
            pride = (pride + 4);
        }
        else if(dailyRand > 50)
        {
            Terminal.WriteLine("You make some delightful tea. Yum!");
            focus = (focus + 4);
            happiness = (happiness + 2);
            pride = (pride + 0);
        }
        else if(dailyRand > 40)
        {
            Terminal.WriteLine("You play some games. It was fun.");
            focus = (focus + 1);
            happiness = (happiness + 2);
            pride = (pride + 0);
        }
        else if(dailyRand > 30)
        {
            Terminal.WriteLine("You zonk out and watch TV for a while.");
            focus = (focus + 0);
            happiness = (happiness + 2);
            pride = (pride + 0);
        }
        else if(dailyRand > 20)
        {
            Terminal.WriteLine("You have a few drinks at home to take the edge off.");
            focus = (focus - 1);
            happiness = (happiness + 4);
            pride = (pride - 1);
        }
        else if(dailyRand > 10)
        {
            Terminal.WriteLine("You meditate. It clears your mind. You feel more focused.");
            focus = (focus + 7);
            happiness = (happiness + 0);
            pride = (pride + 1);
        }
        else if(dailyRand > 0)
        {
            Terminal.WriteLine("You can't think of anything to do. You're bored.");
            focus = (focus + 0);
            happiness = (happiness + 0);
            pride = (pride + 0);
        }
    }


    void GirlList()
    {
        Terminal.WriteLine("You have a friend.");
        if (Lina == true)
        {
            Terminal.WriteLine("You know Lina");
        }
        if (Jenna == true)
        {
            Terminal.WriteLine("You know Jenna");
        }
        if (Alex == true)
        {
            Terminal.WriteLine("You know Alex");
        }
        if (Pina == true)
        {
            Terminal.WriteLine("You know Pina");
        }
        if (Sammy == true)
        {
            Terminal.WriteLine("You know Sammy");
        }
        if (Winry == true)
        {
            Terminal.WriteLine("You know Winry");
        }
        AddSpace();
    }

    void MeetGirl()
    {
        throw new NotImplementedException();
    }

    void EnterName(string input)
    {
        character = input;
        AddSpace();
        Terminal.WriteLine("Name: " + input);
        Terminal.WriteLine("Type 'yes' to accept this name or enter another name");
    }

    void DisplayIntro()
    {
        AddSpace();
        Terminal.WriteLine("Welcome to 'Are You Alone'"); // Ask about tutorial later
        AddSpace();
        Terminal.WriteLine("What would your like your character name to be?");
    }

    void RefreshScreen() 
    {
        Terminal.ClearScreen();
        AddSpace();
        Terminal.WriteLine("Are You Alone?            Current Money: " + money + "           Current Spoons: " + spoons);
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
