using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {

    // Many  game variables will be stored here

    [SerializeField] int spoons = 3;
    [SerializeField] string character = "default";
    enum Screen { Name, Gameplay, Gameover };
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
    [SerializeField] int workAbscence = 0;
    [SerializeField] int schoolAbscence = 0;
    [SerializeField] int friendAttitude = 50;
    [SerializeField] int onlineAttitude = 50;
    bool friendToday;
    bool onlineToday;

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
    }

    void MainScreen()
    {
        ClearScreen();
        currentScreen = Screen.Gameplay;
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
        Terminal.WriteLine("These are the girls you know:");
        GirlList();
        AddSpace();
        Terminal.WriteLine("Enter an action!");
        Terminal.WriteLine("(You can enter 'actions' to see a list)");
        AddSpace();
        if (spoons == 0)
        {
            ClearScreen();
            Terminal.WriteLine("Out of spoons. Next turn in 5 seconds.");
            Invoke("Morning", 5f);
        }
    }

    // Morning Cleanup
    void Morning()
    {
        Terminal.WriteLine("You wake up.");
        turn++;
        spoons = 3;
        CheckDays();
        dailyRand = UnityEngine.Random.Range(0, 101);
        Abscences();
        Attitudes();
        MainScreen();
    }

    private void Attitudes() // Does daily attitude adjustments and then resets daily encounters
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

    private void Abscences() // Checks if the player showed up to work/school and fires them if they've missed too much
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

    private void CheckDays() // Check and set whether each day is a school day/work day
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
                ClearScreen();
                MainScreen();
            }
            else
            EnterName(input);
        }
        else if (currentScreen == Screen.Gameplay) // Pass info to input manager
        {
            InputManager(input);
        }
        else
        {
            throw new NotImplementedException(); // Handle weird screen cases
        }
    }     // Pass user input to other methods via this

    void ActionList() //List available actions to player
    {
        ClearScreen();
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

    private void Work() // Work your job. TODO: Double shifts at some maybe?
    {
        ClearScreen();
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

    private void Date() //TODO IMPLEMENT ME
    {
        throw new NotImplementedException();
    }

    private void Flirt() //TODO IMPLEMENT ME
    {
        throw new NotImplementedException();
    }

    private void School()
    {
        ClearScreen();
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

    private void JobSearch()
    {
        ClearScreen();
        Terminal.WriteLine("You search for a job...");
        if (dailyRand == 100)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Paint Store! You're on the schedule soon!");
            happiness = (happiness + 10);
            employed = true;
            currentJob = Job.PaintStore;
        }
        if (dailyRand > 70 && dailyRand <= 73)
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
        if (dailyRand >20 && dailyRand <= 23)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Pizza Place! You're on the schedule soon!");
            happiness = (happiness + 7);
            employed = true;
            currentJob = Job.PizzaPlace;
        }
        if (dailyRand == 92)
        {
            AddSpace();
            Terminal.WriteLine("You get a job as a Traveling Salesman! You're on the schedule soon!");
            happiness = (happiness + 10);
            employed = true;
            currentJob = Job.TravelingSales;
        }
        if (dailyRand == 60)
        {
            AddSpace();
            Terminal.WriteLine("You get a job at a Game Company! You're on the schedule soon!");
            happiness = (happiness + 15);
            employed = true;
            currentJob = Job.GameCompany;
        }
        else
        {
            AddSpace();
            Terminal.WriteLine("You don't have any luck, though...");
            happiness--;
            focus--;
            pride++;
        }
    } // TODO Ask if player wants to take new job

    private void Friends()
    {
        friendToday = true;
        ClearScreen();
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

    private void Online()
    {
        onlineToday = true;
        ClearScreen();
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

    private void Hobby()  //TODO IMPLEMENT HOBBY
    {
        dailyRand = UnityEngine.Random.Range(0, 101);
        ClearScreen();
    }


    void GirlList()
    {
        if (Lina == true)
        {
            Terminal.WriteLine("You know Linea");
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

    void ClearScreen()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("    ***Are You Alone?***                        Current Spoons: "+spoons);
        Terminal.WriteLine("        Type 'home' to get back to the home screen at any time.");
        AddSpace();
    }

    void AddSpace() //Quick Method for adding spaces
    {
        Terminal.WriteLine("");
    }

    private void FriendWin() // Win with perfect attitude towards friend. Secret ending! Implement me plox.
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
                ClearScreen();
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
