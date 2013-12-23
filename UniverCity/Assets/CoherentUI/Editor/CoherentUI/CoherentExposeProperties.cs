using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
 
public static class CoherentExposeProperties
{
	public static void Expose( CoherentPropertyField[] properties )
	{
		GUILayoutOption[] emptyOptions = new GUILayoutOption[0];
		EditorGUILayout.BeginVertical( emptyOptions );
		foreach ( CoherentPropertyField field in properties )
		{
 
			EditorGUILayout.BeginHorizontal( emptyOptions );
 
			switch ( field.Type )
			{
			case SerializedPropertyType.Integer:
					field.SetValue( EditorGUILayout.IntField( field.Name, (int)field.GetValue(), emptyOptions ) ); 
				break;
			 
			case SerializedPropertyType.Float:
					field.SetValue( EditorGUILayout.FloatField( field.Name, (float)field.GetValue(), emptyOptions ) );
				break;
 
			case SerializedPropertyType.Boolean:
					field.SetValue( EditorGUILayout.Toggle( field.Name, (bool)field.GetValue(), emptyOptions ) );
				break;
 
			case SerializedPropertyType.String:
					field.SetValue( EditorGUILayout.TextField( field.Name, (String)field.GetValue(), emptyOptions ) );
				break;
 
			case SerializedPropertyType.Vector2:
					field.SetValue( EditorGUILayout.Vector2Field( field.Name, (Vector2)field.GetValue(), emptyOptions ) );
				break;
 
			case SerializedPropertyType.Vector3:
					field.SetValue( EditorGUILayout.Vector3Field( field.Name, (Vector3)field.GetValue(), emptyOptions ) );
				break;
  
			case SerializedPropertyType.Enum:
						field.SetValue(EditorGUILayout.EnumPopup(field.Name, (Enum)field.GetValue(), emptyOptions));
				break;
 
			default:
 
				break;
 
			}
 
			EditorGUILayout.EndHorizontal();
 
		}
		EditorGUILayout.EndVertical();
	}
	
	public static CoherentPropertyField[] GetProperties( System.Object obj )
	{
		List<CoherentPropertyField> fields = new List<CoherentPropertyField>();
 
		PropertyInfo[] infos = obj.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );
 
		foreach ( PropertyInfo info in infos )
		{
 
			if ( ! (info.CanRead && info.CanWrite) )
				continue;
 
			object[] attributes = info.GetCustomAttributes( true );
 
			bool isExposed = false;
 
			foreach( object o in attributes )
			{
				var t = o.GetType();
				if ( t == typeof( CoherentExposePropertyAttribute )
					#if UNITY_STANDALONE
					|| t == typeof(CoherentExposePropertyStandaloneAttribute)
					#elif UNITY_IPHONE || UNITY_ANDROID
					|| t == typeof(CoherentExposePropertyiOSAttribute)
					#endif
					)
				{
					isExposed = true;
					break;
				}
			}
 
			if ( !isExposed )
				continue;
 
			SerializedPropertyType type = SerializedPropertyType.Integer;
 
			if( CoherentPropertyField.GetPropertyType( info, out type ) )
			{
				CoherentPropertyField field = new CoherentPropertyField( obj, info, type );
				fields.Add( field );
			}
 
		}
 
		return fields.ToArray();
 
	}
 
}
 
public class CoherentPropertyField
{
	System.Object m_Instance;
	PropertyInfo m_Info;
	SerializedPropertyType m_Type;
 
	MethodInfo m_Getter;
	MethodInfo m_Setter;
 
	public SerializedPropertyType Type
	{
		get
		{
			return m_Type;	
		}
	}
 
	public String Name
	{	
		get
		{
			return ObjectNames.NicifyVariableName( m_Info.Name );	
		}
	}
 
	public CoherentPropertyField( System.Object instance, PropertyInfo info, SerializedPropertyType type )
	{	
		m_Instance = instance;
		m_Info = info;
		m_Type = type;
 
		m_Getter = m_Info.GetGetMethod();
		m_Setter = m_Info.GetSetMethod();
	}
 
	public System.Object GetValue() 
	{
		return m_Getter.Invoke( m_Instance, null );
	}
 
	public void SetValue( System.Object value )
	{
		if (!Equal(value))
		{
			Undo.RegisterUndo((UnityEngine.Object)m_Instance, this.Name);
			m_Setter.Invoke( m_Instance, new System.Object[] { value } );
		}
	}
 
	public static bool GetPropertyType( PropertyInfo info, out SerializedPropertyType propertyType )
	{
		propertyType = SerializedPropertyType.Generic;
 
		Type type = info.PropertyType;
 
		if ( type == typeof( int ) )
		{
			propertyType = SerializedPropertyType.Integer;
			return true;
		}
 
		if ( type == typeof( float ) )
		{
			propertyType = SerializedPropertyType.Float;
			return true;
		}
 
		if ( type == typeof( bool ) )
		{
			propertyType = SerializedPropertyType.Boolean;
			return true;
		}
 
		if ( type == typeof( string ) )
		{
			propertyType = SerializedPropertyType.String;
			return true;
		}	
 
		if ( type == typeof( Vector2 ) )
		{
			propertyType = SerializedPropertyType.Vector2;
			return true;
		}
 
		if ( type == typeof( Vector3 ) )
		{
			propertyType = SerializedPropertyType.Vector3;
			return true;
		}
 
		if ( type.IsEnum )
		{
			propertyType = SerializedPropertyType.Enum;
			return true;
		}
 
		return false;
	}
	
	private bool Equal(System.Object other)
	{
		switch ( m_Type )
		{
		case SerializedPropertyType.Integer:
				return (int)GetValue() == (int)other; 
		 
		case SerializedPropertyType.Float:
				return (float)GetValue() == (float)other;

		case SerializedPropertyType.Boolean:
				return (bool)GetValue() == (bool)other;

		case SerializedPropertyType.String:
				return (string)GetValue() == (string)other;

		case SerializedPropertyType.Vector2:
				return (Vector2)GetValue() == (Vector2)other;

		case SerializedPropertyType.Vector3:
				return (Vector3)GetValue() == (Vector3)other;

		case SerializedPropertyType.Enum:
				return (Enum)GetValue() == (Enum)other;

		default:

			break;

		}
		return false;
	}
}