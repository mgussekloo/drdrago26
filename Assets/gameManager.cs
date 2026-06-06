using System;
using System.Globalization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.IO;


using System.Data;
using Mono.Data.SqliteClient;

public class gameManager : MonoBehaviour {

	public List<GameObject> playersList = new List<GameObject>();
	public List<GameObject> spriteCharactersList = new List<GameObject>();
	public List<GameObject> wayPointMissions = new List<GameObject>();
   // public List<string> allTypeCards = new List<string>();
    //public GameObject[] wayPointMissions;
    public GameObject startPointMission;
	public GameObject endPointMission;
	public List<GameObject> listActivePlayers = new List<GameObject>();
	public GameObject loadGameLook;
	public GameObject diceScreen;
	public GameObject monthOverview;
	public GameObject finishedScreen;
    public GameObject drawCardsObject;
    public GameObject cardPrefab;
    public bool missionFinishedStatus = false;
	
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int moneyToShow;
	private int activatedNumberPlayer=0;
	private int monthInt = 1;

    private GameObject endturnScreen;
    private GameObject gameLook;
   // private GameObject gameManager;

    public int minLose = 1000;
    public int maxLose = 2000;
    public Sprite faceSpriteShow;
    public AudioClip otherClip;
    


    public Sprite[] cardImages;
    private List<cardInputInfo> allTypeCards = new List<cardInputInfo>();

    void Start()
    {

        //   allTypeCards.Add("fallingMoney", List<int> intValues = new List<int>(1000));
        //   allTypeCards.Add("flight", 1);
        //   allTypeCards.Add("gamble", 1);
        //   allTypeCards.Add("insurance", 1);
        //   allTypeCards.Add("move", 1);
        //   allTypeCards.Add("move", 2);
        //   allTypeCards.Add("move", 3);
        //   allTypeCards.Add("move", 4);
        //  allTypeCards.Add("move", 5);
        //  allTypeCards.Add("move", 6);
      
       // var type1 = new cardInputInfo("fallingMoney", 1000, 10000);
        allTypeCards.Add(new cardInputInfo("discountEstate", 25, 20000));
        allTypeCards.Add(new cardInputInfo("flight", 1, 10000));
        allTypeCards.Add(new cardInputInfo("gamble", 1, 40000));
        allTypeCards.Add(new cardInputInfo("insurance", 1, 10000));
        allTypeCards.Add(new cardInputInfo("move", 1, 13000));
        allTypeCards.Add(new cardInputInfo("move", 2, 14000));
        allTypeCards.Add(new cardInputInfo("move", 3, 11000));
        allTypeCards.Add(new cardInputInfo("move", 4, 12000));
        allTypeCards.Add(new cardInputInfo("move", 5, 16000));
        allTypeCards.Add(new cardInputInfo("move", 6, 15000));
        allTypeCards.Add(new cardInputInfo("nails", 6, 15000));
        allTypeCards.Add(new cardInputInfo("nessie", 6, 15000));
        allTypeCards.Add(new cardInputInfo("nosteering", 6, 15000));
        allTypeCards.Add(new cardInputInfo("discountEstate", 90, 150000));
        allTypeCards.Add(new cardInputInfo("discountEstate", 50, 100000));
        allTypeCards.Add(new cardInputInfo("slot", 2, 15000));
        allTypeCards.Add(new cardInputInfo("slot", 3, 25000));
        allTypeCards.Add(new cardInputInfo("slot", 4, 35000));
        allTypeCards.Add(new cardInputInfo("rentForward", 6, 15000));
        allTypeCards.Add(new cardInputInfo("purchaseEstate", 6, 15000));
        generateGameCards();
    }

    public class cardInputInfo
    {
        public string cardTypeName { get; set; }
        public int intValueForType { get; set; }
        public int cardPrice { get; set; }
        public cardInputInfo(string cardTypeName, int intValueForType, int cardPrice)
        {
            this.cardTypeName = cardTypeName;
            this.intValueForType = intValueForType;
            this.cardPrice = cardPrice;
        }
    }

    public void submitCharacter(GameObject charPlayer,GameObject charSprites){
		
		if(playersList.IndexOf(charPlayer) < 0){
			playersList.Add(charPlayer);
			spriteCharactersList.Add(charSprites);
		}
		else{
			for(int i = 0; i< playersList.Count; i++){
				if(playersList[i] == charPlayer){
					playersList[i].GetComponent<selectPlayer>().AIPlayer = charPlayer.GetComponent<selectPlayer>().AIPlayer;
					break;
				}
			}
		}	
	}
	
