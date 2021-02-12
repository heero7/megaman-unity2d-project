using System;

[Serializable]
public class Transition 
{
    public Decision[] decisions; // Decision we're evaluating.
    public State trueState; // If decision is true
    public State falseState; // If decision is false;
}