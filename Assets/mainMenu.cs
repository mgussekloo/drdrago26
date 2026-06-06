using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class mainMenu : MonoBehaviour {

	public GameObject newGameFrame;
	public GameObject allWayPoints;
	public GameObject gameManagerObject;
	public GameObject charactersObjects;
	public GameObject characterSubmitObject;
	public GameObject instantiatePlayers;
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int charIDs;
    private int playerID;
    private int AI_Player;
    private string startPlMission;
	private string endPlMission;
	private string playerName;
	private GameObject startPosPLMission;
	private GameObject endPosPLMission;
	
	// Use this for initialization
	public void onClickNewGame () {
		transform.parent.gameObject.SetActive(false);
		newGameFrame.SetActive(true);
	}
	public void onClickLoadGame () {
		
        string _constr = "";
        if (Application.isEditor){
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else{
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;

        }
       // string _constr="URI=file:"+ Application.persistentDataPath +"/drdragoDB.db";
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		
	
		transform.parent.gameObject.SetActive(false);
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="SELECT id, name, PC, charID, positionPlayer, positionMission FROM `playerInfo`";
		_dbr=_dbcm.ExecuteReader();
		int amountOfChilds = charactersObjects.transform.childCount;    
        int amountOfPlayers = 0;
        while ( _dbr.Read()){
            playerID = _dbr.GetInt32(0);
            playerName = _dbr.GetString(1);
            AI_Player = _dbr.GetInt32(2);
            charIDs = _dbr.GetInt32(3);
            startPlMission = _dbr.GetString(4);
            endPlMission = _dbr.GetString(5);

            for(int x = 0; x<amountOfChilds; x++){
                int testID = charactersObjects.transform.GetChild(x).GetComponent<characterUIInfo>().charID;
                if (charIDs == testID){
                    //load the saved game at correct position
                    gameManagerObject.GetComponent<gameManager>().loadGameCharacters(charactersObjects.transform.GetChild(x).gameObject);
                    int amountOfChilds2 = allWayPoints.transform.childCount;

                    for(int i = 0; i< amountOfChilds2; i++){
                        int amountOfWaypoints = allWayPoints.transform.GetChild(i).transform.childCount;

                        for(int z = 0; z< amountOfWaypoints; z++){
                            if(startPlMission == allWayPoints.transform.GetChild(i).GetChild(z).transform.name){
                                startPosPLMission = allWayPoints.transform.GetChild(i).GetChild(z).transform.gameObject;
                            }
                            else if(endPlMission == allWayPoints.transform.GetChild(i).GetChild(z).transform.name){
                                endPosPLMission = allWayPoints.transform.GetChild(i).GetChild(z).transform.gameObject;	
                            }
                            else if(startPosPLMission!=null && endPosPLMission!=null){
                                break;
                            }
                        }
                        if(startPosPLMission!=null && endPosPLMission!=null){
                            instantiatePlayers.GetComponent<submitSelectedPlayer>().InstantiatePlayers(playerID, amountOfPlayers, playerName, startPosPLMission, endPosPLMission, AI_Player);
                            amountOfPlayers = amountOfPlayers +1;
                            break;
                        }
                     }
                 }
             }
         }
         instantiatePlayers.GetComponent<submitSelectedPlayer>().kickStartGame();    
         startPosPLMission = null;
         endPosPLMission = null; 	
	}
}