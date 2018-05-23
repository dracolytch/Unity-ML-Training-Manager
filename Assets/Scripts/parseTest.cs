using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class parseTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var buffer = "{\"name\":\"k\",\"data\":[[1526348483.9139702, 2500, 2.061878204345703], [1526348511.2569523, 5000, 2.1722617149353027], [1526348528.1978269, 7500, 1.9611366987228394], [1526348553.8797052, 10000, 2.598255157470703], [1526348579.890926, 12500, 2.935533046722412], [1526348596.9141357, 15000, 6.694854259490967], [1526348622.9030383, 17500, 5.635344505310059], [1526348640.2471588, 20000, 5.800227642059326]]}";
        var runData = JsonConvert.DeserializeObject<RunData>(buffer);
        Debug.Log(runData.name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
