using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AIController : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }      // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; }         // the character we are controlling
        public Transform target;                                            // target to aim for
        public Transform[] targets;

        private FloatObject floatingObject;

        // States
        static readonly int IDLE = 0;
        static readonly int CURIOUS = 1;
        static readonly int SCARED = 2;
        static readonly int RUNNING = 3;
        static readonly int EXIT = 4;
        // Game states & scare level
        public static readonly float[] levels = new float[] { 10f, 40f, 80f, 100f };
        public int state = SCARED;
        public float scared_level = 0f; // 0 - 100
        public GameObject scare_level_UI;
        private ScareLevel level_slider;

        private AudioSource audio_source;
        public AudioClip[] audio_clips;
        public GameObject door_opener;

        public float counter = 5f;
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            floatingObject = GameObject.Find("Player").GetComponent<FloatObject>();
            level_slider = scare_level_UI.GetComponent<ScareLevel>();
            audio_source = GetComponent<AudioSource>();

            agent.SetDestination(targets[0].position);
        }

        private void Update()
        {
            switch (state)
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
                    return;
            }
            //change_scare_level(-0.01f);
            if (counter < 2)
                counter += Time.deltaTime;
        }

        public void idle_state()
        {
            if (scared_level > levels[IDLE])
            {
                state = CURIOUS;
                agent.SetDestination(target.position);
                audio_source.clip = audio_clips[state];
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
                audio_source.clip = audio_clips[state];
                return;
            }

            if (target != null)
                agent.SetDestination(target.position);
            if (agent.remainingDistance > agent.stoppingDistance - 1)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
                character.Move(Vector3.zero, false, false);
        }

        // Running away from objects
        public void scared_state()
        {
            if (scared_level > levels[SCARED])
            {
                state = RUNNING;
                audio_source.clip = audio_clips[state];
                audio_source.loop = true;
                audio_source.Play();
                //return;
            }

            if (target != null)
                agent.SetDestination(target.position);
            if (agent.remainingDistance < 8 - agent.stoppingDistance)
            {
                character.Move(-1 * agent.desiredVelocity, false, false);
                //play_audio();
            }
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
                audio_source.clip = audio_clips[state];
                audio_source.loop = false;
                audio_source.Play();
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

        public void play_audio()
        {
            if (!audio_source.isPlaying)
                audio_source.Play();
        }

        void OnTriggerEnter(Collider c)
        {
                
            if (counter < 2 || c.gameObject != floatingObject.carriedObject)
                return;

            counter = 0f;
            Debug.Log("Enter " + c.gameObject.name);

            change_scare_level(5);
            if (state == 3)
                return;
            play_audio();

            //if (c.transform.tag != "Object")
            //    return;
            //collision_scared(c, 0.1f);
            //if (state == 3)
            //    return;
            //play_audio();
            //switch (state)
            //{
            //    case 0:
            //        collision_scared(c, 0.1f);
            //        play_audio();
            //        break;
            //    case 1:
            //        //collision_scared(c, 0.1f);
            //        play_audio();
            //        break;
            //        //case 2:
            //        //    scared_state();
            //        //    break;
            //        //case 3:
            //        //    running_state();
            //        //    break;
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

        public void collision_scared(Collision c, float scale = 1)
        {
            if (c.gameObject.tag != "Object")
                return;
            if (c.impulse.x == 0 && c.impulse.y == 0 && c.impulse.z == 0) // Collision too light (relativeVelocity)
                return;
            float scare_factor = c.relativeVelocity.magnitude * scale;
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
