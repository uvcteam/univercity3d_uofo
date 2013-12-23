#pragma strict
	
var _timeToShowProgress = 0.0f;
var _progressVisible = false;

// Use this for initialization
function Start() {
}

// Update is called once per frame
function Update() {
	if (_progressVisible) {
		_timeToShowProgress -= Time.deltaTime;
		
		if (_timeToShowProgress < 0) {
			NativeDialogs.Instance.HideProgressDialog();
			_progressVisible = false;
		}
	}
}

function OnGUI() {
	var width = Screen.width/2;
	var height = Screen.width / 8;
	var drawRect = new Rect((Screen.width - width)/2, height, width, height);
	var cancellable = false;
	
	if (GUI.Button(drawRect, "Alert dialog") ) {
		NativeDialogs.Instance.ShowMessageBox("Dialog title", "Localized string йцукен", ["Button1", "Button2", "Button3"], cancellable, function(button) {
			var message = "Pressed button is " + button;
			NativeDialogs.Instance.ShowMessageBox("Alert dialog", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Login/password") ) {
		NativeDialogs.Instance.ShowLoginPasswordMessageBox("Authorization", "", ["Ok", "Cancel"], cancellable, function(login, password, button) {
			var message = String.Format("Login:{0}\nPassword:{1}\nButton pressed:{2}", login, password, button);
			NativeDialogs.Instance.ShowMessageBox("Login/password", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Filled login/password") ) {
		NativeDialogs.Instance.ShowLoginPasswordMessageBox("Authorization", "", "test login", "test password", ["Ok", "Cancel"], cancellable, function(login, password, button) {
			var message = String.Format("Login:{0}\nPassword:{1}\nButton pressed:{2}", login, password, button);
			NativeDialogs.Instance.ShowMessageBox("Login/password", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Prompt") ) {
		NativeDialogs.Instance.ShowPromptMessageBox("Prompt", "", ["Ok", "Cancel"], cancellable, function(prompt, button) {
			var message = String.Format("Value:{0}\nButton pressed:{1}", prompt, button);
			NativeDialogs.Instance.ShowMessageBox("Prompt", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Filled prompt") ) {
		NativeDialogs.Instance.ShowPromptMessageBox("Prompt", "", "test prompt", ["Ok", "Cancel"], cancellable, function(prompt, button) {
			var message = String.Format("Value:{0}\nButton pressed:{1}", prompt, button);
			NativeDialogs.Instance.ShowMessageBox("Prompt", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Secure prompt") ) {
		NativeDialogs.Instance.ShowSecurePromptMessageBox("Secure prompt", "", ["Ok", "Cancel"], cancellable, function(prompt, button) {
			var message = String.Format("Value:{0}\nButton pressed:{1}", prompt, button);
			NativeDialogs.Instance.ShowMessageBox("Secure prompt", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Filled secure prompt") ) {
		NativeDialogs.Instance.ShowSecurePromptMessageBox("Secure prompt", "", "test secure prompt", ["Ok", "Cancel"], cancellable, function(prompt, button) {
			var message = String.Format("Value:{0}\nButton pressed:{1}", prompt, button);
			NativeDialogs.Instance.ShowMessageBox("Secure prompt", message, ["Ok"], cancellable, function(b) {});
		});
	}
	
	drawRect.y += height;
	if (GUI.Button(drawRect, "Progress dialog")) {
		NativeDialogs.Instance.ShowProgressDialog("Progress title", "Progress message", cancellable, false);
		_timeToShowProgress = 3;
		_progressVisible = true;
	}
	
#if UNITY_IPHONE
	drawRect.y += height;
	if (GUI.Button(drawRect, "MBProgress dialog")) {
		NativeDialogs.Instance.ShowProgressDialog("Progress title", "Progress message", cancellable, true);
		_timeToShowProgress = 3;
		_progressVisible = true;
	}
#endif
}