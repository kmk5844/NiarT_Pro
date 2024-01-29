using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Train_DataTable : ScriptableObject
{
	//public List<EntityType> Manual; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Train> Information_Train; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Stage> Information_Stage; // Replace 'EntityType' to an actual type that is serializable.
}