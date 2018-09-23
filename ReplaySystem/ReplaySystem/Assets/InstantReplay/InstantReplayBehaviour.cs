using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class InstantReplayBehaviour : PlayableBehaviour
{

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		Commander.EndAndReplay();
	}
}
