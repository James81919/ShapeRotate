using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float splitOffset;
    public List<GameObject> levels;

    [Header("UI")]
    public GameObject nextButton;

    [HideInInspector] public bool isLevelComplete;

    private Shape[] shapes;

    private int packID, levelID;

    private void Start()
    {
        Invoke("CreateFirstPuzzle", Time.deltaTime);

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(false);
        }
    }

    private void CreateFirstPuzzle()
    {
        CreatePuzzle(1, 1);
    }

    public void CreatePuzzle(int _packID, int _levelID)
    {
        nextButton.SetActive(false);

        packID = _packID;
        levelID = _levelID;

        GetAllShapes();
        SplitPuzzle();
    }
    public void ClearPuzzle()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            Destroy(shapes[i]);
        }
    }

    private void GetAllShapes()
    {
        if (levelID <= 0 || levelID > levels.Count)
        {
            Debug.LogError("NO LEVEL WITH ID of " + levelID);
            return;
        }

        for (int i = 0; i < levels.Count; i++)
        {
            if (i == levelID - 1)
                levels[i].SetActive(true);
            else
                levels[i].SetActive(false);
        }
        // Gets all the shapes
        shapes = levels[levelID - 1].GetComponentsInChildren<Shape>();

        // Attaches CheckIsLevelComplete function
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].checkIsLevelComplete = CheckIsLevelComplete;
            shapes[i].InitialiseShape();
        }

        int rand = Random.Range(0, 3);
        float rot = 0;
        switch (rand)
        {
            case 0: rot = 0; break;
            case 1: rot = -90; break;
            case 2: rot = 180; break;
            case 3: rot = 90; break;
        }
        levels[levelID - 1].GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, rot);
    }

    private void CreatePieces()
    {

    }
    private void SplitPuzzle()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].SetShapePosition(splitOffset);
        }
    }
    private void CombinePuzzle()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].CompletePiece();
        }
    }

    public void CheckIsLevelComplete()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            if (!shapes[i].isCorrectRotation)
            {
                isLevelComplete = false;
                return;
            }
        }

        isLevelComplete = true;
        CombinePuzzle();
        nextButton.SetActive(true);
    }
    public void Button_RestartLevel()
    {
        LevelLoader.Instance.LoadLevel("game");
    }
    public void Button_NextLevel()
    {
        LevelLoader.Instance.Fade(() => {
            ClearPuzzle();
            CreatePuzzle(packID, levelID + 1);
        });
    }
}
