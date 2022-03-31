using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => PlaySound());
    }

    public void PlaySound()
    {
        AudioManager.instance.PlaySFX("UI_ButtonClick");
        //HapticFeedback.Vibrate(10);
    }
}
