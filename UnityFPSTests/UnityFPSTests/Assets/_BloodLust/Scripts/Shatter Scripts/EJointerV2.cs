using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EJointerV2 : MonoBehaviour {
	public Transform[] Pieces;
	private List<Transform> _forms;
	private List<Rigidbody> _bodies;

	// Use this for initialization
	void Awake()
	{
		_forms = new List<Transform>();
		_forms.AddRange(Pieces.OrderByDescending(x => x.transform.position.y));
		_bodies = new List<Rigidbody>();

		for (int i = 0; i < _forms.Count - 1; i++)
		{
			_bodies.Add(_forms[i].GetComponent<Rigidbody>());
			_forms[i].GetComponent<EJoint>().masta = this;
		}
	}

	public void GetNearest(Vector3 v)
	{
		List<Transform> l = new List<Transform>();
		l.AddRange(Pieces.OrderBy(x=>(x.position - v).magnitude));
		int maxN = 0;
		maxN += Mathf.Max(_forms.FindIndex(x => x == l[0]), maxN);
		//for (int i = 0; i < 3 && i < l.Count; i++)
		//{

		//}

		for (int i = 0; i < maxN; i++)
		{
			Rigidbody r = _forms[0].GetComponent<Rigidbody>();
			r.isKinematic = false;
			r.velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
			_forms[0].tag = "Untagged";
			Destroy(_forms[0].GetComponent<EJoint>());
			_forms.RemoveAt(0);
		}
	}
}
