using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMan : MonoBehaviour
{
	public static ParticleMan Instance;
	public ParticleSystem[] Emitters;
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
		ParticleSystem ps = Instance.Emitters[index];
		ps.transform.position = position;
		ps.transform.rotation = Quaternion.identity;
		ps.transform.forward = direction;
		Instance.Emitters[index].Emit(count);
	}
}
