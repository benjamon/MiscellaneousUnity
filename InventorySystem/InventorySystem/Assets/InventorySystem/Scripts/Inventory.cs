using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventorySlot[] ItemSlots;

	// Use this for initialization
	void OnEnable()
	{
		for (int i = 0; i < ItemSlots.Length; i++)
		{
			ItemSlots[i].Hide();
		}
	}

	public bool ContainItem(BaseItem item)
	{
		for (int i = 0; i < ItemSlots.Length; i++)
		{
			if (ItemSlots[i].Item == null)
			{
				Debug.Log("STORED BABY");
				ItemSlots[i].StoreItem(item);
				return true;
			}
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
