using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        DungeonGenerator dungeonGenerator = (DungeonGenerator)target;

        //if (GUILayout.Button("Generate")) {
        //    dungeonGenerator.StartGeneration();
        //}
    }
}