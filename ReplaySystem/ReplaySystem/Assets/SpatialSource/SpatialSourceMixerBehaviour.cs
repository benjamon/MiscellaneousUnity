using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SpatialSourceMixerBehaviour : PlayableBehaviour
{
	// NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        AudioSource trackBinding = playerData as AudioSource;

		if (info.weight > 0f && !trackBinding.isPlaying)
		{
			trackBinding.spatialBlend = 1f;
			trackBinding.spatialize = true;
			trackBinding.outputAudioMixerGroup = null;
			trackBinding.Play();
		}

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<SpatialSourceBehaviour> inputPlayable = (ScriptPlayable<SpatialSourceBehaviour>)playable.GetInput(i);
            SpatialSourceBehaviour input = inputPlayable.GetBehaviour ();
			
            
            // Use the above variables to process each frame of this playable.
            
        }
    }
}
