using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozzleGrabbable : GrabbableBehaviour
{
	public Transform Nozzle;

	public override void GrabUpdate()
	{
		base.GrabUpdate();
		ParticleMan.Emit(1, 1, Nozzle.position, Nozzle.forward);
	}
}
