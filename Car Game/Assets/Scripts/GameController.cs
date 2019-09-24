using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static GameController Instance;
	public static ScreenManager Manager;
	public int playerCount = 2;
	public GameObject Canvas;
	public string[] CarSelection;

	private IEnumerator Start()
	{
		Manager = new ScreenManager(this);
		Instance = this;
		ScreenManager.PlayerCount = playerCount;
		yield return new WaitForEndOfFrame();
		ScreenManager.GenerateProfiles();
	}

	public void SpawnCar(CameraController c)
	{
		GameObject g = Instantiate(Resources.Load($"CarPrefabs/{CarSelection[0]}") as GameObject);
		g.transform.position = c.transform.position;
		g.transform.right = c.transform.forward;
		c.SetFollowTarget(g.transform);
		g.GetComponent<CarController>().joystick = c.joystick;
	}
}