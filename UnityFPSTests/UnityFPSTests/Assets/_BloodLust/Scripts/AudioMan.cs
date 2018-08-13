using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioMan : MonoBehaviour {
    private static List<AudioSound> Sounds;

    public AudioSound[] soundArray;

    private void Awake()
    {
        Sounds = new List<AudioSound>();
        Sounds.AddRange(soundArray);
    }

    public static AudioSource PlaySound(SoundName name)
    {
        if (Sounds.Exists(x => x.name == name))
        {
            return ChooseSource(Sounds.Find(x => x.name == name));
        }
        return null;
    }

	public void PlaySoundLocal(SoundName soundName)
	{
		PlaySound(soundName);
	}

    private static AudioSource ChooseSource(AudioSound sound)
    {
        int tries = sound.Sources.Length;
        int i = 0;
        while (tries > 0)
        {
            i = UnityEngine.Random.Range(0, sound.Sources.Length - 1);
            tries--;
            if (!sound.Sources[i].isPlaying || tries == 0)
            {
                sound.Sources[i].Play();
                return sound.Sources[i];
            }
        }
        return null;
    }
}

[Serializable]
public class AudioSound
{
    public SoundName name;
    public AudioSource[] Sources;
}

public enum SoundName
{
	GunFired,
	Footstep
}