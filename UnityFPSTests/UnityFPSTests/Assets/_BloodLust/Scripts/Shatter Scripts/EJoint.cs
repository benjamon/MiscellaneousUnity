using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJoint : DestructableBehaviour
{
	public EJointerV2 masta;

	// Use this for initialization
	void Start () {
		
	}

	public override void TakeDamage(float damage, Collision c)
	{
		if (masta != null)
			masta.GetNearest(c.contacts[0].point);
	}	
}
