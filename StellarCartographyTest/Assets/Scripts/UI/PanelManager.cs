using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelManager : MonoBehaviour
{
    private bool _isSelected = true;

    public bool isSelected
    {
        get
        {
            return _isSelected;
        }
    }
    private Panel[] _panels;
    public Panel mainPanel;

    private Stack<Panel> openedPanels = new Stack<Panel>();
    private CanvasGroup _canvasGroup;

    private float lastBack;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _panels = GetComponentsInChildren<Panel>();
        foreach (var panel in _panels)
        {
            panel.onOpen.AddListener(()=>OnPanelOpened(panel));
        }

        _canvasGroup.enabled = true;
        _canvasGroup.alpha = 1;
    }

    public void OnPanelOpened(Panel panel)
    {
        //print($"Panel {panel.name} opened ");
        if(panel.saveInHistory)
            openedPanels.Push(panel);
        foreach (var p in _panels)
        {
            if(p == panel)
                continue;
            p.ManagerClose();
        }

        //PrintStack();
    }


    public void OpenMainPanel()
    {
        if(!mainPanel)
            return;
        
        mainPanel.Open();
    }

    public void Back()
    {
        if(Time.time - lastBack < 0.1f)
            return;
        lastBack = Time.time;
        print("Back");   
        if(openedPanels.Count>=1){}
            openedPanels.Pop().Close();
        if(openedPanels.Count ==0)
            return;
        
        openedPanels.Pop().Open();
        //PrintStack();
    }

    public void CloseAll()
    {
        foreach (var p in _panels)
        {
            p.Close();
        }
    }

    public void SetPanels(bool value)
    {
        _isSelected = value;
        if (value)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;   
            OpenMainPanel();
        }
        else
        {
            CloseAll();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void PrintStack()
    {
        Panel[] panels = openedPanels.ToArray();
        string panelsInStack = "Panels in Stack: ";
        foreach (var panel in panels)
        {
            panelsInStack += panel.name + ", ";
        }

        print(panelsInStack);
    }
}
