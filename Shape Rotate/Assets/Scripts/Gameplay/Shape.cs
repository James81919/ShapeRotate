using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RotationDirection
{
    TOP = 0, // 0 degrees
    LEFT = 1, // 90 degrees
    BOTTOM = 2, // 180 degrees
    RIGHT = 3 // 270 degrees
}

public class Shape : MonoBehaviour
{
    public RotationDirection startRotation;
    public List<RotationDirection> alternateCorrectRotations;
    public Vector2 splitDirection = new Vector2(1, 1);
    public bool isCorrectRotation;

    [SerializeField] private GameObject outlineParent;
    [SerializeField] private GameObject tileOutlinePrefab;

    private RectTransform rectTransform;
    [HideInInspector] public Button button;

    public Action checkIsLevelComplete;

    [Header("Shape Size")]
    private int width;
    private int height;

    private bool isRotating = false;
    private bool isLocked = false;

    private Vector3 completePosition;
    [HideInInspector] public bool isComplete = false;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(RotateShape);
    }

    private void CheckIsCorrectRotation()
    {
        float rot = rectTransform.localRotation.eulerAngles.z;

        if (rot >= -0.01f && rot <= 0.01f)
        {
            isCorrectRotation = true;
            return;
        }

        for (int i = 0; i < alternateCorrectRotations.Count; i++)
        {
            if (rot == GetRotationFromRotationDirection(alternateCorrectRotations[i]))
            {
                isCorrectRotation = true;
                return;
            }
        }

        isCorrectRotation = false;
    }
    private float GetRotationFromRotationDirection(RotationDirection _rotationDirection)
    {
        float rotation = 0;

        switch (_rotationDirection)
        {
            case RotationDirection.TOP: rotation = 0; break;
            case RotationDirection.RIGHT: rotation = 270; break;
            case RotationDirection.BOTTOM: rotation = 180; break;
            case RotationDirection.LEFT: rotation = 90; break;
        }

        return rotation;
    }

    public void InitialiseShape(Action _checkIsLevelComplete, PuzzleShapeData _shapeData)
    {
        checkIsLevelComplete = _checkIsLevelComplete;

        rectTransform = GetComponent<RectTransform>();
        completePosition = rectTransform.localPosition;

        startRotation = (RotationDirection)_shapeData.startRotation;

        alternateCorrectRotations = _shapeData.alternateCorrectRotations;

        splitDirection = new Vector2(_shapeData.anchorPointX, _shapeData.anchorPointY);

        // Set rotation
        switch (startRotation)
        {
            case RotationDirection.TOP: rectTransform.rotation = Quaternion.Euler(0, 0, 0); break;
            case RotationDirection.LEFT: rectTransform.rotation = Quaternion.Euler(0, 0, 90); break;
            case RotationDirection.BOTTOM: rectTransform.rotation = Quaternion.Euler(0, 0, 180); break;
            case RotationDirection.RIGHT: rectTransform.rotation = Quaternion.Euler(0, 0, 270); break;
        }

        SetupShapeOutline();

        CheckIsCorrectRotation();
    }
    public void RotateShape()
    {
        if (!isRotating && !isComplete && !isLocked)
        {
            isRotating = true;
            LeanTween.rotateAround(rectTransform, new Vector3(0, 0, 1), -90, 0.25f).setOnComplete(() =>
            {
                isRotating = false;
                CheckIsCorrectRotation();
                checkIsLevelComplete();
            });
        }
    }
    public void RotateToCorrectRotation()
    {
        if (!isRotating && !isComplete)
        {
            isRotating = true;
            LeanTween.rotateAround(rectTransform, new Vector3(0, 0, 1), -90, 0.25f).setOnComplete(() =>
            {
                isRotating = false;
                CheckIsCorrectRotation();
                checkIsLevelComplete();
                if (!isCorrectRotation)
                {
                    RotateToCorrectRotation();
                }
                else
                {
                    LockPiece();
                }
            });
        }
    }
    public void SetShapePosition(float _splitOffset)
    {
        transform.localPosition += new Vector3(splitDirection.x, splitDirection.y, 0) * _splitOffset;
    }
    public void CompletePiece()
    {
        isComplete = true;
        DisableOutline();
        LeanTween.move(rectTransform, completePosition, 0.25f).setEase(LeanTweenType.easeInBack);
    }

    public void LockPiece()
    {
        // Lock shape
        isLocked = true;

        // Make outline appear around shape
        EnableOutline();
    }

    private void SetupShapeOutline()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);

            if (child.name != "Outline")
                Instantiate(tileOutlinePrefab, outlineParent.transform).transform.position = child.position;
        }

        DisableOutline();
    }

    private void EnableOutline()
    {
        outlineParent.SetActive(true);
    }
    private void DisableOutline()
    {
        outlineParent.SetActive(false);
    }
}
