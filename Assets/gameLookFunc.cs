using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

public class gameLookFunc : MonoBehaviour {

	public GameObject playerObject;
	public Sprite[] numbersToLoad;
	public Sprite[] charFaces;
	public Sprite[] spriteWaypoints;
	private char[] keyword ;
    public int stepsTillGoal;

	public void showCurrentPointGameLook(GameObject currentWayPoint){
		if(currentWayPoint.tag == "wayPointBlue"){
			this.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite =  spriteWaypoints[0];
			this.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text =  "Win";
		}
		else if(currentWayPoint.tag == "wayPointRed"){
			this.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite =  spriteWaypoints[4];
			this.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text =  "Lose";
		}
		else if(currentWayPoint.tag == "wayPointGray"){
			this.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite =  spriteWaypoints[2];
			this.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text =  "City";
		}
		else if(currentWayPoint.tag == "wayPointYellow"){
			this.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite =  spriteWaypoints[5];
			this.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text =  "Get card";
		}
		else if(currentWayPoint.tag == "wayPointPurple"){
			this.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite =  spriteWaypoints[3];
			this.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text =  "Buy card";
		}
		else if(currentWayPoint.tag == "wayPointCapital"){
			this.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>().sprite =  spriteWaypoints[1];
			this.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text =  "Capital";
		}
	}
	
	public void setStepsTillGoal(int steps){
        stepsTillGoal = steps;
		//Debug.Log("hjfjkadshflkasdhflkas" + playerObject.GetComponent<pathFinding>().amountSteps);
		string tmpInt = steps.ToString();
		//string[] arr = new string [tmpInt.Length];
		//Debug.Log("string lenght" + tmpInt.Length);
		keyword = tmpInt.ToCharArray();
		//Debug.Log(" keyword " + keyword[0]);
		generateStepsShow(1);//childNumber
	}

    public bool checkMissionFinished(){
        if(stepsTillGoal == 0){
            return true;
        }
        else{
            return false;
        }
    }
	
	public void setPlayerFace(int charImage, string charName){
			this.transform.GetChild(3).GetComponent<Image>().sprite =  charFaces[charImage];
			this.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = charName;
		//	Helmut Pohl
	//	Luigi Maserotti
	//	Jane Blonda
	//	vera cruise
	//	Mikro Mawasaki
	//	James Blond
	//	zora meander
	//	Armino Gesserti
	}
	
	private void generateStepsShow(int childNumber){
		
		if (keyword.Length == 1){
			
		//	Debug.Log("you have to load the child " + childNumber);
			this.transform.GetChild(childNumber).transform.GetChild(0).GetComponent<Image>().sprite = numbersToLoad[0];
			int tmpInteger =  keyword[0];
			tmpInteger -=48;
			this.transform.GetChild(childNumber).transform.GetChild(1).GetComponent<Image>().sprite = numbersToLoad[tmpInteger] ;
		}
		else{
			for(int i=0; i<keyword.Length;i++){	
				int tmpInteger =  keyword[i];
				tmpInteger -=48;
				this.transform.GetChild(childNumber).transform.GetChild(i).GetComponent<Image>().sprite = numbersToLoad[tmpInteger];
			}	
		}
	}
	
	public void setThrownDices(int steps){
		string tmpInt = steps.ToString();	
		keyword = tmpInt.ToCharArray();
		generateStepsShow(2);		
	}	

    public void setMissionName(string missionName){
        this.transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().text = missionName;
    }
}