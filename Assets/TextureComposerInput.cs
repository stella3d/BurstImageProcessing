using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace BurstImageProcessing
{
    public class TextureComposerInput : MonoBehaviour
    {
        [SerializeField] [Tooltip("The texture we will copy our processed data from")]
        Texture2D m_Texture;

        [SerializeField] protected SharedPixelBuffer32 m_SharedPixelBuffer;

        Texture2D m_DynamicTexture;

        [SerializeField] Renderer m_TargetRenderer;

        IntPtr m_ProcessedDataPtr;

        int m_WebcamTextureLength;

        NativeArray<Color32> m_NativeWebcamTexture;

        void OnEnable()
        {
            m_DynamicTexture = new Texture2D(m_Texture.width, m_Texture.height, TextureFormat.RGBA32, false);
            m_TargetRenderer.material.mainTexture = m_DynamicTexture;

            m_NativeWebcamTexture = m_Texture.GetRawTextureData<Color32>();
            Debug.Log("native texture length: " + m_NativeWebcamTexture.Length);
            
            m_SharedPixelBuffer.Initialize(m_NativeWebcamTexture);
        }

        void Update()
        {
            //m_SharedPixelBuffer.UpdateImageData(m_NativeWebcamTexture);
        }

        void LateUpdate()
        {
            var byteCount = m_SharedPixelBuffer.GetPixelBufferPtr(out m_ProcessedDataPtr);
            m_DynamicTexture.LoadRawTextureData(m_ProcessedDataPtr, byteCount);
            m_DynamicTexture.Apply(false);
        }
    }
}