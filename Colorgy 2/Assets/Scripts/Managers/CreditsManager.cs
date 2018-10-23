using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour {

	private MenuManager menuManager;
	private SoundFXManager soundManager;
	public GameObject creditPrefab;

	private Credit[] credits;
	private int curCredit = 0;
	private float timer = 74.0f;
	public GameObject particles;
	private bool isTiming = true;


	void Update(){
		particles.transform.Rotate(new Vector3(0,0,0.045f));
		if(isTiming){
			
			timer -= Time.deltaTime;
			if(timer <= 0){
				isTiming = false;
				ParticleSystem p = particles.GetComponent<ParticleSystem>();
				p.Stop();
			}
		}

	}
	public void StopCredits(){
		Destroy(gameObject,10.0f);



	}


	public void LoadCredits(MenuManager mm,SoundFXManager sm){
		menuManager = mm;
		soundManager = sm;


		credits = new Credit[12];

		credits[0] = new Credit("thank you for playing",5.0f);
		credits[1] = new Credit("Colorgy",5.0f);
		credits[2] = new Credit("game developed by",5.0f);
		credits[3] = new Credit("Spencer Symington",5.0f);
		credits[4] = new Credit("dedicated to \nAnjali",5.0f);
		credits[5] = new Credit("you are the best",5.0f);
		credits[6] = new Credit("thank you \nMom and Dad",5.0f);
		credits[7] = new Credit("and my sister \nSabrina",5.0f);
		credits[8] = new Credit("and to everyone else",5.0f);
		credits[9] = new Credit("Thank you for beliving in me",5.0f);
		credits[10] = new Credit("this has been",5.0f);
		credits[11] = new Credit("Colorgy",5.0f);

	}
	public void CreateCreditOb(){
		if(curCredit >= credits.Length){
			return;
			//TODO end the credit loop and go back to the main menu
		}

		GameObject g = Instantiate(creditPrefab);
		g.transform.SetParent(transform,false);
		//TODO position it
		CreditScript creditScript = g.GetComponent<CreditScript>();
		creditScript.SetUp(credits[curCredit],this);
		curCredit++;

	}

	public class Credit{
		string textTop;

		float displayTime;

		public Credit(string t1,float t){
			textTop = t1;

			displayTime = t;

		}
		public string GetTopText(){
			return textTop;
		}

	}

}
