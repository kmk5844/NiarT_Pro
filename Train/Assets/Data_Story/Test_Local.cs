using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Test_Local : ScriptableObject
{
	public List<Test_Local_Entity> Test; // Replace 'EntityType' to an actual type that is serializable.
}
