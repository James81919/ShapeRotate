using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Durations")]
    [SerializeField] private float fadeInDuration = 1;
    [SerializeField] private float fadeOutDuration = 1;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup rotateShapeTutorial;

    private Shape firstShapeRef;

    private bool canContinue = false;

    private void Start()
    {
        canvas.enabled = false;
        rotateShapeTutorial.gameObject.SetActive(false);
    }

    public void StartTutorial(Shape _firstShapeRef)
    {
        if (!IsTutorialComplete())
        {
            firstShapeRef = _firstShapeRef;

            StartCoroutine(StartTutorialProcess());
        }
    }
    private IEnumerator StartTutorialProcess()
    {
        canvas.enabled = true;

        ShowRotateShapeTutorial();

        yield return new WaitUntil(() => canContinue);

        SetTutorialComplete(true);
        canvas.enabled = false;
    }

    private void ShowRotateShapeTutorial()
    {
        canContinue = false;

        rotateShapeTutorial.alpha = 0;
        rotateShapeTutorial.gameObject.SetActive(true);

        LeanTween.alphaCanvas(rotateShapeTutorial, 1, fadeInDuration);
    }
    private void HideRotateShapeTutorial()
    {
        LeanTween.alphaCanvas(rotateShapeTutorial, 0, fadeOutDuration).setOnComplete(() =>
        {
            canContinue = true;
            rotateShapeTutorial.gameObject.SetActive(false);
        });
    }

    public bool IsTutorialComplete()
    {
        return PlayerPrefs.GetInt("IsTutorialComplete", 0) == 1 ? true : false;
    }
    public void SetTutorialComplete(bool _b)
    {
        PlayerPrefs.SetInt("IsTutorialComplete", _b ? 1 : 0);
    }

    // UI Buttons
    public void Continue()
    {
        firstShapeRef.RotateShape();

        HideRotateShapeTutorial();
    }
}
