using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] protected Canvas popupCanvas;

    private void Start()
    {
        OnStart();
        popupCanvas.enabled = false;
    }

    public virtual void OpenPopup()
    {
        popupCanvas.enabled = true;
    }

    public virtual void ClosePopup()
    {
        popupCanvas.enabled = false;
    }

    // Override functions
    public virtual void OnStart() {}
}
