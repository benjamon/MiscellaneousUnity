using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, IActor {
	public Vector3 Velocity;
	float maxAge = 1f;
	float Age = 0f;
	Vector3 startPosition;

	void OnEnable()
	{
		DirectorGuy.RegisterSelf(this);
		startPosition = transform.position;
	}

	void OnDisable()
	{
		DirectorGuy.UnregisterSelf(this);
	}

	private void FixedUpdate()
	{
		ProcessUpdate(1f);
	}

	public void ProcessCommand(BaseCommand command)
	{
		throw new System.NotImplementedException();
	}

	public void UndoCommand(BaseCommand command)
	{
		throw new System.NotImplementedException();
	}

	public void ProcessUpdate(float m)
	{
		transform.position += m * (Velocity / 60f);
		Age += m * (1/60f);
		if (Age > maxAge || Age < 0)
			Destroy(gameObject);
	}

	public void ResetActor()
	{
		transform.position = startPosition;
	}
}
