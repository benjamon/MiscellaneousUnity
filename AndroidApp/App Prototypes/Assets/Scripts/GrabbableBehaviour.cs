using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehaviour : MonoBehaviour
{
	[HideInInspector] public int FingerId = -1;
	internal Collider Col;
	internal Vector3 StartPosition;
	internal float StartDistance;
	internal Camera MainCam;
	internal Transform Cam;

	// Use this for initialization
	void Start ()
	{
		MainCam = Camera.main;
		Cam = MainCam.transform;
		StartPosition = transform.position;
		StartDistance = (transform.position - Cam.position).magnitude;
		Col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (FingerId == -1)
		{
#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit rch = new RaycastHit();
				if (Col.Raycast(MainCam.ScreenPointToRay(Input.mousePosition), out rch, 10))
				{
					FingerId = 0;
					ItemGrabbed();
					return;
				}
			}
#endif
			for (int t = 0; t < Input.touchCount; t++)
			{
				Vector3 touchPosition = Input.GetTouch(t).position;
				if (Input.GetTouch(t).phase == TouchPhase.Began)
				{
					RaycastHit rch = new RaycastHit();
					if (Col.Raycast(MainCam.ScreenPointToRay(touchPosition), out rch, 10))
					{
						FingerId = t;
						ItemGrabbed();
						return;
					}
				}
			}
			transform.position = Vector3.Lerp(transform.position, StartPosition, .1f);
		}
		else
		{
			GrabUpdate();
		}
	}

	public virtual void ItemGrabbed()
	{
	}

	public virtual void GrabUpdate()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			transform.position = Cam.position + MainCam.ScreenPointToRay(Input.mousePosition).direction * StartDistance;
		}

		if (Input.GetMouseButtonUp(0))
		{
			FingerId = -1;
		}
#else
		Touch t = GetFingerTouch();
		if (t.phase <= TouchPhase.Stationary)
		{
			transform.position = Cam.position + MainCam.ScreenPointToRay(t.position).direction * StartDistance;
		}
		else
		{
			FingerId = -1;
		}
#endif
	}

	public Touch GetFingerTouch()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch t = Input.GetTouch(i);
			if (t.fingerId != FingerId)
				continue;
			return t;
		}
		Debug.Log("Problemo");
		return new Touch();
	}
}
