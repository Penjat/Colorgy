using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {
	private GridManager gridManager;

	private static string TAG = "HEX: ";

	int xPos,yPos;
	private int val = 0;
	private int addColorVal = -1;

	private Color color;
	private Color newColor;
	private bool isActive = true;
	private bool willClear;
	private float timer;
	private bool willNova;
	private bool isFadingIn;

	private bool paused;
	private bool willMix;
	private bool createEffect = true;
	private bool isMixing;
	private bool willAbsorb;
	private bool isFading;
	private bool willFlip;

	public Tool activeTool;

	public Animator animator;
	public Renderer rend;

	public GameObject HexExplodePrefab;
	public GameObject hexAbsorbPrefab;
	public GameObject addColorPrefab;

	private float mixTimer = 0.0f;


	private static int EXPLODE_PREFAB = 0;
	private static int ABSORB_PREFAB = 1;

	public Transform placePos;

	void Update(){
		if(paused){
			timer -= Time.deltaTime;
			if(timer<=0){
				paused = false;
				animator.speed = 0.6f;
				//setFade in
				isFadingIn = true;
				color = Calc.GetColor(val-1);
				color.a = 0;
			}


		}
		if(isFadingIn){
			
			float changeRate = 0.4f;
			color.a = color.a + changeRate*Time.deltaTime;
			if(color.a >= 0.5f){
				isFadingIn = false;
				color.a = 0.5f;
			}
			rend.material.color = color;

		}

		if(willMix){
			timer -= Time.deltaTime;
			if(timer<=0){
				//Wave();
				willMix = false;
				//UpdateColor();
				createEffect = true;
				mixTimer = 1.0f;
				isMixing = true;
				newColor = Calc.GetColor(val-1);
				newColor.a = 0.5f;
				if(addColorVal != -1){
					CreateAddColorFX();
					Wave();
					addColorVal = -1;
				}

			}
		}
		if(isMixing){
			float mixRate = 1.0f;
			mixTimer -= Time.deltaTime*mixRate;

			if(mixTimer<=0){
				isMixing = false;
				mixTimer = 0;
				color = newColor;
			}
			Color c = Color.Lerp(newColor, color, mixTimer);
			rend.material.color = c;
		}

		if(willClear){
			timer -= Time.deltaTime;
			if(timer<=0){
				willClear = false;
				Clear(0,activeTool);
			}
		}
		if(willNova){
			timer -= Time.deltaTime;
			if(timer<=0){
				willNova = false;
				//finds if other hexes need to nova
				gridManager.Nova(this);

				//do the nova effect
				activeTool.Activate(this);
				//Clear();
			}
		}
		if(willAbsorb){
			timer -= Time.deltaTime;
			if(timer<=0){
				CreateAbsorb();
				willAbsorb = false;
			}
		}
		if(isFading){
			float fadeRate = 1.0f;
			timer-= Time.deltaTime*fadeRate;
			Color c = new Color(0,0,0,0); 
			rend.material.color = Color.Lerp(c,color,timer);
		}
		if(willFlip){
			timer-= Time.deltaTime;
			if(timer<=0){
				Flip();
				willFlip = false;
			}
		}
	}
	public void StartFade(){
		isFading = true;
		timer = 1;
	}
	public void SetUp(int x,int y,int v,GridManager g){
		xPos = x;
		yPos = y;
		val = v;
		gridManager = g;
		animator.speed = 0;
		paused = true;
		//timer = Random.value;
		timer = y*0.1f + x*0.2f;

	}

	public void SetWillMix(float f){
		
		timer = f;
		willMix = true;
	}
	public void SetFadeIn(){
		
		Color c = rend.material.color;
		c.a = 0;
		rend.material.color = c;

	}
	public void SetColor(bool editMode){
		if(val == 0 ){
			if(editMode){
				//show empty hexes as grey if in editMode
				color = Calc.GetColor(7);
				Color c1 = Calc.GetColor(7);
				color.a = 1;
				c1.a = 0.5f;
				rend.material.color = c1;


				return;
			}
			gameObject.SetActive(false);
			isActive = false;
			return;
		}
		color = Calc.GetColor(val - 1);
		color.a = 1;

		Color c2 =  Calc.GetColor(val - 1);
		c2.a = 0.5f;
		rend.material.color = c2;


	}
	public void UpdateColor(){
		
		Color c =  Calc.GetColor(val - 1);
		c.a = 0.5f;
		rend.material.color = c;
		color = c;
		color.a = 1;
	}
	public void LightUp(bool b){
		if(isFading){
			return;
		}
		
		Color c =  Calc.GetColor(val - 1);
		if(b){
			
			//Debug.Log(TAG + "lighting up: " + GetX() + "  " + GetY() );
			rend.material.SetColor("_Color", new Vector4(c.r,c.g,c.b,1) * 20.0f);
			return;
		}

		c.a = 0;
		rend.material.SetColor("_Color", new Vector4(c.r,c.g,c.b,0.5f));
		//animator.SetBool("light",b);
	}
	public int GetVal(){
		return val;
	}
	public void SetVal(int v){
		val = v;
	}
	public void SetWillClear(float t){
		willClear = true;
		timer = t;
	}
	public void SetWillClear(float t,bool b){
		createEffect = b;
		willClear = true;
		timer = t;
	}

	public void Clear(int i,Tool tool){
		
		gameObject.SetActive(false);
		isActive = false;
		if(createEffect){
			GameObject prefab = GetPrefab(i);
			GameObject g = Instantiate(prefab);
			g.transform.position = transform.position;
			if(g == null){
				Debug.Log(tag + "prefab is null");
				return;
			}

			HexEffect hexEffect = g.GetComponent<HexEffect>();
			if(tool == null){
				//Debug.Log(TAG + "tool is null");
				hexEffect.SetUp(tool,this);
			}else{
				//Debug.Log(TAG + "tool is not null");
				GameObject target = tool.GetTarget();
				hexEffect.SetUp(tool,this);
			}
			//TODO check for null target

			//TODO change for particle effect
			//ParticleSystem p =g.GetComponentInChildren<ParticleSystem>();
			//var main = p.main;


			//main.startColor = color;
			val = 0;
		}
		gridManager.CheckIfComplete();

	}
	public GameObject GetPrefab(int i){
		if(i == ABSORB_PREFAB){
			return hexAbsorbPrefab;
		}
		return HexExplodePrefab;
	}

	public Hex HexCheck(){
		if(isActive){
			return this;
		}
		return null;
	}
	public int GetX(){
		return xPos;
	}
	public int GetY(){
		return yPos;
	}
	public bool GetWillClear(){
		return willClear;
	}
	public void SetNova(float f,ToolNova n){
		activeTool = n;
		willNova = true;
		LightUp(true);
		timer = f;
	}
	public Tool GetActiveTool(){
		//gets the active nova 
		return activeTool;
	}
	public bool GetNova(){
		return willNova;
	}
	public bool IsActive(){
		return isActive;
	}
	public Hex GetNeighbor(int xDir,int yDir){
		return gridManager.GetHex(this,xDir,yDir);
	}

	public void Absorb(int v,Tool tool,float f){
		SetVal(v);
		//UpdateColor();
		activeTool = tool;
		timer = f;
		willAbsorb = true;
	}
	public void Absorb(int v,Tool tool){
		SetVal(v);
		//UpdateColor();
		activeTool = tool;
		CreateAbsorb();
	}

	public void CreateAbsorb(){
		if(val == 0){
			Clear(1,activeTool);
			return;
		}else{
			SetWillMix(0.0f);
		}
		GameObject g = Instantiate(GetPrefab(ABSORB_PREFAB));
		g.transform.position = transform.position;
		HexEffect hexEffect = g.GetComponent<HexEffect>();
		GameObject target = activeTool.GetTarget();
		hexEffect.SetUp(activeTool,this);
	}

	public void CreateAddColorFX(){
		GameObject g = Instantiate(addColorPrefab);
		g.transform.position = transform.position;
		Destroy(g,2.0f);
		g.GetComponent<AddColorEffect>().SetColor(addColorVal);
		//TODO change the color
	}
	public void SetAddColorVal(Tool tool){
		addColorVal = tool.GetVal();
		activeTool = tool;
	}
	public void Wave(){
		Debug.Log(TAG +"waving");
		animator.SetTrigger("wave");
	}
	public void Press(bool b){
		Debug.Log(TAG +"pressing");
		animator.SetBool("pressed",b);
	}
	public void Flip(){
		Debug.Log(TAG +"flipping");
		animator.SetTrigger("flip");
	}
	public void Flip(float t){
		Debug.Log(TAG +"flipping");
		timer = t;
		willFlip = true;
	}
	public Hex[] GetNeighbors(){
		Debug.Log(TAG + "finding neighbors.");
		return gridManager.FindAdjacent(this);
	}
	public Transform GetPlacePos(){
		return placePos;
	}
}
