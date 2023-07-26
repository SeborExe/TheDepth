// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.Wrappers
{

    /// <summary>
    /// Manages a UI panel. When the panel is active and on top, it ensures that one of 
    /// its Selectables is selected if using joystick or keyboard.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Common/UI/UI Panel")]
    public class UIPanel : PixelCrushers.UIPanel
    {
        protected override void OnEnable()
        {
            OnStatDialogue();
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            OnEndDialogue();
            base.OnDisable();
        }

        private void OnStatDialogue()
        {
            MainGameCanvas.Instance.PlayerStatsDisplay.gameObject.SetActive(false);
            Player.Instance.IsInDialogue = true;
        }

        private void OnEndDialogue()
        {
            MainGameCanvas.Instance.PlayerStatsDisplay.gameObject.SetActive(true);
            Player.Instance.IsInDialogue = false;
        }
    }

}
