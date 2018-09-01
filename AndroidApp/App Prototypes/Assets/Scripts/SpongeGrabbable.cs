using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeGrabbable : GrabbableBehaviour
{
	public PaintableObject Dish;

	private Vector3 _prevPosition;
	public int BrushSize = 32;
	public float BrushStrength = .96f;
	public int Bubbles = 1;

	public GameObject SudsPrefab;

	public override void ItemGrabbed()
	{
		base.ItemGrabbed();

#if UNITY_EDITOR
		_prevPosition = Input.mousePosition;
		return;
#endif

		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch t = Input.GetTouch(i);
			if (t.fingerId != FingerId)
				continue;
			if (t.phase == TouchPhase.Began)
				_prevPosition = t.position;
		}

	}

	public override void GrabUpdate()
	{
		base.GrabUpdate();
#if UNITY_EDITOR
		float mmag = (Input.mousePosition - _prevPosition).magnitude;
		for (int i = 0; i < mmag - 1f; i += BrushSize / 4)
		{
			Ray r = MainCam.ScreenPointToRay(Vector3.Lerp(_prevPosition, Input.mousePosition, i / mmag));
			RaycastHit rch = Dish.CleanRay(r, BrushStrength, BrushSize, Bubbles);
			if (rch.transform != null && UnityEngine.Random.Range(0f, 1f) < .01f)
			{
				GameObject g = Instantiate(SudsPrefab, rch.point, Quaternion.identity);
				g.transform.parent = rch.transform;
				g.GetComponent<SudsMaster>().PlaceSuds(rch.normal);
			}
		}
		_prevPosition = Input.mousePosition;
		Dish.ApplyUpdates();
		return;
#endif

		Touch t = GetFingerTouch();
		Vector3 pos = t.position;
		float mag = (pos - _prevPosition).magnitude;
		for (int i = 0; i < mag - 1f; i += BrushSize / 4)
		{
			Ray r = MainCam.ScreenPointToRay(Vector3.Lerp(_prevPosition, t.position, i / mag));
			Dish.CleanRay(r, .96f, BrushSize, Bubbles);
			RaycastHit rch = Dish.CleanRay(r, BrushStrength, BrushSize, Bubbles);
			if (rch.transform != null && UnityEngine.Random.Range(0f, 1f) < .01f)
			{
				GameObject g = Instantiate(SudsPrefab, rch.point, Quaternion.identity);
				g.transform.parent = rch.transform;
				g.GetComponent<SudsMaster>().PlaceSuds(rch.normal);
			}
		}
		Dish.ApplyUpdates();
		_prevPosition = t.position;
	}
}
