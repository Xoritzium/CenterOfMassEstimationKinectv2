using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayRecordingStats : MonoBehaviour
{

    [SerializeField] RecorderManager recorder;
    [SerializeField] private TextMeshProUGUI startRecordingDisplay;
    [SerializeField] private TextMeshProUGUI stopRecordingDisplay;
   
    public void startRecording()
    {
        startRecordingDisplay.text = "started at: " + Time.frameCount;
    }
    public void StopRecording()
    {
        stopRecordingDisplay.text = "stopped at: " + Time.frameCount;
      //  startRecordingDisplay.text = "recorded Frames: " + recorder.GetRecordedFrames();
    }

}
