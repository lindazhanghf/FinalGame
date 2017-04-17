using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

    private State[] states;
    //private State[] states = { new Idle(), new Sprint() }; // 2. states , new C()
    private int current = 0;                            // Index of current State

    public void changeState(int newState)
    {
        states[newState].onEnter(current);
        current = newState;
    }

    // 5. All client requests are simply delegated to the current state object
    public void onEnter(int oldState)
    {
        states[current].onEnter(oldState);
    }
    public int onExit()
    {
        return states[current].onExit();
    }
    public bool execute()
    {
        return states[current].execute();
    }

}

    // 6. Create a state base class that makes the concrete states interchangeable
abstract class State
{
    public void onEnter(int oldState)
    {
        Debug.Log("error");
    }  // 7. The State base 
    public int onExit()
    {
        Debug.Log("error");
        return 0;
    }  //    class specifies 
    public bool execute()
    {
        Debug.Log("error");
        return false;
    }  //    default behavior
}

class Idle : State
{
    private int nextState;

    public new void onEnter(int oldState)
    {
        // System.out.println( "IDLE + onEnter" );
    }
    public new int onExit()
    {
        // System.out.println( "IDLE + onExit" );
        return nextState;
    }

    public new bool execute()
    {
        //if (combinedR >= SPRINT_BOUND)
        //{
        //    nextState = SPRINT_STATE;
        //    return true;
        //}
        return false;
    }
}

