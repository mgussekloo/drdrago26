using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class playerMove : MonoBehaviour {

	public GameObject startPoint;
	public GameObject targetPoint;
	private GameObject tmpTargetPoint;
	public GameObject goalPoint;
	public GameObject loadGameLook;
	public GameObject characterUI;
    public int playerIsAI = 0;
	public int playerID;
	private float step = 5.0f;
	private bool playerIsMoving = false;
	public int rolledDiceStepsLeft = 0;
	public List<GameObject> tmpPathWalked = new List<GameObject>();
	public List<GameObject> playerCards = new List<GameObject>();
	private GameObject loadCardsIn;
	private GameObject instantCardObject;
	public GameObject endTurnButtonObject;
	private bool youAreNotAllowedToMove = false;
	
	private string stringCardName;
    private int intIDDBcard;
    private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private Sprite carEndPoint;

    //touchcontroll variables
    public const float max_swipe_time = 1.0f;
    public const float min_swipe_distance = 0.17f;
    public bool swipedRight = false;
    public bool swipedLeft = false;
    public bool swipedUp = false;
    public bool swipedDown = false;

    private int currentDistanceAI = 0;
    private int futureDistanceAI = 0;


    Vector2 startPos;
    private float startTime;

	// Use this for initialization
	void Start () {	
		/*
		Helmut Pohl
		Luigi Maserotti
		Jane Blonda
		vera cruiseloadPlayerCards
		Mikro Mawasaki
		James Blond
		zora meander
		Armino Gesserti
		*/
		
	}
	
	public void loadPlayerCards(){
		loadCardsIn = loadGameLook.transform.GetChild(5).gameObject;
		
		if(loadCardsIn.transform.childCount > 0){
			Transform[] tmpLoadedCards = loadCardsIn.transform.GetComponentsInChildren<Transform>();
			for(int i=0; i<tmpLoadedCards.Length; i++){
				if(tmpLoadedCards[i].tag == "playerCards"){
					Destroy(tmpLoadedCards[i].gameObject);
				}
			}
	
		}
		
        string _constr = "";
        if (Application.isEditor){
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else{
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;

        }
        //string _constr="URI=file:"+ Application.persistentDataPath +"/drdragoDB.db";
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
     //   Debug.Log("playerID" + playerID);
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="SELECT id, cardName FROM `playerCards` WHERE `playerID`='"+playerID+"'";
		_dbr=_dbcm.ExecuteReader();
		
		GameObject tmpDrawCardsObject = loadGameLook.transform.parent.Find("drawCards").gameObject;
		int pushedCardCount = 0;
		int x = 5;
		int y = 5;
		while( _dbr.Read()){
            intIDDBcard = _dbr.GetInt32(0);
			stringCardName = _dbr.GetString(1);
			for (int i=0; i<tmpDrawCardsObject.transform.childCount; i++){
//				Debug.Log("tmpDrawCardsObject.transform.GetChild(i).name");
				if(stringCardName == tmpDrawCardsObject.transform.GetChild(i).name){
					playerCards[pushedCardCount] = tmpDrawCardsObject.transform.GetChild(i).gameObject;
					
					instantCardObject = Instantiate (playerCards[pushedCardCount]) as GameObject;		
					instantCardObject.transform.parent = loadCardsIn.transform;
					instantCardObject.name = instantCardObject.name.Replace("(Clone)","").Trim();
					instantCardObject.transform.localPosition = new Vector3 (x, y , 0);	
                    instantCardObject.transform.localScale = new Vector3(1.5F, 1.5F, 1F);
                    instantCardObject.transform.GetComponent<cardProperties>().playerObject = this.gameObject;
                    instantCardObject.transform.GetComponent<cardProperties>().dbCardID = intIDDBcard;
                //	instantCardObject.GetComponent<Button>().onClick  = new Button.ButtonClickedEvent();					
                    instantCardObject.GetComponent<Image>().raycastTarget  = false;					
					
					pushedCardCount = pushedCardCount +1;
					x = x -5;
					y = y - 5;
					break; 
				}	
			}	
		}
		x = 0; 
		y=0;
        _dbc.Close();
    }

    public void moveTheAIPlayerNow()
    {
        if (tmpPathWalked.Count == 0)
        {
            tmpPathWalked.Add(startPoint);
        }

        GameObject bestNeighbour = getBestAINeighbour(false);

        if (bestNeighbour == null)
        {
            bestNeighbour = getBestAINeighbour(true);
        }

        if (bestNeighbour != null)
        {
            targetPoint = bestNeighbour;
            applyAIMovementCost();
            moveAIToTargetPoint();
        }
    }

    private GameObject getBestAINeighbour(bool allowImmediateReturn)
    {
        connectedLinks links = startPoint.GetComponent<connectedLinks>();
        GameObject bestNeighbour = null;
        int bestDistance = 9999;

        bestNeighbour = getBetterAINeighbour(bestNeighbour, links.leftNeighbour, ref bestDistance, allowImmediateReturn);
        bestNeighbour = getBetterAINeighbour(bestNeighbour, links.rightNeighbour, ref bestDistance, allowImmediateReturn);
        bestNeighbour = getBetterAINeighbour(bestNeighbour, links.upNeighbour, ref bestDistance, allowImmediateReturn);
        bestNeighbour = getBetterAINeighbour(bestNeighbour, links.downNeighbour, ref bestDistance, allowImmediateReturn);

        return bestNeighbour;
    }

    private GameObject getBetterAINeighbour(GameObject currentBest, GameObject neighbour, ref int bestDistance, bool allowImmediateReturn)
    {
        if (neighbour == null)
        {
            return currentBest;
        }

        if (!allowImmediateReturn && isReturningToPreviousTile(neighbour))
        {
            return currentBest;
        }

        int neighbourDistance = getDistanceToGoal(neighbour);
        if (currentBest == null || neighbourDistance < bestDistance)
        {
            bestDistance = neighbourDistance;
            return neighbour;
        }

        return currentBest;
    }

    private int getDistanceToGoal(GameObject point)
    {
        List<GameObject> tmpPath = this.GetComponent<pathFinding>().correctPathToFind(point, goalPoint);
        int distance = tmpPath.Count - 1;
        tmpPath.Clear();
        this.GetComponent<pathFinding>().clearAllPathFindingInfo();

        if (distance < 0)
        {
            return 9999;
        }

        return distance;
    }

    private bool isReturningToPreviousTile(GameObject point)
    {
        return tmpPathWalked.Count > 0 && point == tmpPathWalked[tmpPathWalked.Count - 1];
    }

    private void applyAIMovementCost()
    {
        if (isReturningToPreviousTile(targetPoint))
        {
            rolledDiceStepsLeft = rolledDiceStepsLeft + 1;
            tmpPathWalked.Remove(tmpPathWalked[tmpPathWalked.Count - 1]);
        }
        else if (rolledDiceStepsLeft > 0)
        {
            rolledDiceStepsLeft = rolledDiceStepsLeft - 1;
            tmpPathWalked.Add(startPoint);
        }
    }

    private void moveAIToTargetPoint()
    {
        connectedLinks links = startPoint.GetComponent<connectedLinks>();
        playerIsMoving = true;

        if (targetPoint == links.leftNeighbour)
        {
            moveLeft();
        }
        else if (targetPoint == links.rightNeighbour)
        {
            moveRight();
        }
        else if (targetPoint == links.upNeighbour)
        {
            moveUp();
        }
        else if (targetPoint == links.downNeighbour)
        {
            moveDown();
        }
        else
        {
            playerIsMoving = false;
        }
    }

    public bool checkIfPathIsShorter(GameObject targetPointAI){
       // Debug.Log("futureDistanceAI " + targetPointAI.name );
       
        List<GameObject> tmpPath = this.GetComponent<pathFinding>().correctPathToFind(targetPointAI, goalPoint);
        futureDistanceAI = tmpPath.Count-1;
      //  Debug.Log("futureDistanceAI " + futureDistanceAI + "currentDistanceAI " + currentDistanceAI);
        if (futureDistanceAI <= currentDistanceAI)
        {
            tmpPath.Clear();
            return true;
        }
        else
        {
           //Debug.Log("dis is not smaller so check next neighbour" + futureDistanceAI + " future - current " + currentDistanceAI);
            tmpPath.Clear();
            return false;
        }
        //return false;
    }

    void Update () {
		
		if(playerIsMoving == false ){
            swipedRight = false;
            swipedLeft = false;
            swipedDown = false;
            swipedUp = false;

            if(Input.touches.Length>0){
                Touch t = Input.GetTouch(0);
                if(t.phase == TouchPhase.Began){
                    startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                    startTime = Time.time;
                }
                if(t.phase==TouchPhase.Ended){
                    if(Time.time - startTime > max_swipe_time){
                        return;
                    }
                    Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                    Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
                    if (swipe.magnitude<min_swipe_distance){
                        return;
                    }
                    if(Mathf.Abs(swipe.x)>Mathf.Abs(swipe.y)){ //horizontal swipe
                        if(swipe.x>0){
                            swipedRight = true;
                        }
                        else{
                            swipedLeft = true;
                        }
                    }
                    else{ //vertical swipe
                        if(swipe.y>0){
                            swipedUp = true;
                        }
                        else{
                            swipedDown = true;
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || swipedLeft){
				
				if(startPoint.GetComponent<connectedLinks>().leftNeighbour != null){
			//	Debug.Log("move left");
					targetPoint = startPoint.GetComponent<connectedLinks>().leftNeighbour;
					checkWalkedPathThisTurn();
					if(!youAreNotAllowedToMove){
					//	Debug.Log(targetPoint.name);
						playerIsMoving = true;
                        
                        moveLeft();
                        
                    }
				}
			}
            else if (Input.GetKeyDown(KeyCode.RightArrow)||swipedRight){
				
				if(startPoint.GetComponent<connectedLinks>().rightNeighbour != null){
				//	Debug.Log("move right");
					targetPoint = startPoint.GetComponent<connectedLinks>().rightNeighbour;
					checkWalkedPathThisTurn();
				 // Debug.Log(targetPoint.name);
					if(!youAreNotAllowedToMove){
						playerIsMoving = true;
                        
                        moveRight();
					}
				}
			}
            else if (Input.GetKeyDown(KeyCode.UpArrow)||swipedUp){
				
				if(startPoint.GetComponent<connectedLinks>().upNeighbour != null){
					//Debug.Log("move up");
					targetPoint = startPoint.GetComponent<connectedLinks>().upNeighbour;
					checkWalkedPathThisTurn();
					//Debug.Log(targetPoint.name);
					if(!youAreNotAllowedToMove){
						playerIsMoving = true;
                      
                        moveUp();
					}
				}
			}
            else if (Input.GetKeyDown(KeyCode.DownArrow)||swipedDown){
				
				if(startPoint.GetComponent<connectedLinks>().downNeighbour != null){
					//Debug.Log("move down");
					targetPoint = startPoint.GetComponent<connectedLinks>().downNeighbour;
					checkWalkedPathThisTurn();
					// Debug.Log(targetPoint.name);
					if(!youAreNotAllowedToMove){
						playerIsMoving = true;
                       
                        //	Debug.Log(targetPoint.transform.GetComponent<connectedLinks>().downWayType);
                        moveDown();
					}
				}
			}
		}
	}

    private void moveLeft()
    {
        if (startPoint.transform.GetComponent<connectedLinks>().leftWayType == "water")
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainLeft;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoLeft;
        }
        if (targetPoint.transform.GetComponent<connectedLinks>().waterPoint == false)
        {
            carEndPoint = characterUI.GetComponent<characterUIInfo>().autoLeft;
        }
        transform.GetChild(1).GetComponent<drdrago>().moveWithPlayer("left");
        StartCoroutine(MovePlayer1Coroutine(carEndPoint));
    }

    private void moveRight()
    {
        if (startPoint.transform.GetComponent<connectedLinks>().rightWayType == "water")
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainRight;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoRight;
        }
        if (targetPoint.transform.GetComponent<connectedLinks>().waterPoint == false)
        {
            carEndPoint = characterUI.GetComponent<characterUIInfo>().autoRight;
        }
        transform.GetChild(1).GetComponent<drdrago>().moveWithPlayer("right");
        StartCoroutine(MovePlayer1Coroutine(carEndPoint));
    }

    private void moveUp()
    {
        if (startPoint.transform.GetComponent<connectedLinks>().upWayType == "water")
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainUp;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoUp;
        }
        if (targetPoint.transform.GetComponent<connectedLinks>().waterPoint == false)
        {
            carEndPoint = characterUI.GetComponent<characterUIInfo>().autoUp;
        }
        transform.GetChild(1).GetComponent<drdrago>().moveWithPlayer("up");
        StartCoroutine(MovePlayer1Coroutine(carEndPoint));
    }

    private void moveDown()
    {
        if (startPoint.transform.GetComponent<connectedLinks>().downWayType == "water")
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().plainDown;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = characterUI.GetComponent<characterUIInfo>().autoDown;
        }
        if (targetPoint.transform.GetComponent<connectedLinks>().waterPoint == false)
        {
            carEndPoint = characterUI.GetComponent<characterUIInfo>().autoDown;
        }
        transform.GetChild(1).GetComponent<drdrago>().moveWithPlayer("down");
        StartCoroutine(MovePlayer1Coroutine(carEndPoint));
    }

    IEnumerator MovePlayer1Coroutine(Sprite carEndPoint) {
	
		GameObject _item = targetPoint;
        Vector3 itemPos = _item.transform.position;
		while (Vector3.Distance(transform.position, itemPos) > .0001) {
			transform.position = Vector3.MoveTowards(transform.position, itemPos, step * Time.deltaTime);
			yield return null;
		}
	//	Debug.Log(carEndPoint.name);
		if(carEndPoint != null && targetPoint.transform.GetComponent<connectedLinks>().waterPoint == false){
			transform.GetComponent<SpriteRenderer>().sprite = carEndPoint;
		}
        startPoint = targetPoint;
        moveEndNextWayPoint();

        if (playerIsAI == 1 && rolledDiceStepsLeft > 0)
        {
            yield return new WaitForSeconds(0.25f);
            moveTheAIPlayerNow();
        }
        else if (playerIsAI == 1 && rolledDiceStepsLeft == 0)
        {
          //  endTurnButtonObject.SetActive(true);
            transform.GetChild(0).GetComponent<AudioSource>().Play();

            string tmpPosName = this.startPoint.name;
            //  Debug.Log("playerID" +tmpPosName);
            string _constr = "";
            if (Application.isEditor)
            {
                string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
                _constr = "URI=file:" + dbPath;
            }
            else
            {
                string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
                _constr = "URI=file:" + dbPath;

            }
            // string _constr="URI=file:"+ Application.persistentDataPath +"/drdragoDB.db";
            _dbc = new SqliteConnection(_constr);
            _dbc.Open();
            _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = "UPDATE playerInfo SET positionPlayer ='" + tmpPosName + "' WHERE id='" + this.playerID + "'";
            _dbr = _dbcm.ExecuteReader();
            _dbc.Close();
            yield return new WaitForSeconds(0.5f);
            // endTurnButtonObject.SetActive(false);

            if (loadGameLook.GetComponent<gameLookFunc>().checkMissionFinished())
            {
                endTurnButtonObject.GetComponent<endTurnButton>().gameManagerObject.GetComponent<gameManager>().missionFinished(this.gameObject);
            }
            else {
                showEndpointPopup();
            }
           // endTurnButtonObject.GetComponent<endTurnButton>().gameManagerObject.GetComponent<gameManager>().activateNexPlayer();

        }
        loadGameLook.GetComponent<gameLookFunc>().showCurrentPointGameLook(startPoint);
        playerIsMoving = false;

        if (playerIsAI == 0 && rolledDiceStepsLeft == 0){
			transform.GetChild(0).GetComponent<AudioSource>().Play();
         //   Debug.Log("set end button true");
			endTurnButtonObject.SetActive(true);
		}
		
    
          //  Debug.Log("player is AI " + playerIsAI + "rolleddiceleft " + rolledDiceStepsLeft);
       
    }



    public void showEndpointPopup()
        //checkEndpointTag
        
    {

        if (startPoint.tag == "wayPointBlue")
        {

            startPoint.GetComponent<blueEndPoint>().confirmEndPoint();

        }
        else if (startPoint.tag == "wayPointRed")
        {
            startPoint.GetComponent<redEndPoint>().confirmEndPoint();
        }
        else if (startPoint.tag == "wayPointGray")
        {
            startPoint.GetComponent<grayEndPoint>().confirmEndPoint();
        }
        else if (startPoint.tag == "wayPointYellow")
        {
            startPoint.GetComponent<yellowEndPoint>().confirmEndPoint(this.gameObject);
        }
        else if (startPoint.tag == "wayPointPurple")
        {
            startPoint.GetComponent<purpleEndPoint>().confirmEndPoint(this.gameObject);
        }
        else if (startPoint.tag == "wayPointCapital")
        {
            startPoint.GetComponent<grayEndPoint>().confirmEndPoint();
        }

    }

    public void showEndpointPopupWithDrago() {

        this.transform.GetChild(1).GetComponent<drdrago>().endOfTurn();
       

    }

    private void checkWalkedPathThisTurn(){
	//Debug.Log(targetPoint + "tmpTargetPoint" );
	//Debug.Log(startPoint + "tmpTargetPoint" );
		if(tmpPathWalked.Count > 0 && targetPoint == tmpPathWalked[tmpPathWalked.Count - 1])
        {
			
				youAreNotAllowedToMove = false;
				endTurnButtonObject.SetActive(false);
				//startPoint = targetPoint; 
				rolledDiceStepsLeft = rolledDiceStepsLeft + 1;
				tmpPathWalked.Remove(tmpPathWalked[tmpPathWalked.Count-1]);	
			}
			else if(rolledDiceStepsLeft > 0 ){
			//	Debug.Log("i am allowed to walk");
				youAreNotAllowedToMove = false;
				rolledDiceStepsLeft = rolledDiceStepsLeft - 1;
            tmpPathWalked.Add(startPoint);
            //startPoint = targetPoint;
        }
			else{
			//	Debug.Log("i am not allowed to walk");
				youAreNotAllowedToMove = true;
			}
		
	
	}
	
	private void moveEndNextWayPoint(){
		this.GetComponent<pathFinding>().clearAllPathFindingInfo();
		List<GameObject> tmpPath = this.GetComponent<pathFinding>().correctPathToFind(targetPoint,goalPoint);
		//Debug.Log(tmpPath.Count);
		loadGameLook.GetComponent<gameLookFunc>().setStepsTillGoal(tmpPath.Count-1);
		loadGameLook.GetComponent<gameLookFunc>().setThrownDices(rolledDiceStepsLeft);
	 }
}