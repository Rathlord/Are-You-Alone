using System;

class Response {
    public string name;
    public string response = "";
    public int happinessChange;
    public int stressChange;
    public int prideChange;
    public int lonelinessChange;
    public int moneyChange;
    public string trigger;
    public Func<int> nextEvent;
    public string responseOutput = "";

    public Response(string name) {
        this.name = name;
    }
    
    public void addResponseLine(string extraLine) {
        response += (response.Length == 0 ? "" : "\n") + extraLine; // If the string doesn't exist yet add "", else add a new line to print stuff on, and at the end add a new line 
    }

    public void addResponseOutput(string extraLine)
    {
        responseOutput += (responseOutput.Length == 0 ? "" : "\n") + extraLine; // If the string doesn't exist yet add "", else add a new line to print stuff on, and at the end add a new line 
    }

    public void setStatChange(int happinessChange, int stressChange, int prideChange, int lonelinessChange, int moneyChange) {
        this.happinessChange = happinessChange;
        this.stressChange = stressChange;
        this.prideChange = prideChange;
        this.lonelinessChange = lonelinessChange;
        this.moneyChange = moneyChange;
    }

    public void setTrigger(string trigger) {
        this.trigger = trigger;
    }

    public void setNextEvent(Func<int> nextEvent)
    {
        this.nextEvent = nextEvent;
    }


}