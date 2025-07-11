using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Game_DataTable: ScriptableObject
{
	public List<Info_Player> Information_Player;
	public List<Info_Train> Information_Train; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Train_Turret_Part> Information_Train_Turret_Part; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Monster> Information_Monster;
	public List<Info_Boss> Information_Boss;
	public List<Info_Stage> Information_Stage; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Mercenary> Information_Mercenary; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Item> Information_Item; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Item_Test> Information_Item2;
    public List<Info_FoodCard> Information_FoodCard;
}