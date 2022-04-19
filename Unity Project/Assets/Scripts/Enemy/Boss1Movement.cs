using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Movement : EnemyMovement
{
    public enum states {
        Hundred,
        Seventy,
        Fifty,
        Twenty
    }

    private EnemyHealth enemHealth;
    private states currentState;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.enemHealth = gameObject.GetComponent<EnemyHealth>();
        currentState = states.Hundred;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch(currentState) {
            case states.Hundred:
                this.moveSpeed = 4f;
                if(enemHealth.getCurrentHealth() / enemHealth.startingHealth <= 0.7f)
                    currentState = states.Seventy;
                break;
            case states.Seventy:
                this.moveSpeed = 6f;
                if(enemHealth.getCurrentHealth() / enemHealth.startingHealth <= 0.5f)
                    currentState = states.Fifty;
                break;
            case states.Fifty:
                this.moveSpeed = 8f;
                if(enemHealth.getCurrentHealth() / enemHealth.startingHealth <= 0.2f)
                    currentState = states.Twenty;
                break;
            case states.Twenty:
                this.moveSpeed = 10f;
                break;
        }
        //Debug.Log("state: " + currentState);
    }
}
