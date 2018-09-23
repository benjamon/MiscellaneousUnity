using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudsMaster : MonoBehaviour
{
	public Transform[] Suds;
	public float SudSpread = .05f;
	public float HydrateTime = 1f;
	public bool IsDying = false;
	private Transform _cam;

	IEnumerator Start()
	{
		Vector3[] localScales = new Vector3[Suds.Length];
		for (int i = 0; i < Suds.Length; i++)
		{
			localScales[i] = Suds[i].localScale;
		}
		float _startTime = Time.time;
		while (Time.time - _startTime < HydrateTime)
		{
			for (int i = 0; i < Suds.Length; i++)
			{
				Suds[i].localScale = Vector3.Lerp(Vector3.zero, localScales[i], (Time.time-_startTime) / HydrateTime);
			}
			yield return new WaitForEndOfFrame();
		}
		for (int i = 0; i < Suds.Length; i++)
		{
			Suds[i].localScale = localScales[i];
		}
	}

	public static void TryKillSuds(Ray r)
	{
		LayerMask mask = LayerMask.GetMask("Rinseable");
		RaycastHit rch = new RaycastHit();
		if (Physics.Raycast(r, out rch, 10f))
		{
			SudsMaster sm = rch.transform.GetComponent<SudsMaster>();
			if (sm != null)
			{
				if (!sm.IsDying)
				{
					sm.Die();
				}
			}
		}
	}

	public void Die()
	{
		GetComponent<Collider>().enabled = false;
		StartCoroutine(BeRinsed());
		IsDying = true;
	}

	IEnumerator BeRinsed()
	{
		float time = Time.time;
		while (Time.time - time < 1f)
		{
			transform.localScale *= .95f;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		Destroy(gameObject);
	}

	// Use this for initialization
	void OnEnableVoid ()
	{
		_cam = Camera.main.transform;
	}

	public void PlaceSuds(Vector3 normal)
	{
		for (int i = 0; i < Suds.Length; i++)
		{
			Suds[i].position = transform.position +
			                   (Vector3.ProjectOnPlane(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), normal) +
			                    normal * Random.Range(0f, 1f)) * SudSpread;
		}
	}

	public 
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.hasChanged)
		{
			for (int i = 0; i < Suds.Length; i++)
			{
				Suds[i].rotation = Quaternion.identity;
				Suds[i].LookAt(_cam);
			}
		}
	}
}
