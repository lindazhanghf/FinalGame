using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AIController : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public Transform[] targets;

        // States
        static readonly int IDLE = 0;
        static readonly int CURIOUS = 1;
        static readonly int SCARED = 2;
        static readonly int RUNNING = 3;
        static readonly int EXIT = 4;

        public static readonly float[] levels = new float[] { 10f, 40f, 80f, 100f };

        public int state = IDLE;
        public float scared_level = 0f; // 0 - 100
        public GameObject scare_level_UI;
        private ScareLevel level_slider;
        public GameObject door_opener;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            level_slider = scare_level_UI.GetComponent<ScareLevel>();

            agent.SetDestination(targets[0].position);
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
                case 4:
                    exit_state();
                    break;
            }
        }

        public void idle_state()
        {
            if (scared_level > levels[IDLE])
            {
                state = CURIOUS;
                agent.SetDestination(target.position);
                return;
            }

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }

        // Running towards objects
        public void curious_state()
        {
            if (scared_level > levels[CURIOUS])
            {
                state = SCARED;
                return;
            }

            if (target != null)
                agent.SetDestination(target.position);
            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }

        // Running away from objects
        public void scared_state()
        {
            if (scared_level > levels[SCARED])
            {
                state = RUNNING;
            }

            if (target != null)
                agent.SetDestination(target.position);
            if (agent.remainingDistance < 10 - agent.stoppingDistance)
                character.Move(-1 * agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);

        }

        // Running around the house scared
        public void running_state()
        {
            if (scared_level > levels[RUNNING]) // Scare_level reaches MAX
            {
                door_opener.SetActive(true);
                agent.SetDestination(targets[targets.Length - 1].position); // Door
                //return;
            }
            //if (target != null)
            //    agent.SetDestination(target.position);
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
                return;
            }
            // else: Reached target

            if (scared_level > levels[RUNNING]) // Reach door
            {
                agent.SetDestination(new Vector3(-11, 2, 35));
                state = EXIT;
                return;
            }

            // Select new random target
            random_target();
        }

        // run out of the house
        public void exit_state()
        {
            //if (target != null)
            //    agent.SetDestination(new Vector3(-11, 2, 35));
            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);  // TODO Level ends
        }


        void OnCollisionEnter(Collision c)
        {
            collision_scared(c);
            //switch (state)
            //{
            //    case 0:
            //        collision_scared(c);
            //        break;
            //    //case 1:
            //    //    curious_state();
            //    //    break;
            //    //case 2:
            //    //    scared_state();
            //    //    break;
            //    //case 3:
            //    //    running_state();
            //    //    break;
            //}
        }

        private System.Random rand = new System.Random();
        void random_target(Transform t=null)
        {
            if (t == null) 
                t = targets[rand.Next(targets.Length)];
            target = t;
            agent.SetDestination(target.position);
        }

        public void collision_scared(Collision c)
        {
            if (c.gameObject.tag != "Object")
                return;
            if (c.impulse.x == 0 && c.impulse.y == 0 && c.impulse.z == 0) // Collision too light (relativeVelocity)
                return;
            float scare_factor = c.relativeVelocity.magnitude;
            change_scare_level(scare_factor);
            Debug.Log(c.transform.parent.name + " " + scare_factor.ToString());
        }

        void change_scare_level(float new_scare)
        {
            scared_level += new_scare;
            level_slider.update_scare_level(Mathf.Floor(scared_level) / 100); // Normalize 0-100 to 0-1;
        }
    }
}
