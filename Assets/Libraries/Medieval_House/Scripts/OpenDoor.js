#pragma strict

var closeClip:AnimationClip ;
var doorObject:GameObject;
var openedDoor:boolean;
function Start () {
    openedDoor=true;
    // closeClip = Resources.Load("Libraries\\Medieval_House\\Animations\\Close_Anim", AnimationClip);
}

function Update () {

}

//function OnTriggerEnter(other:Collider){
//	if (openedDoor){
//		doorObject.GetComponent.<Animation>().Play();
//		openedDoor=false;
//		Debug.Log("open");
//	}
//}

function OnTriggerStay(other:Collider){
	if (other.transform.tag == "AI" && openedDoor){
		doorObject.GetComponent.<Animation>().Play();
		openedDoor=false;
		Debug.Log("open");
    }
}

function OnTriggerExit(other:Collider) {
	if (other.transform.tag == "AI")
	{
		doorObject.GetComponent.<Animation>().clip = closeClip;
		doorObject.GetComponent.<Animation>().Play();
	    // doorObject.GetComponent.<Animation>().animations[1].Play();
	    Debug.Log("close");
	}
}