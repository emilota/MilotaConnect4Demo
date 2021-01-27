// Created and programmed by Eric Milota, 2021

using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class TitleScreenState : BaseState
    {
        public override State State => State.TITLE_SCREEN;

        public override void OnStateEnter(App app)
        {
            app.SceneManager.GotoSceneASync(SceneEnum.SPLASH_SCENE, () =>
            {
            });
        }

        public override void OnStateLeave(App app)
        {
        }

        public override void OnStateUpdate(App app)
        {
        }

        public override void OnStateFixedUpdate(App app)
        {
        }
    }
}
