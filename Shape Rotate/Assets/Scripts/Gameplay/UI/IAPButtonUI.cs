using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;

public class IAPButtonUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;

    private IAPButton iapButton;

    private void Start()
    {
        iapButton = GetComponent<IAPButton>();

        UpdateTitleText();
        UpdateDescriptionText();
        UpdatePriceText();
    }

    public void UpdateTitleText()
    {
        if (titleText)
            titleText.text = CodelessIAPStoreListener.Instance.GetProduct(iapButton.productId).metadata.localizedTitle;
    }

    public void UpdateDescriptionText()
    {
        if (descriptionText)
            descriptionText.text = CodelessIAPStoreListener.Instance.GetProduct(iapButton.productId).metadata.localizedDescription;
    }

    public void UpdatePriceText()
    {
        if (priceText)
            priceText.text = CodelessIAPStoreListener.Instance.GetProduct(iapButton.productId).metadata.localizedPriceString;
    }

    public void ChangeProductID(string _id)
    {
        if (iapButton)
            iapButton.productId = _id;
    }
}
