using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class cardProperties : MonoBehaviour {

	public int price;
	public GameObject playerObject;
    public string typeCard;
    public int moveStepsValue;
    public int discountEstate;
    public int dbCardID;
    public List<GameObject> estateLists = new List<GameObject>();
    // public int amountMoves = 0;
    private GameObject confirmScreen;

    private IDbConnection _dbc;
    private IDbCommand _dbcm;
    private IDataReader _dbr;

    // Use this for initialization
    public void enterBuyCard(){
        Debug.Log("fsfsdf");
		 confirmScreen = this.transform.parent.Find("confirmBuyCard").gameObject ;
		confirmScreen.GetComponent<confirmBuyCard>().buyCard(price,gameObject.name, playerObject);
		confirmScreen.SetActive(true);
		confirmScreen.transform.SetAsLastSibling();
		
		if(playerObject.GetComponent<playerMove>().playerIsAI == 1)
        {
            StartCoroutine(showAIActionsCoroutine());
        }
	}
	
	 IEnumerator showAIActionsCoroutine(){
			yield return new WaitForSeconds(0.4f);
			confirmScreen.GetComponent<confirmBuyCard>().enterConfirmBuyCard();
			yield return new WaitForSeconds(0.2f);
			GameObject returnAI = this.transform.parent.Find("return").gameObject ;
			returnAI.GetComponent<nextPlayer>().onClickDisableParentAlone();
    }
   

public void executeCardAction()
    {
        Debug.Log("execute card");
        if (typeCard == "christmas")
        {
            Debug.Log("sprite name");
        }
        else if(typeCard == "discountEstate")
        {
            //discount in buying estates with 25%
            discountEstateCard(discountEstate);
        }
        else if (typeCard == "flight")
        {
            //allows you to fly to or closer to your destination
        }
        else if(typeCard == "gamble")
        {
            //chance that the player can multiply the points gained this turn
        }
        else if (typeCard == "insurance")
        {
            //player is automatically insured against natural disasters
        }
        else if (typeCard == "move")
        {
            Debug.Log("move " + moveStepsValue + " steps extra");
            int tmpPlayerSteps = playerObject.GetComponent<playerMove>().rolledDiceStepsLeft + moveStepsValue;
            playerObject.GetComponent<playerMove>().rolledDiceStepsLeft = tmpPlayerSteps;
            playerObject.GetComponent<playerMove>().loadGameLook.GetComponent<gameLookFunc>().setThrownDices(tmpPlayerSteps);

            updateDBAndDestroy();

        }
        else if (typeCard == "nails")
        {
            // throw nails on the road and block the others behind you from passing
        }
        else if(typeCard == "nosteering")
        {
            //select another player to miss a turn
        }
        else if (typeCard == "slot")
        {

        }

    }
    private void discountEstateCard(int amountDiscount)
    {
        //Debug.Log("give discount to estate");
        int tmpListOfEstates = estateLists.Count;
        for (int i = 0; i < tmpListOfEstates; i++)
        {
          //  Debug.Log("estateLists[i].GetComponent<buySellRealEstate>().priceProperty" + estateLists[i].GetComponent<buySellRealEstate>().priceProperty);
          //  Debug.Log("amountDiscount" + amountDiscount);
            float discountValue = (100 - (float)amountDiscount) / 100;
          //  Debug.Log(discountValue + "discountValue");
        float tmpDiscountEstate = (float)estateLists[i].GetComponent<buySellRealEstate>().priceProperty * discountValue;
           // Debug.Log("tmpDiscount" + tmpDiscountEstate);
        estateLists[i].GetComponent<buySellRealEstate>().priceProperty = (int)tmpDiscountEstate;
            estateLists[i].GetComponent<buySellRealEstate>().updatePriceProperty();
        }
        updateDBAndDestroy();
        transform.parent.gameObject.SetActive(false);


    }

    public void updateDBAndDestroy()
    {

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
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();

        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "DELETE FROM playerCards WHERE id = " + dbCardID;

        _dbr = _dbcm.ExecuteReader();

        _dbc.Close();
        Destroy(this.gameObject);
    }
}

