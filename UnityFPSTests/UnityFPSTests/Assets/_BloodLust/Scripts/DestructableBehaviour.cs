using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBehaviour : MonoBehaviour
{
	public GameObject Replacement;
	public float health;

	// Use this for initialization
	void Start ()
	{
		gameObject.tag = "Destructable";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void TakeDamage(float damage, Collision col)
	{
		health -= damage;
		if (health < 0)
			Die();
	}

	public virtual void Die()
	{
		GameObject g = Instantiate(Replacement);
		g.transform.position = transform.position;
		g.transform.rotation = transform.rotation;
		g.transform.localScale = transform.localScale;
		gameObject.SetActive(false);
	}
}
