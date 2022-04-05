using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] bool isOn = false;

    [SerializeField] private RectTransform toggleIndicator;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private float offX;
    private float onX;
    [SerializeField] private float tweenTime = 0.25f;

    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;

    private void Start()
    {
        offX = toggleIndicator.anchoredPosition.x;
        onX = backgroundImage.rectTransform.rect.width - (toggleIndicator.rect.width * 1.5f);

        Toggle(isOn);
    }

    public void Toggle(bool _isOn)
    {
        if (isOn != _isOn)
        {
            isOn = _isOn;

            ToggleColor(isOn);
            MoveIndicator(isOn);

            // Play Sound effect

            if (valueChanged != null)
                valueChanged(isOn);
        }
        else
        {
            isOn = _isOn;

            backgroundImage.color = isOn ? onColor : offColor;
            LeanTween.moveX(toggleIndicator, isOn ? onX : offX, 0);
        }
    }
    public bool GetIsOn()
    {
        return isOn;
    }

    private void ToggleColor(bool _isOn)
    {
        if (_isOn)
            LeanTween.color(backgroundImage.rectTransform, onColor, tweenTime);
        else
            LeanTween.color(backgroundImage.rectTransform, offColor, tweenTime);
    }

    private void MoveIndicator(bool _isOn)
    {
        if (_isOn)
            LeanTween.moveX(toggleIndicator, onX, tweenTime);
        else
            LeanTween.moveX(toggleIndicator, offX, tweenTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!isOn);
    }
}
