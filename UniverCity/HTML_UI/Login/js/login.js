function SignInClicked() {
	var values = $(":input").serializeArray();
	if ( (values[0]["value"] === "") || (values[1]["value"] === "")) {
		classie.remove(document.getElementById('error'), 'error-hidden');
	}
	else {
		classie.add(document.getElementById('error'), 'error-hidden');
		document.getElementById("main").innerHTML = "<img src=\"images/logo.png\" /><br /><span>Welcome back <strong><em>Jacob Foster</em></strong></span>";
	}

    engine.call('CheckLoginInformation', values[0]["value"], values[1]["value"]).then(function() {
        console.log("Calling 'CheckLoginInformation' in Unity3D!");
    });
}

function GoToVirtualMall() {
    console.log("Calling 'GoToVirtualMall' in Unity3D");
    engine.call('GoToVirtualMall');
}

function GoToUnionHall() {
    engine.call('GoToUnionHall');
}

function GoToMemoryBank() {
    engine.call('GoToMemoryBank');
}

function GoToExplorer() {
    engine.call('GoToExplorer');
}

function GoToArcade() {
    engine.call('GoToArcade');
}