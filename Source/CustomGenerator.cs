using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGenerator : MonoBehaviour
{
    public GameObject basicSolid;

    public DataTransfer data;

    void Start()
    {
        data = FindObjectOfType<DataTransfer>();
    }

    public void Generate(float[][] ddata)
    {
        
        for (int i = 0; i < ddata.Length; i++) //cycle through each object
        {
            GameObject game = Instantiate(basicSolid, new Vector3(ddata[i][1], ddata[i][2], 0f), Quaternion.Euler(0f, 0f, ddata[i][3]));
            
            game.transform.localScale = new Vector3(ddata[i][4], ddata[i][5], 1f);

            MatchToData(game, i, ddata);

            if(i < 2) // deactivate start and end
            {
                game.SetActive(false);
            }
        }

        this.gameObject.SetActive(false);
    }

    public void MatchToData(GameObject g, int index, float[][] ddata)
    {
        //sets type properly
        //applys custom variable
        //sets color
        
        if(g.GetComponent<Rigidbody2D>() != null)
        {
            g.GetComponent<Rigidbody2D>().sharedMaterial = data.typesMaterials[(int)ddata[index][0]];
        }
        g.GetComponent<SpriteRenderer>().color = data.typesColors[(int)ddata[index][0]];
        
        // removes box collider on start and end ones
        if(g.GetComponent<BoxCollider2D>() != null) 
        {
            if(ddata[index][0] == 3 || ddata[index][0] == 4 || ddata[index][0] == 8 || ddata[index][0] == 9)
            {
                g.GetComponent<BoxCollider2D>().isTrigger = true;
            } else {
                g.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
        
        
        
        //spinners
        if(g.GetComponent<Rigidbody2D>() != null)
        {
            if(ddata[index][0] == 5)
            {
                g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            } else {
                g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
        
        if(ddata[index][0] == 5)
        {
            if(g.GetComponent<Spin>() == null)
            {
                g.AddComponent(typeof(Spin));
            }
            g.GetComponent<Spin>().spinSpeed = ddata[index][6];
            if(g.GetComponent<HingeJoint2D>() == null)
            {
                g.AddComponent(typeof(HingeJoint2D));
            }

            //set hinge position relative to THIS
            g.GetComponent<HingeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
        } else {
            if(g.GetComponent<Spin>() != null)
            {
                Destroy(g.GetComponent<Spin>());
            }
            if(g.GetComponent<HingeJoint2D>() != null)
            {
                Destroy(g.GetComponent<HingeJoint2D>());
            }
        }
        g.transform.parent = transform;


        if(ddata[index][0] == 6)
        {
            g.tag = "UpsideDown";
        } else if (ddata[index][0] == 7){
            g.tag = "RightSideUp";
        } else if (ddata[index][0] == 9){
            g.tag = "EndTeleport";
        } else if (ddata[index][0] == 3 || ddata[index][0] == 4){
            g.tag = "Untagged";
        } else {
            g.tag = "Untagged";
        }

        if(ddata[index][0] == 8)
        {
            if(g.GetComponent<Teleport>() == null)
            {
                g.AddComponent(typeof(Teleport));
            }
            if(GameObject.FindWithTag("EndTeleport") != null) // sets end position
            {
                g.GetComponent<Teleport>().end = GameObject.FindWithTag("EndTeleport").transform;
            } else {
                g.GetComponent<Teleport>().end = g.transform;
            }
            
        } else {
            if(g.GetComponent<Teleport>() != null)
            {
                Destroy(g.GetComponent<Teleport>());
            }
        }

        if(ddata[index][0] == 9)
        {
            if(FindObjectsOfType<Teleport>() != null) //refresh start teleporter if needed
            {
                int j = 0;
                foreach (Teleport t in FindObjectsOfType<Teleport>())
                {
                    MakeTeleportReference(FindObjectsOfType<Teleport>()[j].gameObject);
                    j++;
                }
                
            }
        }
    }

    public void MakeTeleportReference(GameObject g)
    {
        if(g.GetComponent<Teleport>() == null)
        {
            g.AddComponent(typeof(Teleport));
        }

        if(GameObject.FindWithTag("EndTeleport") != null) // sets end position
        {
            g.GetComponent<Teleport>().end = GameObject.FindWithTag("EndTeleport").transform;
        } else {
            g.GetComponent<Teleport>().end = g.transform;
        }
    }
}
