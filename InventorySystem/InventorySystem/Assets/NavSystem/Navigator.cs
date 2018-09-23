using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour {
	public NavMeshAgent Agent;

	Camera _cam;

	// Use this for initialization
	void OnEnable () {
		_cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit rc = new RaycastHit();
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(r, out rc))
			{
				Agent.SetDestination(rc.point);
			}
		}
	}
}
