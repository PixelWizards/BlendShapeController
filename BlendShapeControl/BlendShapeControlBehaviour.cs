using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class BlendShapeControlBehaviour : PlayableBehaviour
{
    //name of gameObject  BlendShapeController is attached to
    public string name;

    public string BCBGuid = Guid.Empty.ToString();
    public BlendShapeKey[] blendShapeKeys = new BlendShapeKey[0];
    //long term serializeable storage of clip data 
    public BlendShapeKeyInfo[] blendShapeKeyInfos = new BlendShapeKeyInfo[0];
    public void GenerateGuid()
    {
        BCBGuid = Guid.NewGuid().ToString();
    }

    public void UpdateBlendValues(int index, float value)
    {
        blendShapeKeyInfos[index].BlendShapeValue = value; 
    }


    //Called by BlendShapeControlTrack from GatherProperties Method
    public string LoadBlendShapeTargets(BlendShapeTarget[] newTargets, List<string> clipGuids)
    {
        if (newTargets.Length == 0)
            return "";

        if (BCBGuid == Guid.Empty.ToString())
        {
            //new BlendShapeControlBehaviour, make guid and target array
            blendShapeKeys = new BlendShapeKey[newTargets.Length];
            blendShapeKeyInfos = new BlendShapeKeyInfo[newTargets.Length];

            GenerateGuid();
            for (int i = 0; i < newTargets.Length; i++)
            {
                BlendShapeKey newKey = ScriptableObject.CreateInstance(typeof(BlendShapeKey)) as BlendShapeKey;
                newKey.init(newTargets[i].BlendShapeName, i, newTargets[i].BlendShapeValue);
                newKey.updateBlendValue += UpdateBlendValues;
                blendShapeKeys[i] = newKey;
                blendShapeKeyInfos[i] = new BlendShapeKeyInfo(newKey.BlendShapeName, i, newKey.BlendShapeValue);
            }
        }
        else
        {
            // BlendShapeControlBehaviour already exists
            blendShapeKeys = new BlendShapeKey[newTargets.Length];

            if (clipGuids.Contains(BCBGuid))
            {
                // BCBGuid duplicates existing one, therefore, this must be a new duplicate
                // and needs a fresh guid
                GenerateGuid();

                // need to copy values from old objects to new object
                for (int i = 0; i < newTargets.Length; i++)
                {
                    BlendShapeKey newKey = ScriptableObject.CreateInstance(typeof(BlendShapeKey)) as BlendShapeKey;
                    newKey.init(blendShapeKeyInfos[i].BlendShapeName, i, blendShapeKeyInfos[i].BlendShapeValue);
                    newKey.updateBlendValue += UpdateBlendValues;
                    blendShapeKeys[i] = newKey;

                }
            }
            else
            {
                //BCBGuid for this clip is unique, therefore do nothing unless list of blend shapes on BlendShapeController has changed 
                //doesn't need a fresh guid
                for (int i = 0; i < newTargets.Length; i++)
                {
                    BlendShapeKey newKey = ScriptableObject.CreateInstance(typeof(BlendShapeKey)) as BlendShapeKey;
                    newKey.init(blendShapeKeyInfos[i].BlendShapeName, i, blendShapeKeyInfos[i].BlendShapeValue);
                    newKey.updateBlendValue += UpdateBlendValues;
                    blendShapeKeys[i] = newKey;
                }
            }
        }
        return BCBGuid;
    }
}

