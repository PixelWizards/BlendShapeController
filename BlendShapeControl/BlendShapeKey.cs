using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;





[Serializable]
public struct BlendShapeKeyInfo
{

    public float BlendShapeValue;
    public string BlendShapeName;
    public int BlendShapeIndex;
    public BlendShapeKeyInfo(string name, int index, float value)
    {
        BlendShapeName = name;
        BlendShapeIndex = index;
        BlendShapeValue = value;
    }
}

[System.Serializable]
public class BlendShapeKey : ScriptableObject
{
    public delegate void UpdateValue(int index, float value);
    public UpdateValue updateBlendValue;
    public string BlendShapeName;
    public int BlendShapeIndex;

    [RangeAttribute(0, 1)]
    public float BlendShapeValue = 0;

    public void init(string name, int index, float value)
    {
        BlendShapeName = name;
        BlendShapeIndex = index;
        BlendShapeValue = value;
    }

    void OnValidate()
    {
   
        if (!Application.isPlaying && updateBlendValue!= null )
        {


            updateBlendValue(BlendShapeIndex, BlendShapeValue);
        }
    }

    void OnDestroy()
    {
        updateBlendValue = null;
    }
}
