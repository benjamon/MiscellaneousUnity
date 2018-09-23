using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IActor {
	public Transform GPointA, GPointB;
	public GameObject BulletPrefab;
	public float FireDelay;

	float _shootTime;
	Vector3 _targetPosition;
	Vector3 _startPosition;
	Vector3 _velocity;
	Camera _camera;

	void OnEnable()
	{
		DirectorGuy.RegisterSelf(this);
		Commander.RegisterActor(this, 0);
		Commander.RegisterActor(this, 1);
		Commander.RegisterActor(this, 2);
		_camera = Camera.main;
		_startPosition = transform.position;
	}

	void OnDisable()
	{
		DirectorGuy.UnregisterSelf(this);
		Commander.UnregisterActor(this, 0);
		Commander.UnregisterActor(this, 1);
		Commander.UnregisterActor(this, 2);
	}

	void FixedUpdate()
	{
		ProcessUpdate(1f);
	}
	
	public void ProcessUpdate(float m)
	{
		if (!Commander.ReplayMode)
		{
			if (_velocity.magnitude > .25)
			{
				Commander.IssueCommand(new MoveCommand
				{
					TargetId = 0,
					MoveX = -_velocity.x,
					MoveY = -_velocity.z
				});
			} else if (_velocity.magnitude > 0f)
			{
				Commander.IssueCommand(new MoveCommand
				{
					TargetId = 0,
					MoveX = -_velocity.x*30f,
					MoveY = -_velocity.z*30f
				});
			}
		}
		transform.position += _velocity * m / 60f;
	}

	public void ProcessCommand(BaseCommand command)
	{
		switch(command.TargetId)
		{
			case 0:
				MoveCommand move = (MoveCommand)command;
				//transform.position += (transform.forward * move.MoveY + transform.right * move.MoveX) / 60f;
				_velocity += (Vector3.forward * move.MoveY + Vector3.right * move.MoveX) / 30f;
				LookAtTarget();
				break;
			case 1:
				PositionCommand pos = (PositionCommand)command;
				SetTarget(pos.Position);
				LookAtTarget();
				break;
			case 2:
				TryFireGun();
				break;
		}
	}
	public void UndoCommand(BaseCommand command)
	{
		switch (command.TargetId)
		{
			case 0:
				MoveCommand move = (MoveCommand)command;
				//transform.position += (transform.forward * move.MoveY + transform.right * move.MoveX) / 60f;
				_velocity -= (Vector3.forward * move.MoveY + Vector3.right * move.MoveX) / 30f;
				LookAtTarget();
				break;
			case 1:
				PositionCommand pos = (PositionCommand)command;
				SetTarget(pos.Position);
				LookAtTarget();
				break;
		}
	}

	void TryFireGun()
	{
		if (Time.time - _shootTime > FireDelay)
		{
			GameObject bullet = Instantiate(BulletPrefab);
			bullet.transform.position = GPointA.position;
			bullet.GetComponent<BulletScript>().Velocity = GPointA.forward * 12f;
			bullet = Instantiate(BulletPrefab);
			bullet.transform.position = GPointB.position;
			bullet.GetComponent<BulletScript>().Velocity = GPointB.forward * 12f;

			_shootTime = Time.time;
		}
	}

	void LookAtTarget()
	{
		transform.rotation = Quaternion.identity;
		transform.LookAt(_targetPosition);
	}

	void SetTarget(Vector3 pos)
	{
		Plane p = new Plane(transform.up, transform.position);
		Ray r = _camera.ScreenPointToRay(pos);
		float d;
		if (p.Raycast(r, out d))
			_targetPosition = r.origin + r.direction * d;
	}

	public void ResetActor()
	{
		transform.position = _startPosition;
		transform.rotation = Quaternion.identity;
		_velocity = Vector3.zero;
	}
}
