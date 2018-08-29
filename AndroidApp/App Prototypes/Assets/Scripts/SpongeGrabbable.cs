using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeGrabbable : GrabbableBehaviour
{
	public PaintableObject Dish;

	private Vector3 _prevPosition;
	private int _brushSize = 32;

	public override void GrabUpdate()
	{
#if UNITY_EDITOR
		base.GrabUpdate();
		Dish.CleanRay(_camera.ScreenPointToRay(Input.mousePosition), .96f, _brushSize);
		return;
#endif
		Touch t = Input.GetTouch(Grabber);
		if (t.phase == TouchPhase.Began)
			_prevPosition = t.position;
		Vector3 pos = t.position;
		float mag = (pos - _prevPosition).magnitude;
		for (int i = 0; i < mag - 1f; i += _brushSize / 4)
		{
			Dish.CleanRay(_camera.ScreenPointToRay(Vector3.Lerp(_prevPosition, t.position, i / mag)), .96f, _brushSize);
		}
		Dish.CleanRay(_camera.ScreenPointToRay(t.position), .96f, _brushSize);
		base.GrabUpdate();
		_prevPosition = t.position;
	}
}
