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
    
    


    private void Start()
    {
        StartCoroutine(HandleUnitCountUp());
        SetCount(unitCount);
        SetColor(_team.color);
    }

    public void SetTeam(Team team)
    {
        this._team = team;
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
        areaColor = Color.Lerp(Color.black, color, 0.9f);
        area.color = areaColor;
    }

    IEnumerator HandleUnitCountUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(unitCountUpTime);
            if(_team == null)
                continue;
            if(!_team.canProduceUnits)
                continue;

            dot.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f); 
            unitCount++;
            SetColor(_team.color);
            SetCount(unitCount);
        }
    }
}
