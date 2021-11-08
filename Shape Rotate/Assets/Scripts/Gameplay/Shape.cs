using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RotationDirection
{
    TOP, // 0 degrees
    LEFT, // 90 degrees
    BOTTOM, // 180 degrees
    RIGHT // 270 degrees
}

public class Shape : MonoBehaviour
{
    public RotationDirection startRotation;
    public List<RotationDirection> alternateCorrectRotations;
    public Vector2 splitDirection = new Vector2(1, 1);
    public bool isCorrectRotation;

    private RectTransform rectTransform;
    [HideInInspector] public Button button;

    public Action checkIsLevelComplete;

    [Header("Shape Size")]
    private int width;
    private int height;

    private bool isRotating = false;

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
            case RotationDirection.RIGHT: rotation = -90; break;
            case RotationDirection.BOTTOM: rotation = 180; break;
            case RotationDirection.LEFT: rotation = 90; break;
        }

        return rotation;
    }

    public void InitialiseShape()
    {
        rectTransform = GetComponent<RectTransform>();
        completePosition = rectTransform.localPosition;

        // Set rotation
        switch (startRotation)
        {
            case RotationDirection.TOP: rectTransform.rotation = Quaternion.Euler(0, 0, 0); break;
            case RotationDirection.LEFT: rectTransform.rotation = Quaternion.Euler(0, 0, 90); break;
            case RotationDirection.BOTTOM: rectTransform.rotation = Quaternion.Euler(0, 0, 180); break;
            case RotationDirection.RIGHT: rectTransform.rotation = Quaternion.Euler(0, 0, 270); break;
        }

        CheckIsCorrectRotation();
    }
    public void RotateShape()
    {
        if (!isRotating && !isComplete)
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
    public void SetShapePosition(float _splitOffset)
    {
        transform.localPosition += new Vector3(splitDirection.x, splitDirection.y, 0) * _splitOffset;
    }
    public void CompletePiece()
    {
        isComplete = true;
        LeanTween.move(rectTransform, completePosition, 0.25f).setEase(LeanTweenType.easeInBack);
    }
}
