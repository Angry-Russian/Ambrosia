using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallToggler : MonoBehaviour
{

	public GameObject wallS; // south wall
	public GameObject wallW; // west wall

	public void setState(bool wallSEnabled, bool wallWEnabled)
	{
		if(wallS != null)
			wallS.SetActive(wallSEnabled);
		if(wallW != null)
			wallW.SetActive(wallWEnabled);
			
	}
}
