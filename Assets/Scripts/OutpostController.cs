using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutpostController : MonoBehaviour {

    // Outpost can be charged up by the player simply being there.
    // Charging rewards currently is building turrets at the four corners of the outpost.  Afterwards, the outpost is autonomous until turret is destroyed.

    public float chargingRate = 1;
    public int chargingRewards = 4;
    
    public GameObject turret;
    public Text displayText;

    private float maxOutpostPower;
    private float outpostPower;
    private float chargingRewardInterval;
    private bool chargingThresholdCrossed;

    private GameObject[] turrets;

    // Use this for initialization
    void Start () {
        outpostPower = 0;
        maxOutpostPower = 100;
        chargingRewardInterval = maxOutpostPower / chargingRewards;
        turrets = new GameObject[4];
	}
	
	// Update is called once per frame
	void Update () {
        bool heroInOutpost = GetComponentInChildren<GameObjectTracker>().HasTargetInRange();
        if (heroInOutpost)
        {
            chargingThresholdCrossed = Mathf.Repeat(outpostPower, chargingRewardInterval) > Mathf.Repeat(outpostPower + chargingRate, chargingRewardInterval);
            if (chargingThresholdCrossed)
            {
                GiveChargingReward();
            }
            outpostPower = Mathf.Min(outpostPower + chargingRate, maxOutpostPower);

            displayText.text = string.Format("Outpost Power: {0:P2}", outpostPower/100);
        }

	}

    public void TakeDamage(int damageAmount)
    {
        // Currently the outposts are indestructable.
    }


    // Currently builds turrets when charging
    void GiveChargingReward()
    {
        AddTurret();
    }

    void AddTurret()
    {
        for(int i = 0; i < turrets.Length; ++i)
        {
            if(turrets[i] == null)
            {
                float turretX = 7 * Mathf.Pow(-1, i) + this.transform.position.x; 
                float turretZ = 7 * (Mathf.Floor(i / 2)*2 -1) + this.transform.position.z;
                GameObject newTurret = Instantiate(turret, new Vector3(turretX, 11, turretZ), Quaternion.Euler(new Vector3(0, 0, 0)));
                newTurret.transform.Find("Turret Base").tag = "Hero Team";
                newTurret.transform.Find("Turret Head").tag = "Hero Team";
                newTurret.GetComponentInChildren<GameObjectTracker>().TrackingTag = "Bad Guy Team";
                turrets[i] = newTurret;
                break;
            }
        }
    }

    void DoAutonomousStuff()
    {

    }
}
