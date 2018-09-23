using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozzleGrabbable : GrabbableBehaviour
{
	public Transform Nozzle;
	public float spread = .1f;

	public override void GrabUpdate()
	{
		base.GrabUpdate();
		ParticleMan.Emit(1, 2, Nozzle.position, Nozzle.forward);
		for (int i = 0; i < 24; i++)
		{
			SudsMaster.TryKillSuds(new Ray(Nozzle.position, Nozzle.forward + new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * spread));
		}
	}
}
