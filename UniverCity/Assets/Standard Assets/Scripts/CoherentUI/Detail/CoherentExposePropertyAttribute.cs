using UnityEngine;
using System;
using System.Collections;

[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyInfo : Attribute
{
	public enum FoldoutType
	{
		General = 0,
		Rendering,
		AdvancedRendering,
		Input,
		Scripting,
		
		Count
	};
		
	public CoherentExposePropertyInfo(FoldoutType category = FoldoutType.General)
	{
		this.Category = category;
	}
		
	public string PrettyName
	{ get; set; }
	
	public string Tooltip
	{ get; set; }
	
	public FoldoutType Category
	{ get; set;}
	
	public bool IsStatic
	{ get; set; }
}

[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyAttribute : CoherentExposePropertyInfo
{}

[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyStandaloneAttribute : CoherentExposePropertyInfo
{}

[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyiOSAttribute : CoherentExposePropertyInfo
{}