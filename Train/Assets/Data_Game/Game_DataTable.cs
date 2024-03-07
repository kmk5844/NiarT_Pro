using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Game_DataTable: ScriptableObject
{
	public List<Info_Train> Information_Train; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Stage> Information_Stage; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Mercenary> Information_Mercenary; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Item> Information_Item; // Replace 'EntityType' to an actual type that is serializable.
	//public List<Info_Level> Information_Level; // Replace 'EntityType' to an actual type that is serializable.
	//public List<Info_LevelCost> Information_LevelCost; // Replace 'EntityType' to an actual type that is serializable.
}