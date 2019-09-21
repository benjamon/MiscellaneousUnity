using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public Wheel FrontLeft, FrontRight, BackLeft, BackRight;

	[Header("SUSPENSION")]
	public float WheelRadius;
	public float SuspensionDistance;
	public float SuspensionStrength;

	[Header("INPUT STRENGTH")]
	public float TurnSpeed;
	public float HorsePower;
	public float TireGripStrength;
	public float MaxTorque;
	public float TopSpinSpeed;
	public float BrakeStrength;
	public float MaxTurnAngle;

	[Header("DYNAMICS")]
	public float ShockAbsorb;
	public float InertiaAbsorb;
	public float InertiaReroute;
	public float SpinControl;
	
	[Header("IN AIR")]
	public float AirControl;

	public CarParameters CarParams;

    public ParticleSystem dirtSystem;
	public Rigidbody body;

	float wheelY;
	float wheelAngle;

	public TextMeshProUGUI DebugText;

	private void Awake()
	{
		wheelY = FrontLeft.Parent.localPosition.y;
	}

	bool CheckWheel(Wheel w, float angle, bool drive)
	{
		Transform t = w.Parent;
		Vector3 wheelP = t.localPosition;
		Vector3 oldPosition = t.position;
		Vector3 oldLocalPosition = t.localPosition;
		wheelP.y = wheelY;
		t.localPosition = wheelP;

		if (drive)
		{
			t.transform.localRotation = Quaternion.identity;
            t.Rotate(Vector3.up, angle);
			w.SpinDistance += Input.GetAxis("Vertical") * HorsePower * Time.deltaTime;
		}
		w.SpinDistance = Mathf.Clamp(w.SpinDistance, -TopSpinSpeed, TopSpinSpeed);

        if (Input.GetButton("Brake"))
        {
            w.SpinDistance = TimeScaledMultiply(w.SpinDistance, 1f / BrakeStrength);
			//body.AddForceAtPosition(forceOnWheel * -pointVelocity.normalized * body.mass * BrakeStrength, t.position);
		}

		//Converts wheel speed to spin
		w.Geo.Rotate(w.Parent.forward, Time.deltaTime * -(180f * w.SpinDistance) / (Mathf.PI * WheelRadius), Space.World);

		if (Physics.Raycast(t.position + t.up * WheelRadius, -t.up, out RaycastHit hit, WheelRadius * 2f + SuspensionDistance, LayerMask.NameToLayer("Road")))
		{
			Vector3 pointVelocity = body.GetPointVelocity(hit.point); //takes velocity at tire position....

			//UP
			Vector3 normalVelocity = pointVelocity;
			normalVelocity = Vector3.Project(normalVelocity, t.up) *
				(((normalVelocity - t.up).magnitude > (normalVelocity - t.up).magnitude) ? -1f : 1f);
			w.VertSpeed = normalVelocity.magnitude;

			//SIDE
			Vector3 speedAgainstTread = Vector3.Project(pointVelocity, w.Parent.forward);
			w.counterTread = speedAgainstTread;

            float counterSpeed = speedAgainstTread.magnitude;
            if (counterSpeed > 7.5f)
            {
                dirtSystem.transform.position = hit.point;
                dirtSystem.transform.forward = hit.normal;
                dirtSystem.Emit(4);
            }

			//FORWARD
			Vector3 speedWithTread = Vector3.Project(pointVelocity, w.Parent.right);
			float currentWheelSpeed = (speedWithTread.magnitude * Vector3.Dot(speedWithTread, w.Parent.right));

			float suspensionActivation = Mathf.Clamp01((WheelRadius * 2f + SuspensionDistance - hit.distance) / SuspensionDistance);
			w.SuspensionActivation = suspensionActivation;

			w.lastForceApplied = suspensionActivation * t.up * SuspensionStrength * body.mass - //upward suspension force
				normalVelocity * ShockAbsorb - //counteract vertical momentum
				speedAgainstTread * InertiaAbsorb + //counter skid momentum
				speedAgainstTread.magnitude * t.right * InertiaReroute + //re-add skid momentum lost to forward direction
				Mathf.Clamp(w.SpinDistance - currentWheelSpeed, -MaxTorque, MaxTorque) * TireGripStrength * w.Parent.right;
            
            w.SpinDistance = Mathf.Lerp(w.SpinDistance, currentWheelSpeed, .5f);

			//Put wheel on the ground
			t.position = Vector3.Lerp(oldPosition, t.position - transform.up * (hit.distance - WheelRadius * 2f), .2f);
            w.OnGround = true;

			return true;
		} else //if wheel not on ground
		{
			wheelP.y = wheelY + Mathf.Min(0,Vector3.Project(-transform.up * SuspensionDistance, Vector3.up).magnitude * ((transform.up.y > 0f) ? -1f : 1f));
			t.localPosition = Vector3.Lerp(oldLocalPosition, wheelP, .2f);
			w.SuspensionActivation = 0f;
            w.lastForceApplied = Vector3.zero;
            w.OnGround = false;
            return false;
		}
	}

    void ApplyWheelForce(Wheel w)
    {
        if (w.OnGround)
        {
            body.AddForceAtPosition(w.lastForceApplied * Time.deltaTime * 60f, w.Parent.position);
            body.angularVelocity = body.angularVelocity.normalized * TimeScaledMultiply(body.angularVelocity.magnitude,
                1f - Mathf.Clamp01(w.counterTread.magnitude / SpinControl));

            if (w.Drive)
            {
                Vector3 forwardVector = Vector3.Project(body.velocity, transform.right);
                float forwardVelocity = forwardVector.magnitude * (((forwardVector - transform.right).magnitude < (forwardVector + transform.right).magnitude) ? -1f : 1f);

                //Steering rotation of the wheel as a whole
                float rotAmount = -forwardVelocity * wheelAngle * Time.deltaTime * .3f;

                if (Mathf.Abs(wheelAngle) < 1f && Input.GetAxis("Horizontal") == 0f)
                    wheelAngle = 0f;
                else if (wheelAngle > 0f)
                    wheelAngle = Mathf.Max(0f, wheelAngle - Mathf.Abs(rotAmount));
                else if (wheelAngle < 0f)
                    wheelAngle = Mathf.Min(0f, wheelAngle + Mathf.Abs(rotAmount));
            }
        }
    }

	public void Update()
	{
		DebugText.text = "";
		bool fl = CheckWheel(FrontLeft, wheelAngle, FrontLeft.Drive);
		bool fr = CheckWheel(FrontRight, wheelAngle, FrontRight.Drive);
		bool bl = CheckWheel(BackLeft, -wheelAngle, BackLeft.Drive);
		bool br = CheckWheel(BackRight, -wheelAngle, BackRight.Drive);

        ApplyWheelForce(FrontLeft);
        ApplyWheelForce(FrontRight);
        ApplyWheelForce(BackLeft);
        ApplyWheelForce(BackRight);

        //Air Tricks
        if (!(fl || fr || bl || br))
		{
			if (Input.GetButton("Brake"))
			{
                Vector3 torque = 2f * Input.GetAxis("AimY") * transform.forward * AirControl +
                     -Input.GetAxis("AimX") * transform.right * AirControl;
                body.AddTorque(torque * 60f * Time.deltaTime);
				body.angularVelocity = body.angularVelocity.normalized * TimeScaledMultiply(body.angularVelocity.magnitude, .975f);
			}
		}
		
		wheelAngle = Mathf.Clamp(wheelAngle + TurnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), -MaxTurnAngle, MaxTurnAngle);

		//Reset the car
		if (Input.GetButtonDown("Cancel"))
		{
			transform.position = transform.position + Vector3.up;
			transform.rotation = Quaternion.identity;
			body.velocity *= 0f;
			body.angularVelocity *= 0f;
		}

		DebugText.text = Mathf.Round(body.velocity.magnitude * 2.2369356f) + "mph";
	}

    float TimeScaledMultiply(float f, float m)
    {
        float v = f * m;
        return f - (f - v) * Time.deltaTime * 60f;
    }

	private void OnDrawGizmos()
	{
		DrawWheelDebug(FrontLeft);
		DrawWheelDebug(BackLeft);
		DrawWheelDebug(FrontRight);
		DrawWheelDebug(BackRight);
	}

	void DrawWheelDebug(Wheel w)
	{
		Transform t = w.Parent;
		Vector3 p = t.position;
		Vector3 linep = t.position + transform.up;
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(p, WheelRadius);
		//Gizmos.color = Color.green;
		//Gizmos.DrawLine(linep, linep + transform.up * w.ForceOnGround);
		//linep += transform.forward * .1f;
		//Gizmos.color = Color.red;
		//Gizmos.DrawLine(linep, linep + transform.up * w.VertSpeed);
		linep += transform.right * .1f;
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(linep, linep + w.Parent.right * w.SpinDistance);
		linep += transform.right * .1f;
		Gizmos.color = Color.black;
		Gizmos.DrawLine(linep, linep + w.lastForceApplied * .1f);
	}
}

[Serializable]
public class Wheel
{
	public Transform Parent;
	public Transform Geo;
	public bool Drive;
    public float SpinDistance;
    public float SuspensionActivation;
    public float VertSpeed;
    public Vector3 lastForceApplied;
    public bool OnGround;
    public Vector3 counterTread;
}