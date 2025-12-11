using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Quest_DataTable : ScriptableObject
{
	public List<Info_Q_List> Q_List; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Q_Information> Q_Destination; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Q_Information> Q_Material; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Q_Information> Q_Monster; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Q_Information> Q_Escort; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Q_Information> Q_Convoy; // Replace 'EntityType' to an actual type that is serializable.
	public List<Info_Q_Information_Boss> Q_Boss; // Replace 'EntityType' to an actual type that is serializable.
}