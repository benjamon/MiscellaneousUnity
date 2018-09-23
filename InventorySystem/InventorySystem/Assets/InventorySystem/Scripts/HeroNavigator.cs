using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroNavigator : MonoBehaviour
{
	public NavMeshAgent Agent;
	public Inventory HeroInventory;

	Transform _targetItem;
	Camera _cam;

	// Use this for initialization
	void OnEnable()
	{
		_cam = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit rc = new RaycastHit();
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(r, out rc))
			{
				Agent.SetDestination(rc.point);
				if (rc.transform.CompareTag("Containable"))
				{
					_targetItem = rc.transform;
				}
			}
		}
		
		if (_targetItem != null)
		{
			Debug.Log("TARGETTING");
			if ((transform.position - _targetItem.position).magnitude < 2f)
			{
				HeroInventory.ContainItem(_targetItem.GetComponent<ItemReference>().ItemData);
				_targetItem = null;
			}
		}
	}
}
