// using UnityEngine;
// using UnityEditor;

//  [CustomEditor(typeof(OutputScript))]
//  public class OutputScriptEditor : Editor
//  {
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        var myScript = target as OutputScript;
//        myScript.placeGameObject = GUILayout.Toggle(myScript.placeGameObject, "Place Game Object");

//        if(myScript.placeGameObject)
//        myScript.placeObject = EditorGUILayout.ObjectField("Place Object", myScript.placeObject, typeof(GameObject), true) as GameObject; 
//    }
//  }
