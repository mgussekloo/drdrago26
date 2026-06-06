using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

public class submitSelectedPlayer : MonoBehaviour {
	// Use this for initialization
	public GameObject gameManagerObject;
	public GameObject playerPrefab;
	private List<GameObject> playerObjects = new List<GameObject>();
	
	//public GameObject activatedButtonCharacter;
	public GameObject loadGameLook;
	public GameObject diceScreen;
	
	public GameObject waypointParentObject;
	
    private int tmpIdPlayer;
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	
	void Start () {	
		//////////////////////////////DELETE DB SO GAME STARTS WITH EMPTY DB FOR TESTING PURPOSE!!!!/////////////////////////////////////////////
		Debug.Log("DELETE DB SO GAME STARTS WITH EMPTY DB FOR TESTING PURPOSE! ");
     //   Debug.Log(Application.streamingAssetsPath +"/drdragoDB.db");

        string _constr="";
        if (Application.isEditor)
        {
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else
        {
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            string dbTemplatePath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");

            if (!System.IO.File.Exists(dbPath))
            {
                System.IO.File.Copy(dbTemplatePath, dbPath, true);
            }
            _constr = "URI=file:" + dbPath;
        }

        Debug.Log("filestreaming "+_constr);

        _dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="DELETE FROM playerCards";
		_dbr=_dbcm.ExecuteReader();
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="DELETE FROM playerInfo";
		_dbr=_dbcm.ExecuteReader();
        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "UPDATE cities SET ownerFace='0M', playerID=null";
        _dbr = _dbcm.ExecuteReader();
        _dbc.Close();

	}

 /*   public void ProcessFolder(string f){
        Debug.Log("Folder: " + f);

        var txtFiles = Directory.GetFiles(f);
        foreach (string currentFile in txtFiles)
        {
            Debug.Log("File222: " + currentFile);
        }

        string[] subs = Directory.GetDirectories(f);
        foreach (string sub in subs)
            ProcessFolder(sub);
    }
*/
	public void submitPlayers(){
				
		transform.parent.gameObject.SetActive(false);
        gameManagerObject.GetComponent<gameManager>().startPositionNewGame();
		gameManagerObject.GetComponent<gameManager>().genMission();

		GameObject startPosMission = gameManagerObject.GetComponent<gameManager>().startPointMission;
		GameObject endPosMission = gameManagerObject.GetComponent<gameManager>().endPointMission;
		List<GameObject> tmpListGamePlayers = gameManagerObject.GetComponent<gameManager>().playersList;
  
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

        _dbc=new SqliteConnection(_constr);
		_dbc.Open();
		
		for(int i = 0; i<tmpListGamePlayers.Count; i++){
			int tmpAIPlayer = 0;
			if(tmpListGamePlayers[i].GetComponent<selectPlayer>().AIPlayer){
				tmpAIPlayer = 1;
			}
			else{
				tmpAIPlayer = 0;
			}
		//	Debug.Log("tmpListGamePlayers[i].name" + tmpListGamePlayers[i].name);
			tmpListGamePlayers[i].name = tmpListGamePlayers[i].name.Replace("Button","").Trim();
			int insCharID = tmpListGamePlayers[i].GetComponent<selectPlayer>().characterSubmitObject.GetComponent<characterUIInfo>().charID;
	//		Debug.Log(insCharID);
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="INSERT INTO playerInfo (profit,name, PC, playerMoney, charID, positionPlayer, positionMission) VALUES (0, '"+tmpListGamePlayers[i].name +"', '"+tmpAIPlayer+"', 100000, "+insCharID+", '"+startPosMission.name+"','"+endPosMission.name+"')";
			_dbr=_dbcm.ExecuteReader();
			
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="SELECT id FROM `playerInfo`  WHERE `name`='"+tmpListGamePlayers[i].name+"'";
			_dbr=_dbcm.ExecuteReader();
			
			while( _dbr.Read()){
				tmpIdPlayer = _dbr.GetInt32(0);
			}
			
            InstantiatePlayers(tmpIdPlayer, i, tmpListGamePlayers[i].name,startPosMission,endPosMission, tmpAIPlayer);
	
            _dbcm=_dbc.CreateCommand();
			/////////random kaart trekken maken/////////////////////////////
            _dbcm.CommandText="INSERT INTO playerCards (playerID, cardName) VALUES ('"+tmpIdPlayer+"', 'fallingMoney')";
			_dbr=_dbcm.ExecuteReader();
		}
		_dbc.Close();
		
		kickStartGame();		
	}
	public void InstantiatePlayers(int idPlayer, int instantPlayer, string ListPlayerName, GameObject startPosMission, GameObject endPosMission,int AITrueFalse){
			GameObject tmpObject =  Instantiate (playerPrefab) as GameObject;
			playerObjects.Add(tmpObject);
			playerObjects[instantPlayer].GetComponent<playerMove>().playerID = idPlayer;
			playerObjects[instantPlayer].name = playerObjects[instantPlayer].name.Replace("player (Clone)","'"+ListPlayerName+"'").Trim();
			playerObjects[instantPlayer].GetComponent<playerMove>().startPoint = startPosMission;
			playerObjects[instantPlayer].GetComponent<playerMove>().goalPoint = endPosMission;
			playerObjects[instantPlayer].GetComponent<playerMove>().loadGameLook = loadGameLook;
            playerObjects[instantPlayer].GetComponent<playerMove>().playerIsAI = AITrueFalse; 
                   // Debug.Log("object name: "+ loadGameLook.transform.GetChild(6).gameObject.name);
            playerObjects[instantPlayer].GetComponent<playerMove>().endTurnButtonObject = loadGameLook.transform.GetChild(6).gameObject;
            playerObjects[instantPlayer].GetComponent<playerMove>().characterUI = gameManagerObject.GetComponent<gameManager>().spriteCharactersList[instantPlayer];
			playerObjects[instantPlayer].transform.parent = waypointParentObject.transform;
			playerObjects[instantPlayer].transform.localPosition = playerObjects[instantPlayer].GetComponent<playerMove>().startPoint.transform.localPosition;
	}
	public void kickStartGame(){
		gameManagerObject.GetComponent<gameManager>().listActivePlayers = playerObjects;
		gameManagerObject.GetComponent<gameManager>().startGame();	
	}
}