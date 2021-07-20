using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    Vector3 screenPoint; private Vector3 offset;

    CreateMaster cm;
    public bool selected;

    bool rotating = false;
    bool scaling = false;
    public GameObject outlinePrefab;
    GameObject outline;

    Vector3 scaleStart;

    void Start()
    {
        screenPoint = Vector3.zero;
        offset = Vector3.zero;
        outline = Instantiate(outlinePrefab, transform.position, transform.rotation);
        outline.SetActive(false);
        cm = FindObjectOfType<CreateMaster>();
    }

    void Update()
    {
        if(cm.paused)
        {
            return;
        }
        if(Input.GetKeyDown("r") && selected)
        {
            if(rotating)
            {
                rotating = false;
            } else {
                rotating = true;
                
            }
        }

        if(Input.GetKeyDown("t") && selected)
        {
            if(scaling)
            {
                scaling = false;
            } else {
                scaling = true;
                scaleStart = transform.localScale;
                
            }
        }

        if(Input.GetMouseButtonDown(0) && (scaling || rotating)){
            scaling = false;
            rotating = false;
        }

        if((Input.GetKeyDown("x") || Input.GetKeyDown(KeyCode.Delete)) && selected)// Delete
        {
            if(gameObject.tag == "EndPoint")
            {
                return;  //this parts makes it so you can't delete start and end
            }
            int i = cm.gameObjects.IndexOf(this.gameObject);
            cm.gameObjects.Remove(this.gameObject);
            cm.parts.RemoveAt(i);
            Destroy(outline);
            Destroy(this.gameObject);
        }

        if(Input.GetKeyDown("d") && selected) //duplicate
        {
            if(gameObject.tag == "EndPoint")
            {
                return;  //this parts makes it so you can't duplicate start and end
            }
            int i = cm.gameObjects.IndexOf(this.gameObject);
            cm.CreateObject();
            int index = cm.gameObjects.Count - 1;

            //deep copying
            cm.parts[index].type = cm.parts[i].type;
            cm.parts[index].x = cm.parts[i].x;
            cm.parts[index].y = cm.parts[i].y;
            cm.parts[index].rotation = cm.parts[i].rotation;
            cm.parts[index].xScale = cm.parts[i].xScale;
            cm.parts[index].yScale = cm.parts[i].yScale;
            cm.parts[index].custom = cm.parts[i].custom;

            cm.UpdatePart(index);
        }

        if(rotating)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //store rotation
            int i = cm.gameObjects.IndexOf(this.gameObject);
            cm.parts[i].rotation = Mathf.Round(transform.rotation.eulerAngles.z * 100f) /100f;

            cm.InspectorUpdate();
        }

        if(scaling)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float bias = transform.rotation.eulerAngles.z;
            if(bias > 180f){
                bias -= 180f;
            }

            if((Mathf.Abs(angle - bias) < 135f && Mathf.Abs(angle - bias) > 45f))
            {
                //scale y
                transform.localScale = Vector3.Scale(new Vector3(1f, dir.magnitude, 1f), scaleStart);
            } else {
                //scale x
                transform.localScale = Vector3.Scale(new Vector3(dir.magnitude, 1f, 1f), scaleStart);
            }
            

            //store rotation
            int i = cm.gameObjects.IndexOf(this.gameObject);
            cm.parts[i].xScale = Mathf.Round(transform.localScale.x * 100f) /100f;
            cm.parts[i].yScale = Mathf.Round(transform.localScale.y * 100f) /100f;

            cm.InspectorUpdate();
        }

        if(selected)
        {
            outline.SetActive(true);
            outline.transform.position = transform.position;
            outline.transform.localScale = transform.localScale;
            outline.transform.localScale += new Vector3(0.2f, 0.2f, 0f);
            outline.transform.rotation = transform.rotation;
        } else {
            outline.SetActive(false);
        }
    }

    //draggable stuff
    void OnMouseDown()
    {
        if(cm.paused){
            return;
        }
        Select();
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset =  transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    }

    void OnMouseDrag()
    {
        if(cm.paused){
            return;
        }
        //cm.isDragging = true;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)) + offset;
        transform.position = new Vector3(Mathf.Round(curPosition.x * 100f) /100f, Mathf.Round(curPosition.y * 100f) /100f, 0f);

        //set new coordinates
        int i = cm.gameObjects.IndexOf(this.gameObject);
        cm.parts[i].x = transform.position.x;
        cm.parts[i].y = transform.position.y;

        cm.InspectorUpdate();
    }

    void LateUpdate(){
        cm.isDragging = false;
    }

    public void Select()
    {
        if(cm.selectedPart != null && cm.selectedPart != this)
        {
            cm.selectedPart.Deselect();
        }

        cm.selectedPart = this;
        selected = true;
        cm.InspectorUpdate();
        cm.InspectorChange();
    }

    public void Deselect()
    {
        cm.selectedPart = null;
        selected = false;
        rotating = false;
        scaling = false;
    }











    void OnDestroy()
    {
        Destroy(outline);
    }
}
