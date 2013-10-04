using UnityEngine;
using System.Collections;
using System;

public class TestSceneScript : MonoBehaviour {
	
	private string _selectedDateString1 = "N/A";
	private string _selectedDateString2 = "N/A";
	
	private string _selectedTimeString1 = "N/A";
	private string _selectedTimeString2 = "N/A";
	
	private string[] _itemList = new string[] { "item 1", "item 2", "item 3" };
	
	private string _selectedItemString1 = "N/A";
	private string _selectedItemString2 = "N/A";
	
	GUIStyle _labelStyle;
	
	// Use this for initialization
	void Start() {
		_labelStyle = new GUIStyle();
		_labelStyle.alignment = TextAnchor.MiddleCenter;
		_labelStyle.normal.textColor = Color.white;
		
		_selectedDateString1 = DateTime.Now.ToString("yyyy-MM-dd");
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	Rect toScreenRect(Rect rect) {
		Vector2 lt = new Vector2(rect.x, rect.y);
		Vector2 br = lt + new Vector2(rect.width, rect.height);
		
		lt = GUIUtility.GUIToScreenPoint(lt);
		br = GUIUtility.GUIToScreenPoint(br);
		
		return new Rect(lt.x, lt.y, br.x-lt.x, br.y-lt.y);
	}
	
	void OnGUI() {
		
		float width = Screen.width/2;
		float height = Screen.width / 10;
		Rect drawRect = new Rect((Screen.width - width)/2, height, width, height);
			
		// show date picker with current date selected
		if (GUI.Button(drawRect, "Date picker") ) {
			NativePicker.Instance.ShowDatePicker(toScreenRect(drawRect), (long val) => {
				_selectedDateString1 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
			});
		}
			
		if (NativePicker.iPhonePlatform) {
			// hide picker button
			// due to iPhone design guidelines picker should appear at bottom of the screen (like keyboard)
			// and should be hidden manually
			Rect closeButtonRect = new Rect(drawRect);
			closeButtonRect.x += closeButtonRect.width;
			closeButtonRect.width = Screen.width / 4;
			if (GUI.Button(closeButtonRect, "Hide picker")) {
				NativePicker.Instance.HidePicker();
			}
		}
		
		// display selected date		
		drawRect.y += height;
		GUI.Label(drawRect, "Selected date: " + _selectedDateString1, _labelStyle);
		
		
		// show date picker with custom date selected
		drawRect.y += height;
		if (GUI.Button(drawRect, "Date picker: 2012-12-23") ) {
			NativePicker.Instance.ShowDatePicker(toScreenRect(drawRect), NativePicker.DateTimeForDate(2012, 12, 23), (long val) => {
				_selectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
			});
		}
		
		// display selected date
		drawRect.y += height;
		GUI.Label(drawRect, "Selected date: " + _selectedDateString2, _labelStyle);
		
		
		// show time picker with current time selected
		drawRect.y += height;
		if (GUI.Button(drawRect, "Time picker") ) {
			NativePicker.Instance.ShowTimePicker(toScreenRect(drawRect), (long val) => {
				_selectedTimeString1 = NativePicker.ConvertToDateTime(val).ToString("H:mm");
			});
		}
		
		// display selected time
		drawRect.y += height;
		GUI.Label(drawRect, "Selected time: " + _selectedTimeString1, _labelStyle);
		
		
		// show time picker with custom time selected
		drawRect.y += height;
		if (GUI.Button(drawRect, "Time picker: 14:45") ) {
			NativePicker.Instance.ShowTimePicker(toScreenRect(drawRect), NativePicker.DateTimeForTime(14, 45, 0), (long val) => {
				_selectedTimeString2 = NativePicker.ConvertToDateTime(val).ToString("H:mm");
			});
		}
		
		// display selected time
		drawRect.y += height;
		GUI.Label(drawRect, "Selected time: " + _selectedTimeString2, _labelStyle);
		
		
		// show picker with custom item list
		drawRect.y += height;
		if (GUI.Button(drawRect, "Custom picker") ) {
			NativePicker.Instance.ShowCustomPicker(toScreenRect(drawRect), _itemList, (long val) => {				
				_selectedItemString1 = _itemList[val];
			});
		}
		
		// display selected item
		drawRect.y += height;
		GUI.Label(drawRect, "Selected item: " + _selectedItemString1, _labelStyle);
		
		
		// show show custom picker with selected item
		drawRect.y += height;
		if (GUI.Button(drawRect, "Custom picker: item 2") ) {
			NativePicker.Instance.ShowCustomPicker(toScreenRect(drawRect), _itemList, 1, (long val) => {
				_selectedItemString2 = _itemList[val];
			});
		}
		
		// display selected item
		drawRect.y += height;
		GUI.Label(drawRect, "Selected item: " + _selectedItemString2, _labelStyle);
	}
}

