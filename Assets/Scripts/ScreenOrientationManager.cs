// Created and programmed by Eric Milota, 2021

using System;
using UnityEngine;
using UnityEngine.UI;

namespace MilotaConnect4Demo
{
    public enum ScreenOrientationManagerMode
    {
        NONE,
        ANY_LANDSCAPE_ORIENTATION_OK,   // Landscape (LandscapeLeft or LandscapeRight) only allowed
        ANY_PORTRAIT_ORIENTATION_OK    // Portrait (Portrait or PortraitUpsideDown) only allowed
    }

    public class ScreenOrientationManager
    {
        public const ScreenOrientationManagerMode DEFAULT_SCREEN_ORIENTATION_MANAGER_MODE = ScreenOrientationManagerMode.NONE;
        public const int DEFAULT_MS_BETWEEN_UPDATES = 100;  // every 100 ms or so
        public const int DEFAULT_MS_TO_RESTORE = 500;   // 1/2 sec

        private ScreenOrientationManagerMode mScreenOrientationManagerMode = DEFAULT_SCREEN_ORIENTATION_MANAGER_MODE;
        private int mMSBetweenUpdates = DEFAULT_MS_BETWEEN_UPDATES;
        private int mMSToRestore = DEFAULT_MS_TO_RESTORE;
        private ScreenOrientation mScreenOrientationRestore = (ScreenOrientation) 0;
        private Int64 mTimeStampLastUpdated = 0;
        private Int64 mTimeStampRestored = 0;
        private bool mHasFocus = false;
        private bool mIsPaused = false;

        public ScreenOrientationManagerMode ScreenOrientationManagerMode { get { return mScreenOrientationManagerMode; } }
        public ScreenOrientation ScreenOrientationRestore { get { return mScreenOrientationRestore; } }

        public ScreenOrientationManager(
            ScreenOrientationManagerMode screenOrientationManagerMode = DEFAULT_SCREEN_ORIENTATION_MANAGER_MODE,
            int msBetweenUpdates = DEFAULT_MS_BETWEEN_UPDATES,
            int msToRestore = DEFAULT_MS_TO_RESTORE)
        {
            Init(
                screenOrientationManagerMode, 
                msBetweenUpdates, 
                msToRestore);
        }

        public void Init(
            ScreenOrientationManagerMode screenOrientationManagerMode,
            int msBetweenUpdates,
            int msToRestore)
        {
            mScreenOrientationManagerMode = screenOrientationManagerMode;
            mMSBetweenUpdates = msBetweenUpdates;
            mMSToRestore = msToRestore;

            switch (mScreenOrientationManagerMode)
            {
                case ScreenOrientationManagerMode.NONE:
                    {
                        break;
                    }
                case ScreenOrientationManagerMode.ANY_LANDSCAPE_ORIENTATION_OK:
                    {
                        Screen.autorotateToLandscapeLeft = true;
                        Screen.autorotateToLandscapeRight = true;
                        Screen.autorotateToPortrait = false;
                        Screen.autorotateToPortraitUpsideDown = false;
                        Screen.orientation = ScreenOrientation.AutoRotation;
                        RememberRestoreValue();
                        break;
                    }
                case ScreenOrientationManagerMode.ANY_PORTRAIT_ORIENTATION_OK:
                    {
                        Screen.autorotateToLandscapeLeft = false;
                        Screen.autorotateToLandscapeRight = false;
                        Screen.autorotateToPortrait = true;
                        Screen.autorotateToPortraitUpsideDown = true;
                        Screen.orientation = ScreenOrientation.AutoRotation;
                        RememberRestoreValue();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            mTimeStampRestored = 0;
            mTimeStampLastUpdated = 0;
            mIsPaused = false;
            mHasFocus = true;
        }

        private void RememberRestoreValue()
        {
            mScreenOrientationRestore =
                (Screen.orientation == ScreenOrientation.LandscapeRight
                    ? ScreenOrientation.LandscapeRight
                    : ScreenOrientation.LandscapeLeft);
        }

        public void OnScreenOrientationManagerUpdate()
        {
            if ((mTimeStampRestored != 0) && (!mIsPaused) && (mHasFocus))
            {
                Int64 timestamp = Util.GetMS(); // now!
                if ((mTimeStampLastUpdated == 0) ||
                    ((timestamp - mTimeStampLastUpdated) >= mMSBetweenUpdates ))
                {
                    mTimeStampLastUpdated = timestamp;

                    // update now!
                    UpdateNow();

                    int ms = (int)(timestamp - mTimeStampRestored);
                    if (ms >= mMSToRestore)
                    {
                        // done!
                        mTimeStampRestored = 0;
                    }
                }
            }
        }

        private void UpdateNow()
        {
            switch (mScreenOrientationManagerMode)
            {
                case ScreenOrientationManagerMode.NONE:
                    {
                        break;
                    }
                case ScreenOrientationManagerMode.ANY_LANDSCAPE_ORIENTATION_OK:
                    {
                        if (mScreenOrientationRestore == ScreenOrientation.LandscapeLeft)
                        {
                            Screen.orientation = ScreenOrientation.LandscapeLeft;

                            Screen.autorotateToLandscapeLeft = true;
                            Screen.autorotateToLandscapeRight = true;
                            Screen.autorotateToPortrait = false;
                            Screen.autorotateToPortraitUpsideDown = false;
                            Screen.orientation = ScreenOrientation.AutoRotation;
                        }
                        else if (mScreenOrientationRestore == ScreenOrientation.LandscapeRight)
                        {
                            Screen.orientation = ScreenOrientation.LandscapeRight;

                            Screen.autorotateToLandscapeLeft = true;
                            Screen.autorotateToLandscapeRight = true;
                            Screen.autorotateToPortrait = false;
                            Screen.autorotateToPortraitUpsideDown = false;
                            Screen.orientation = ScreenOrientation.AutoRotation;
                        }
                        break;
                    }
                case ScreenOrientationManagerMode.ANY_PORTRAIT_ORIENTATION_OK:
                    {
                        if (mScreenOrientationRestore == ScreenOrientation.Portrait)
                        {
                            Screen.orientation = ScreenOrientation.Portrait;

                            Screen.autorotateToLandscapeLeft = false;
                            Screen.autorotateToLandscapeRight = false;
                            Screen.autorotateToPortrait = true;
                            Screen.autorotateToPortraitUpsideDown = true;
                            Screen.orientation = ScreenOrientation.AutoRotation;
                        }
                        else if (mScreenOrientationRestore == ScreenOrientation.PortraitUpsideDown)
                        {
                            Screen.orientation = ScreenOrientation.PortraitUpsideDown;

                            Screen.autorotateToLandscapeLeft = false;
                            Screen.autorotateToLandscapeRight = false;
                            Screen.autorotateToPortrait = true;
                            Screen.autorotateToPortraitUpsideDown = true;
                            Screen.orientation = ScreenOrientation.AutoRotation;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void OnScreenOrientationApplicationFocus(bool focus)
        {
            mHasFocus = focus;
            if (mHasFocus)
            {
                // we've just come back
                mTimeStampRestored = Util.GetMS();  // now!
                mTimeStampLastUpdated = 0;
                UpdateNow();
            }
            else
            {
                // we're going away
                mTimeStampRestored = 0;
                mTimeStampLastUpdated = 0;
                RememberRestoreValue();
            }
        }

        public void OnScreenOrientationApplicationPause(bool pause)
        {
            mIsPaused = pause;
        }
    }
}

