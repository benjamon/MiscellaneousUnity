using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointBuilder : MonoBehaviour
{
	public Transform[] Pieces;
	private List<Transform> _forms;
	private List<Rigidbody> _bodies;
	public float BreakForce = 1000;

	// Use this for initialization
	void Awake ()
	{
		List<Transform> _forms = new List<Transform>();
		_forms.AddRange(Pieces.OrderByDescending(x=>x.transform.position.y));
		_bodies = new List<Rigidbody>();
		for (int i = 0; i < _forms.Count; i++)
		{
			_bodies.Add(_forms[i].GetComponent<Rigidbody>());
		}

		for (int i = 0; i < _forms.Count - 1; i++)
		{
			_forms[i].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(.5f,.8f), Random.Range(.5f, .8f), Random.Range(.5f, .8f));
			_bodies.Add(_forms[i].GetComponent<Rigidbody>());
			FixedJoint fj = _forms[i].gameObject.AddComponent<FixedJoint>();
			fj.connectedBody = _forms[i + 1].GetComponent<Rigidbody>();
			fj.breakForce = BreakForce;
			fj.breakTorque = BreakForce;
			//fj.anchor = _forms[i].position;
			//fj.connectedAnchor = _forms[i + 1].position;
			_bodies[i].velocity = Vector3.zero;
		}
		_bodies.Add(_forms[_forms.Count-1].GetComponent<Rigidbody>());
		FixedJoint _fj = _forms[_forms.Count-1].gameObject.AddComponent<FixedJoint>();
		_fj.breakForce = 10000f;
		_fj.breakTorque = 10000f;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
