using UnityEngine;


//This is the base behavior for all items, used ot give different items different behaviors
namespace ItemNamespace
{
    public abstract class ItemBaseBehaviour : MonoBehaviour
    {
        protected ItemBase belongingTo;
        public abstract void Use(ItemBase itemBase);
        public abstract void StopAnimation();
        public void SetBelongingTo(ItemBase _belongingTo) { belongingTo = _belongingTo; }
    }
}
