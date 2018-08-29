using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMan : MonoBehaviour
{
	public static ParticleMan Instance;
	public ParticleCollection[] Emitters;
	// Use this for initialization
	void OnEnable ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void Emit(int index, int count, Vector3 position, Vector3 direction)
	{
		ParticleSystem ps = Instance.Emitters[index].Emitters
			[UnityEngine.Random.Range(0, Instance.Emitters[index].Emitters.Length)];
		ps.transform.position = position;
		ps.transform.rotation = Quaternion.identity;
		ps.transform.forward = direction;
		ps.Emit(count);
	}
}

[Serializable]
public struct ParticleCollection
{
	public string Title;
	public ParticleSystem[] Emitters;
}