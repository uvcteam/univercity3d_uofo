// The target we are following
var target : Transform;

function LateUpdate () {
	// Early out if we don't have a target
	if (!target)
		return;
	
	transform.position = target.position;
	
	// Always look at the target
	transform.LookAt (target);
}