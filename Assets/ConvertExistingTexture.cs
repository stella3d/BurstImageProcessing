using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


public class ConvertExistingTexture : MonoBehaviour
{

	[SerializeField]
	Texture2D m_Texture;

	void Start ()
	{
		var pixels= m_Texture.GetRawTextureData<Color32>();
	}
	
	void Update () {
		
	}
}
