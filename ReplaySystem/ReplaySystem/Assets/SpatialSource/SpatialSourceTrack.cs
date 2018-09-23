using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.7830189f, 0.4816762f, 0.2400766f)]
[TrackClipType(typeof(SpatialSourceClip))]
[TrackBindingType(typeof(AudioSource))]
public class SpatialSourceTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SpatialSourceMixerBehaviour>.Create (graph, inputCount);
    }
}
