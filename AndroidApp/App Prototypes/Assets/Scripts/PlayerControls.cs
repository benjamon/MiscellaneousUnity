using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	public Transform XRoter, YRoter;
	public Vector2 Constraints;
	private Transform _cam;
	private Vector3 _lasPosition;
	private Vector2 _rotVel;

	// Use this for initialization
	void Start ()
	{
		_cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update()
	{
		for (int t = 0; t < Input.touchCount; t++)
		{

			Touch tuch = Input.GetTouch(t);
			Vector3 touchPosition = tuch.position;
			if (touchPosition.x / (float) Screen.width < Constraints.x &&
			    touchPosition.y / (float) Screen.height < Constraints.y)
			{
				if (tuch.phase == TouchPhase.Moved)
				{
					_rotVel.x = -tuch.deltaPosition.x * .2f;
					_rotVel.y = -tuch.deltaPosition.y * .2f;
				}
			}
		}

#if UNITY_EDITOR
		if (Input.mousePosition.x / (float) Screen.width < Constraints.x &&
		    Input.mousePosition.y / (float) Screen.height < Constraints.y)
		{
			if (Input.GetMouseButtonDown(1))
			{
				_lasPosition = Input.mousePosition;
			}
			if (Input.GetMouseButton(1))
			{
				Vector3 delta = Input.mousePosition - _lasPosition;
				_rotVel.x = -delta.x * .2f;
				_rotVel.y = -delta.y * .2f;
				_lasPosition = Input.mousePosition;
			}
		}
#endif


		XRoter.Rotate(Vector3.up, _rotVel.x);
		YRoter.Rotate(Vector3.right, _rotVel.y);
		_rotVel *= .94f;
	}
}
