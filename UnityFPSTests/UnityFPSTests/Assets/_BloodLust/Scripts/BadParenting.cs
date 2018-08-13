using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BadParenting : MonoBehaviour
{
	public Transform[] Pieces;
	private List<Transform> _forms;

	public float BreakForce;
	public float Mass;
	//private List<Rigidbody> _bodies;

	// Use this for initialization
	void Start ()
	{
		_forms = new List<Transform>();
		_forms.AddRange(Pieces.OrderBy(x => x.transform.position.y));

		for (int i = _forms.Count - 1; i > 0; i--)
		{
			//_forms[i].GetComponent<MeshRenderer>().material.color = new Color(Random.Range(.5f, .8f), Random.Range(.5f, .8f), Random.Range(.5f, .8f));
			_forms[i].parent = _forms[i - 1];
		}

		_forms[0].parent = null;
		RootSegment rs = _forms[0].gameObject.AddComponent<RootSegment>();
		rs.BreakForce = BreakForce;
		rs.Mass = Mass;
		_forms.RemoveAt(0);
		rs.Forms = _forms;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
