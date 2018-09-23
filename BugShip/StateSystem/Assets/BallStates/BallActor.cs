using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallActor : MonoBehaviour {
	public BallState CurrentState;

	[SerializeField]
	ParticleSystem P;

	MeshRenderer _mesh;
	Material _mat;

	// Use this for initialization
	void OnEnable () {
		_mesh = GetComponent<MeshRenderer>();
		_mat = _mesh.material;

		CurrentState = new GrowState
		{
			GrowRate = .1f
		};

		CurrentState.Enter(this);
	}
	
	public void EmitParticle(int num)
	{
		P.Emit(num);
	}

	// Update is called once per frame
	void Update () {
		EnterState(CurrentState.Update());
	}

	void EnterState(BallState NewState)
	{
		if (NewState != null)
		{
			CurrentState.Exit();
			NewState.Enter(this);
			CurrentState = NewState;
		}
	}

	public void SetColor(Color c)
	{
		_mat.color = c;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		EnterState(CurrentState.Collide(collision));
	}

	public void OnCollisionStay2D(Collision2D collision)
	{
		EnterState(CurrentState.Collide(collision));
	}
}
