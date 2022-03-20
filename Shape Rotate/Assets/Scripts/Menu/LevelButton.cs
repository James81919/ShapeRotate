using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public Image lockedIcon;

    public Color lockedColour;

    public void SetupButton(GameManager _gameManager, int _packID, int _levelID, bool isUnlocked)
    {
        if (isUnlocked)
        {
            levelNumberText.gameObject.SetActive(true);
            lockedIcon.gameObject.SetActive(false);

            levelNumberText.text = (_levelID + 1).ToString();
            GetComponent<Button>().onClick.AddListener(() => _gameManager.LoadPuzzle(_packID, _levelID));
        }
        else
        {
            lockedIcon.gameObject.SetActive(true);
            levelNumberText.gameObject.SetActive(false);

            GetComponent<Image>().color = lockedColour;
        }
    }
}
