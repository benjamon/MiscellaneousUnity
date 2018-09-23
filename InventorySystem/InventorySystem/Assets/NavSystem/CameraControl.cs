using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public float CamSpeed;

	private void Update()
	{
		transform.position += (Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical")) * CamSpeed * Time.deltaTime;
	}
}
