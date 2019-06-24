using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener(GameEvent.DESTROY_KILLERS, OnKillObject);
    }

    void OnKillObject()
    {
        try
        {
        Destroy(gameObject);
        }
        catch(MissingReferenceException mre) {}
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
