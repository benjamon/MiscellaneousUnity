using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.6313577f, 0.1367925f, 1f)]
[TrackClipType(typeof(InstantReplayClip))]
[TrackBindingType(typeof(Commander))]
public class InstantReplayTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<InstantReplayMixerBehaviour>.Create (graph, inputCount);
    }
}
