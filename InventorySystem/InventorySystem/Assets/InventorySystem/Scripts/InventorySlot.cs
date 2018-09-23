using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	public Inventory Container;
	public BaseItem Item;
	public Text Title;
	public Image Icon;

	public void StoreItem(BaseItem newItem)
	{
		Debug.Log("ITEM");
		if (Item != null)
		{
			Item.SwapPlaces(newItem);
		} else
		{
			newItem.ApplyToSlot(this);
		}
	}

	public void Show()
	{
		Title.enabled = true;
		Icon.enabled = true;
	}

	public void Hide()
	{
		Title.enabled = false;
		Icon.enabled = false;
	}
}
