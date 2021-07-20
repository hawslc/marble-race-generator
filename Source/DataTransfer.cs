using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour
{
    public int length;
    public int maxLength;

    public List<AudioClip> clips = new List<AudioClip>();

    private static DataTransfer instance;
    bool paused;

    public float[][] trackTemp;
    public List<Track> tracks = new List<Track>();
    public List<string> names = new List<string>();

    public bool customOnly;

    public List<Color> typesColors = new List<Color>();
    public List<PhysicsMaterial2D> typesMaterials = new List<PhysicsMaterial2D>();

    void Awake()
    {
        paused = false;
        DontDestroyOnLoad (this);
            
        if (instance == null) 
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(gameObject); 
        }
    }

    void Update()
    {
        if(GetComponent<AudioSource>().isPlaying == false || Input.GetKeyDown("s"))
        {
            if(!paused)
            {
                GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Count)];
                GetComponent<AudioSource>().time = Random.Range(1, 15);
                GetComponent<AudioSource>().Play();
            }
            
        }

        if(Input.GetKeyDown("m"))
        {
            if(GetComponent<AudioSource>().isPlaying == false){
                GetComponent<AudioSource>().Play();
                paused = false;
            } else {
                GetComponent<AudioSource>().Pause();
                paused = true;
            }
        }

    }



    public void TrackConvert()
    {
        Track temp = new Track();

        temp.list = new PartObject[trackTemp.Length];

        for(int i = 0; i < trackTemp.Length; i++)
        {
            //each part
            temp.list[i] = new PartObject();

            temp.list[i].list = new float[7];

            for(int k = 0; k < trackTemp[i].Length; k++)
            {  
                //each number
                temp.list[i].list[k] = trackTemp[i][k];
            }
        }
        tracks.Add(temp);
    }

    public void TrackConvertBack(int index)
    {
        float[][] temp = new float[tracks[index].list.Length][];

        for(int i = 0; i < temp.Length; i++)
        {
            //each part
            temp[i] = new float[7];

            for(int k = 0; k < temp[i].Length; k++)
            {  
                //each number
                temp[i][k] = tracks[index].list[i].list[k];
            }
        }
        trackTemp = temp;
    }
}



[System.Serializable]
public class PartObject
{
    public float[] list;
}

[System.Serializable]
public class Track
{
    public PartObject[] list;
}
