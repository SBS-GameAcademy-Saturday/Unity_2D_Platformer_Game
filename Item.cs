using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = 0)]
public class Item : ScriptableObject
{
	public int ItemID;
	public int ItemType;
	public bool Comsumable;
}
