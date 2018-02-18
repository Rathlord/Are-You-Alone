using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour {
    List<MyEvent> events;                                                                                                       // Declare a list called events that contains MyEvent object (other class)
    MyEvent currentEvent;                                                                                                       // Instantiate a real version of MyEvent
    int happiness;
    int stress;
    int friendAttitude;

    // Use this for initialization
    void Start() {
        InitializeEvents();                                                                                                     // Run InitializeEvents, which puts all the events into the list called events, ordered numerically?
        StartEvent(0);                                                                                                          // Starts event 0 at runtime, which is actually not what I want to do but can be replaced later
    }

    void OnUserInput(string input) {                                                                                            // Passes input to ExecuteEvent class
        ExecuteEvent(input);
    }

    void InitializeEvents() {
        events = new List<MyEvent>();                                                                                           // Initialized the events list

        MyEvent myEvent = new MyEvent("You can't remember how you got here.");                                                  // Declare and initialize object MyEvent called myEvent, and pass the same-named MyEvent the attached string
        myEvent.addLine("Where do we go?");                                                                                     // Pass addLine method of myEvent the attahed string

        Response response = new Response("Walk Into The Field");                                                                // Declare and initialize the Response object as response and pass the Response function the string
        response.setTrigger("1"); // What the user will enter to execute this response.                                         // Sets the trigger as per setTrigger
        response.addResponseLine("You start walking towards the field.");                                                       // Adds a response to the trigger
        response.setStatChange(+1, -1, 0); // Happiness, stress, friend attitude                                                // Sets the potential stat changes for the action
        response.setNextEvent(1); // Next event in events list this response will take you to.                                  // Which event the given response would point to
        myEvent.addResponse(response); // Our response is finished being set up so we add it to the event.                      // Basically initializes the response

        // Begin setting up next possible response for our first event.
        response = new Response("Head Down Into The Cave");                                                                     // See above
        response.setTrigger("2");
        response.addResponseLine("You take a deep breath and head into the darkness.");
        response.setStatChange(-1, +2, 0);
        response.setNextEvent(2);
        myEvent.addResponse(response);

        // We decided we're done adding possible responses to our event, so we add it to the list.
        events.Add(myEvent);

        // Now we can create our second event. This would be event ID #1 with the one above being 0.
        // Therefore this is called by Walk into the field trigger on line 32.
        myEvent = new MyEvent("Beautiful grass.");
        myEvent.addLine("There's a stranger in the distance.");

        response = new Response("Walk towards the stranger.");
        response.setTrigger("1");
        response.addResponseLine("You start walking.");
        response.setStatChange(+1, -1, 0);
        response.setNextEvent(0);
        myEvent.addResponse(response);

        // We decided we're done adding possible responses to our event, so we add it to the list.
        events.Add(myEvent);
    }

    void StartEvent(int eventId) {
        currentEvent = events[eventId];
        Terminal.WriteLine(currentEvent.question + "\n");

        foreach(Response response in currentEvent.responses) {
            Terminal.WriteLine("Press " + response.trigger + " to " + response.name);
        }
    }

    void ExecuteEvent(string input) {
        foreach(Response response in currentEvent.responses) {
            if(input == response.trigger) {
                ExecuteResponse(response);
                return;
            }
        }
        Terminal.WriteLine("Invalid Entry.");
    }

    void ExecuteResponse(Response response) {
        updateStats(response);
        Terminal.ClearScreen();
        Terminal.WriteLine(response.response);
        StartEvent(response.nextEvent);
    }

    void updateStats(Response response) {
        happiness += response.happinessChange;
        stress += response.stressChange;
        friendAttitude += response.friendAttitudeChange;
    }
}
