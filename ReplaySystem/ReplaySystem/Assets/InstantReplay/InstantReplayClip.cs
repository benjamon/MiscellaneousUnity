using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class InstantReplayClip : PlayableAsset, ITimelineClipAsset
{
    public InstantReplayBehaviour template = new InstantReplayBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.ClipIn; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<InstantReplayBehaviour>.Create (graph, template);
        InstantReplayBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
