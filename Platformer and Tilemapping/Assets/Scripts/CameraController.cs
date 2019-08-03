using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class CameraController : MonoBehaviour
{
public GameObject player;
private Vector3 offset;
public Camera thisCam;
    void Start()
    {
        //Screen.SetResolution(1600,900,false);
        thisCam.orthographic = true;
        offset = transform.position - player.transform.position;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }

    void Update()
    {
    if(player.GetComponent<PlayerController>().lossSound.isPlaying == true)
        {
            float camSize = thisCam.orthographicSize;
            Debug.Log(camSize);
            float time = 0;
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time/5);
            thisCam.orthographicSize = Mathf.Lerp(camSize, 1.0f,t * 5);
            Debug.Log(camSize);
        }
    }
}