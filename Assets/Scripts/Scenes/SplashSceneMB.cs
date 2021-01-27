// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class SplashSceneMB : MonoBehaviour
    {
        private App mApp = null;

        //-----------------------------------

        public int FooterBlinkRateInMS = Const.FOOTER_BLINK_RATE_IN_MS;

        public GameObject GOLeaveAppButtonText;
        public GameObject GOFooterText;

        //-----------------------------------

        public void Awake()
        {
            mApp = App.InitIfNeeded();
            mApp.SetSplashSceneMB(this);

            Util.SetGameObjectTextMeshProText(this.GOLeaveAppButtonText, Localize.TITLE_SCREEN_LEAVE_BUTTON_TEXT);
            Util.SetGameObjectTextMeshProText(this.GOFooterText, Localize.TITLE_SCREEN_FOOTER_MESSAGE);
            UpdateFooterMessage();

            mApp.SceneManager.FinishedWithSceneAwake(SceneEnum.SPLASH_SCENE);
        }

        ~SplashSceneMB()
        {
            mApp.SetSplashSceneMB(null);
            mApp = null;
        }

        public void UpdateFooterMessage()
        {
            if (Util.Blink(this.FooterBlinkRateInMS))
                this.GOFooterText.SetActive(true);
            else
                this.GOFooterText.SetActive(false);
        }

        public void OnClickLeaveAppButton()
        {
            mApp.OnAppClick(Click.LEAVE_APP_BUTTON);
        }

        public void OnClickStartGameButton()
        {
            mApp.OnAppClick(Click.START_GAME_BUTTON);
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
    }
}

