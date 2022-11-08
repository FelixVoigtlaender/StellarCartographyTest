using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Panel : MonoBehaviour
{
    private PanelManager _panelManager;
    public Selectable firstSelected;
    private EventSystem _eventSystem;
    private CanvasGroup _canvasGroup;



    public bool openOnStart = false;
    public bool keepOpen = false;
    public bool saveInHistory = true;
    private bool isOpen = false;

    public UnityEvent onOpen;
    public UnityEvent onClose;
    public UnityEvent<bool> onChanged;
    public UnityEvent onBack;
    
    private void Awake()
    {
        _panelManager = GetComponentInParent<PanelManager>();
        _eventSystem = FindObjectOfType<EventSystem>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
       
       
       SetPanel(openOnStart);
    }


    public void SetPanel(bool flag)
    {
        
        isOpen = flag;
        float alpha = flag ? 1 : 0;
        _canvasGroup.DOFade(alpha, 0.3f);
        _canvasGroup.interactable = flag;
        _canvasGroup.blocksRaycasts = flag;
        
        if(flag && firstSelected)
            firstSelected.Select();

        
        onChanged?.Invoke(flag);
        if(flag)
            onOpen?.Invoke();
        else
            onClose?.Invoke();
    }

    [ContextMenu("Open")]
    public void Open()
    {
        SetPanel(true);
    }

    public void Close()
    {
        SetPanel(false);
    }

    public void ManagerClose()
    {
        if(!keepOpen)
            Close();
    }

    public void Toggle()
    {
        if(!_panelManager.isSelected)
            return;
        
        SetPanel(!isOpen);
        
    }

    public void Back()
    {
        if (isOpen && gameObject.activeInHierarchy && _panelManager)
        {
            _panelManager.Back();
            onBack?.Invoke();
        }
    }
}
