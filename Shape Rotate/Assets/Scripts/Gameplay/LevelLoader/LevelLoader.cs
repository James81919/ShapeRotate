using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    public float transitionTime = 1f;

    [Header("UI")]
    public Canvas canvas;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            canvasGroup = canvas.GetComponent<CanvasGroup>();
            canvas.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string _sceneName)
    {
        Fade(() => {
            SceneManager.LoadScene(_sceneName);
        });
    }

    public void Fade(Action _duringFadeAction)
    {
        FadeOut(() => {
            _duringFadeAction();
            FadeIn(() => { });
        });
    }

    // Fades out of scene to black
    private void FadeOut(Action _onComplete)
    {
        canvas.enabled = true;
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, transitionTime).setOnComplete(_onComplete);
    }

    // Fades in to scene
    private void FadeIn(Action _onComplete)
    {
        LeanTween.alphaCanvas(canvasGroup, 0, transitionTime).setOnComplete(() => {
            canvas.enabled = false;
            _onComplete();
        });
    }
}
