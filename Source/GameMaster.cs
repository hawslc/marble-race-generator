using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public int length;
    public float time;
    public bool generate;

    public Transform startPos;

    public List<GameObject> tracks = new List<GameObject>();
    public GameObject endTrack;
    public GameObject explosion;
    GameObject marble;
    public List<GameObject> marbles = new List<GameObject>();
    public GameObject generatorPrefab;

    void Start()
    {
        if(FindObjectOfType<DataTransfer>() != null){
            length = FindObjectOfType<DataTransfer>().length;
        }


        if (generate)
        {
            if(FindObjectOfType<DataTransfer>() != null){
                length = FindObjectOfType<DataTransfer>().length;
                print(length);
                if(FindObjectOfType<DataTransfer>().customOnly){
                    GenerateTrackCustom();
                } else {
                    GenerateTrackAll();
                }
            }
        }
        foreach(Marble m in FindObjectsOfType<Marble>()){
            m.GetComponent<TrailRenderer>().enabled = false;
        }

    }

    void Update()
    {
        Time.timeScale = time;

        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        TimeSet();

        if(Input.GetMouseButtonDown(0)){
            Instantiate(explosion, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f), transform.rotation);
        }
        if(Input.GetMouseButtonDown(2)){
            Instantiate(marbles[Random.Range(0, marbles.Count)], new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f), Quaternion.identity);
        }
    }

    void GenerateTrackAll()
    {
        Transform track;
        List<bool> used = new List<bool>();

        DataTransfer data = FindObjectOfType<DataTransfer>();

        List<GameObject> allTracks = new List<GameObject>();

        //go through custom tracks and make each one
        for (int i = 0; i < data.tracks.Count; i++)
        {
            allTracks.Add(Instantiate(generatorPrefab, transform.position, Quaternion.identity));
            data.TrackConvertBack(i);
            allTracks[i].GetComponent<CustomGenerator>().Generate(data.trackTemp);
        }

        foreach (GameObject g in tracks)
        {
            allTracks.Add(g);
        }

        for (int i = 0; i < allTracks.Count; i++)
        {
            used.Add(false);
        }

        int r = 0;
        r = Random.Range(0, allTracks.Count);
        Vector3 pos = startPos.position;

        if(length > 40){
            length = 40;
        }

        for (int i = 0; i < length; i++)
        {
            while(used[r] == true)
            {
                r = Random.Range(0, allTracks.Count);
            }

            track = Instantiate(allTracks[r], pos, Quaternion.identity).transform;
            track.position -= allTracks[r].transform.GetChild(0).position;
            used[r] = true;
            pos = track.GetChild(1).position;
            track.gameObject.SetActive(true);
        }


        //spawn end
        track = Instantiate(endTrack, pos, Quaternion.identity).transform;
        track.position -= endTrack.transform.GetChild(0).position;

    }

    void GenerateTrackCustom()
    {
        Transform track;
        List<bool> used = new List<bool>();

        DataTransfer data = FindObjectOfType<DataTransfer>();

        List<GameObject> allTracks = new List<GameObject>();

        //go through custom tracks and make each one
        for (int i = 0; i < data.tracks.Count; i++)
        {
            allTracks.Add(Instantiate(generatorPrefab, transform.position, Quaternion.identity));
            data.TrackConvertBack(i);
            allTracks[i].GetComponent<CustomGenerator>().Generate(data.trackTemp);
        }


        for (int i = 0; i < allTracks.Count; i++)
        {
            used.Add(false);
        }

        int r = 0;
        r = Random.Range(0, allTracks.Count);
        Vector3 pos = startPos.position;

        if(length > allTracks.Count){
            length = allTracks.Count;
        }

        for (int i = 0; i < length; i++)
        {
            while(used[r] == true)
            {
                r = Random.Range(0, allTracks.Count);
            }

            track = Instantiate(allTracks[r], pos, Quaternion.identity).transform;
            track.position -= allTracks[r].transform.GetChild(0).position;
            used[r] = true;
            pos = track.GetChild(1).position;
            track.gameObject.SetActive(true);
        }


        //spawn end
        track = Instantiate(endTrack, pos, Quaternion.identity).transform;
        track.position -= endTrack.transform.GetChild(0).position;

    }






























































   void TimeSet(){
        if (Input.GetKey("0"))
        {
            time = 0;
        }
        if (Input.GetKey("1"))
        {
            time = 1;
        }
        if (Input.GetKey("2"))
        {
            time = 2;
        }
        if (Input.GetKey("3"))
        {
            time = 3;
        }
        if (Input.GetKey("4"))
        {
            time = 4;
        }
        if (Input.GetKey("5"))
        {
            time = 5;
        }
        if (Input.GetKey("6"))
        {
            time = 6;
        }
        if (Input.GetKey("7"))
        {
            time = 7;
        }
        if (Input.GetKey("8"))
        {
            time = 8;
        }
        if (Input.GetKey("9"))
        {
            time = 9;
        }




        if (Input.GetKey("z"))
        {
            time = 5;
        }
        if (Input.GetKey("x"))
        {
            time = 12;
        }
        if (Input.GetKey("c"))
        {
            time = 18;
        }
        if (Input.GetKey("v"))
        {
            time = 30;
        }
        if (Input.GetKey("b"))
        {
            time = 60;
        }

        if (Input.GetKey("q") || Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }

        if (Input.GetKey("f"))
        {
            float maxX = 0f;


            foreach(Marble m in FindObjectsOfType<Marble>()){
                if(m.transform.position.x > maxX && m.transform.position.y > -1500f){
                    maxX = m.transform.position.x;
                    marble = m.gameObject;
                }
            }

            Camera.main.transform.position = new Vector3(marble.transform.position.x, marble.transform.position.y, -10);
        }


        if (Input.GetKey("l"))
        {
            float minX = 50000f;

            foreach(Marble m in FindObjectsOfType<Marble>()){
                if(m.transform.position.x < minX && m.transform.position.y > -1500f){
                    minX = m.transform.position.x;
                    marble = m.gameObject;
                }
            }

            Camera.main.transform.position = new Vector3(marble.transform.position.x, marble.transform.position.y, -10);
        }

   }
}
