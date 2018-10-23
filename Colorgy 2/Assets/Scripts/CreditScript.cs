using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

	private CreditsManager creditManger;
	public Animator animator;
	public Text text1;

	private bool isTiming;
	private float timer;

	void Update(){
		if(isTiming){
			timer -= Time.deltaTime;
			if(timer < 0){
				isTiming = false;
				//animator.SetTrigger("fadeOut");
				Destroy(gameObject,2.0f);
				creditManger.CreateCreditOb();
			}
		}
	}

	public void SetUp(CreditsManager.Credit credit,CreditsManager cm){

		text1.text = credit.GetTopText();
	
		creditManger = cm;
		isTiming = true;
		timer = 6.0f;
	}
}
