using UnityEngine;

namespace Homebrew
{
    public class ManagerUpdateComponent : MonoBehaviour
    {

        private ManagerUpdate mng;

        public void Setup(ManagerUpdate mng)
        {
            this.mng = mng;
        }


        private void Update()
        {
            mng.Tick();
        }

        private void FixedUpdate()
        {
            mng.TickFixed();
        }


        private void LateUpdate()
        {
            mng.TickLate();
        }


    }
}