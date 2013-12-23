using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CoherentUIView))]
public class CoherentViewEditor : Editor {
	
	private CoherentUIView m_Target;
	private CoherentPropertyField[] m_Fields;
	
	public void OnEnable() {
		m_Target = target as CoherentUIView;
		m_Fields = CoherentExposeProperties.GetProperties(m_Target);
	}
	
	public override void OnInspectorGUI() {
		if(m_Target == null)
			return;
		this.DrawDefaultInspector();
		CoherentExposeProperties.Expose(m_Fields);
	}
}
