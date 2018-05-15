using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float StartingHP;
    public Text displayText;

    // TODO: Maybe move this to a more global class
    public DeathEvent OnDeath;

    [Serializable]
    public class DeathEvent : UnityEvent<GameObject> { };

    internal float CurrentHP;
    private float respawnTime = 5;
    private float timer;

	// Use this for initialization
	void Start () {
        CurrentHP = StartingHP;
        updateDisplayText();
    }
	
	// Update is called once per frame
	void Update () {

        if (timer > 0)
        {
            timer = Mathf.Max(0, timer - Time.deltaTime);
            if (timer == 0)
            {
                respawn();
            }
            updateDisplayText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            float damage = collision.gameObject.GetComponent<BulletDamage>().damage;
            CurrentHP -= damage;
            updateDisplayText();
            Destroy(collision.gameObject);

            if(CurrentHP <= 0 && timer == 0)
            {
                kill();
                timer = respawnTime;
            }
        }
    }

    private void updateDisplayText()
    {
        if(CurrentHP > 0)
        {
            displayText.text = "Player Health: " + CurrentHP;
        }
        else
        {
            displayText.text = string.Format("Respawn time: {0:0.00}", timer);
        }

    }

    private void kill()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        OnDeath.Invoke(this.gameObject);
    }

    private void respawn()
    {
        this.transform.SetPositionAndRotation(new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
        CurrentHP = StartingHP;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
