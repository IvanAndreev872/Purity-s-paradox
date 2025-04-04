using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generation), true)]

public class RandomDungeonGeneratorEditor : Editor
{
    Generation generator;

    private void Awake() 
    {
        generator = (Generation)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 
        if(GUILayout.Button("Generate")) 
        {
            generator.GenerateDungeon();
        }
    }
}
