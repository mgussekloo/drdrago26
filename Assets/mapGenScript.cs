using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;
public class mapGenScript : MonoBehaviour {

	public GameObject wayPointsMap;
	
	public List<GameObject> donePoints;
	public GameObject[] roads;
	private GameObject tmpRoad;
	// Use this for initialization
	void Start () {
		
		for(int i = 0; i < wayPointsMap.transform.childCount; i++){
				
				if(wayPointsMap.transform.GetChild(i).GetComponent<connectedLinks>().leftNeighbour != null){
					//Debug.Log("create road to left" + wayPointsMap[i].GetComponent<connectedLinks>().leftNeighbour.transform.position.x);
					float tmpDeltaDistance = Mathf.Abs( wayPointsMap.transform.GetChild(i).GetComponent<connectedLinks>().leftNeighbour.transform.position.x - wayPointsMap.transform.GetChild(i).transform.position.x);
					tmpDeltaDistance = (tmpDeltaDistance / 0.3f) - 1;
					//Debug.Log(tmpDeltaDistance);
					float tmpPointx = wayPointsMap.transform.GetChild(i).transform.position.x - 0.3f;
					for(int j=0; j<tmpDeltaDistance; j++){
						tmpRoad = Instantiate (roads[0]) as GameObject;
						tmpRoad.transform.parent = gameObject.transform;
					//Debug.Log(wayPointsMap[i].transform.position.x );
						tmpRoad.transform.position = new Vector3 (tmpPointx,wayPointsMap.transform.GetChild(i).transform.position.y  , 0);
						tmpPointx = tmpPointx-0.3f;
					}
					
				}
				//if(wayPointsMap[i].GetComponent<connectedLinks>().rightNeighbour != null){
				//		Debug.Log("create road to right");
				//}
				//if(wayPointsMap[i].GetComponent<connectedLinks>().downNeighbour != null){
				//		Debug.Log("create road to down");	
				//}
				if(wayPointsMap.transform.GetChild(i).GetComponent<connectedLinks>().upNeighbour != null){
						
					//	Debug.Log("create road to left" + wayPointsMap[i].GetComponent<connectedLinks>().upNeighbour.transform.position.y);
					float tmpDeltaDistance = Mathf.Abs( wayPointsMap.transform.GetChild(i).GetComponent<connectedLinks>().upNeighbour.transform.position.y - wayPointsMap.transform.GetChild(i).transform.position.y);
					tmpDeltaDistance = (tmpDeltaDistance / 0.2f) - 1;
					//Debug.Log(tmpDeltaDistance);
					float tmpPointy = wayPointsMap.transform.GetChild(i).transform.position.y + 0.2f;
					for(int j=0; j<tmpDeltaDistance; j++){
						tmpRoad = Instantiate (roads[1]) as GameObject;
						tmpRoad.transform.parent = gameObject.transform;
					//Debug.Log(wayPointsMap[i].transform.position.x );
						tmpRoad.transform.position = new Vector3 (wayPointsMap.transform.GetChild(i).transform.position.x,tmpPointy  , 0);
						tmpPointy = tmpPointy + 0.2f;
					}
						
				}
		}
		
	}
	
}
