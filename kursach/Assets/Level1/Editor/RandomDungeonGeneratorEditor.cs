using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGeneration), true)]

public class RandomDungeonGeneratorEditor : Editor
{
    DungeonGeneration generator;

    private void Awake() 
    {
        generator = (DungeonGeneration)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 
        if(GUILayout.Button("Create Dungeon")) 
        {
            generator.GenerateDungeon();
        }
    }
}
