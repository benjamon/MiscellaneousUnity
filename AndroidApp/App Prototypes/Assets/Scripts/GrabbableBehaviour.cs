using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehaviour : MonoBehaviour
{
	[HideInInspector] public int Grabber = -1;
	internal Collider _col;
	internal Vector3 _startPosition;
	internal float _startDistance;
	internal Camera _camera;
	internal Transform _cam;

	// Use this for initialization
	void Start ()
	{
		_camera = Camera.main;
		_cam = _camera.transform;
		_startPosition = transform.position;
		_startDistance = (transform.position - _cam.position).magnitude;
		_col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Grabber == -1)
		{
#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit rch = new RaycastHit();
				if (_col.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out rch, 10))
				{
					Grabber = 0;
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
					if (_col.Raycast(_camera.ScreenPointToRay(touchPosition), out rch, 10))
					{
						Grabber = t;
						return;
					}
				}
			}
			transform.position = Vector3.Lerp(transform.position, _startPosition, .1f);
		}
		else
		{
			GrabUpdate();
		}
	}

	public virtual void GrabUpdate()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			transform.position = _cam.position + _camera.ScreenPointToRay(Input.mousePosition).direction * _startDistance;
		}

		if (Input.GetMouseButtonUp(0))
		{
			Grabber = -1;
		}
#else
		Touch t = Input.GetTouch(Grabber);
		if (t.phase <= TouchPhase.Stationary)
		{
			transform.position = _cam.position + _camera.ScreenPointToRay(t.position).direction * _startDistance;
		}
		else
		{
			Grabber = -1;
		}
#endif
	}
}
