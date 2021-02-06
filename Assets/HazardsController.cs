using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardsController : MonoBehaviour, IDamageDealer
{
    [SerializeField] private float damageOnContact = 1000f;
    public float GiveDamage()
    {
        return damageOnContact;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
