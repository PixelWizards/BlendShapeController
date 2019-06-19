using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBlendShapeOwner
{

}

[System.Serializable]
public class BlendShapeTarget : ScriptableObject
{
    [SerializeField]
    public string BlendShapeName;
    [SerializeField]
    public int BlendShapeIndex;
    [SerializeField]
    [RangeAttribute(0, 1)]
    public float BlendShapeValue = 0;
    [RangeAttribute(0, 1)]
    [SerializeField]
    public float BlendShapeDefaultValue = 0;
    [SerializeField]
    public IBlendShapeOwner owningBehaviour;
    [SerializeField]
    public SkinnedMeshRenderer TargetMesh;

    public void init(string name, int index, SkinnedMeshRenderer mesh = null)
    {
        BlendShapeName = name;
        BlendShapeIndex = index;
        TargetMesh = mesh;
    }

    void OnValidate()
    {
        if (TargetMesh != null)
        {
            BlendShapeValue = BlendShapeDefaultValue;
            TargetMesh.SetBlendShapeWeight(BlendShapeIndex, BlendShapeDefaultValue * 100);
        }
    }
}
