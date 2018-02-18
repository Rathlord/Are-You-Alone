using System;
using System.Collections.Generic;

class MyEvent {
    public string question;
    public List<Response> responses;

    public MyEvent(string question) {
        this.question = question;
        responses = new List<Response>();
    }
    
    public void addLine(string toAsk) {
        question += (question == "" ? "" : "\n") + toAsk;
    }

    public void addResponse(Response response) {
        responses.Add(response);
    }
}