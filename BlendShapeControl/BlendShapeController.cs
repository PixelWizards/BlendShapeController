using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BlendShapeController : MonoBehaviour
{
    [HideInInspector]
    public BlendShapeTarget[] blendShapeTargets;
    private SkinnedMeshRenderer targetMeshRenderer;
    public virtual void OnEnable()
    {
        targetMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (blendShapeTargets == null || blendShapeTargets.Length == 0)
        {
            ReloadBlendShapeTargets();
        }
    }
    public virtual void ReloadBlendShapeTargets()
    {
        Mesh targetMesh = targetMeshRenderer.sharedMesh;
        int blendShapeCount = targetMesh.blendShapeCount;
        blendShapeTargets = new BlendShapeTarget[blendShapeCount];
        for (int i = 0; i < blendShapeCount; i++)
        {
            BlendShapeTarget newTarget = ScriptableObject.CreateInstance(typeof(BlendShapeTarget)) as BlendShapeTarget;
            newTarget.init(targetMesh.GetBlendShapeName(i), i,  targetMeshRenderer);
            blendShapeTargets[i] = newTarget;
        }
    }

    public virtual void SetBlendShapeValues(float[] blendWeights)
    {
        if (blendWeights.Length == 0 || (blendWeights.Length != blendShapeTargets.Length))
            return;

        int blendshapeCount = blendShapeTargets.Length;

        for (int i = 0; i < blendshapeCount; i++)
        {
            blendShapeTargets[i].BlendShapeValue = blendWeights[i];
            targetMeshRenderer.SetBlendShapeWeight(blendShapeTargets[i].BlendShapeIndex, blendWeights[i] * 100);
        }
    }
}