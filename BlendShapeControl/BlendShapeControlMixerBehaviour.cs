using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BlendShapeControlMixerBehaviour : PlayableBehaviour
{
    BlendShapeController m_TrackBinding;
    bool m_FirstFrameHappened;
    float[] blendShapeDefaults;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
   //     Debug.Log(" Mixer ProcessFrame");
        m_TrackBinding = playerData as BlendShapeController;

        if (m_TrackBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_FirstFrameHappened = true;

            if (m_TrackBinding.blendShapeTargets != null && m_TrackBinding.blendShapeTargets.Length > 0)
            {
                blendShapeDefaults = new float[m_TrackBinding.blendShapeTargets.Length];

                for (int i = 0; i < m_TrackBinding.blendShapeTargets.Length; i++)
                {
                    blendShapeDefaults[i] = m_TrackBinding.blendShapeTargets[i].BlendShapeDefaultValue;
                }
            }
        }

        int inputCount = playable.GetInputCount();

        float[] blendShapeWeights = new float[m_TrackBinding.blendShapeTargets.Length];

        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<BlendShapeControlBehaviour> inputPlayable = (ScriptPlayable<BlendShapeControlBehaviour>)playable.GetInput(i);
            BlendShapeControlBehaviour input = inputPlayable.GetBehaviour();


            if (blendShapeWeights.Length > 0 && input.blendShapeKeys != null && input.blendShapeKeys.Length == blendShapeWeights.Length)
            {
                for (int j = 0; j < blendShapeWeights.Length; j++)
                {
                    blendShapeWeights[j] += input.blendShapeKeys[j].BlendShapeValue * inputWeight;
                }
            }
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately(inputWeight, 0f))
                currentInputs++;
        }
        float remainingweight = 1 - totalWeight;
        for (int i = 0; i < blendShapeWeights.Length; i++)
        {
            blendShapeWeights[i] += blendShapeDefaults[i] * remainingweight;
        }

        m_TrackBinding.SetBlendShapeValues(blendShapeWeights);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        m_FirstFrameHappened = false;

        if (m_TrackBinding == null)
            return;
        if (m_TrackBinding.blendShapeTargets != null && blendShapeDefaults.Length > 0)
        {
            m_TrackBinding.SetBlendShapeValues(blendShapeDefaults);
        }
    }
}
