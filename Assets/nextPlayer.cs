using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;
public class nextPlayer : MonoBehaviour {
	
	public GameObject rollDiceOject;
	public GameObject gameManager;
    public GameObject cardsDeckObject;
	//private List<Transform> tmpRealEstate = new List<Transform>();
		
	public void onClickEnter(){
        gameObject.GetComponent<Button>().interactable = true;

        gameObject.SetActive(false);
        //rollDiceOject.GetComponent<rollDice>().startAIPlayers();
        if (gameManager.GetComponent<gameManager>().missionFinishedStatus)
        {
            gameManager.GetComponent<gameManager>().genMission();
            gameManager.GetComponent<gameManager>().setNewMissionPlayers();
            gameManager.GetComponent<gameManager>().loadGameLook.GetComponent<gameLookFunc>().playerObject.transform.GetComponent<playerMove>().showEndpointPopup();
        }
        else
        {
            gameManager.GetComponent<gameManager>().activateNexPlayer();
        }
	//	rollDiceOject.SetActive(true);	
	}
	
	public void onClickDisableParent(){
		transform.parent.GetComponent<buySellScreenControl>().destroyChildEstates();
		transform.parent.gameObject.SetActive(false);
		gameManager.GetComponent<gameManager>().activateNexPlayer();
	//	rollDiceOject.SetActive(true);
		
	}
	
	public void onClickafterShowMonth(){
		
		transform.parent.GetComponent<showMonthOverviewStats>().destroyChildPlayerStats();
		transform.parent.gameObject.SetActive(false);
		//rollDiceOject.GetComponent<rollDice>().startAIPlayers();
		gameManager.GetComponent<gameManager>().loadNextPlayerThings();
		
	//	rollDiceOject.SetActive(true);	
	}
	
	public void onClickDisableParentAlone(){
	
		transform.parent.gameObject.SetActive(false);
		gameManager.GetComponent<gameManager>().activateNexPlayer();
	//	rollDiceOject.SetActive(true);
	}

    public void disableCardParentObject()
    {

        //move all the cards back
        transform.parent.gameObject.SetActive(false);
        //transform.GetComponent<Button>().interactable = false;
        cardsDeckObject.SetActive(true);
        cardsDeckObject.transform.GetComponent<Button>().interactable = true;
        int countOffChilds = transform.parent.childCount;

        int x = 5;
        int y = 5;
        if (countOffChilds > 0)
        {
            for (int i = 1; i < countOffChilds; i++)
            {
                transform.parent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
              //  Debug.Log("transform.parent.GetChild(i).localPosition" + transform.parent.GetChild(i).localPosition);
                transform.parent.GetChild(i).parent = cardsDeckObject.transform;
                int childToChange = i - 1;
               // Debug.Log("transform.parent.GetChild(i).localPosition afterrrrr" + transform.parent.GetChild(childToChange).parent.localPosition);
                cardsDeckObject.transform.GetChild(childToChange).localPosition = new Vector3(x, y, 0);
                x = x - 5;
                y = y - 5;

                cardsDeckObject.transform.GetChild(childToChange).GetComponent<Image>().raycastTarget = false;
            }
        }
    }
}
