using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
	public static PaintableObject Instance;
	public MeshRenderer Dish;
	public GameObject Grime;
	private Texture2D _grimeMap;
	private Camera _camera;
	private Color[] _brushColors;
	private Vector3[] _touchHistory;
	private int _imageSize = 156, _brushSize = 14;
	private Collider _dishCollider;
	public TextMeshPro DebugText;

	void OnEnable()
	{
		Instance = this;
		_grimeMap = new Texture2D(_imageSize, _imageSize, TextureFormat.ARGB32, false);
		_grimeMap.name = "Grime Map";
		Color[] pixelColors = _grimeMap.GetPixels();
		_brushColors = new Color[_brushSize * _brushSize];
		_camera = Camera.main;
		_touchHistory = new Vector3[10];
		_dishCollider = GetComponent<Collider>();

		Debug.Log(pixelColors.Length);

		for (int y = 0; y < _imageSize; y++)
		{
			for (int x = 0; x < _imageSize; x++)
			{
				pixelColors[x % _imageSize + y * _imageSize
				] = Color.black;
			}
		}

		_grimeMap.SetPixels(pixelColors, 0);

		ApplyGrime();

		for (int i = 0; i < _brushColors.Length; i++)
		{
			_brushColors[i] = Color.black;
		}

		Dish.material.SetTexture("_GrimeMask", _grimeMap);
	}

	public void ApplyGrime()
	{
		Collider c = Dish.GetComponent<Collider>();
		for (int i = 0; i < 56; i++)
		{
			Vector3 v = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
			Ray r = new Ray(c.transform.position + v, -v + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * .12f);
			RaycastHit rch = new RaycastHit();
			if (c.Raycast(r, out rch, 3f))
			{
				DrawOnTexture(rch.textureCoord, .75f);
				//Transform t = Instantiate(Grime, rch.point, Quaternion.identity).transform;
				//t.forward = rch.normal;
				//t.parent = transform;
			}
		}
		_grimeMap.Apply();
	}

	public void ApplyUpdates()
	{
		_grimeMap.Apply();
	}

	public void CleanTexture(Vector2 pixPosition, float maskMult , int brushSize)
	{
		int x = Mathf.Max(Mathf.FloorToInt(_grimeMap.width * pixPosition.x) - brushSize / 2, 0);
		int y = Mathf.Max(Mathf.FloorToInt(_grimeMap.height * pixPosition.y) - brushSize / 2, 0);
		int endX = Mathf.Min(_imageSize - x, brushSize);
		int endY = Mathf.Min(_imageSize - y, brushSize);
		Color[] pixels =
			_grimeMap.GetPixels(x, y, endX, endY);
		for (int n = 0; n < pixels.Length; n++)
		{
			pixels[n] *= maskMult;
		}
		_grimeMap.SetPixels(x,y, endX, endY, pixels);
	}

	public void DrawOnTexture(Vector2 pixPosition, float maskMult)
	{
		int bsize = 48;
		int x = Mathf.Max(Mathf.FloorToInt(_grimeMap.width * pixPosition.x) - bsize / 2, 0);
		int y = Mathf.Max(Mathf.FloorToInt(_grimeMap.height * pixPosition.y) - bsize / 2, 0);
		int endX = Mathf.Min(_imageSize - x, bsize);
		int endY = Mathf.Min(_imageSize - y, bsize);
		Color[] pixels =
			_grimeMap.GetPixels(x, y, endX, endY);
		for (int n = 0; n < pixels.Length; n++)
		{
			float ix = n % bsize;
			float iy = n / bsize;
			pixels[n] = pixels[n] + maskMult * Color.white * (1f-Mathf.Abs((bsize / 2f - ix) / (bsize / 2f))) * (1f - Mathf.Abs((bsize / 2f - iy) / (bsize / 2f)));
		}
		_grimeMap.SetPixels(x, y, endX, endY, pixels);
	}

	public RaycastHit CleanRay(Ray r, float strength, int brushSize, int Bubbles = 0)
	{
		RaycastHit rch = new RaycastHit();
		if (_dishCollider.Raycast(r, out rch, 50))
		{
			if (rch.transform == transform)
			{
				TryBubl(rch.point, Bubbles);
				CleanTexture(rch.textureCoord, strength, brushSize);
			}
		}
		return rch;
	}

	void TouchSponges()
	{
		for (int t = 0; t < Input.touchCount; t++)
		{
			Vector3 touchPosition = Input.GetTouch(t).position;
			DebugText.text += $"\nTouch {t}: {touchPosition}";
			if (Input.GetTouch(t).phase == TouchPhase.Began)
			{
				_touchHistory[t] = touchPosition;
			}
			if (Input.GetTouch(t).phase <= TouchPhase.Moved)
			{
				float mag = (_touchHistory[t] - touchPosition).magnitude;
				for (int i = 0; i < mag - 1f; i += _brushSize / 4)
				{
					CleanRay(_camera.ScreenPointToRay(Vector3.Lerp(_touchHistory[t], touchPosition, i / mag)), .96f, _brushSize);
				}
				CleanRay(_camera.ScreenPointToRay(touchPosition), .96f, _brushSize);
				_touchHistory[t] = touchPosition;
			}
		}
	}

	void MouseSponge()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (Input.GetMouseButtonDown(0))
		{
			_touchHistory[0] = mousePosition;
		}
		if (Input.GetMouseButton(0))
		{
			RaycastHit rch = new RaycastHit();
			float mag = (_touchHistory[0] - mousePosition).magnitude;
			for (int i = 0; i < mag - 1f; i += _brushSize / 4)
			{
				CleanRay(_camera.ScreenPointToRay(Vector3.Lerp(_touchHistory[0], mousePosition, i / mag)), .96f, _brushSize);
			}
			CleanRay(_camera.ScreenPointToRay(mousePosition), .96f, _brushSize);
			_touchHistory[0] = mousePosition;
		}
		_grimeMap.Apply();

		DebugText.text += $"\n({Mathf.RoundToInt(CalculatePerecentage() * 100f)}%)";
	}

	public void TryBubl(Vector3 position, int Bubbles)
	{
		if (Bubbles > 0 && Random.Range(0f, 1f) < .2f)
			ParticleMan.Emit(0, Bubbles, position, Vector3.up);
	}

	float CalculatePerecentage()
	{
		float pixelAmt = 0f;
		Color[] pixelArray = _grimeMap.GetPixels();

		for (int i = 0; i < pixelArray.Length; i++)
		{
			pixelAmt += pixelArray[i].r;
		}

		return Mathf.Clamp(pixelAmt / (_imageSize * _imageSize), 0f, 1f);
	}
}
