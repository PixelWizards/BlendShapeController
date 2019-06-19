using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlendShapeControlClip))]
public class BlendShapeControlClipEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SerializedObject so = base.serializedObject;
        so.UpdateIfRequiredOrScript();


        SerializedProperty behaviourProperty = so.FindProperty("template");



        EditorGUILayout.LabelField(behaviourProperty.FindPropertyRelative("BCBGuid").stringValue);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        //identify Skinned mesh with blend shapes
        GUILayout.Label(behaviourProperty.FindPropertyRelative("name").stringValue + " Blend Shapes", EditorStyles.boldLabel);

        // Only uncomment if problems are occurring and you need to visually examine the data being stored
       // EditorGUILayout.PropertyField(behaviourProperty.FindPropertyRelative("blendShapeKeyInfos"), true);

        //get array of BlendShapeKeys
        var BSTArray = behaviourProperty.FindPropertyRelative("blendShapeKeys");
        
        //iterate through BlendShapeTargets array if array isn't empty
        if (BSTArray != null && BSTArray.arraySize > 0)
        {
            for (int i = 0; i < BSTArray.arraySize; i++)
            {
                GUILayout.BeginVertical("Box");
                var target = BSTArray.GetArrayElementAtIndex(i);
                //not sure if retrieving multiple SerializedObjects is best practice, seems to work though
                var targetprop = new SerializedObject(target.objectReferenceValue);
                targetprop.UpdateIfRequiredOrScript();
                //informational label
                string blendShapeInfo = "Name: " + targetprop.FindProperty("BlendShapeName").stringValue + "   Index: " + targetprop.FindProperty("BlendShapeIndex").intValue.ToString();
                EditorGUILayout.LabelField(blendShapeInfo);


                //blemd shape value (normalized form 0 to 1)
                EditorGUILayout.PropertyField(targetprop.FindProperty("BlendShapeValue"), true);


                targetprop.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();
        so.ApplyModifiedProperties();
    }
}

