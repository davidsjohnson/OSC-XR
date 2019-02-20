namespace OSCXR
{
    using System.Collections.Generic;
    using UnityEngine;

    public class OscParticleTrigger : BaseOscController
    {
        [Header("On Particle Enter Messages")]
        public bool sendParticleEnter = true;

        [Header("On Particle Stay Messages")]
        public bool sendTriggerStay = true;

        [Header("On Particle Exit Messages")]
        public bool sendTriggerExit = true;

        public int maxParticles = 15;

        //Reactor Interactable
        [Header("Unity Game Object Controller")]
        public GameObject controlObject = null;

        private struct ParticleStruct
        {
            public ParticleSystem.Particle p;
            public int id;
        }

        ParticleSystem ps;
        List<ParticleSystem.Particle> pEnter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> pStay = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> pExit = new List<ParticleSystem.Particle>();

        Dictionary<uint, ParticleStruct> tracked = new Dictionary<uint, ParticleStruct>();
        List<int> availableIDs;

        protected override void OnEnable()
        {
            base.OnEnable();

            oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/particleTrigger") : oscAddress;     // Set up name and address
            controlObject = controlObject ?? gameObject;                                                        // Set up Control Object

            ps = GetComponent<ParticleSystem>();
            availableIDs = new List<int>(new int[maxParticles]);
        }

        protected override void ControlRateUpdate()
        {
            base.ControlRateUpdate();

            // Send position of tracked particles
            foreach (var kv in tracked)
            {
                Vector3 localPos = kv.Value.p.position;
                SendOscMessage(string.Format("{0}/stay", OscAddress), kv.Value.id, localPos.x, localPos.y, localPos.z);
                //Debug.Log(string.Format("Stay ID: {0} - Pos: {1}", kv.Value.id, localPos));
            }

        }

        private void OnParticleTrigger()
        {
            int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, pEnter);
            int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, pExit);

            // Remove particles as they exit the object
            foreach (var p in pExit)
            {
                if (tracked.ContainsKey(p.randomSeed)) { 
                    Vector3 localPos = p.position;
                    SendOscMessage(string.Format("{0}/exit", OscAddress), tracked[p.randomSeed].id, localPos.x, localPos.y, localPos.z);
                    //Debug.Log(string.Format("Leaving ID: {0} - Pos: {1}", tracked[p.randomSeed].id, localPos));

                    availableIDs[tracked[p.randomSeed].id] = 0;          // make ID available again
                    tracked.Remove(p.randomSeed);                        // remove particle 
                }
            }
            
            // Add particles as they enter the object
            foreach (var p in pEnter)
            {
                Debug.Log("Entering P: " + p.randomSeed);
                int availID = availableIDs.IndexOf(0);
                if (availID != -1)
                {
                    tracked[p.randomSeed] = new ParticleStruct()
                    {
                        id = availID,
                        p = p
                    };

                    availableIDs[availID] = 1;            // make ID in use

                    Vector3 localPos = p.position;
                    SendOscMessage(string.Format("{0}/enter", OscAddress), availID, localPos.x, localPos.y, localPos.z);
                    //Debug.Log(string.Format("Entering ID: {0} - Pos: {1}", tracked[p.randomSeed].id, localPos));
                }
            }
        }
    }
}