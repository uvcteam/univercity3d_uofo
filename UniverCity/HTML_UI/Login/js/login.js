function SignInClicked() {
	var values = $(":input").serializeArray();
	if ( (values[0]["value"] === "") || (values[1]["value"] === "")) {
		classie.remove(document.getElementById('error'), 'error-hidden');
	}
	else {
		classie.add(document.getElementById('error'), 'error-hidden');
		document.getElementById("main").innerHTML = "Welcome back <strong><em>Jacob Foster</em></strong>";
	}
}