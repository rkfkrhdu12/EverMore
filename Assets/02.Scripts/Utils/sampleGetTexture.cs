using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

[Serializable]
public class stringTexture2D : SerializableDictionary<string, Texture2D>
{
}

public class sampleGetTexture : MonoBehaviour
{
    [SerializeField]
    private AssetLabelReference unitPhotos;

    [SerializeField]
    private stringTexture2D st;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Addressables.LoadResourceLocationsAsync(unitPhotos).Completed += op =>
            {
                foreach (var data in op.Result)
                    Addressables.LoadAssetAsync<Texture2D>(data.PrimaryKey).Completed += handle =>
                        st.Add(data.PrimaryKey, handle.Result);
            };

        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (var texture2d in st.Values) 
                Addressables.Release(texture2d);
        }
    }
}
