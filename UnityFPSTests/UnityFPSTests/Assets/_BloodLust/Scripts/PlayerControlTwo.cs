using System.Collections;
using UnityEngine;

public class PlayerControlTwo : MonoBehaviour
{
	public Vector2 LookSensitivity;
	public float Acceleration = 20f;
	public float AirCelleration = 20f;
	public float JumpSpeed = 20f;
	public float UpwardBias = 3f;
	public float MaxSpeed = 10f;
	public float GroundedFriction = .99f;
	public float GravityDelay;
	public float GravityPower;
	public AnimationCurve BoostIntensity, FovIntensityCurve;
	public float BoostSpeed, BoostDuration;

	private Rigidbody _rb;
	[SerializeField] private Camera _camera;
	private Transform _cam;
	private float _lastGrounded;
	private bool _isGrounded, _boosting, _canBoost;

	private Vector3 _bestFooting;
	private Vector3 _surfNormal;
	private Vector3 _boostAmount;
	private float _lookY;

	// Use this for initialization
	void OnEnable ()
	{
		_rb = GetComponent<Rigidbody>();
		_cam = _camera.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_boosting)
		{
			ProcessGravity();
			UpdateMovement();
			UpdateMouseLook();
		}
	}

	void OnDrawGizmos()
	{
		if (_rb == null)
			return;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position - Vector3.up, transform.position - Vector3.up + _bestFooting * .25f);

		Vector2 inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		float dt = Time.deltaTime;
		Vector3 oldVelocity = _rb.velocity;

		Vector3 transferredVector = (_isGrounded) ? (Vector3.ProjectOnPlane(transform.forward * inputMove.y, _bestFooting) +
		                                             Vector3.ProjectOnPlane(transform.right * inputMove.x, _bestFooting)) :
			(transform.forward * inputMove.y + transform.right * inputMove.x);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position - Vector3.up, transform.position - Vector3.up + transferredVector.normalized * 3f);
	}

	IEnumerator Boost(Vector3 direction)
	{
		_boosting = true;
		_canBoost = false;
		float start = Time.time;
		float fov = _camera.fieldOfView;
		float normalizeDuration = (Time.time - start) / BoostDuration;
		while (normalizeDuration < 1)
		{
			ParticleMan.Emit(3,1,_cam.position + _cam.forward, Vector3.up);
			_rb.velocity = direction.normalized * BoostIntensity.Evaluate(normalizeDuration) * BoostSpeed;
			_camera.fieldOfView = fov - fov * FovIntensityCurve.Evaluate(normalizeDuration);
			normalizeDuration = (Time.time - start) / BoostDuration;
			yield return null;
		}
		Vector3 oldVel = _rb.velocity;
		oldVel.y = 2;
		_rb.velocity = oldVel;
		_boosting = false;
	}

	void UpdateMovement()
	{
		float dt = Time.deltaTime;

		Vector2 inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		Vector3 oldVelocity = _rb.velocity;

		Vector3 transferredVector = (_isGrounded) ? (Vector3.ProjectOnPlane(transform.forward * inputMove.y, _surfNormal) + 
			Vector3.ProjectOnPlane(transform.right * inputMove.x, _surfNormal)) : 
			(transform.forward * inputMove.y + transform.right * inputMove.x);

		if (Input.GetMouseButtonDown(1) && _canBoost)
		{
			StartCoroutine(Boost(_cam.forward));
			return;
		}


		if (_isGrounded)
		{
			float fricitonDot = Vector3.Dot(transferredVector.normalized, oldVelocity.normalized);
			oldVelocity.x *= Mathf.Max(GroundedFriction, fricitonDot);
			oldVelocity.z *= Mathf.Max(GroundedFriction, fricitonDot);
			oldVelocity -= _bestFooting.normalized * (1f-Mathf.Abs(fricitonDot)) * 10f;
			Vector3 xz = (new Vector3(oldVelocity.x, 0f, oldVelocity.z));
			if (xz.magnitude > MaxSpeed)
			{
				oldVelocity = xz.normalized * MaxSpeed + Vector3.up * oldVelocity.y;
			}
			_canBoost = true;
		}

		oldVelocity += transferredVector * dt * ((_isGrounded) ? Acceleration : AirCelleration);

		if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
		{
			oldVelocity = (_bestFooting + new Vector3(transferredVector.x, UpwardBias, transferredVector.y)  + _rb.velocity / MaxSpeed).normalized * JumpSpeed;
		}

		_rb.velocity = oldVelocity;
	}

	void UpdateMouseLook()
	{
		Cursor.lockState = CursorLockMode.Locked;
		float dt = Time.deltaTime;

		Vector2 mouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		transform.Rotate(Vector3.up, mouseMove.x * LookSensitivity.x * dt);

		_lookY = Mathf.Clamp(_lookY + -mouseMove.y * LookSensitivity.y * dt, -60f, 60f);
		_cam.localRotation = Quaternion.identity;
		_cam.Rotate(Vector3.right, _lookY);

	}

	void ProcessGravity()
	{

		_isGrounded = (Time.time - _lastGrounded) <= Time.deltaTime * 2f;
		_rb.velocity -= Vector3.up * Time.deltaTime * ((Time.time - _lastGrounded > GravityDelay) ? GravityPower : 5f);
	}

	void OnCollisionEnter(Collision col)
	{
		float prevBest = -1f;
		for (int i = 0; i < col.contacts.Length; i++)
		{
			float currentNormal = Vector3.Dot(col.contacts[i].normal, Vector3.up);
			if (currentNormal > prevBest && currentNormal > .9f)
			{
				prevBest = currentNormal;
				_bestFooting = col.contacts[i].normal;
			}
			if (currentNormal > .3f)
			{
				_lastGrounded = Time.time;
				if (!_isGrounded)
				{
					Vector3 ov = _rb.velocity;
					ov.x *= .3f;
					ov.z *= .3f;
					_rb.velocity = ov;
				}
			}
		}
	}

	void OnCollisionStay(Collision col)
	{
		float prevBest = -1f;
		for (int i = 0; i < col.contacts.Length; i++)
		{
			float currentNormal = Vector3.Dot(col.contacts[i].normal, Vector3.up);
			if (currentNormal < prevBest)
			{
				prevBest = currentNormal;
				_bestFooting = col.contacts[i].normal;
				_surfNormal = GetSurfaceNormal(col.contacts[i]);
			}
			if (currentNormal > .3f)
			{
				_lastGrounded = Time.time;
			}
		}
	}

	Vector3 GetSurfaceNormal(ContactPoint pt)
	{
		RaycastHit rch = new RaycastHit();
		Ray r = new Ray(pt.point + pt.normal * .01f, -pt.normal * .02f);
		if (Physics.Raycast(r, out rch))
		{
			return Vector3.Lerp(rch.normal, Vector3.up, .7f);
		}
		return Vector3.up;
	}
}