    public void generateGameCards()
    {
        int amountGameCards = cardImages.Length;
        float tmpX = -500.0f;
        float tmpY = 400.0f;
        int nextRowCard = 0;
        for (int j = 0; j< amountGameCards; j++)
        {
            GameObject tmpInstantiateCard = Instantiate(cardPrefab) as GameObject;
            tmpInstantiateCard.transform.parent = drawCardsObject.transform;
            tmpInstantiateCard.GetComponent<Image>().sprite = cardImages[j];
            tmpInstantiateCard.transform.name = tmpInstantiateCard.name.Replace("card1(Clone)", "" + cardImages[j].name + "").Trim();

            if(nextRowCard > 4)
            {
                tmpX = -500.0f;
                tmpY = tmpY - 250;
                nextRowCard = 0;
            }
            tmpInstantiateCard.transform.localPosition = new Vector3(tmpX, tmpY, 0);
            tmpX = tmpX + 200;
            nextRowCard = nextRowCard + 1;

            setDefaultCardProperties(tmpInstantiateCard,j);

            if (allTypeCards[j].cardTypeName == "move")
            {
                tmpInstantiateCard.GetComponent<cardProperties>().moveStepsValue = allTypeCards[j].intValueForType;
               
            }
            else if(allTypeCards[j].cardTypeName == "discountEstate")
            {
                tmpInstantiateCard.GetComponent<cardProperties>().discountEstate = allTypeCards[j].intValueForType;
            }
        }
    }
    private void setDefaultCardProperties(GameObject defaultCardSet, int selectedCard)
    {
        defaultCardSet.GetComponent<cardProperties>().typeCard = allTypeCards[selectedCard].cardTypeName;
        defaultCardSet.GetComponent<cardProperties>().price = allTypeCards[selectedCard].cardPrice;
    }

    public void loadGameCharacters(GameObject charPlayer){
		if(playersList.IndexOf(charPlayer) < 0){
			playersList.Add(charPlayer);
			spriteCharactersList.Add(charPlayer);
		}
		else{
			for(int i = 0; i< playersList.Count; i++){
				if(playersList[i] == charPlayer){
					playersList[i].GetComponent<characterUIInfo>().AIPlayer = charPlayer.GetComponent<characterUIInfo>().AIPlayer;
					break;
				}
			}
		}
	}
	
	public void removeCharacter(GameObject charPlayer, GameObject charSprites){
		playersList.Remove(charPlayer);
		spriteCharactersList.Remove(charSprites);
	}
	
	//new game will gen startpoint and endpoint for mission
    public void startPositionNewGame(){
        int randomWaypoint = UnityEngine.Random.Range(0, 0);
        startPointMission = wayPointMissions[randomWaypoint];
    }
	public void genMission(){
        missionFinishedStatus = false;
        Debug.Log(wayPointMissions.Count + "wayPointMissions.Count");
		int randomWaypoint = UnityEngine.Random.Range(1, wayPointMissions.Count);
		endPointMission = wayPointMissions[randomWaypoint];
		wayPointMissions.Remove(endPointMission);
        loadGameLook.GetComponent<gameLookFunc>().setMissionName(endPointMission.name);	
	}
    public void setNewMissionPlayers(){
        //  listActivePlayers
        int countPlayers = listActivePlayers.Count;
        for (int i = 0; i < countPlayers; i++){
            listActivePlayers[i].GetComponent<playerMove>().goalPoint = endPointMission;
        } 


    }
	
	public void startGame(){
        //	numOfDaysThisMonth = System.DateTime.DaysInMonth(yearInt, monthInt);

        loadNextPlayer();
	}
	
	public void activateNexPlayer(){
		activatedNumberPlayer = activatedNumberPlayer +1;
		loadNextPlayer();
	}
	
