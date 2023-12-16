using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StartupService : MonoBehaviour
{
    [Inject] private AssetManager assets;
    [Inject] private ISceneServices scenes;
    [Inject] private IScreenService screens;
    [Inject] private UGS ugs;
    [Inject] private IConfigService configs;
    private StartupContext context = new StartupContext();

    private async void Start()
    {
        screens.Open<StartupScreen>(context);
        
        await ugs.Initialize(); 
        context.SetProgress(10);
        await assets.InitializeContent();
        context.SetProgress(30);
        await scenes.LoadScene("Main");
    }


    //show loading
    //initialize online services
    //check for asset update
    //get game config
    //get player data
    //load starting Point
    //transition to starting point
}
