using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StaminaSlider : MonoBehaviour
    {
        [SerializeField] private Stamina stamina;
        [SerializeField] private Slider slider;

        private void Update()
        {
            //it's not necessary to do this every frame
            slider.value = stamina.GetCurrentStamina();
        }
    }
}