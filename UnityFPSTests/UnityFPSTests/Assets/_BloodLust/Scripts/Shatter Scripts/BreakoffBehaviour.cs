using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoffBehaviour : DestructableBehaviour
{
	private int killnum = 0;
	public BreakoffBehaviour friendo = null;

	// Use this for initialization
	void Start ()
	{
		gameObject.tag = "Destructable";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void TakeDamage(float damage, Collision col)
	{
		health -= damage;
		if (health < 0)
			Die();
	}

	void Die()
	{
		transform.parent = null;
		GetComponent<Rigidbody>().isKinematic = false;
		gameObject.tag = "Untagged";
		if (friendo != null)
		{
			friendo.transform.parent = null;
			friendo.GetComponent<Rigidbody>().isKinematic = false;
			friendo.tag = "Untagged";
			Destroy(friendo);
		}
		Destroy(this);
	}
}
