using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager
{
	public static GameController GameCon;
	private static int _playerCount;
	public static int PlayerCount
	{
		set
		{
			_playerCount = value;
		}
		get
		{
			return _playerCount;
		}
	}

	static CameraProfile[] Profiles;

	static GameObject _cameraPrefab;
	static GameObject CameraPrefab
	{
		get
		{
			if (_cameraPrefab == null)
				_cameraPrefab = Resources.Load("PlayerCamera") as GameObject;
			return _cameraPrefab;
		}
	}

	static GameObject _uiImagePrefab;
	static GameObject UIImagePrefab
	{
		get
		{
			if (_uiImagePrefab == null)
				_uiImagePrefab = Resources.Load("RenderImage") as GameObject;
			return _uiImagePrefab;
		}
	}

	public ScreenManager(GameController gc)
	{
		GameCon = gc;
	}

	public static void GetTexture(int playerNumber)
	{
		if (playerNumber > _playerCount)
			throw new ScreenManagerException();

		if (Profiles == null)
			GenerateProfiles();
	}

	public static void GenerateProfiles()
	{
		if (Profiles != null)
			Debug.Log("Profiles already exist!");

		Profiles = new CameraProfile[PlayerCount];

		//RenderTexture rtAsset = Resources.Load("/RenderTextures/Screen1") as RenderTexture;
		for (int i = 0; i < Profiles.Length; i++)
		{
			GameObject g = GameObject.Instantiate(CameraPrefab);
			RawImage img = GameObject.Instantiate(UIImagePrefab, GameCon.Canvas.transform).GetComponent<RawImage>();
			RenderTexture rt = GetNewRenderTexture();
			
			CameraProfile p = new CameraProfile
			{
				Camera = g.GetComponent<Camera>(),
				Controller = g.GetComponent<CameraController>(),
				Texture = rt,
				ImageElement = img
			};
			
			p.Camera.targetTexture = p.Texture;
			img.texture = rt;
			p.Controller.joystick = i + 1;
			PlaceScreenRect(p);
		}
	}

	static void ApplyResolution(RenderTexture tex)
	{
		Resolution res = Screen.currentResolution;
		switch (_playerCount)
		{
			case 1:
				tex.width = res.width;
				tex.height = res.height;
				break;
			case 2:
				tex.width = res.width / 2;
				tex.height = res.height;
				break;
			default:
				tex.width = res.width / 2;
				tex.height = res.height / 2;
				break;
		}
	}
	static RenderTexture GetNewRenderTexture()
	{
		int x = Screen.currentResolution.width;
		int y = Screen.currentResolution.height;

#if UNITY_EDITOR
		Vector2 v = UnityEditor.Handles.GetMainGameViewSize();
		x = Mathf.FloorToInt(v.x);
		y = Mathf.FloorToInt(v.y);
#endif
		Debug.Log($"Screen width: {x} height: {y}");
		RenderTexture rt;
		switch (_playerCount)
		{
			case 1:
				rt = new RenderTexture(x, y, -1, RenderTextureFormat.ARGB32);
				break;
			case 2:
				rt = new RenderTexture(x / 2, y, -1, RenderTextureFormat.ARGB32);
				break;
			default:					 
				rt = new RenderTexture(x / 2, y / 2, -1, RenderTextureFormat.ARGB32);
				break;
		}
		rt.antiAliasing = 1;
		rt.filterMode = FilterMode.Point;
		return rt;
	}

	static void PlaceScreenRect(CameraProfile p)
	{
		RectTransform r = p.ImageElement.rectTransform;
		float hw = GameCon.Canvas.GetComponent<CanvasScaler>().referenceResolution.x / 2f;
		float hh = GameCon.Canvas.GetComponent<CanvasScaler>().referenceResolution.y / 2f;
		bool left = ((p.Controller.joystick - 1) % 2 == 0);
		bool top = (p.Controller.joystick < 3);
		switch (_playerCount)
		{
			case 1:
				r.offsetMin = Vector2.zero;
				r.offsetMax = Vector2.zero;
				break;
			case 2:
				r.offsetMin = new Vector2((left) ? 0f : (hw), 0f);
				r.offsetMax = new Vector2((left) ? (-hw) : 0f, 0f);
				break;
			default:
				r.offsetMin = new Vector2((left) ? 0f : (hw), (top) ? 0f : (hh));
				r.offsetMax = new Vector2((left) ? (-hw) : 0f, (top) ? (-hh) : 0f);
				break;
		}
	}
}

[Serializable]
public struct CameraProfile
{
	public CameraController Controller;
	public RenderTexture Texture;
	public Camera Camera;
	public RawImage ImageElement;
}