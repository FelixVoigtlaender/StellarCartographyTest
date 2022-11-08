using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Team[] activeTeams;
    public Team neutralTeam;
    public List<Team> teams;
    public State[] states;

    public MultipleSlider multipleSlider;

    public State[] startStates;

    protected override void Awake()
    {
        base.Awake();
        teams = new List<Team>();
        teams.AddRange(activeTeams);
        teams.Add(neutralTeam);
        
        
        states = FindObjectsOfType<State>();
    }


    private void Start()
    {

        
        
        HandleStartStates();
        StartCoroutine(HandleMapPercentage());
    }

    public void HandleStartStates()
    {
        for (int i = 0; i < startStates.Length; i++)
        {
            startStates[i].SetTeam(activeTeams[i%activeTeams.Length]);
        }
    }

    IEnumerator HandleMapPercentage()
    {
        // Setup dictionaries
        Dictionary<Team, Slider> teamToSlider = new Dictionary<Team, Slider>();
        foreach (var team in teams)
        {
            teamToSlider.Add(team, multipleSlider.AddSlider(team.color));
        }
        
        
        // Handle Sliders
        while (true)
        {
            // reset count
            foreach (var team in teams)
            {
                team.count = 0;
            }
            
            // count
            foreach (var state in states)
            {
                state._team.count++;
            }
            
            // float percentage
            float totalPercent = 0;
            foreach (var team in teams)
            {
                float percent = team.count / (float)states.Length;
                totalPercent += percent;

                teamToSlider[team].DOValue(totalPercent,0.2f);
            }
            
            yield return new WaitForFixedUpdate();
            
        }
    }
    
    
}
