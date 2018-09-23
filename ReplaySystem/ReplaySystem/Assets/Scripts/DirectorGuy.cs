using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorGuy : MonoBehaviour {
	static List<IActor> bigList;

	// Use this for initialization
	void OnEnable () {
		TrySetUp();
	}

	static void TrySetUp()
	{
		if (bigList == null)
			bigList = new List<IActor>();
	}

	public static void RegisterSelf(IActor mb)
	{
		TrySetUp();
		if (!bigList.Contains(mb))
		{
			bigList.Add(mb);
		}
	}
	public static void UnregisterSelf(IActor mb)
	{
		if (bigList.Contains(mb))
		{
			bigList.Remove(mb);
		}
	}

	public static void ForceUpdate(float m)
	{
		for (int i = 0; i < bigList.Count; i++)
		{
			bigList[i].ProcessUpdate(m);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
