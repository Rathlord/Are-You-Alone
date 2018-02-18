using System;

class Response {
    public string name;
    public string response = "";
    public int happinessChange;
    public int stressChange;
    public int friendAttitudeChange;
    public string trigger;
    public Func<int> nextEvent;

    public Response(string name) {
        this.name = name;
    }
    
    public void addResponseLine(string extraLine) {
        response += (response.Length == 0 ? "" : "\n") + extraLine; // If the string doesn't exist yet add "", else add a new line to print stuff on, and at the end add a new line 
    }

    public void setStatChange(int happinessChange, int stressChange, int friendAttitudeChange) {
        this.happinessChange = happinessChange;
        this.stressChange = stressChange;
        this.friendAttitudeChange = friendAttitudeChange;
    }

    public void setTrigger(string trigger) {
        this.trigger = trigger;
    }

    public void setNextEvent(Func<int> nextEvent)
    {
        this.nextEvent = nextEvent;
    }


}