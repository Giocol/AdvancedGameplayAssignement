using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;

namespace Game
{
    [CreateAssetMenu]
    public class Stamina : ScriptableObject
    {
        [SerializeField] private float maxStamina = 1.0f;
        [SerializeField] private float currentStamina = .0f;
        
        public void Init()
        {
            currentStamina = maxStamina;
        }

        public float GetCurrentStamina()
        {
            return currentStamina;
        }
        
        public bool ModifyStamina(float amountToAdd)
        {
            currentStamina = Mathf.Clamp(currentStamina + amountToAdd, 0, maxStamina);
            return (currentStamina == 0); // returns true if stamina is empty, a bit hacky, i know.
        }
    }
}