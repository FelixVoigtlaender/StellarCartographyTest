using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Team[] activeTeams;
    public Team neutralTeam;
    public List<Team> teams;
    public State[] states;

    public MultipleSlider multipleSlider;

    public State[] startStates;

    public Panel gamePanel;
    public Panel menuPanel;
    public Panel endPanel;

    public bool isInGame = false;

    public Selection selection;
    public Enemy enemy;

    public TextMeshProUGUI endGameText;

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

        StopGame();
        
        HandleStartStates();
        StartCoroutine(HandleMapPercentage());
    }



    public void SelectTeam(bool isRed)
    {
        int myIndex = isRed ? 0 : 1;
        enemy.team = activeTeams[myIndex% activeTeams.Length];
        selection.team = activeTeams[(myIndex + 1) % activeTeams.Length];

    }
    
    
    public void StartGame()
    {
        //Time.timeScale = 1;
        gamePanel.Open();
        isInGame = true;
    }

    public void StopGame()
    {
        menuPanel.Open();
        isInGame = false;
        //Time.timeScale = 0;
    }

    public void EndGame()
    {
        print("ENDING GAME");

        endPanel.Open();
        isInGame = false;
        //Time.timeScale = 0;
    }

    public void OnLost()
    {
        EndGame();
        endGameText.text = "YOU LOST";
    }

    public void OnWin()
    {
        EndGame();
        endGameText.text = "YOU WON";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            
            
            CheckLooseState();
            
            
            
            yield return new WaitForFixedUpdate();
            
        }
    }

    public void CheckLooseState()
    {
        if(!isInGame)
            return;
        
        if(selection.team.count == 0)
            OnLost();
        if(enemy.team.count == 0)
            OnWin();
    }
    
    
}
