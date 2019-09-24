using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Vector3 v = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
		transform.LookAt(transform.position + v, Vector3.up);
	}
#endif

	private void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		//Gizmos.DrawWireSphere(Vector3.zero, .5f);
		Vector3 size = new Vector3(4f, 1f, 2f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(Vector3.up * size.y * .5f, size);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.up * size.y * .5f, size * .85f);
	}
}
