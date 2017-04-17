using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

    private State[] states;
    //private State[] states = { new Curious(), new Scared(), new End() };
    private int current = 0;                            // Index of current State

    public void changeState(int newState)
    {
        states[newState].onEnter(current);
        current = newState;
    }

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
    }  
    public int onExit()
    {
        Debug.Log("error");
        return 0;
    } 
    public bool execute()
    {
        Debug.Log("error");
        return false;
    } 
}

//class Curious : State
//{
//    private int nextState;

//    public new void onEnter(int oldState)
//    {
//    }
    
//    public new int onExit()
//    {
//        return nextState;
//    }

//    public new bool execute()
//    {
//        //if (target != null)
//        //    agent.SetDestination(target.position);

//        //if (agent.remainingDistance > agent.stoppingDistance)
//        //    character.Move(agent.desiredVelocity, false, false);
//        //else
//        //    character.Move(Vector3.zero, false, false);

//        // Run towards the sound source
//        return false;
//    }
//}

//class Scared : State
//{
//    private int nextState;

//    public new void onEnter(int oldState)
//    {
//    }

//    public new int onExit()
//    {
//        return nextState;
//    }

//    public new bool execute()
//    {
//        // Running away from the sound source
//        return false;
//    }
//}

//class End : State
//{
//    private int nextState;

//    public new void onEnter(int oldState)
//    {
//    }

//    public new int onExit()
//    {
//        return nextState;
//    }

//    public new bool execute()
//    {
//        // Running out of the house
//        return false;
//    }
//}