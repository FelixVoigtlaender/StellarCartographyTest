using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selection : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public LayerMask stateLayer;
    private List<State> selectedStates = new List<State>();
    public GameObject arrowPrefab;

    public List<Arrow> arrows = new List<Arrow>();
    public Team team;


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D collider2D = Physics2D.OverlapPoint(position, stateLayer);
        if(!collider2D)
            return;
        State state = collider2D.GetComponentInParent<State>();
        if(!state)
            return;
        
        if(!state.CanSelect(team))
            return;
        
        AddSelection(state);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        
        Vector2 position = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D collider2D = Physics2D.OverlapPoint(position, stateLayer);
        if (!collider2D)
        {
            ClearSelections();
            return;
        }
        State goalState = collider2D.GetComponentInParent<State>();
        if(!goalState)
        {
            ClearSelections();
            return;
        }
        
        foreach (var state in selectedStates)
        {
            state.SendUnits(goalState);
        }
        
        
        ClearSelections();
        
    }



    void AddSelection(State state)
    {
        if(selectedStates.Contains(state))
            return;

        selectedStates.Add(state);
    }

    void ClearSelections()
    {
        selectedStates.Clear();
    }

    private void Update()
    {
        HandleSelections();
    }


    void CreateArrow()
    {
        GameObject arrowObj = Instantiate(arrowPrefab);
        arrows.Add(arrowObj.GetComponent<Arrow>());
    }
    void HandleSelections()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var state in selectedStates)
        {
            Debug.DrawLine(state.transform.position,mousePosition);
        }

        int maxCount = Mathf.Max(selectedStates.Count, arrows.Count);
        for (int i = 0; i < maxCount; i++)
        {
            if(arrows.Count<=i)
                CreateArrow();

            if (selectedStates.Count > i)
            {
                arrows[i].gameObject.SetActive(true);
                arrows[i].start = selectedStates[i].transform.position;
                arrows[i].end = mousePosition;
                arrows[i].sprite.color = selectedStates[i]._team.color;
            }
            else
            {
                arrows[i].gameObject.SetActive(false);
            }
        }


    }

}
