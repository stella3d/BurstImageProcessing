using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BurstImageProcessing
{
    public class WebcamComposerInput : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Which webcam to use")]
        int m_WebcamIndex;

        [SerializeField]
        [Tooltip("select a resolution compatible with your webcam.  you will need to move the camera if you change this")]
        Vector2Int m_WebcamTextureSize = new Vector2Int(1024, 576);

        [SerializeField]
        [Tooltip("The texture we will copy our processed data into")]
        Texture2D m_Texture;

        WebCamDevice m_CamDevice;
        WebCamTexture m_CamTexture;

        Color32[] m_Data;

        [SerializeField]
        EffectComposer m_Composer;

        void OnEnable()
        {
            m_Data = new Color32[m_WebcamTextureSize.x * m_WebcamTextureSize.y];
            m_Composer.ReInitialize(m_Data);

            if (m_Composer == null)
                m_Composer = GetComponent<EffectComposer>();

            if (m_WebcamIndex >= WebCamTexture.devices.Length)
                m_WebcamIndex = WebCamTexture.devices.Length - 1;

            m_CamDevice = WebCamTexture.devices[m_WebcamIndex];
            m_CamTexture = new WebCamTexture(m_CamDevice.name, m_WebcamTextureSize.x, m_WebcamTextureSize.y);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = m_CamTexture;

            m_CamTexture.Play();
        }

        void Update()
        {
            m_CamTexture.GetPixels32(m_Data);
            m_Composer.UpdateImageData(m_Data);
        }

        void LateUpdate()
        {
            m_Composer.GetProcessedData(m_Data);
            m_Texture.SetPixels32(0, 0, m_WebcamTextureSize.x, m_WebcamTextureSize.y, m_Data);
            m_Texture.Apply(false);
        }
    }
}