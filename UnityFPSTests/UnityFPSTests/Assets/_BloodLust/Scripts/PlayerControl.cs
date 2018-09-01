using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerControl : MonoBehaviour
{
	public PostProcessProfile Ppp;
	public Transform Head, Arm, ArmTarget, Feet;
	public GunControl Gc;
	public float MaxMovespeed = 10f, MoveForce = 100f, JumpForce = 1000f;
	[Range(0f, 1f)] public float GroundedFriction;
	public float LookSensitivity = 1f;
	private Vector2 _mouseMove, _viewAngle, _moveInput;
	private Vector3 _lookPoint, _armSway, _initArm;
	private Ray _lookRay;
	private RaycastHit _rayHit;
	private DepthOfField _dof;
	private Camera _cam;
	private Rigidbody _rig;
	private float cDof, initFov;
	private bool _grounded, _boosting;

	// Use this for initialization
	void OnEnable () {
		_rayHit = new RaycastHit();
		_lookRay = new Ray();
		_dof = Ppp.GetSetting<DepthOfField>();
		_rig = GetComponent<Rigidbody>();
		_cam = Camera.main;
		_initArm = Arm.localPosition;
		initFov = _cam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_rig.ResetCenterOfMass();
		ApplyMouseMovement();

		UpdateArmPosition();

		_grounded = Physics.Raycast(new Ray(Feet.position, -Feet.up), .4f);
		_rig.useGravity = !_grounded;

		if (_grounded && Input.GetButtonDown("Jump"))
		{
			_rig.AddForce(Vector3.up * JumpForce);
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Gc.PullTrigger();
			_cam.fieldOfView += 5f;
		}

		UpdateMovement();

		cDof = Mathf.Lerp(cDof, (Head.position - _lookPoint).magnitude, .15f);
		_dof.focusDistance.value = cDof;

		_cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, initFov, .1f);

		AimDownSites();
	}

	private void UpdateMovement()
	{

		_moveInput.x = Input.GetAxisRaw("Horizontal");
		_moveInput.y = Input.GetAxisRaw("Vertical");

		Vector3 movement = _moveInput.x * transform.right + _moveInput.y * transform.forward;

		if (movement.magnitude > .5f)
			_rig.AddForce(movement.normalized * MoveForce);
		else if (_grounded)
			_rig.velocity = (_rig.velocity.magnitude > .1f) ? GroundedFriction *_rig.velocity : Vector3.zero;
		//_rig.velocity = new Vector3(_rig.velocity.x * GroundedFriction, _rig.velocity.y, _rig.velocity.z * GroundedFriction);

		Vector2 moveVel = new Vector2(_rig.velocity.x, _rig.velocity.z);
		moveVel = moveVel.normalized * Mathf.Min(moveVel.magnitude, MaxMovespeed);

		_rig.velocity = new Vector3(moveVel.x, _rig.velocity.y, moveVel.y);
	}

	private void AimDownSites()
	{
		if (Input.GetMouseButton(1))
		{
			_cam.fieldOfView -= 2.5f;
			Arm.localPosition = Vector3.Lerp(Arm.localPosition, ArmTarget.localPosition, .16f);
			_dof.aperture.value = Mathf.Lerp(_dof.aperture.value, .6f, .17f);
		}
		else
		{
			Arm.localPosition = Vector3.Lerp(Arm.localPosition, _initArm, .1f);
			_dof.aperture.value = Mathf.Lerp(_dof.aperture.value, 24f, .12f);
		}
	}

	private void ApplyMouseMovement()
	{
		//Head Position Sutff
		_mouseMove.x = Input.GetAxisRaw("Mouse X");
		_mouseMove.y = Input.GetAxisRaw("Mouse Y");
		Cursor.lockState = CursorLockMode.Locked;
		_viewAngle += _mouseMove * LookSensitivity;
		transform.rotation = Quaternion.identity;
		_viewAngle.x = _viewAngle.x % 360f;	
		_viewAngle.y = Mathf.Clamp(_viewAngle.y, -60f, 60f);
		transform.Rotate(Vector3.up, _viewAngle.x);
		Head.localRotation = Quaternion.identity;
		Head.Rotate(Vector3.right, -_viewAngle.y);
	}

	private void UpdateArmPosition()
	{
		_lookPoint = Head.position + Head.forward * 10f;

		_lookRay.origin = Head.position;
		_lookRay.direction = Head.forward;

		if (Physics.Raycast(_lookRay, out _rayHit))
		{
			_lookPoint = _rayHit.point;
		}

		Quaternion _oldRot = Arm.localRotation;

		Arm.localRotation = Quaternion.identity;
		Arm.LookAt(_lookPoint);
		if (!_grounded)
		{
			Arm.Rotate(Vector3.right, 4f * Mathf.Sin(Time.time * 7f));
			Arm.Rotate(Vector3.up, 4f * Mathf.Cos(Time.time * 5f));
		}
		Arm.localRotation = Quaternion.Lerp(_oldRot, Arm.localRotation, .15f);
	}
}
