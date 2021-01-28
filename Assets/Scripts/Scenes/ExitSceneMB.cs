// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class ExitSceneMB : MonoBehaviour
    {
        private App mApp = null;

        //---------------------------------------

        public int FooterBlinkRateInMS = Const.FOOTER_BLINK_RATE_IN_MS;

        public GameObject GOBigMessage;
        public GameObject GOFooterMessage;

        //---------------------------------------

        public void Awake()
        {
            mApp = App.InitIfNeeded();
            mApp.SetExitSceneMB(this);

            Util.SetGameObjectTextMeshProText(this.GOBigMessage, Localize.THANKS_FOR_PLAYING_BIG_MESSAGE);
            Util.SetGameObjectTextMeshProText(this.GOFooterMessage, Localize.THANKS_FOR_PLAYING_FOOTER_MESSAGE);
            UpdateFooterMessage();

            mApp.SceneManager.FinishedWithSceneAwake(SceneEnum.SPLASH_SCENE);
        }

        ~ExitSceneMB()
        {
            mApp.SetExitSceneMB(null);
            mApp = null;
        }

        public void UpdateFooterMessage()
        {
            if (Util.Blink(this.FooterBlinkRateInMS))
                this.GOFooterMessage.SetActive(true);
            else
                this.GOFooterMessage.SetActive(false);
        }

        public void OnClickQuitApplicationButton()
        {
            mApp.OnAppClick(Click.QUIT_APPLICATION_BUTTON);
        }

        public void Update()
        {
            mApp.OnAppUpdate();
        }

        public void FixedUpdate()
        {
            UpdateFooterMessage();
            mApp.OnAppFixedUpdate();
        }

        public void OnApplicationFocus(bool focus)
        {
            mApp.OnAppApplicationFocus(focus);
        }

        public void OnApplicationPause(bool pause)
        {
            mApp.OnAppApplicationPause(pause);
        }

        public void OnApplicationQuit()
        {
            mApp.OnAppApplicationQuit();
        }
    }
}

