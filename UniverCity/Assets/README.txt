NativePicker is iOS/Android plugin that can display native date/time/custom item list pickers

To start working with NativePicker, put NativePicker prefab to the scene. Then you can access plugin functionality
using NativePicker.Instance object.
If script links are broken after loading test project, attach Plugins\NativePicker\NativePicker.cs to NativePicker object, and TestScene\TestScene.cs to Main Camera object

There are several methods:

/**
 * Show date picker
 * position - popover pointing rect (required by iPad platform)	 
 * 
 * long parameter of onValueSelectedAction will be set to selected date in unix timestamp format. 
 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
 */
public void ShowDatePicker(Rect position, Action<long> onValueSelectedAction)


/**
 * Show date picker
 * position - popover pointing rect (required by iPad platform)
 * date - date to select in the picker	 
 * 
 * long parameter of onValueSelectedAction will be set to selected date in unix timestamp format. 
 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
 */
public void ShowDatePicker(Rect position, DateTime date, Action<long> onValueSelectedAction)


  /**
 * Show time picker
 * position - popover pointing rect (required by iPad platform)	 
 * 
 * long parameter of onValueSelectedAction will be set to selected time in unix timestamp format. 
 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
 */
public void ShowTimePicker(Rect position, Action<long> onValueSelectedAction)


/**
 * Show time picker
 * position - popover pointing rect (required by iPad platform)
 * time - time to select in the picker	 
 * 
 * long parameter of onValueSelectedAction will be set to selected time in unix timestamp format. 
 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
 */
public void ShowTimePicker(Rect position, DateTime time, Action<long> onValueSelectedAction)


/**
 * Show picker with custom item list
 * position - popover pointing rect (required by iPad platform)	 
 * items - custom item list
 * 
 * long parameter of onValueSelectedAction will be set to selected item index. 	 
 */
public void ShowCustomPicker(Rect position, string[] items, Action<long> onValueSelectedAction)


/**
 * Show picker with custom item list
 * position - popover pointing rect (required by iPad platform)	 
 * items - custom item list
 * selectedItem - item to select
 * 
 * long parameter of onValueSelectedAction will be set to selected item index. 	 
 */
public void ShowCustomPicker(Rect position, string[] items, int selectedItem, Action<long> onValueSelectedAction)


/**
 * Convert unix timestamp to DateTime object
 * val - unix timestamp value	 
 */
public static DateTime ConvertToDateTime(long val)


/**
 * Convert DateTime object to unix timestamp
 * dateTime - DateTime object to convert
 */
public static long ConvertToUnixTimestamp(DateTime dateTime)


/**
 * Create DateTime object from provided date	 
 */
public static DateTime DateTimeForDate(int year, int month, int day)


/**
 * Create DateTime object from provided time
 */
public static DateTime DateTimeForTime(int hour, int minute, int second)


/**
 * Hide picker, used for iPhone platform
 */
public void HidePicker()
  
  
According to Apple UI guidelines picker should be displayed at botton of the screen on iPhone device, and only inside popover controller on iPad device, that's why Show* functions has position parameter.
  
You can fing example of usage of these methods in TestSceneScript.cs file.