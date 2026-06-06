using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class endTurnButton : MonoBehaviour {

	// Use this for initialization
	public GameObject playerObject;
	public GameObject gameManagerObject;
	
	private int idPlayer;
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;

    private GameObject endturnScreen;
    // Update is called once per frame
    public void onClickEnter()
    {

        playerObject = this.transform.parent.GetComponent<gameLookFunc>().playerObject;
        this.gameObject.SetActive(false);
        playerObject.GetComponent<playerMove>().tmpPathWalked.Clear();
        //	Debug.Log("add startpoint" + playerObject.GetComponent<playerMove>().startPoint);
        playerObject.GetComponent<playerMove>().tmpPathWalked.Add(playerObject.GetComponent<playerMove>().startPoint);

        string tmpPosName = playerObject.GetComponent<playerMove>().startPoint.name;
        //	Debug.Log("playerID" +tmpPosName);
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
        _dbcm.CommandText = "UPDATE playerInfo SET positionPlayer ='" + tmpPosName + "' WHERE id='" + playerObject.GetComponent<playerMove>().playerID + "'";
        _dbr = _dbcm.ExecuteReader();
        _dbc.Close();

        if (playerObject.GetComponent<playerMove>().loadGameLook.GetComponent<gameLookFunc>().checkMissionFinished())
        {
            gameManagerObject.GetComponent<gameManager>().missionFinished(playerObject);
        }
        else
        {

            // playerObject.GetComponent<playerMove>().checkEndpointTag();

            

            if (playerObject.transform.GetChild(1).gameObject.active==true)
                {
                     playerObject.GetComponent<playerMove>().showEndpointPopupWithDrago();

                 } else
                 {
                     playerObject.GetComponent<playerMove>().showEndpointPopup();
                    

            }
        }
    }
}