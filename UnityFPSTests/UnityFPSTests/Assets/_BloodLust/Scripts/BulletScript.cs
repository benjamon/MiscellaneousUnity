using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BulletScript : MonoBehaviour
{
	public float deathAfterContact = 1f;
	public Light p;
	private Vector3 _oldVelocity;
	private Rigidbody _rb;
	private TrailRenderer _lr;

	// Use this for initialization
	IEnumerator Start ()
	{
		_rb = GetComponent<Rigidbody>();
		_lr = GetComponent<TrailRenderer>();
		float f = Time.time;
		while (Time.time - f < 5f)
		{
			_lr.widthMultiplier = _lr.widthMultiplier * .95f;
			p.range *= .97f;
			yield return null;
		}
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		_oldVelocity = _rb.velocity;
	}

	void OnCollisionEnter(Collision col)
	{
		ParticleMan.Emit(0, Random.Range(2,6), transform.position, col.contacts[0].normal);
		ParticleMan.Emit(2, Random.Range(1,4), transform.position, (GetComponent<Rigidbody>().velocity.normalized*.5f + col.contacts[0].normal));
		if (col.transform.CompareTag("Ground"))
		{
			StartCoroutine(Die());
		}
		if (col.transform.CompareTag("Destructable"))
		{
			_rb.velocity = _oldVelocity * .9f;
			col.gameObject.GetComponent<DestructableBehaviour>().TakeDamage(col.relativeVelocity.magnitude, col);
			StartCoroutine(Die());
		}
	}

	IEnumerator Die()
	{
		yield return new WaitForSeconds(Random.Range(.15f, .3f));
		Destroy(gameObject);
	}
}
