using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CreateMaster : MonoBehaviour
{
    //Main variables
    public List<Object> parts = new List<Object>();
    public List<GameObject> gameObjects = new List<GameObject>();


    //Other Stuff
    public GameObject basicSolid;
    public Part selectedPart;
    public GameObject start;
    public GameObject end;

    public GameObject inspector;
    public TMP_InputField xPosition;
    public TMP_InputField yPosition;
    public TMP_InputField rotation;
    public TMP_InputField xScale;
    public TMP_InputField yScale;
    public TMP_InputField custom;
    public TMP_Dropdown objectType;

    public List<Color> typesColors = new List<Color>();
    public List<PhysicsMaterial2D> typesMaterials = new List<PhysicsMaterial2D>();

    public Object defaultObject = new Object();
    public bool isDragging;

    public List<GameObject> marbles = new List<GameObject>();
    public GameObject pauseUI;
    public GameObject saveUI;
    public TMP_InputField saveInput;
    public TMP_Dropdown loadInput;
    public GameObject loadUI;
    public GameObject spinnerUI;
    public GameObject deleteUI;
    public GameObject importUI;
    public GameObject exportUI;
    public TMP_Dropdown deleteInput;
    public TMP_Dropdown exportInput;
    public TMP_InputField importInput;
    public TMP_InputField exportOutput;

    public bool paused;
    AudioSource sound;


    /*Here are all the types and their respective indexes
    0: solid
    1: bouncy
    2: very bouncy
    3: starts
    4: end
    5: spinner
    6: gravity reverser
    7: gravity unreverser
    8: start teleporter
    9: end teleporter


    */

    void Start()
    {
        sound = GetComponent<AudioSource>();
        Resume();
        saveUI.SetActive(false);
        loadUI.SetActive(false);
        deleteUI.SetActive(false);
        importUI.SetActive(false);
        exportUI.SetActive(false);

        CreateEndpoint(true);
        CreateEndpoint(false);
        CreateObject();

        DataTransfer data = FindObjectOfType<DataTransfer>();
        if(data != null)
        {
            data.typesColors = typesColors;
            data.typesMaterials = typesMaterials;
        }
    }


    
    void Update()
    {
        if(Input.GetKeyDown("c") && exportUI.activeSelf == false && saveUI.activeSelf == false)
        {
            CreateObject();
        }
        

        if(selectedPart == null)
        {
            inspector.SetActive(false);
        } else {
            inspector.SetActive(true);
        }

        if(Input.GetMouseButtonDown(2))
        {
            Instantiate(marbles[Random.Range(0, marbles.Count)], new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f), Quaternion.identity);
        }

        if(Input.GetKeyDown("q") || Input.GetKeyDown(KeyCode.Escape))
        {
            sound.Play();
            if(paused){
                Resume();
            } else {
                Pause();
            }
        }

        if (Input.GetKey("v"))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKey("b"))
        {
            Time.timeScale = 3;
        }
        if (Input.GetKey("n"))
        {
            Time.timeScale = 7;
        }
        if (Input.GetKey("m"))
        {
            Time.timeScale = 20;
        }
    }



    public void CreateObject()
    {
        int index = parts.Count;

        Object ob = new Object();
        //hard copy
        ob.type = defaultObject.type;
        ob.x = defaultObject.x;
        ob.y = defaultObject.y;
        ob.rotation = defaultObject.rotation;
        ob.xScale = defaultObject.xScale;
        ob.yScale = defaultObject.yScale;
        ob.custom = defaultObject.custom;
        
        parts.Add(ob);
        //so the goal here is to make a new GameObject
        gameObjects.Add(Instantiate(basicSolid, new Vector3(parts[index].x, parts[index].y, 0f), Quaternion.Euler(0f, 0f, parts[index].rotation)));
        GameObject g = gameObjects[index];

        g.transform.localScale = new Vector3(parts[index].xScale, parts[index].yScale, 1f);

        MatchToData(g, index);
    }



    public void CreateEndpoint(bool isStart)
    {
        int index = parts.Count;

        Object ob = new Object();
        //hard copy
        ob.x = defaultObject.x;
        ob.y = defaultObject.y;
        ob.rotation = defaultObject.rotation;
        ob.xScale = defaultObject.xScale;
        ob.yScale = defaultObject.yScale;
        ob.custom = defaultObject.custom;
        
        
        parts.Add(ob);
        //so the goal here is to make a new GameObject
        if(isStart)
        {
            gameObjects.Add(Instantiate(start, new Vector3(parts[index].x, parts[index].y, 0f), Quaternion.Euler(0f, 0f, parts[index].rotation)));
            ob.type = 3;
        } else {
            gameObjects.Add(Instantiate(end, new Vector3(parts[index].x, parts[index].y, 0f), Quaternion.Euler(0f, 0f, parts[index].rotation)));
            ob.type = 4;
        }
        
        GameObject g = gameObjects[index];

        g.transform.localScale = new Vector3(parts[index].xScale, parts[index].yScale, 1f);

        if(isStart)
        {
            g.transform.position += new Vector3(-2f, 0, 0);
            ob.x = -2;
        } else {
            g.transform.position += new Vector3(2f, 0, 0);
            ob.x = 2;
        }

        MatchToData(g, index);
    }



    

    
    
    public void InspectorUpdate()
    {
        if(selectedPart == null)
        {
            return;
        }

        //standardize 2nd next line of code to make it more efficent
        isDragging = true;
        int index = gameObjects.IndexOf(selectedPart.gameObject);

        xPosition.text = parts[index].x.ToString();
        yPosition.text = parts[index].y.ToString();
        rotation.text = parts[index].rotation.ToString();
        xScale.text = parts[index].xScale.ToString();
        yScale.text = parts[index].yScale.ToString();
        custom.text = parts[index].custom.ToString();
        objectType.value = parts[index].type;

        isDragging = false;
    }

    public void InspectorChange()
    {
        
        //reads values from inspector and applies them to the object
        if(isDragging)
        {
            return;
        }
        if(selectedPart == null || !gameObjects.Contains(selectedPart.gameObject))
        {
            return;
        }
        int index = gameObjects.IndexOf(selectedPart.gameObject);


        //position
        string text = xPosition.text;
        float num = 0; bool sucess = float.TryParse(text, out num);
        if(sucess) {
            xPosition.text = num.ToString();
            parts[index].x = num;
        }

        text = yPosition.text;
        num = 0; sucess = float.TryParse(text, out num);
        if(sucess) {
            yPosition.text = num.ToString();
            parts[index].y = num;
        }

        //rotation
        text = rotation.text;
        num = 0; sucess = float.TryParse(text, out num);
        if(sucess) {
            rotation.text = num.ToString();
            parts[index].rotation = num;
        }

        //scale
        text = xScale.text;
        num = 0; sucess = float.TryParse(text, out num);
        if(sucess) {
            xScale.text = num.ToString();
            parts[index].xScale = num;
        }

        text = yScale.text;
        num = 0; sucess = float.TryParse(text, out num);
        if(sucess) {
            yScale.text = num.ToString();
            parts[index].yScale = num;
        }
        
        //type  - can't change type OF start or end
        if(parts[index].type != 3 && parts[index].type != 4)
        {
            if(objectType.value != 3 && objectType.value != 4){ // also cant change type TO start or end
                parts[index].type = objectType.value;
            } else {
                objectType.value = parts[index].type;
            }
        } else {
            objectType.value = parts[index].type;
        }
        

        //custom (if applicable)
        text = custom.text;
        num = 0; sucess = float.TryParse(text, out num);
        if(sucess) {
            custom.text = num.ToString();
            parts[index].custom = num;
        }
        
        //now we apply these changes to the part
        UpdatePart(index);
        
        UpdateCustomInspector(index);
        
    }


    public void UpdatePart(int index)
    {
        GameObject p = gameObjects[index];

        p.transform.position = new Vector3(parts[index].x, parts[index].y, 0f);
        p.transform.rotation = Quaternion.Euler(0f, 0f, parts[index].rotation);
        p.transform.localScale = new Vector3(parts[index].xScale, parts[index].yScale, 1f);
        
        MatchToData(p, index);
        
    }

    public void DestroyObject(int index)
    {
        if(parts.Count > index)
        {
            
            parts.RemoveAt(index);
            
            if(gameObjects.Count > index){
                Destroy(gameObjects[index]);
                
                gameObjects.RemoveAt(index);
            }
        }
    }
    
    public void BackgroundClick()
    {
        if(selectedPart != null)
        {
            selectedPart.Deselect();
        }
    }


    public void UpdateCustomInspector(int index)
    {
        if(parts[index].type == 5)
        {
            spinnerUI.SetActive(true);
            custom.gameObject.SetActive(true);
        } else {
            spinnerUI.SetActive(false);
            custom.gameObject.SetActive(false);
        }
    }


    public void MatchToData(GameObject g, int index)
    {
        //sets type properly
        //applys custom variable
        //sets color
        
        if(g.GetComponent<Rigidbody2D>() != null)
        {
            g.GetComponent<Rigidbody2D>().sharedMaterial = typesMaterials[parts[index].type];
        }
        g.GetComponent<SpriteRenderer>().color = typesColors[parts[index].type];
        
        // removes box collider on start and end ones and also the teleporters
        if(g.GetComponent<BoxCollider2D>() != null)
        {
            if(parts[index].type == 3 || parts[index].type == 4 || parts[index].type == 8 || parts[index].type == 9)
            {
                g.GetComponent<BoxCollider2D>().isTrigger = true;
            } else {
                g.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
        
        
        
        //spinners
        if(g.GetComponent<Rigidbody2D>() != null)
        {
            if(parts[index].type == 5)
            {
                g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            } else {
                g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
        
        if(parts[index].type == 5)
        {
            if(g.GetComponent<Spin>() == null)
            {
                g.AddComponent(typeof(Spin));
            }
            g.GetComponent<Spin>().spinSpeed = parts[index].custom;
            if(g.GetComponent<HingeJoint2D>() == null)
            {
                g.AddComponent(typeof(HingeJoint2D));
            }
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
        
        
        //set hinge position relative to first static rigidbody
        if(parts.Count > 1 && parts[index].type == 5)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if(gameObjects[i].GetComponent<Rigidbody2D>() != null)
                {
                    if(gameObjects[i].GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static)
                    {
                        g.GetComponent<HingeJoint2D>().connectedBody = gameObjects[i].GetComponent<Rigidbody2D>();
                        i = gameObjects.Count + 7;
                    }
                }
            }
        }

        //now gravity reversers
        if(parts[index].type == 6)
        {
            g.tag = "UpsideDown";
        } else if (parts[index].type == 7){
            g.tag = "RightSideUp";
        } else if (parts[index].type == 9){
            g.tag = "EndTeleport";
        } else if (parts[index].type == 3 || parts[index].type == 4){
            g.tag = "EndPoint";
        } else {
            g.tag = "Untagged";
        }

        if(parts[index].type == 8)
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

        if(parts[index].type == 9)
        {
            if(FindObjectsOfType<Teleport>() != null) //refresh start teleporter if needed
            {
                int j = 0;
                foreach (Teleport t in FindObjectsOfType<Teleport>())
                {
                    MakeTeleportReference(FindObjectsOfType<Teleport>()[j].gameObject, gameObjects.IndexOf(FindObjectOfType<Teleport>().gameObject));
                    j++;
                }
                
            }
        }
    }

    public void MakeTeleportReference(GameObject g, int index)
    {
        if(parts[index].type == 8)
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
    }
























    //GUI stuff
    public void Exit()
    {
        SceneManager.LoadScene("Main"); 
        sound.Play();
    }

    public void Resume()
    {
        paused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        sound.Play();
    }

    public void Pause()
    {
        paused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0;
        sound.Play();

        if(selectedPart != null){
            selectedPart.Deselect();
        }
    }

    public void SaveButton()
    {
        pauseUI.SetActive(false);
        saveUI.SetActive(true);
        sound.Play();

        saveInput.text = "";
    }

    public void LoadButton()
    {
        pauseUI.SetActive(false);
        loadUI.SetActive(true);
        sound.Play();

        SetupLoadDropdown();
    }

    public void DeleteButton()
    {
        pauseUI.SetActive(false);
        deleteUI.SetActive(true);
        sound.Play();

        SetupDeleteDropdown();
    }

    public void ExportButton()
    {
        pauseUI.SetActive(false);
        exportUI.SetActive(true);
        sound.Play();

        SetupExportDropdown();
    }

    public void ImportButton()
    {
        pauseUI.SetActive(false);
        importUI.SetActive(true);
        sound.Play();

        importInput.text = "";
    }

    public void ExitSave()
    {
        pauseUI.SetActive(true);
        saveUI.SetActive(false);
        loadUI.SetActive(false);
        deleteUI.SetActive(false);
        importUI.SetActive(false);
        exportUI.SetActive(false);
        sound.Play();
    }


    public void ClearAll(){

        DestroyAllObjects();
        CreateEndpoint(true);
        CreateEndpoint(false);
        sound.Play();

        CreateObject();
    }


    public void Export()
    {
        DataTransfer data = FindObjectOfType<DataTransfer>();
            
        sound.Play();
        if(data == null)
        {
            return;
        }

        if(data.tracks.Count <= 0)
        {
            return;
        }

        int index = exportInput.value;
        string export = "Name: " + data.names[index] + " Numbers: ";

        data.TrackConvertBack(index);

        for (int i = 0; i < data.trackTemp.Length; i++)
        {
            export += "{";
            for (int k = 0; k < data.trackTemp[i].Length; k++)
            {
                export += data.trackTemp[i][k].ToString() + ", ";
            }
            export += "}";
        }
        


        TextEditor te = new TextEditor();
        te.text = export;
        te.SelectAll();
        te.Copy();

        //also update output area
        exportOutput.text = export;
    }

    public void Import()
    {
        
        sound.Play();
        DataTransfer data = FindObjectOfType<DataTransfer>();

        if(data == null)
        {
            return;
        }

        string s = importInput.text;
        char[] c = s.ToCharArray();

        if(c.Length < 25)
        {
            ImportError();
            return;
        }

        if(c[0] == 'N' && c[1] == 'a' && c[2] == 'm' && c[3] == 'e' && c[4] == ':'  && c[5] == ' ')
        {
            //cool
        } else {
            ImportError();
            return;
        }

        //so first we get the name
        int numbersStart = 0; // this is the index of the char "N" in numbers
        
        for (int i = 5; i < c.Length - 10; i++)
        {
            if(c[i] == 'N')
            {
                if(c[i + 1] == 'u')
                {
                    if(c[i + 2] == 'm')
                    {
                        numbersStart = i;
                    }
                }
            }
        }
        char[] name = new char[numbersStart - 7];
        for (int i = 0; i < name.Length; i++)
        {
            name[i] = c[i + 6];
        }

        //set name
        data.names.Add(new string(name));

        //get how many objects in array
        int count = 0;
        
        for (int i = 0; i < c.Length; i++)
        {
            if(c[i] == '{')
            {
                count++;
            }
        }

        int[] startBrackets = new int[count]; // list of the position of each starting bracket

        int currentInt = 0;
        for (int i = 0; i < c.Length; i++)
        {
            if(c[i] == '{')
            {
                startBrackets[currentInt] = i;
                currentInt++;
            }
        }

        //now we go through the list and import it
        float[][] nums = new float[count][];

        for (int i = 0; i < count; i++)
        {
            //each object
            int currentPos = startBrackets[i];
            int lastPos = startBrackets[i];

            nums[i] = new float[7];

            for (int j = 0; j < 7; j++)
            {
                for(int k = 0; k < 100; k++)
                {
                    //find start of float
                    int asdf; bool work = int.TryParse(c[currentPos].ToString(), out asdf);
                    if(work || c[currentPos] == '-')
                    {
                        k = 150;
                        lastPos = currentPos;
                    }
                    currentPos++;
                }

                for(int k = 0; k < 100; k++)
                {
                    if(c[currentPos] == ',')
                    {
                        k = 150;
                    } else {
                        currentPos++;
                    }
                }

                char[] floatValue = new char[currentPos - lastPos];
                for (int k = 0; k < floatValue.Length; k++)
                {
                    floatValue[k] = c[k + lastPos];
                }
                string value = new string(floatValue);
                float result;
                float.TryParse(value, out result);
                nums[i][j] = result;
                
            }
        }
        
        data.trackTemp = nums;
        data.TrackConvert();


        ExitSave();
    }

    public void ImportError()
    {
        ExitSave();
    }


    public void Save()
    {
        
        sound.Play();
        //oh no

        DataTransfer data = FindObjectOfType<DataTransfer>();

        if(data == null)
        {
            return;
        }

        float[][] t = new float[parts.Count][];
        foreach (Object o in parts)
        {
            float[] temp = new float[7];
            temp[0] = (float)o.type;
            temp[1] = o.x;
            temp[2] = o.y;
            temp[3] = o.rotation;
            temp[4] = o.xScale;
            temp[5] = o.yScale;
            temp[6] = o.custom;

            t[parts.IndexOf(o)] = temp;
        }

        data.trackTemp = t;
        data.TrackConvert();

        string name = saveInput.text;

        if(name == "")
        {
            name = "Unnamed Track";
        }
        data.names.Add(name);

        //and finally exit out of menu
        ExitSave();
    }





    public void Load()
    {
        sound.Play();
        //sets data to load based off dropdown
        int index = loadInput.value;
        //this will be FUN
        
        DataTransfer data = FindObjectOfType<DataTransfer>();

        if(data == null)
        {
            return;
        }

        if(data.tracks.Count <= 0)
        {
            return;
        }
        
        data.TrackConvertBack(index);
        
        //Destroy current loaded objects
        DestroyAllObjects();
        
        
        //goes through each object and assigns variables after making new objects
        
        for (int i = 0; i < data.trackTemp.Length; i++)
        {
            Object ob = new Object();
            //hard copy
            ob.x = defaultObject.x;
            ob.y = defaultObject.y;
            ob.rotation = defaultObject.rotation;
            ob.xScale = defaultObject.xScale;
            ob.yScale = defaultObject.yScale;
            ob.custom = defaultObject.custom;

            parts.Add(ob);
        }
        
        for (int i = 0; i < data.trackTemp.Length; i++)
        {
            parts[i].type = (int)data.trackTemp[i][0];
            parts[i].x = data.trackTemp[i][1];
            parts[i].y = data.trackTemp[i][2];
            parts[i].rotation = data.trackTemp[i][3]; 
            parts[i].xScale = data.trackTemp[i][4];
            parts[i].yScale = data.trackTemp[i][5];
            parts[i].custom = data.trackTemp[i][6];
        }

        //spawn in new objects with new data
        for (int i = 0; i < parts.Count; i++)
        {
            gameObjects.Add(Instantiate(basicSolid, new Vector3(parts[i].x, parts[i].y, 0f), Quaternion.Euler(0f, 0f, parts[i].rotation)));
            GameObject g = gameObjects[i];

            g.transform.localScale = new Vector3(parts[i].xScale, parts[i].yScale, 1f);

            MatchToData(g, i);
        }

        //and finally exit out of menu
        ExitSave();
    }



    public void DestroyAllObjects()
    {
        int max = parts.Count;
        for (int index = 0; index < max; index++)
        {
            parts.RemoveAt(0);
            
            Destroy(gameObjects[0]);
            
            gameObjects.RemoveAt(0);
        }
    }

    public void SetupLoadDropdown() 
    {
        //sets dropdown menu to a list of options
        loadInput.ClearOptions();

        DataTransfer data = FindObjectOfType<DataTransfer>();
        if(data == null)
        {
            return;
        }
        if(data.tracks.Count <= 0)
        {
            return;
        }

        loadInput.AddOptions(data.names);
    }

    public void SetupDeleteDropdown() 
    {
        //sets dropdown menu to a list of options
        deleteInput.ClearOptions();

        DataTransfer data = FindObjectOfType<DataTransfer>();
        if(data == null)
        {
            return;
        }
        if(data.tracks.Count <= 0)
        {
            return;
        }

        deleteInput.AddOptions(data.names);
    }

    public void SetupExportDropdown() 
    {
        //sets dropdown menu to a list of options
        exportInput.ClearOptions();

        DataTransfer data = FindObjectOfType<DataTransfer>();
        if(data == null)
        {
            return;
        }
        if(data.tracks.Count <= 0)
        {
            return;
        }

        exportInput.AddOptions(data.names);
    }

    public void Delete()
    {
        
        sound.Play();
        int index = deleteInput.value;

        DataTransfer data = FindObjectOfType<DataTransfer>();
        if(data == null)
        {
            return;
        }
        if(data.tracks.Count <= 0)
        {
            return;
        }

        data.tracks.RemoveAt(index); 
        data.names.RemoveAt(index);

        ExitSave();
    }




}
















































































[System.Serializable]
public class Object
{
    public int type;

    public float x;
    public float y;

    public float rotation;
    public float xScale;
    public float yScale;

    public float custom;
}
