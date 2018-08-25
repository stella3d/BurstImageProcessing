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

        [SerializeField]
        protected SharedPixelBuffer32 m_SharedPixelBuffer;

        Texture2D m_DynamicTexture;

        [SerializeField]
        Renderer m_TargetRenderer;

        WebCamDevice m_CamDevice;
        WebCamTexture m_CamTexture;

        Color32[] m_Data;

        IntPtr m_ProcessedDataPtr;

        NativeArray<Color32> m_NativePixels; 

        const int k_SizeOfColor32 = 4;

        void OnEnable()
        {
            m_Texture = new Texture2D(m_WebcamTextureSize.x, m_WebcamTextureSize.y);
            m_Data = new Color32[m_WebcamTextureSize.x * m_WebcamTextureSize.y];
            m_NativePixels = new NativeArray<Color32>(m_Data, Allocator.Persistent);

            m_SharedPixelBuffer.Initialize(m_NativePixels);

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

        void Update()
        {
            IntPtr texturePtr = m_CamTexture.GetNativeTexturePtr();
            m_Texture.LoadRawTextureData(texturePtr, m_CamTexture.width * m_CamTexture.height * k_SizeOfColor32);
            
            //m_NativePixels = m_Texture.GetRawTextureData<Color32>();
            //m_SharedPixelBuffer.UpdateImageData(ref m_NativePixels);
        }

        void LateUpdate()
        {
            var byteCount = m_SharedPixelBuffer.GetPixelBufferPtr(out m_ProcessedDataPtr);
            m_DynamicTexture.LoadRawTextureData(m_ProcessedDataPtr, byteCount);
            m_DynamicTexture.Apply(false);
        }

        void OnDisable()
        {
            m_NativePixels.Dispose();
        }
    }
}