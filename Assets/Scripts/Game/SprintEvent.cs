using UnityEngine;

namespace Game
{
    public class SprintEvent : Lara.LaraEvent
    { 
        private Stamina     m_stamina;
        private float       m_sprintSpeedMultiplier = 2;
        private float       m_fGravitySpeed;
        private bool        m_isDone= false ;

        public SprintEvent(Lara lara, Stamina stamina) : base(lara)
        {
            m_stamina = stamina;
        }

        public override void OnUpdate()
        {
            m_fGravitySpeed = Utils.CalculateGravitySpeed(m_fGravitySpeed, Controller.isGrounded, Time.deltaTime);
            Vector3 vGravityVelocity = Vector3.down * m_fGravitySpeed;

            if (m_stamina.ModifyStamina(-Time.deltaTime))
                m_isDone = true;
            else
                Controller.Move(Transform.forward * (Time.deltaTime * Lara.MOVE_SPEED * m_sprintSpeedMultiplier) +
                                vGravityVelocity * Time.deltaTime);
        }

        public override bool IsDone()
        {
            return !Sprint || m_isDone;
        }
    }
}