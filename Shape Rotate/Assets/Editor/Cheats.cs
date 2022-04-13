using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Cheats : EditorWindow
{
    public Texture ventureGamesLogo;
    public Texture coinTexture;

    private int coins;

    #region TAB BOOLS
    private bool coinTabOpen = false;
    private bool levelTabOpen = false;
    private bool tutorialTabOpen = false;
    #endregion

    Vector2 scrollPos;

    private List<PuzzlePackSaveData> puzzlePacks;

    [MenuItem("Window/Venture Games/Cheats")]
    static void Init()
    {
        Cheats window = (Cheats)EditorWindow.GetWindow(typeof(Cheats));
        window.Show();
    }

    private void OnGUI()
    {
        GUIStyle ventureLogoStyle = new GUIStyle(GUI.skin.label);
        ventureLogoStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Space(5);
        GUILayout.Label(AssetPreview.GetAssetPreview(ventureGamesLogo), ventureLogoStyle);
        GUILayout.Space(5);
        DrawTitle("Cheats", 0, 0, 2);

        GUIStyle headingStyle = new GUIStyle(GUI.skin.label);
        headingStyle.alignment = TextAnchor.MiddleCenter;
        headingStyle.imagePosition = ImagePosition.ImageAbove;
        headingStyle.fixedHeight = 50;

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        #region --- Coins Cheats ---
        DrawTab("Coins", ref coinTabOpen, () =>
        {
            GUILayout.Label(new GUIContent(CoinManager.GetCoinAmount().ToString(), AssetPreview.GetAssetPreview(coinTexture)), headingStyle);
            coins = EditorGUILayout.IntField("Coins To Add:", coins);

            if (GUILayout.Button("Set"))
            {
                CoinManager.SetCoinAmount(coins);
                coins = 0;
            }
            if (GUILayout.Button("Add"))
            {
                CoinManager.AddCoins(coins);
            }
            EditorGUILayout.Space(2);
        });
        #endregion

        #region --- Level Cheats ---
        DrawTab("Levels", ref levelTabOpen, () =>
        {
            puzzlePacks = PuzzleLoader.LoadPuzzlePacks();

            for (int i = 0; i < puzzlePacks.Count; i++)
            {
                DrawTitle("Pack " + i);

                PuzzlePackSaveData packData = puzzlePacks[i];
                packData.isUnlocked = EditorGUILayout.Toggle("Is Unlocked", puzzlePacks[i].isUnlocked);
                packData.currentLevel = EditorGUILayout.IntField("Levels Unlocked", puzzlePacks[i].currentLevel);
                puzzlePacks[i] = packData;

                EditorGUILayout.Space();
            }

            PuzzleLoader.SavePuzzlePacks(puzzlePacks);

        });
        #endregion

        #region --- Tutorial Cheats ---
        DrawTab("Tutorial", ref tutorialTabOpen, () =>
        {
            PlayerPrefs.SetInt("IsTutorialComplete", (EditorGUILayout.Toggle("Is Tutorial Complete", PlayerPrefs.GetInt("IsTutorialComplete", 0) > 0)) ? 1 : 0);
        });
        #endregion

        GUILayout.EndScrollView();
    }

    private void DrawTab(string _title, ref bool _tabOpen, Action _content)
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        DrawTitle(_title, ref _tabOpen);
        if (_tabOpen)
        {
            DrawDivider(1, 0, 2, new Color(0.7f, 0.7f, 0.7f));

            _content();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawDivider(int _thickness, int _paddingTop, int _paddingBottom)
    {
        GUILayout.Space(_paddingTop);
        Rect rect = EditorGUILayout.GetControlRect(false, _thickness);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f));
        GUILayout.Space(_paddingBottom);
    }
    private void DrawDivider(int _thickness, int _paddingTop, int _paddingBottom, Color _color)
    {
        GUILayout.Space(_paddingTop);
        Rect rect = EditorGUILayout.GetControlRect(false, _thickness);
        EditorGUI.DrawRect(rect, _color);
        GUILayout.Space(_paddingBottom);
    }
    private void DrawTitle(string _text, ref bool _tabOpen, int _dividerThickness = 1)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        //DrawDivider(_dividerThickness, 20, 10);
        _tabOpen = EditorGUILayout.Foldout(_tabOpen, _text);//, titleStyle);
        //DrawDivider(_dividerThickness, 10, 10);
    }
    private void DrawTitle(string _text, int _paddingTop = 10, int _paddingBottom = 10, int _dividerThickness = 1)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        DrawDivider(_dividerThickness, _paddingTop, 10);
        EditorGUILayout.LabelField(_text, titleStyle);
        DrawDivider(_dividerThickness, 10, _paddingBottom);
    }
}
