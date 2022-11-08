using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    public SpriteRenderer area;
    public SpriteRenderer dot;
    public TextMeshProUGUI text;
    public int unitCount = 0;
    public float unitCountUpTime = 0.5f;
    public Team _team;

    public GameObject unitPrefab;
    
    


    private void Start()
    {
        StartCoroutine(HandleUnitCountUp());
        SetCount(unitCount);
        SetColor(_team.color);
    }

    public void SetTeam(Team team)
    {
        this._team = team;
        HandleVis();
    }

    public Team GetTeam()
    {
        return _team;
    }

    public bool CanSelect(Team team)
    {
        return team == _team;
    }

    public void SetCount(int count)
    {
        this.unitCount = count;
        text.text = $"{count}";
    }

    public void SetColor(Color color)
    {
        dot.color = color;

        float percentage = Mathf.Clamp01(unitCount / 10f);
        Color areaColor = Color.Lerp(Color.grey, color, percentage);
        areaColor = Color.Lerp(Color.black, color, 0.5f);
        area.color = areaColor;
    }

    public void Conquer(Team team)
    {
        if (team != this._team)
        {
            if (unitCount == 0)
            {
                SetTeam(team);
                unitCount++;
            }
            else
            {
                unitCount--;
            }
        }
        else
        {
            unitCount++;
        }

        HandleVis();
    }


    public void HandleVis()
    {
        SetColor(_team.color);
        SetCount(unitCount);
    }
    
    
    IEnumerator HandleUnitCountUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(unitCountUpTime);
            if(!GameManager.Instance.isInGame)
                continue;
            
            
            if(_team == null)
                continue;
            if(!_team.canProduceUnits)
                continue;

            dot.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f); 
            unitCount++;
            
            HandleVis();
        }
    }

    public void SendUnits(State state)
    {
        if(state==this)
            return;

        StartCoroutine(HandleSendUnit(state));
    }

    IEnumerator HandleSendUnit(State goalState)
    {
        Vector2 start = transform.position;
        Vector2 end = goalState.transform.position;
        Vector2 dif = end - start;

        yield return new WaitForFixedUpdate();

        int units = unitCount;
        unitCount = 0;
        
        for (int i = 0; i < units; i++)
        {
            // Create Path
            int n = i % 5;
            List<Vector3> path = new List<Vector3>();
            float angleStep = 20;
            
            // Start
            path.Add(start);
            Vector2 startOffset = GetDirByIndex(dif.normalized, n, angleStep,false);
            path.Add(startOffset*1 + start);
            
            // End
            Vector2 endOffset = GetDirByIndex(-dif.normalized, n, angleStep, true);
            path.Add(end + endOffset*1);
            path.Add(end);

            // Innit Unit
            GameObject unitObj = Instantiate(unitPrefab, transform.position, Quaternion.identity);
            Unit unit = unitObj.GetComponent<Unit>();
            unit.Setup(path,goalState,_team);

            
            if (i > 0 && n == 0)
                yield return new WaitForSeconds(1f);

        }
    }

    Vector2 GetDirByIndex(Vector2 dir, int i, float angleStep, bool invert)
    {
        
        int sign = (i%2 == 0) ? -1 : 1;
        sign *= invert ? -1 : 1;
        int step = Mathf.CeilToInt(((float) i) / 2f);
        dir = Quaternion.Euler(0,0,angleStep * (step * sign)) * dir;

        return dir;
    }
}
