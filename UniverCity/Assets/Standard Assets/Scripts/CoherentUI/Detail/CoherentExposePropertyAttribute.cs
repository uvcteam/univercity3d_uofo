using UnityEngine;
using System;
using System.Collections;
 
[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyAttribute : Attribute
{}

[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyStandaloneAttribute : Attribute
{}

[AttributeUsage( AttributeTargets.Property )]
public class CoherentExposePropertyiOSAttribute : Attribute
{}