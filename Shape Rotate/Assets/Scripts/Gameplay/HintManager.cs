using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HintManager
{
    private const string playerPrefsName = "HintsRemaining";
    private const int startingHintNum = 3;

    public static void UseHint(List<Shape> _shapeList)
    {
        //if (!CanUseHint())
        //{
        //    return;
        //}

        //Shape correctShape;

        //// Chooses shape to rotate
        //List<Shape> oneCorrectRotationShapes = new List<Shape>();
        //List<Shape> twoCorrectRotationShapes = new List<Shape>();
        //List<Shape> fourCorrectRotationShapes = new List<Shape>();

        //for (int i = 0; i < _shapeList.Count; i++)
        //{
        //    if (!_shapeList[i].isCorrectRotation)
        //    {
        //        switch (_shapeList[i].alternateCorrectRotations.Count)
        //        {
        //            case 0: oneCorrectRotationShapes.Add(_shapeList[i]); break;
        //            case 1: twoCorrectRotationShapes.Add(_shapeList[i]); break;
        //            case 3: fourCorrectRotationShapes.Add(_shapeList[i]); break;
        //        }
        //    }
        //}

        //if (oneCorrectRotationShapes.Count > 0)
        //    correctShape = oneCorrectRotationShapes[Random.Range(0, oneCorrectRotationShapes.Count)];
        //else if (twoCorrectRotationShapes.Count > 0)
        //    correctShape = twoCorrectRotationShapes[Random.Range(0, twoCorrectRotationShapes.Count)];
        //else if (fourCorrectRotationShapes.Count > 0)
        //    correctShape = fourCorrectRotationShapes[Random.Range(0, fourCorrectRotationShapes.Count)];
        //else
        //    return;

        //AddHints(-1);

        //correctShape.RotateToCorrectRotation();
    }

    public static int GetHintsRemaining()
    {
        return PlayerPrefs.GetInt(playerPrefsName, startingHintNum);
    }

    public static bool CanUseHint()
    {
        return PlayerPrefs.GetInt(playerPrefsName, startingHintNum) > 0;
    }

    public static void AddHints(int _hintNum)
    {
        PlayerPrefs.SetInt(playerPrefsName, GetHintsRemaining() + _hintNum);
    }

    public static void SetHints(int _hintNum)
    {
        PlayerPrefs.SetInt(playerPrefsName, _hintNum);
    }
}
