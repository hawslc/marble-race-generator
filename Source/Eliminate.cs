using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eliminate : MonoBehaviour
{
    public GameObject destroy;
    
    //destroy objects on collision

    void OnCollisionEnter2D(Collision2D c){
        
        if(c.gameObject.tag == "Marble"){
            if(destroy != null){
                Destroy(destroy);
            }
        }
    }
}
