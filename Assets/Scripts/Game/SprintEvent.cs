using UnityEngine;

namespace Game
{
    public class SprintEvent : Lara.LaraEvent
    {
        private float       m_sprintSpeedMultiplier = 2;
        private float       m_fGravitySpeed;

        public SprintEvent(Lara lara) : base(lara)
        {
        }

        public override void OnUpdate()
        {
            m_fGravitySpeed = Utils.CalculateGravitySpeed(m_fGravitySpeed, Controller.isGrounded, Time.deltaTime);
            Vector3 vGravityVelocity = Vector3.down * m_fGravitySpeed;
            
            Controller.Move(Transform.forward * (Time.deltaTime * Lara.MOVE_SPEED * m_sprintSpeedMultiplier) + vGravityVelocity * Time.deltaTime);
        }

        public override bool IsDone()
        {
            return !Sprint;
        }
    }
}