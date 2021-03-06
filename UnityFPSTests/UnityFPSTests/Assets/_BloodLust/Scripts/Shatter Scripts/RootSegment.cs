﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootSegment : DestructableBehaviour
{
	public List<Transform> Forms;
	public float BreakForce = 5f;
	public float Density = 1f;
	public Vector3 StartVelocity;
	private Rigidbody body;

	// Use this for initialization
	void OnEnable ()
	{
		Forms = new List<Transform>();
		//GetComponent<MeshRenderer>().material.color = new Color(.6f,.4f,.4f);
		Forms.AddRange(GetComponentsInChildren<Transform>());
		body = gameObject.AddComponent<Rigidbody>();

	}

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		CalculateBodyStuff();
		body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
	}

	public override void TakeDamage(float damage, Collision col)
	{
	}

	public void CalculateBodyStuff()
	{
		body.ResetCenterOfMass();
		body.SetDensity(Density);
		body.mass = body.mass;
	}

	public void GetNearest(Vector3 v, float force, int pieces)
	{
		List<Transform> l = new List<Transform>();
		if (Forms.Count == 0)
		{
			return;
		}
		l.AddRange(Forms.OrderBy(x => (x.position - v).magnitude));
		int maxN = 0;
		maxN += Mathf.Max(Forms.FindIndex(x => x == l[0]), maxN);

		ParticleMan.Emit(4, Random.Range(0,(int)(force/10f)), v, Vector3.up);


		for (int i = 0; i < pieces && i < l.Count; i++)
		{
			if (l[i].GetComponent<Rigidbody>() != null)
				continue;
			l[i].parent = null;
			Forms.RemoveAt(0);
			RootSegment rs = l[i].gameObject.AddComponent<RootSegment>();
			rs.BreakForce = BreakForce;
			rs.StartVelocity = (l[i].position - v).normalized * Mathf.Clamp(force, 0f, 1f);
		}

		Forms = new List<Transform>();
		Forms.AddRange(GetComponentsInChildren<Transform>());
		CalculateBodyStuff();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.CompareTag("Bullet") || col.impulse.magnitude > BreakForce)
		{
			if (col.contacts[0].thisCollider.gameObject.name != name)
			{
				GetNearest(col.contacts[0].point, col.impulse.magnitude, col.transform.CompareTag("Bullet") ? 8 : 1);
				ParticleMan.Emit(3, 5, col.contacts[0].point, Vector3.up);
				AudioMan.PlaySound(SoundName.StoneCrunch, col.contacts[0].point);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
