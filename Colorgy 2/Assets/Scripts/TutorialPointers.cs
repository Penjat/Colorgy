using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPointers : MonoBehaviour {

	private static string TAG = "TUTORIAL POINTER: ";
	private ToolManager toolManager;
	private GridManager gridManager;
	public Animator animator;

	private int state = 0;

	void Start(){
		Debug.Log(TAG + "starting.");
		GameObject managers = GameObject.Find("Managers");
		toolManager = managers.GetComponent<ToolManager>();
		gridManager = managers.GetComponent<GridManager>();
		animator.speed = 0.0f;

		toolManager.SetPointers(this);
		FindPos();

	}
	public void Show(){
			animator.speed = 0.5f;

	}

	public void FindPos(){
		Debug.Log(TAG + "finding pos.");
		if(state == 0){
			ButtonTool[] tools = toolManager.GetTools();
			transform.position = tools[0].transform.position;
			return;
		}

		Hex centerHex = gridManager.GetHex(4,4);
		transform.position = centerHex.transform.position + new Vector3(0.0f,1.8f,0.0f);

	}
	public void End(){
		Debug.Log(TAG + "ending");
		toolManager.SetPointers(null);
		Destroy(gameObject);

	}
	public void SetState(int i){
		state = i;
	}
}
