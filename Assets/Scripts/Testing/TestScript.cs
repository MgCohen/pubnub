using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;

using UnityEngine.AddressableAssets;
using Zenject;
//using PubNub;

public class TestScript : MonoBehaviour
{
    [Inject] private IScreenService screens;

    [Inject][NaughtyAttributes.Button]
    private void Test()
    {
        Debug.Log(screens);
    }

    private void Awake()
    {
        Debug.Log(1);
    }


}
