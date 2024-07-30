using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferentViews : MonoBehaviour
{

    [SerializeField] MultiSourceManager multiSource;
    [SerializeField] RawImage view;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        view.texture = multiSource.GetColorTexture();
    }
}
