using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Team team;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(5, 13f));
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 13f));
            if(!GameManager.Instance.isInGame)
                continue;
            State[] myStates = GetMyStates();
            State[] otherStates = GetOtherStates();
            if (Random.Range(0, 1f) < 0.3f)
            {
                DumpAttack(myStates,otherStates);
            }
            else
            {
                SmartAttack(myStates,otherStates);
            }
        }
    }

    public State[] GetMyStates()
    {
        List<State> states = new List<State>();
        foreach (var state in GameManager.Instance.states)
        {
            if(state._team == team)
                states.Add(state);
        }

        return states.ToArray();
    }
    
    public State[] GetOtherStates()
    {
        List<State> states = new List<State>();
        foreach (var state in GameManager.Instance.states)
        {
            if(state._team != team)
                states.Add(state);
        }

        return states.ToArray();
    }

    public void DumpAttack(State[] myStates, State[] otherStates)
    {
        if(myStates.Length == 0 || otherStates.Length ==0)
            return;

        
        State myState = GetRandomState(myStates);
        State otherState = GetRandomState(otherStates);
        myState.SendUnits(otherState);
    }

    public void SmartAttack(State[] myStates, State[] otherStates)
    {
        if(myStates.Length == 0 || otherStates.Length ==0)
            return;

        List<State> myStatesList = new List<State>(myStates);
        myStatesList.Sort((p1, p2) => p1.unitCount.CompareTo(p2.unitCount));
        State myState = myStatesList[myStatesList.Count-1];
        
        
        List<State> otherStatesList = new List<State>(otherStates);
        for (int i = otherStatesList.Count-1; i >= 0; i--)
        {
            if(otherStatesList[i].unitCount>myState.unitCount)
                otherStatesList.RemoveAt(i);
        }
        
        if(otherStatesList.Count==0)
            return;

        otherStatesList.Sort((p1, p2) => p1.unitCount.CompareTo(p2.unitCount));
        State otherState = otherStatesList[0];
        
        
        
        
        myState.SendUnits(otherState);
    }

    public State GetRandomState(State[] states)
    {
        if (states.Length == 0)
            return null;
        
        
        State state = states[Random.Range(0, states.Length)];
        return state;
    }
    
    
}
