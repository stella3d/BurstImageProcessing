using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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

        Texture2D m_DynamicTexture;

        [SerializeField]
        Renderer m_TargetRenderer;

        WebCamDevice m_CamDevice;
        WebCamTexture m_CamTexture;

        Color32[] m_Data;

        IntPtr m_ProcessedDataPtr;

        [SerializeField]
        EffectComposer m_Composer;

        NativeArray<Color32> m_NativeColor;

        unsafe void OnEnable()
        {
            m_Data = new Color32[m_WebcamTextureSize.x * m_WebcamTextureSize.y];
            m_NativeColor = new NativeArray<Color32>(m_Data, Allocator.Persistent);

            m_Composer.ReInitialize(m_Data);

            if (m_Composer == null)
                m_Composer = GetComponent<EffectComposer>();

            if (m_WebcamIndex >= WebCamTexture.devices.Length)
                m_WebcamIndex = WebCamTexture.devices.Length - 1;

            m_CamDevice = WebCamTexture.devices[m_WebcamIndex];
            m_CamTexture = new WebCamTexture(m_CamDevice.name, m_WebcamTextureSize.x, m_WebcamTextureSize.y);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = m_CamTexture;

            m_DynamicTexture = new Texture2D(m_WebcamTextureSize.x, m_WebcamTextureSize.y, TextureFormat.RGBA32, false);
            m_TargetRenderer.material.mainTexture = m_DynamicTexture;

            m_CamTexture.Play();
        }

        private void OnDisable()
        {
            m_NativeColor.Dispose();
        }

        void Update()
        {
            m_CamTexture.GetPixels32(m_Data);
            m_Composer.UpdateImageData(m_Data);
        }

        void LateUpdate()
        {
            var byteCount = m_Composer.GetProcessedDataPtr(out m_ProcessedDataPtr);
            m_DynamicTexture.LoadRawTextureData(m_ProcessedDataPtr, byteCount);
            m_DynamicTexture.Apply(false);
        }
    }
}