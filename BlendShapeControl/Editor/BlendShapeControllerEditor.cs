using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlendShapeController))]
public class BlendShapeControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("", EditorStyles.helpBox);
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Blend Shapes", EditorStyles.boldLabel);
        SerializedObject so = base.serializedObject;
        BlendShapeController blendShapeControl = (BlendShapeController)target;
        so.UpdateIfRequiredOrScript();
        var BSTArray = so.FindProperty("blendShapeTargets");
        int BSTArrayLength = BSTArray.arraySize;

        if (GUILayout.Button("Reload BlendShape Targets", GUILayout.Width(200)))
        {
            blendShapeControl.ReloadBlendShapeTargets();
        }
        GUILayout.EndHorizontal();
        if (BSTArray != null && BSTArrayLength > 0)
        {
            for (int i = 0; i < BSTArrayLength; i++)
            {
                GUILayout.BeginVertical("Box");
                var target = BSTArray.GetArrayElementAtIndex(i);

                SerializedObject targetprop = new SerializedObject(target.objectReferenceValue);
                targetprop.UpdateIfRequiredOrScript();

                    string blendShapeInfo = "Name: " + targetprop.FindProperty("BlendShapeName").stringValue + "   Index: " + targetprop.FindProperty("BlendShapeIndex").intValue.ToString();
               EditorGUILayout.LabelField(blendShapeInfo);
              //  EditorGUILayout.LabelField("Name", targetprop.FindProperty("BlendShapeName").stringValue);
            //    EditorGUILayout.LabelField("BlendShape Index", targetprop.FindProperty("BlendShapeIndex").intValue.ToString());
                GUI.enabled = false;
                EditorGUILayout.PropertyField(targetprop.FindProperty("BlendShapeValue"), true);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(targetprop.FindProperty("BlendShapeDefaultValue"), true);
                targetprop.ApplyModifiedProperties();
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();
        so.ApplyModifiedProperties();
    }
}

