using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Level_DataTable : ScriptableObject
{
	public List<Info_Level> Information_Level; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_LevelCost> Information_LevelCost; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Level_Train_EngineTier> Level_Max_EngineTier;
	public List<Info_Level_Mercenary_Engine_Driver> Level_Mercenary_Engine_Driver; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Level_Mercenary_Engineer> Level_Mercenary_Engineer; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Level_Mercenary_Long_Ranged> Level_Mercenary_Long_Ranged; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Level_Mercenary_Short_ranged> Level_Mercenary_Short_Ranged; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Level_Mercenary_Medic> Level_Mercenary_Medic; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Level_Mercenary_Bard> Level_Mercenary_Bard;
}