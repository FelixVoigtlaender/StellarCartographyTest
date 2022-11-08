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


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D collider2D = Physics2D.OverlapPoint(position, stateLayer);
        if(!collider2D)
            return;
        State state = collider2D.GetComponentInParent<State>();
        if(!state)
            return;
        
        AddSelection(state);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
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

    void HandleSelections()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var state in selectedStates)
        {
            Debug.DrawLine(state.transform.position,mousePosition);
        }
        
        
    }

}
