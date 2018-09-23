using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SpatialSourceClip : PlayableAsset, ITimelineClipAsset
{
    public SpatialSourceBehaviour template = new SpatialSourceBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpatialSourceBehaviour>.Create (graph, template);
        SpatialSourceBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
