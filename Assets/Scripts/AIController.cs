using System;
using UnityEngine;
//using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AIController : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

        // States
        public static readonly int IDLE = 0;
        public static readonly int CURIOUS = 1;
        public static readonly int SCARED = 2;
        public static readonly int RUNNING = 3;

        public static readonly float[] levels = new float[] { 10f, 40f, 80f, 100f };

        public int state;
        public float scared_level = 0f;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            state = CURIOUS;
            SetTarget(target);
        }


        private void Update()
        {
            switch(state)
            {
                case 0:
                    idle_state();
                    break;
                case 1:
                    curious_state();
                    break;
                case 2:
                    scared_state();
                    break;
                case 3:
                    running_state();
                    break;
            }
        }

        public void idle_state()
        {
            if (scared_level > levels[IDLE])
            {
                state = CURIOUS;
                return;
            }
        }

        public void curious_state()
        {
            if (scared_level > levels[CURIOUS])
            {
                state = SCARED;
                return;
            }

            // Running towards sound source
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);

        }

        public void scared_state()
        {
            if (scared_level > levels[SCARED])
            {
                state = RUNNING;
            }

        }

        public void running_state()
        {
            if (scared_level > levels[RUNNING])
            {
                // TODO run out of the house
                return;
            }

        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
