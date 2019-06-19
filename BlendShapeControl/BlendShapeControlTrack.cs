using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

[TrackColor(1f, 0f, 1f)]
[TrackClipType(typeof(BlendShapeControlClip))]
[TrackBindingType(typeof(BlendShapeController))]
public class BlendShapeControlTrack : TrackAsset
{
    //_mTrackBinding serves as output target for track but alos as a source of properties for the BlendShapeControlBehaviour
    public BlendShapeController _mTrackBinding;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        if (_mTrackBinding == null)
        {
            BlendShapeController trackBinding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as BlendShapeController;
            _mTrackBinding = trackBinding;
        }
        //iterator for all currently existing clips
        IEnumerable<TimelineClip> clips = GetClips();
        //check if track binding has been set 
        if (_mTrackBinding != null)
        {
            List<string> clipGuids = new List<string>();
            //check each clip
            foreach (var item in clips)
            {
                //rename clip if not customized by user
                if (item.displayName == "BlendShapeControlClip")
                {
                    item.displayName = _mTrackBinding.transform.root.name + " " + _mTrackBinding.name;
                }

                //get blendShapeControlClip encapulated in TimelineClip
                BlendShapeControlClip blendShapeControlClip = item.asset as BlendShapeControlClip;

                //get BlendShapeControlBehaviour encapulated in BlendShapeControlClip
                BlendShapeControlBehaviour blendShapeControlBehaviour = blendShapeControlClip.template;

                //check just in case there aren't any blendshapes on the attached controller
                if (_mTrackBinding.blendShapeTargets.Length > 0)
                {
                    //update  blendShapeTargets from _mTrackBinding to BlendShapeController
                    clipGuids.Add(blendShapeControlBehaviour.LoadBlendShapeTargets(_mTrackBinding.blendShapeTargets, clipGuids));
                }
                //this is just informational so the inmspector will make it clear what object is being edited
                blendShapeControlBehaviour.name = _mTrackBinding.transform.root.name + " " + _mTrackBinding.name;
            }
        }
        return ScriptPlayable<BlendShapeControlMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        //get the track binding for this track, if any
        BlendShapeController trackBinding = director.GetGenericBinding(this) as BlendShapeController;
        _mTrackBinding = trackBinding;
        if (trackBinding == null)
            return;
        var serializedObject = new UnityEditor.SerializedObject(trackBinding);
        var iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.hasVisibleChildren)
                continue;
            driver.AddFromName<BlendShapeController>(trackBinding.gameObject, iterator.propertyPath);
        }
#endif
        base.GatherProperties(director, driver);
    }
}
