using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {
	private static string TAG = "MENU BUTTON: ";

	public int val;
	public Renderer rend;
	public Animator animator;

	public int Press(){
		Debug.Log("pressing");
		if(animator){
			animator.SetTrigger("press");
		}

		return val;
	}

	public void LightUp(bool b){

		Color c =  Color.gray;
		if(b){


			rend.material.SetColor("_Color", Color.white * 20.0f);
			return;
		}

		//c.a = 0;
		rend.material.SetColor("_Color", new Vector4(c.r,c.g,c.b,1.0f));
		//animator.SetBool("light",b);
	}

}
