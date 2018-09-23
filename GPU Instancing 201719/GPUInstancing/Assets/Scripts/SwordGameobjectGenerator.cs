using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SwordGameobjectGenerator : MonoBehaviour {
	public GameObject prefab;
	public float rd = 5f;
	private List<GameObject> allChildren;
	private int count;

	// Use this for initialization
	void OnEnable () {
		allChildren = new List<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGUI()
	{
		if (GUILayout.Button("add 10000 swords"))
		{
			for (int i = 0; i < 10000; i++)
			{
				GameObject g = Instantiate(prefab);
				g.transform.parent = transform;
				g.transform.localPosition = new Vector3(Random.Range(-rd, rd), Random.Range(-rd, rd), Random.Range(-rd, rd));
				g.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
				g.transform.Rotate(Vector3.right,Random.Range(0f, 360f));
				allChildren.Add(g);
			}
			count = allChildren.Count;
		}
		if (GUILayout.Button("destroy swords"))
		{
			foreach(GameObject g in allChildren)
			{
				DestroyImmediate(g);
			}
			allChildren = new List<GameObject>();
		}
		GUILayout.Label($"Mesh Count: {count}");
	}
}
