using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

public class pathFinding : MonoBehaviour {

	List<zoekObject> cameFrom = new List<zoekObject>();
	public List<GameObject> totalPath = new List<GameObject>();
	private int currentCost;
	private	int neighbourCost;
	
	public class zoekObject{
		public GameObject waypoint;
		public GameObject parentWaypoint;
		public int cost;
	}
	
	public void clearAllPathFindingInfo(){
		cameFrom.Clear();
		totalPath.Clear();
	}
	
	public List<GameObject> correctPathToFind(GameObject startPoint, GameObject endPoint){
        clearAllPathFindingInfo();

        List<GameObject> open = new List<GameObject>();
		List<GameObject> closed = new List<GameObject>();
		int safety=0;
		
		open.Add(startPoint);
		zoekObject mijnZoekObject ;
		mijnZoekObject = new zoekObject();
		mijnZoekObject.waypoint = startPoint;
		mijnZoekObject.cost = 0;
		cameFrom.Add(mijnZoekObject);
		
		while (open.Count > 0 ) {
			safety = safety + 1;
			if (safety > 999){
				Debug.Log("safety reached no pathfinding working");
				break;
			}
			GameObject current = open[0];
		//	Debug.Log(current.name + " current");
			if (endPoint.transform.position.x == current.transform.position.x && endPoint.transform.position.y == current.transform.position.y){
				return makePath(current);
			}
			open.Remove(current);
			closed.Add(current);
			
			List<GameObject> localNeighbours = new List<GameObject>();

            localNeighbours.Add(current.GetComponent < connectedLinks > ().leftNeighbour);
			localNeighbours.Add(current.GetComponent < connectedLinks > ().rightNeighbour);
            localNeighbours.Add(current.GetComponent < connectedLinks > ().upNeighbour);
            localNeighbours.Add(current.GetComponent<connectedLinks>().downNeighbour);

            for (int i = 0; i < localNeighbours.Count; i++) {
				
            // als deze neighbor er helemaal niet is, gaan we door.
				if (localNeighbours[i] == null) {
					continue;
				}
				// zit de neighbor in closed? dan gaan we door met de volgende neighbor. 
				if (closed.Contains(localNeighbours[i])  ) {
					continue;
				}
				// als deze neighbor nog niet in de closed zit, dan moeten we kijken
				// of de neighbor al in de open zit. als dat niet zo is, voegen we hem toe.
				if (open.IndexOf(localNeighbours[i]) < 0 ) {
					open.Add(localNeighbours[i]);	
				}
				// we moeten nog onthouden hoe we hier gekomen zijn.
				// als we nog nooit op deze neighbor zijn geweest
				// is dit altijd de goedkoopste weg naar deze neighbor, prima dus
				//Debug.Log("costcount " + cost.Count);
				bool beenByNeighbour = false;
				for(int j =0; j< cameFrom.Count; j++){
					if (cameFrom[j].waypoint == localNeighbours[i]){
						beenByNeighbour = true;
					}
					else {
						beenByNeighbour = false;
					}
				}
				if (!beenByNeighbour) {
					//zoek mijn current cost en tel er 1 bij op
					for(int x =0; x< cameFrom.Count; x++){
						if(cameFrom[x].waypoint == current){
							mijnZoekObject = new zoekObject();
							mijnZoekObject.waypoint = localNeighbours[i];
							mijnZoekObject.parentWaypoint = current;
							mijnZoekObject.cost = cameFrom[x].cost +  1;
							cameFrom.Add(mijnZoekObject);
							break;
						}
					}				
					continue;
				}
				// maar zo niet, dan slaan we dit pad alleen op als hij efficienter is
				for(int jj =0; jj< cameFrom.Count; jj++){
					if(cameFrom[jj].waypoint == current){
						 currentCost = cameFrom[jj].cost +  1;
					}
					if(cameFrom[jj].waypoint == localNeighbours[i]){
						neighbourCost = cameFrom[jj].cost;
					}
				}
				if (currentCost < neighbourCost) {
					for(int jjj =0; jjj < cameFrom.Count; jjj++){
						if(cameFrom[jjj].waypoint == localNeighbours[i]){
							cameFrom[jjj].cost = currentCost;
							cameFrom[jjj].waypoint = localNeighbours[i];
							cameFrom[jjj].parentWaypoint = current;
							break;
						}
					}
					continue;
				}
			}
		}
		return new List<GameObject>();	
	}
	public List<GameObject> makePath( GameObject objCurrent){
		totalPath.Clear();
		//totalPath.Add(objCurrent);	
		while (objCurrent != null){	
			for(int jjjj =0; jjjj< cameFrom.Count; jjjj++){
				if(cameFrom[jjjj].waypoint == objCurrent){
					objCurrent = cameFrom[jjjj].parentWaypoint;
					totalPath.Add(objCurrent);
				}
			}
		}
		return totalPath;
	}
}