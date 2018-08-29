using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoodParent : MonoBehaviour
{
	public Transform[] Pieces;
	public BranchArray[] Branches;
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

		for (int i = 0; i < Branches.Length; i++)
		{
			AttachBranch(Branches[i]);
		}

		_forms[0].parent = null;
		RootSegment rs = _forms[0].gameObject.AddComponent<RootSegment>();
		rs.BreakForce = BreakForce;
		rs.Density = Mass;
		_forms.RemoveAt(0);
		rs.Forms = _forms;
	}

	void AttachBranch(BranchArray branch)
	{
		List<Transform> branchList = new List<Transform>();
		branchList.AddRange(branch.BranchPieces.OrderBy(x => x.transform.position.y));

		for (int i = branchList.Count - 1; i > 0; i--)
		{
			branchList[i].parent = branchList[i - 1];
		}

		branchList[0].parent = branch.BranchRoot;
	}

	// Update is called once per frame
	void Update()
	{

	}
}

[Serializable]
public struct BranchArray
{
	public Transform BranchRoot;
	public Transform[] BranchPieces;
}