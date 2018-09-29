using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingController))]
public class Building : Editor {

    Vector2 scrollPos;

    public override void OnInspectorGUI() {
        // Do default stuff
        base.OnInspectorGUI();

        // The component we're editing
        BuildingController bg = (BuildingController)target;

        // Title label
        EditorGUILayout.LabelField("Grid Type Representation");

        IEnumerable<Block> sortedBlocks = from b in bg.placedBlocks
                                          orderby b.position.y, b.position.x ascending
                                          select b;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Height(150));


        int y = 0;
        string output = "";
        foreach (var b in sortedBlocks) {
            // If the row is the same, just add the block
            if (y == b.position.y) {
                output += "[" + b.type + "| " + b.position.x + ", " + y + "]";
            } else {
                // print the previous row and reset and start the next
                EditorGUILayout.LabelField(output);
                output = "[" + b.type + "| " + b.position.x + ", " + y + "]";
                y = b.position.y;
            }
        }
        EditorGUILayout.LabelField(output);

        EditorGUILayout.EndScrollView();

    }
}
