using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform FollowTarget;
	public Vector3 FollowPosition;
	public Vector3 LookPosition;
	public float LerpSpeed;
	public float MinVelocity;

	private Rigidbody body;
	private Vector3 lastUsableVelocity = Vector3.right;
	
	public void Awake()
	{
		if (FollowTarget != null)
			body = FollowTarget.GetComponent<Rigidbody>();
	}

	public void SetFollowTarget(Transform t)
	{
		FollowTarget = t;
		body = FollowTarget.GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (body != null)
		{
			Quaternion q = transform.rotation;
			transform.rotation = Quaternion.identity;
			transform.LookAt(TranslateOffset(LookPosition));
			Quaternion q2 = transform.rotation;
			transform.rotation = Quaternion.Lerp(q, q2, LerpSpeed);
			transform.position = Vector3.Lerp(transform.position, TranslateOffset(FollowPosition), LerpSpeed);
		}
	}

	Vector3 TranslateOffset(Vector3 offset)
	{
		Vector3 p = FollowTarget.position;
		if (body.velocity.magnitude > MinVelocity)
			lastUsableVelocity = Vector3.Lerp(lastUsableVelocity, body.velocity, .15f);

		Vector3 bodyrght = Vector3.ProjectOnPlane(lastUsableVelocity, Vector3.up).normalized;
		Vector3 bodyfwd = Vector3.ProjectOnPlane(Vector3.Cross(lastUsableVelocity, Vector3.up), Vector3.up).normalized;

		return p + bodyfwd * offset.z + bodyrght * offset.x + Vector3.up * offset.y;
	}
}
