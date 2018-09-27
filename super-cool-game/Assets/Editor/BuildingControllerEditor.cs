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

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Height(150));

        //for (int i = 0; i < sortedGrid1.Count; i += 3) {
        //    // The output
        //    string res = "";

        //    // TODO: Test it correctly with values
        //    for (int j = 0; j < 3; j++) {
        //        Block b1 = sortedGrid1[i + j];
        //        res += "[" + b1.type + "] ";
        //    }

        //    // Add spacing
        //    res += "....... ";

        //    // Not so sure about this
        //    for (int j = 2; j >= 0; j--) {
        //        Block b2 = sortedGrid2[i + j];
        //        res += "[" + b2.type + "] ";
        //    }

        //    EditorGUILayout.LabelField(res);
        //}

        EditorGUILayout.EndScrollView();

    }
}
