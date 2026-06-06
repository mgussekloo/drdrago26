using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class enemyMove : MonoBehaviour {
/*
	public GameObject startPoint;
	public GameObject targetPoint;
	private GameObject tmpTargetPoint;
	public GameObject goalPoint;
	
	public GameObject loadGameLook;
	//public GameObject allCharacters;
	
	public GameObject characterUI;
	private float step = 5.0f;
	private bool playerIsMoving = false;
	public int rolledDiceStepsLeft = 0;
	public List<GameObject> tmpPathWalked = new List<GameObject>();
	public GameObject endTurnButtonObject;
	private bool youAreNotAllowedToMove = false;
	
	// Use this for initialization
	void Start () {	
		/*
		Helmut Pohl
		Luigi Maserotti
		Jane Blonda
		vera cruise
		Mikro Mawasaki
		James Blond
		zora meander
		Armino Gesserti
		
		tmpPathWalked.Add(startPoint); 
	}
	
	public void selectedCharacter(GameObject characterID){
		characterUI = characterID;
		transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoLeft;		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(playerIsMoving == false ){	
			if (Input.GetKeyDown(KeyCode.LeftArrow)){
				if(startPoint.GetComponent<connectedLinks>().leftNeighbour != null){
			//	Debug.Log("move left");
					targetPoint = startPoint.GetComponent<connectedLinks>().leftNeighbour;
					checkWalkedPathThisTurn();
					if(!youAreNotAllowedToMove){
					//	Debug.Log(targetPoint.name);
						playerIsMoving = true;
						if(targetPoint.transform.GetComponent<connectedLinks>().waterPoint){
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainLeft;
						}
						else{
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoLeft;
						}
						StartCoroutine(MovePlayer1Coroutine());
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow)){
				if(startPoint.GetComponent<connectedLinks>().rightNeighbour != null){
				//	Debug.Log("move right");
					targetPoint = startPoint.GetComponent<connectedLinks>().rightNeighbour;
					checkWalkedPathThisTurn();
				 // Debug.Log(targetPoint.name);
					if(!youAreNotAllowedToMove){
						playerIsMoving = true;
						if(targetPoint.transform.GetComponent<connectedLinks>().waterPoint){
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainRight;
						}
						else{
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoRight;
						}
						StartCoroutine(MovePlayer1Coroutine());
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow)){
				if(startPoint.GetComponent<connectedLinks>().upNeighbour != null){
					//Debug.Log("move up");
					targetPoint = startPoint.GetComponent<connectedLinks>().upNeighbour;
					checkWalkedPathThisTurn();
					//Debug.Log(targetPoint.name);
					if(!youAreNotAllowedToMove){
						playerIsMoving = true;
						if(targetPoint.transform.GetComponent<connectedLinks>().waterPoint){
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainUp;
						}
						else{
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoUp;
						}
						StartCoroutine(MovePlayer1Coroutine());
							
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow)){
				if(startPoint.GetComponent<connectedLinks>().downNeighbour != null){
					//Debug.Log("move down");
					targetPoint = startPoint.GetComponent<connectedLinks>().downNeighbour;
					checkWalkedPathThisTurn();
					// Debug.Log(targetPoint.name);
					if(!youAreNotAllowedToMove){
						playerIsMoving = true;
						if(targetPoint.transform.GetComponent<connectedLinks>().waterPoint){
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainDown;
						}
						else{
							transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoDown;
						}
						StartCoroutine(MovePlayer1Coroutine());
					}
				}
			}
		}
	}
	
	IEnumerator MovePlayer1Coroutine() {
	
		GameObject _item = targetPoint;
         Vector3 itemPos = _item.transform.position;
		while (Vector3.Distance(transform.position, itemPos) > .0001) {
		  transform.position = Vector3.MoveTowards(transform.position, itemPos, step * Time.deltaTime);
		  yield return null;
		}
		
		
		moveEndNextWayPoint();		
		startPoint = targetPoint;
		if(rolledDiceStepsLeft == 0){
			transform.GetChild(0).GetComponent<AudioSource>().Play();
			endTurnButtonObject.SetActive(true);
		}
		playerIsMoving = false;
	}
	  
	private void checkWalkedPathThisTurn(){
	Debug.Log(targetPoint + "tmpTargetPoint" );
	Debug.Log(startPoint + "tmpTargetPoint" );
	
		if(tmpPathWalked.Count > 1){
			Debug.Log(tmpPathWalked[tmpPathWalked.Count-1] + "tmpPathWalked count -2");
			if(targetPoint == tmpPathWalked[tmpPathWalked.Count-2]){
				Debug.Log("tmpTargetPoint == tmpPathWalked" );
				youAreNotAllowedToMove = false;
				endTurnButtonObject.SetActive(false);
				targetPoint = targetPoint;
				rolledDiceStepsLeft = rolledDiceStepsLeft + 1;
				tmpPathWalked.Remove(tmpPathWalked[tmpPathWalked.Count-1]);	
			}
			else if(rolledDiceStepsLeft > 0 ){
				Debug.Log("i am allowed to walk");
				youAreNotAllowedToMove = false;
				rolledDiceStepsLeft = rolledDiceStepsLeft - 1;
				tmpPathWalked.Add(targetPoint);
				targetPoint = targetPoint;
			}
			else{
				Debug.Log("i am not allowed to walk");
				youAreNotAllowedToMove = true;
			}
		}
		else if(rolledDiceStepsLeft > 0){
			youAreNotAllowedToMove = false;
			rolledDiceStepsLeft = rolledDiceStepsLeft - 1;
				tmpPathWalked.Add(targetPoint);
				targetPoint = targetPoint;
		}
		else{
			youAreNotAllowedToMove = true;
		}
	}
	
	private void moveEndNextWayPoint(){
		this.GetComponent<pathFinding>().clearAllPathFindingInfo();
		this.GetComponent<pathFinding>().pathToFind(targetPoint,goalPoint);
		loadGameLook.GetComponent<gameLookFunc>().setStepsTillGoal(this.GetComponent<pathFinding>().amountSteps);
		loadGameLook.GetComponent<gameLookFunc>().setThrownDices(rolledDiceStepsLeft);
	  }*/
}