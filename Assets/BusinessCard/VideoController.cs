using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public GameObject content, video1, video2, video3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(content.transform.localPosition.x < 300 && content.transform.localPosition.x>-400)
        {
            //video1.GetComponent<VideoPlayer>().enabled = false;
            //video3.GetComponent<VideoPlayer>().enabled = false;
            //video2.GetComponent<VideoPlayer>().enabled = true;
            video1.GetComponent<VideoPlayer>().Pause();
            video3.GetComponent<VideoPlayer>().Pause();
            video2.GetComponent<VideoPlayer>().Play();
        }
        else if(content.transform.localPosition.x <-400)
        {
            //video2.GetComponent<VideoPlayer>().enabled = false;
            //video3.GetComponent<VideoPlayer>().enabled = true;
            video2.GetComponent<VideoPlayer>().Pause();
            video3.GetComponent<VideoPlayer>().Play();
        }
        else if(content.transform.localPosition.x > 300 && content.transform.localPosition.x < 700)
        {
            //video1.GetComponent<VideoPlayer>().enabled = true;
            video2.GetComponent<VideoPlayer>().Pause();
            video1.GetComponent<VideoPlayer>().Play();
            //video2.GetComponent<VideoPlayer>().enabled = false;
        }
    }
}
