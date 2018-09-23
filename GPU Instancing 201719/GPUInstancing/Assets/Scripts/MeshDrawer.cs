using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshDrawer : MonoBehaviour {
	public Mesh[] meshes;
	public Material mat;
	public float rd = 5f;
	public float scale = .1f;
	List<Matrix4x4[]> _matrixArray;
	MaterialPropertyBlock mpb;
	Transform t;
	Transform _cam;

// Use this for initialization
void OnEnable () {
		_matrixArray = new List<Matrix4x4[]>();
		mpb = new MaterialPropertyBlock();
		t = new GameObject().transform;
		t.parent = transform;
		_cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		int n = 0;
		Vector3 cf = _cam.position + _cam.forward * 2f;
		foreach(Matrix4x4[] m in _matrixArray)
		{
			n++;
			float t = Time.time * 5f + n;
			Vector3 v = (Vector3.right * Mathf.Sin(t) + Vector3.up * Mathf.Cos(t)) * .01f;// + Vector3.up * Random.Range(-.2f, .2f) + Vector3.right * Random.Range(-.2f, .2f);
			for (int i = 0; i < m.Length; i++)
			{
				Vector4 v4 = new Vector4(m[i][0, 3], m[i][1, 3], m[i][2, 3], m[i][3, 3]);
				Vector3 v3 = v4;
				Vector4 f = v * Mathf.Clamp(2f - (v3 - cf).magnitude, 0, 1f);
				m[i].SetColumn(3, v4 + f);
			}
			Graphics.DrawMeshInstanced(meshes[Mathf.FloorToInt((Time.time * 3f) % meshes.Length)], 0, mat, m, 1023, mpb, UnityEngine.Rendering.ShadowCastingMode.Off, false);
		}
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Add 10230"))
		{
			Transform t = new GameObject().transform;
			t.parent = transform;
			for (int j = 0; j < 10; j++)
			{
				Matrix4x4[] arr = new Matrix4x4[1023];
				for (int i = 0; i < 1023; i++)
				{
					t.localPosition = new Vector3(Random.Range(-rd, rd), Random.Range(-rd, rd), Random.Range(-rd, rd));
					t.localScale = Vector3.one * Random.Range(0, rd*scale);
					t.Rotate(Vector3.up, Random.Range(0f, 360f));
					t.Rotate(Vector3.right, Random.Range(0f, 360f));
					arr[i] = t.localToWorldMatrix;
				}
				_matrixArray.Add(arr);
			}
			DestroyImmediate(t.gameObject);
		}
		GUILayout.Label($"Count: {_matrixArray.Count * 1023}");
	}

	async void UpdateShit()
	{

	}
}