	private void loadNextPlayer(){
        if (activatedNumberPlayer >= listActivePlayers.Count){ 
			///////month is over give overview plus top rate of players.////////////////////////////////////////////
			showRaceOverview();
			monthInt = monthInt+1;
			
            if(monthInt > 12){
				monthInt = 1;
			}
			
            String monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthInt).ToString();
			loadGameLook.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = monthName + " (1)";
			activatedNumberPlayer = 0;
		}
        else{
			loadNextPlayerThings();			
		}
	}
	
	public void loadNextPlayerThings(){
		List<GameObject> tmpPath = listActivePlayers[activatedNumberPlayer].GetComponent<pathFinding>().correctPathToFind(listActivePlayers[activatedNumberPlayer].GetComponent<playerMove>().startPoint,listActivePlayers[activatedNumberPlayer].GetComponent<playerMove>().goalPoint);
		loadGameLook.GetComponent<gameLookFunc>().setStepsTillGoal(tmpPath.Count-1);
		loadGameLook.GetComponent<gameLookFunc>().setPlayerFace(listActivePlayers[activatedNumberPlayer].GetComponent<playerMove>().characterUI.GetComponent<characterUIInfo>().charID,listActivePlayers[activatedNumberPlayer].GetComponent<playerMove>().characterUI.name);	
		showPlayerMoney(listActivePlayers[activatedNumberPlayer].GetComponent<playerMove>().playerID);
		loadGameLook.GetComponent<gameLookFunc>().playerObject = listActivePlayers[activatedNumberPlayer];
		loadGameLook.GetComponent<gameLookFunc>().playerObject.transform.GetComponent<playerMove>().loadPlayerCards();	
        loadGameLook.transform.GetChild(5).GetComponent<Button>().interactable = false;
		Camera.main.GetComponent<camMovement>().activePlayer = listActivePlayers[activatedNumberPlayer];

        ////////////////if PC dan AI functionaliteit bouwen hier. 
        diceScreen.SetActive(true);
        diceScreen.GetComponent<Button>().interactable = true;

        if (loadGameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI == 1)
        {
            diceScreen.GetComponent<Button>().interactable = false;
            diceScreen.GetComponent<rollDice>().enterAIRoll();

           // Debug.Log("yes I am AI");
        }

	}
	
	private void showRaceOverview(){
		monthOverview.SetActive(true);
		monthOverview.GetComponent<showMonthOverviewStats>().loadMonthOverview(listActivePlayers);
	}
	
	public void missionFinished(GameObject finishedPlayer){
		finishedScreen.SetActive(true);
		finishedScreen.GetComponent<showPlayerFinish>().loadPlayerWin(finishedPlayer);
	}
	
	public void showPlayerMoney(int? playerID){
       // Debug.Log("show me the money " + playerID);
        string _constr = ""; 
        if (Application.isEditor){
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else{
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;

        }
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="SELECT playerMoney FROM `playerInfo`  WHERE `id`='"+playerID+"'";
		_dbr=_dbcm.ExecuteReader();
	
		while( _dbr.Read()){
			 moneyToShow = _dbr.GetInt32(0);
		}
		_dbc.Close();
        string tmpMoneyString = string.Format("{0:+#;-#;+0}", moneyToShow);
       string tmpMoneyString2 = string.Format("${0}", tmpMoneyString);
        loadGameLook.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = tmpMoneyString2;
	}

    public void playerLooseMoney(Boolean triggerDrDrago)
    {
         int moneyToShow=0;
        endturnScreen = GameObject.Find("Canvas");
      // gameManager = GameObject.Find("gameManager");
        gameLook = endturnScreen.transform.Find("gameLook").gameObject;
        endturnScreen = endturnScreen.transform.Find("endTurnActionScreen").gameObject;

        endturnScreen.SetActive(true);
        endturnScreen.GetComponent<AudioSource>().clip = otherClip;
        endturnScreen.GetComponent<AudioSource>().Play();
        int loseMoney = UnityEngine.Random.Range(minLose, maxLose);

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
        //string _constr="URI=file:"+ Application.persistentDataPath +"/drdragoDB.db";
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "SELECT playerMoney FROM `playerInfo`  WHERE `id`='" + gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID + "'";
        _dbr = _dbcm.ExecuteReader();

        while (_dbr.Read())
        {
            moneyToShow = _dbr.GetInt32(0);
        }

        int tmpMoneyPlayer = moneyToShow - loseMoney;

        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "UPDATE playerInfo SET playerMoney = " + tmpMoneyPlayer + ", revenue = -" + loseMoney + " WHERE `id`='" + gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID + "'";
        _dbr = _dbcm.ExecuteReader();
        _dbc.Close();

        showPlayerMoney(gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID);
        endturnScreen.transform.GetChild(0).GetComponent<Text>().text = "Whoehahahahahaha " + loseMoney + " I found this amount in your appartment";
        endturnScreen.transform.GetChild(1).GetComponent<Image>().sprite = faceSpriteShow;

        if (gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI == 1 || triggerDrDrago==true)
        {
            endturnScreen.GetComponent<Button>().interactable = false;
            StartCoroutine(showAIActionsCoroutine(triggerDrDrago));
        }
    }

    IEnumerator showAIActionsCoroutine(Boolean triggerDrDrago)
    {
        
        yield return new WaitForSeconds(2.0f);
      //  Debug.Log("disableactive");
        endturnScreen.GetComponent<Button>().interactable = true;
        endturnScreen.SetActive(false);

        //rollDiceOject.GetComponent<rollDice>().startAIPlayers();
        if (!triggerDrDrago)
        {
            endturnScreen.GetComponent<nextPlayer>().onClickEnter();
        }
    }

}
