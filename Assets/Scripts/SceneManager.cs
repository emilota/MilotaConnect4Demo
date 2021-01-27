// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace MilotaConnect4Demo
{
    public class SceneManager
    {
        public static Dictionary<SceneEnum,string> gSceneEnumToSceneNameDict = new Dictionary<SceneEnum,string>()
        {
            { SceneEnum.SPLASH_SCENE, "SplashScene" },
            { SceneEnum.GAME_SCENE, "GameScene" },
            { SceneEnum.EXIT_SCENE, "ExitScene" }
        };

        public const string NO_SCENE_NAME = "";

        public delegate void OnSceneReadyCallback();

        //-------------------------------------------

        public class CurrentInfoClass
        {
            private UnityEngine.SceneManagement.Scene mScene = default(UnityEngine.SceneManagement.Scene);
            private string mSceneName = NO_SCENE_NAME;

            public UnityEngine.SceneManagement.Scene Scene => mScene;
            public string SceneName => mSceneName;

            public CurrentInfoClass()
            {
                ResetValues();
            }

            public void ResetValues()
            {
                mScene = default(UnityEngine.SceneManagement.Scene);
                mSceneName = NO_SCENE_NAME;
            }

            public void UpdateValues()
            {
                mScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                mSceneName = mScene.name ?? NO_SCENE_NAME;
            }
        }

        //-------------------------------------------

        public class LoadingInfoClass
        {
            public SceneEnum SceneEnum = SceneEnum.NONE;
            public string SceneName = NO_SCENE_NAME;
            public OnSceneReadyCallback OnSceneReadyCallback = null;
            public bool FinishedLoading = false;
            public bool DoneWithAwake = false;

            public LoadingInfoClass(
                SceneEnum sceneEnum,
                string sceneName,
                OnSceneReadyCallback onSceneReadyCallback,
                bool finishedLoading, 
                bool doneWithAwake)
            {
                this.SceneEnum = sceneEnum;
                this.SceneName = (sceneName ?? NO_SCENE_NAME);
                this.OnSceneReadyCallback = onSceneReadyCallback;
                this.FinishedLoading = finishedLoading;
                this.DoneWithAwake = doneWithAwake;
            }
        }

        //-------------------------------------------

        private SceneEnum mSceneEnum = SceneEnum.NONE;
        private CurrentInfoClass mCurrentInfo = new CurrentInfoClass();
        private List<LoadingInfoClass> mLoadingInfoList = new List<LoadingInfoClass>();

        //-------------------------------------------

        public SceneEnum SceneEnum => mSceneEnum;
        public CurrentInfoClass CurrentInfo => mCurrentInfo;
        public bool IsSceneLoading => (mLoadingInfoList.Count > 0 ? true : false);

        //-------------------------------------------

        public SceneManager() { Init(); }

        public void Init()
        {
            Uninit();

            mSceneEnum = SceneEnum.NONE;
            this.CurrentInfo.UpdateValues();
            mLoadingInfoList.Clear();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoadedHandler;
        }

        public void Uninit()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoadedHandler;

            mSceneEnum = SceneEnum.NONE;
            this.CurrentInfo.ResetValues();
            mLoadingInfoList.Clear();
        }

        private LoadingInfoClass FindLoadingInfoBySceneEnum(SceneEnum sceneEnum)
        {
            for (int index = 0; index < mLoadingInfoList.Count; index++)
            {
                LoadingInfoClass loadingInfo = mLoadingInfoList[index];
                if (loadingInfo.SceneEnum == sceneEnum)
                    return loadingInfo; // found one!
            }
            return null;
        }

        private LoadingInfoClass FindLoadingInfoBySceneName(string sceneName)
        {
            if (String.IsNullOrEmpty(sceneName))
                return null;

            for(int index = 0; index < mLoadingInfoList.Count; index++)
            {
                LoadingInfoClass loadingInfo = mLoadingInfoList[index];
                if (String.Compare(loadingInfo.SceneName, sceneName, true) == 0)
                    return loadingInfo; // found one!
            }
            return null;
        }

        private void OnSceneLoadedHandler(
            UnityEngine.SceneManagement.Scene scene,
            UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
        {
            // scene loaded

            if (scene != null)
            {
                LoadingInfoClass loadingInfo = FindLoadingInfoBySceneName(scene.name);
                if ((loadingInfo != null) && (!loadingInfo.FinishedLoading))
                {
                    loadingInfo.FinishedLoading = true;
                    if ((loadingInfo.FinishedLoading) && (loadingInfo.DoneWithAwake))
                    {
                        // done!  Let's pull from list and call ready callback now!
                        mLoadingInfoList.Remove(loadingInfo);
                        loadingInfo.OnSceneReadyCallback?.Invoke();
                    }
                }
            }
        }

        public void GotoSceneASync(SceneEnum sceneEnum, OnSceneReadyCallback onSceneReadyCallback, bool force = false)
        {
            SceneEnum sceneEnumOld = mSceneEnum;
            SceneEnum sceneEnumNew = sceneEnum;
            if ((sceneEnumOld == sceneEnumNew) && (!force))
            {
                onSceneReadyCallback?.Invoke();
                return;
            }

            switch (sceneEnumOld)
            {
                case SceneEnum.NONE:    // leaving this scene
                    break;
                case SceneEnum.SPLASH_SCENE: // leaving this scene
                    break;
                case SceneEnum.GAME_SCENE:  // leaving this scene
                    break;
                case SceneEnum.EXIT_SCENE: // leaving this scene
                    break;
                default:    // leaving this scene
                    break;
            }

            mSceneEnum = sceneEnumNew;
            mCurrentInfo.UpdateValues();
            //UnityEngine.Debug.Log("CURRENT SCENE: \"" + this.CurrentSceneName + "\"");

            switch (sceneEnumNew)
            {
                case SceneEnum.NONE:    // entering this scene
                    break;
                case SceneEnum.SPLASH_SCENE: // entering this scene
                    break;
                case SceneEnum.GAME_SCENE:  // entering this scene
                    break;
                case SceneEnum.EXIT_SCENE: // entering this scene
                    break;
                default:    // entering this scene
                    break;
            }

            // now, let's see if we can/should actually load this scene
            string sceneWeWant = "";
            if (gSceneEnumToSceneNameDict.ContainsKey(sceneEnumNew))
                sceneWeWant = gSceneEnumToSceneNameDict[sceneEnumNew];

            if ((!String.IsNullOrEmpty(sceneWeWant)) &&
                (string.Compare(mCurrentInfo.SceneName, sceneWeWant) != 0))
            {
                // time to change scenes....let's load

                //UnityEngine.Debug.Log("GOING TO SCENE: \"" + sceneWeWant + "\"");
                LoadingInfoClass loadingInfo = FindLoadingInfoBySceneName(sceneWeWant);
                if (loadingInfo != null) 
                {
                    // uh oh, we are currently already going into this scene.
                    UnityEngine.Debug.Log("GOING TO SCENE: \"" + sceneWeWant + "\" ERROR...SCENE IS STILL LOADING OR HASN'T COMPLETED WITH AWAKE YET!");
                }
                else
                {
                    loadingInfo = new LoadingInfoClass(
                        sceneEnumNew,
                        sceneWeWant,
                        onSceneReadyCallback,
                        false, // finish loading?
                        false); // done with awake?
                    mLoadingInfoList.Add(loadingInfo);

                    UnityEngine.SceneManagement.SceneManager.LoadScene(
                        sceneWeWant,
                        UnityEngine.SceneManagement.LoadSceneMode.Single);
                }
            }
            else
            {
                onSceneReadyCallback?.Invoke();
            }
        }

        public void FinishedWithSceneAwake(SceneEnum sceneEnumStart)
        {
            if (sceneEnumStart == SceneEnum.NONE)
            {
                Debug.LogError(
                    "Error, called FinishedWithSceneAwake() on scene NONE.  Ignoring.");
                return;
            }

            if ((mSceneEnum == SceneEnum.NONE) && (sceneEnumStart != SceneEnum.NONE))
            {
                // first time!
                GotoSceneASync(sceneEnumStart, null);
            }
            else
            {
                // subsequent time
                LoadingInfoClass loadingInfo = FindLoadingInfoBySceneEnum(mSceneEnum);
                if (loadingInfo != null)
                {
                    if (!loadingInfo.DoneWithAwake)
                    {
                        loadingInfo.DoneWithAwake = true;
                        if ((loadingInfo.FinishedLoading) && (loadingInfo.DoneWithAwake))
                        {
                            // done!  Let's pull from list and call ready callback now!
                            mLoadingInfoList.Remove(loadingInfo);
                            loadingInfo.OnSceneReadyCallback?.Invoke();
                        }
                        else
                        {
                            // ok, we're done with Awake now, but we're probably not done loading yet.
                            // Fall through and let the loading code figure it out
                        }
                    }
                    else
                    {
                        Debug.LogError(
                            "Error, called FinishedWithSceneAwake() trying to finish load of " +
                            Convert.ToString(loadingInfo.SceneEnum) +
                            " but DoneWithAwake is already set!!  Ignoring!");
                    }
                }
            }
        }
    }
}


