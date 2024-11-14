﻿// Copyright (C) 2015 ricimi. All rights reserved.
// This code can only be used under the standard Unity Asset Store EULA,
// a copy of which is available at https://unity.com/legal/as-terms.

using UnityEngine;

namespace Ricimi
{
    // This class represents the sound button that is used in several places in the demo.
    // It handles the logic to enable and disable the demo's sounds and store the player
    // selection to PlayerPrefs.
    public class SoundButton : MonoBehaviour
    {
        private SpriteSwapper m_spriteSwapper;
        private bool m_on;

        private void Start()
        {
            m_spriteSwapper = GetComponent<SpriteSwapper>();
            m_on = PlayerPrefs.GetInt("sound_on") == 1;
            if (!m_on)
                m_spriteSwapper.SwapSprite();
        }

        public void Toggle()
        {
            m_on = !m_on;
            AudioListener.volume = m_on ? 1 : 0;
            PlayerPrefs.SetInt("sound_on", m_on ? 1 : 0);
        }

        public void ToggleSprite()
        {
            m_on = !m_on;
            m_spriteSwapper.SwapSprite();
        }
    }
}