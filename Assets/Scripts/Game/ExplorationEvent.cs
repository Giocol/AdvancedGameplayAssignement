using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class ExplorationEvent : Lara.LaraEvent
    {
        private float       m_fSpeed;
        private float       m_fGravitySpeed;
        private Stamina     m_stamina;

        public ExplorationEvent(Lara lara) : base(lara)
        {
            m_stamina = Resources.Load<Stamina>("ScriptableObjects/Stamina");
            m_stamina.Init();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // update gravity / falling
            m_fGravitySpeed = Utils.CalculateGravitySpeed(m_fGravitySpeed, Controller.isGrounded, Time.deltaTime);
            Vector3 vGravityVelocity = Vector3.down * m_fGravitySpeed;

            // forward / back
            bool bForward = Movement.y > 0.4f;
            m_fSpeed = Mathf.MoveTowards(m_fSpeed, (bForward ? 1.0f : 0.0f), Time.deltaTime * 3.0f);
            if (m_fSpeed > 0.001f)
            {
                Controller.Move(m_fSpeed * Transform.forward * Time.deltaTime * Lara.MOVE_SPEED + vGravityVelocity * Time.deltaTime);

                // trigger movement actions
                if (bForward)
                {
                    TriggerInteraction(InteractionManager.ActionType.Movement);
                }
            }

            // update animator
            Animator.SetFloat("Speed", m_fSpeed);

            // left / right
            int iRotation = Mathf.Abs(Movement.x) > 0.4f ? (int)Mathf.Sign(Movement.x) : 0;
            if (iRotation != 0)
            {
                Transform.Rotate(Vector3.up, iRotation * Time.deltaTime * 90.0f);
            }

            // trigger active actions
            if (Interact)
            {
                TriggerInteraction(InteractionManager.ActionType.Active);
            }

            // jump?
            if (Jump)
            {
                Lara.PushEvent(new JumpEvent(Lara));
            }

            if (Sprint)
            {
                Lara.PushEvent(new SprintEvent(Lara, m_stamina));
            }

            if (Pause)
            {
                PausePopup.Create<PausePopup>(Lara);
            }

            m_stamina.ModifyStamina(+Time.deltaTime);
        }

        public override bool IsDone()
        {
            return false;
        }

        protected void TriggerInteraction(InteractionManager.ActionType actionType)
        {
            // get interactions in range
            HashSet<InteractionManager.IInteraction> interactions = InteractionManager.Instance.GetInteractions(Transform.position, 4.0f, actionType);

            // remove unperformable interactions
            interactions.RemoveWhere(ii => !ii.CanInteract(Lara));

            // get closest interaction
            float fBestDistance = float.MaxValue;
            InteractionManager.IInteraction bestInteraction = null;
            foreach (InteractionManager.IInteraction ii in interactions)
            {
                float fDistance = ii.GetInteractionDistance(Lara);
                if (fDistance < fBestDistance)
                {
                    fBestDistance = fDistance;
                    bestInteraction = ii;
                }
            }

            // perform interaction
            if (bestInteraction != null)
            {
                bestInteraction.PerformInteraction(Lara);
            }
        }
    }
}