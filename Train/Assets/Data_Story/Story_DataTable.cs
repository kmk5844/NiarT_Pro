using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Story_DataTable : ScriptableObject
{
	public List<Story_Branch_Entity> Story_Branch;
	public List<Story_Entity> Story; // Replace 'EntityType' to an actual type that is serializable.
}
