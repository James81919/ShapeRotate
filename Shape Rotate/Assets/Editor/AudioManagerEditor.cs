using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    string soundName;

    private AudioManager audioManager
    {
        get
        {
            return target as AudioManager;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Sort"))
        {
            audioManager.sfxList.Sort(delegate (SoundEffect x, SoundEffect y) { return string.Compare(x.name, y.name); });
        }
    }
}
