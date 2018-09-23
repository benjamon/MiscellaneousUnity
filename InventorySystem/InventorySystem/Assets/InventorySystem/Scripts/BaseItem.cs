using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseItem {
	public GameObject GroundPrefab;
	public InventorySlot Slot;
	[Space(12)]
	public string Name;
	public Sprite Icon;

	public ItemState State = ItemState.Ground;
	GameObject GroundInstance;

	public virtual void SwapPlaces(BaseItem item)
	{
		if (item.State == ItemState.Inventory)
		{
			InventorySlot sloot = item.Slot;

		}
	}

	public void ApplyToSlot(InventorySlot slot)
	{
		slot.Item = this;
		slot.Icon.sprite = Icon;
		slot.Title.text = Name;
		slot.Show();
		State = ItemState.Inventory;
	}

	public void DropOnFloor(Vector3 position)
	{
		if (State != ItemState.Ground)
		{
			GroundInstance = GameObject.Instantiate(GroundPrefab, position, Quaternion.identity);
			State = ItemState.Ground;
			Slot.Item = null;
			Slot.Hide();
			Slot = null;
		}
	}
}

public enum ItemState
{
	Ground,
	Inventory,
	Equipped
}