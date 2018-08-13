using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BadParentingTree : MonoBehaviour
{
	public Transform[] Pieces;
	[Header("0 is the parent to the rest")]
	public BreakABranch[] Branches;

	private List<Transform> _forms;

	public float BreakForce;
	public float Mass;
	//private List<Rigidbody> _bodies;

	// Use this for initialization
	void Start()
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
		foreach (BreakABranch branch in Branches)
		{
			AttachBranch(branch);
		}
	}

	void AttachBranch(BreakABranch branch)
	{
		branch.Pieces[0].parent = branch.Root;
		for (int i = 0; i < branch.Pieces.Length; i++)
		{
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}

[Serializable]
public struct BreakABranch
{
	public Transform Root;
	public Transform[] Pieces;
}