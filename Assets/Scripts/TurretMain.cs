using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretMain : MonoBehaviour, IDamageable {

    public int HP;
    public Text displayText;

    // Use this for initialization
    void Start()
    {
        UpdateDisplayText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        displayText.text = "Turret HP: " + HP;
    }
}
