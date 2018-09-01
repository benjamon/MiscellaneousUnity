using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.ParticleSystem;

public class GunControl : MonoBehaviour
{
	public Transform Reticle;
	public GameObject BulletPrefab;
	public Transform FunPoint;
	public float BulletSpeed;
	public GameObject GunFlash;
	public AnimationCurve Curve;

	public Vector3 recoil;

	private Vector3 _restPosition;


	// Use this for initialization
	void OnEnable ()
	{
		Reticle.parent = null;
		_restPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, _restPosition, .25f);
		transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, .12f);
	}

	void LateUpdate()
	{
		RaycastHit rch = new RaycastHit();

		if (Physics.Raycast(FunPoint.position, FunPoint.forward, out rch))
		{
			Reticle.gameObject.SetActive(true);
			Reticle.position = rch.point;
		}
		else
		{
			Reticle.position = FunPoint.position + FunPoint.forward * 15f;
			Reticle.gameObject.SetActive(false);
		}
	}

	public void PullTrigger()
	{
		GameObject bullet = Instantiate(BulletPrefab);
		AudioMan.PlaySound(SoundName.GunFired);
		bullet.transform.position = FunPoint.position;
		bullet.GetComponent<Rigidbody>().velocity = FunPoint.forward * BulletSpeed;
		transform.localPosition -= Vector3.forward * recoil.z;
		transform.Rotate(Vector3.right, recoil.x);
		MakeParticleLine();
		StartCoroutine(FlashGun());
	}

	public void MakeParticleLine()
	{

		int n = 60;

		ParticleSystem system = ParticleMan.Instance.Emitters[1].Emitters[0];

		system.Emit(n);
		Particle[] ps = new Particle[system.particleCount];
		system.GetParticles(ps);
		//Color32 a = new Color32(150, 120, 0, 45);
		//Color32 b = new Color32(130, 120, 120, 120);
		for (int i = ps.Length - n; i < ps.Length; i++)
		{
			int j = (i - ps.Length + n);
			float f = ((float)j) / n;
			float amp = Curve.Evaluate(f);

			Vector3 v = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))*6f;


			ps[i].position = Vector3.Lerp(FunPoint.position, Reticle.position + v * amp, f);
			//ps[i].startColor = Color32.Lerp(b, a, f);
			ps[i].startSize = f * 12f - 11f;
			if (f < .1f)
				ps[i].velocity = f * (FunPoint.forward + 
					FunPoint.up * Random.Range(-.4f, .4f) + FunPoint.right * Random.Range(-.4f, .4f));
		}
		ParticleMan.Instance.Emitters[1].Emitters[0].SetParticles(ps, ps.Length);
	}

	public IEnumerator FlashGun()
	{
		GunFlash.SetActive(true);
		yield return new WaitForSeconds(.025f);
		GunFlash.SetActive(false);
	}
}
