NatiiveDialogs is iOS/Android plugin that can display native Alert dialogs:
- standart alert dialog with caption, message and buttons
- login/password dialog
- prompt dialog, so you can get some data from user
- secure prompt dialog, user input is masked by asterisk character
- progress dialog with spinning progress indicator

To start working with native dialogs, put the NativeDialogs prefab to the scene. Then you can access plugin functionality
using NativeDialogs.Instance object.

There are several methods:

/**
 * Show alert dialog.
 * caption - alert title
 * message - alert message
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * string parameter of onClickAction will be set to text of button clicked
 */
public void ShowMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string> onClickAction)

/**
 * Show login/password dialog.
 * caption - dialog title
 * message - dialog message
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * first string parameter of onClickAction will be set to login entered
 * second string parameter of onClickAction will be set to password entered
 * third string parameter of onClickAction will be set to text of button clicked
 */
public void ShowLoginPasswordMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string, string, string> onClickAction)

/**
 * Show login/password dialog.
 * caption - dialog title
 * message - dialog message
 * login - string to put into login field
 * password - string to put into password field
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * first string parameter of onClickAction will be set to login entered
 * second string parameter of onClickAction will be set to password entered
 * third string parameter of onClickAction will be set to text of button clicked
 */
public void ShowLoginPasswordMessageBox(string caption, string message, string login, string password, string[] buttons, bool cancellable, Action<string, string, string> onClickAction)

/**
 * Show prompt dialog.
 * caption - dialog title
 * message - dialog message
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * first string parameter of onClickAction will be set to data entered
 * second string parameter of onClickAction will be set to text of button clicked
 */
public void ShowPromptMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string, string> onClickAction)

/**
 * Show prompt dialog.
 * caption - dialog title
 * message - dialog message
 * prompt - string to put into text field
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * first string parameter of onClickAction will be set to data entered
 * second string parameter of onClickAction will be set to text of button clicked
 */
public void ShowPromptMessageBox(string caption, string message, string prompt, string[] buttons, bool cancellable, Action<string, string> onClickAction)

/**
 * Show secure prompt dialog. User input is masked by asterisk character
 * caption - dialog title
 * message - dialog message	 
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * first string parameter of onClickAction will be set to data entered
 * second string parameter of onClickAction will be set to text of button clicked
 */
public void ShowSecurePromptMessageBox(string caption, string message, string[] buttons, bool cancellable, Action<string, string> onClickAction)

/**
 * Show secure prompt dialog. User input is masked by asterisk character
 * caption - dialog title
 * message - dialog message
 * prompt - string to put into text field
 * buttons - list of buttons
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * 
 * first string parameter of onClickAction will be set to data entered
 * second string parameter of onClickAction will be set to text of button clicked
 */
public void ShowSecurePromptMessageBox(string caption, string message, string prompt, string[] buttons, bool cancellable, Action<string, string> onClickAction)

/**
 * Show window with spinning progress indicator. It will dismiss previously shown progress window if needed.
 * caption - window title
 * message - window message
 * cancellable - Sets whether this dialog could be canceled on tap outside the dialog or back button. (ignored for iOS)
 * mbProgress - (iOS only) set to true if you want to use MBProgressHUD (https://github.com/jdg/MBProgressHUD)
 * instead of standard blue progress indicator.
 * On iOS 7 MBProgressHUD is only progress indicator, so this parameter is ignored for iOS 7
 */
public void ShowProgressDialog(string caption, string message, bool cancellable, bool mbProgress)

/**
 * Hide window with spinning progress indicator
 */
public void HideProgressDialog()


You can fing example of usage of these methods in TestSceneScript.cs file.