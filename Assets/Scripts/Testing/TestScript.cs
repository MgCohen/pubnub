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
    [Inject] private ScreenService screens;
    [SerializeField] private TestScreen testScreen;
        
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(screens.CurrentScreen);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            screens.Close<TestScreen>();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            screens.Close(testScreen);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            screens.CloseCurrentScreen();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            screens.Open<TestScreen>();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            screens.Open<TestScreen2>(new TestScreenContext2(Color.red));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            screens.Open<TestScreen2>(new TestScreenContext2(Color.blue));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            screens.Close(screens.CurrentScreen);
        }
    }
}
