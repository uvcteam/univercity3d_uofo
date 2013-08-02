//This script can be deleted at any time. It is purely to test animations on the newly submitted
//monster character.

// Make the script also execute in edit mode.
@script ExecuteInEditMode()

function OnGUI () {
    if (GUI.Button (Rect (20,10,100,25), "walk anim"))
        animation.Play("walk");
        
	if (GUI.Button (Rect (120,10,100,25), "sprint anim"))
        animation.Play("sprint");
        
    if (GUI.Button (Rect (220,10,100,25), "jump anim"))
        animation.Play("jump");
        
    if (GUI.Button (Rect (320,10,100,25), "idle anim"))
        animation.Play("idle");
        
    if (GUI.Button (Rect (420,10,100,25), "crouch anim"))
        animation.Play("crouch");
        
    if (GUI.Button (Rect (520,10,100,25), "dive anim"))
        animation.Play("dive");
        
    if (GUI.Button (Rect (620,10,100,25), "catch anim"))
        animation.Play("catch");
        
    if (GUI.Button (Rect (720,10,100,25), "burrow anim"))
        animation.Play("burrow");
        
        
    if (GUI.Button (Rect (20,60,100,25), "capture anim"))
        animation.Play("capture");
        
	if (GUI.Button (Rect (120,60,100,25), "dance 01 anim"))
        animation.Play("dance01");
        
    if (GUI.Button (Rect (220,60,100,25), "dance 02 anim"))
        animation.Play("dance02");
        
    if (GUI.Button (Rect (320,60,100,25), "dance 03 anim"))
        animation.Play("dance03");
        
    if (GUI.Button (Rect (420,60,100,25), "freeze anim"))
        animation.Play("freeze");
        
    if (GUI.Button (Rect (520,60,100,25), "skateboard anim"))
        animation.Play("skateboard");
        
    if (GUI.Button (Rect (620,60,100,25), "speak anim"))
        animation.Play("speak");
        
    if (GUI.Button (Rect (720,60,100,25), "throw anim"))
        animation.Play("throw");
}