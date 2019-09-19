using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new car profile", menuName = "Create New Car Profile")]
public class CarParameters : ScriptableObject
{
	[Header("SUSPENSION")]
	public float WheelRadius;
	public float SuspensionDistance;
	public float SuspensionStrength;

	[Header("INPUT STRENGTH")]
	public float TurnSpeed;
	public float HorsePower;
	public float TireGripStrength;
	public float MaxTorque;
	public float BrakeStrength;
	public float MaxTurnAngle;

	[Header("DYNAMICS")]
	public float ShockAbsorb;
	public float InertiaAbsorb;
	public float InertiaReroute;
	public float SpinControl;

	[Header("IN AIR")]
	public float AirControl;
}
