using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class TestPigFllowPlayer : MonoBehaviour
    {
        private NavMeshAgent2D agent;

        public Transform target;

        void Start()
        {
            this.agent = this.GetComponent<NavMeshAgent2D>();
        }
        
        private void LateUpdate()
        {
            if (this.target == null)
                return;


            this.agent.destination = this.target.transform.position;
        }

    }
}
