using System.Collections.Generic;
using UnityEngine;

namespace EnhancedGravTrapZero.Monobehaviours
{
    internal class AllowedObjectManager : MonoBehaviour
    {
        public int techTypeListIndex
        {
            get => _techTypeListIndex;
            set => _techTypeListIndex = value % Main_Plugin.AllowedTypes.Count;
        }
        private int _techTypeListIndex = 0;

        public string GetCurrentListName()
        {
            return Main_Plugin.AllowedTypes[techTypeListIndex].name;
        }

        public bool IsValidTarget(GameObject obj) //Called on each frame for each attracted object
        {
            bool result = false;
            TechType techType = GetObjectTechType(obj);
            Pickupable component = obj.GetComponent<Pickupable>();
            List<TechType> allowedTypes = Main_Plugin.AllowedTypes[techTypeListIndex].techTypes;

            if(techType == TechType.EscapePod)
            {
                return false;
            }

            if (!component || !component.attached)
            {
                for (int i = 0; i < allowedTypes.Count; i++)
                {
                    if (allowedTypes[i] == techType)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private TechType GetObjectTechType(GameObject obj)
        {
            if (obj.TryGetComponent<Pickupable>(out var p))
                return p.GetTechType();

            return CraftData.GetTechType(obj);
        }

        public void HandleAttracted(GameObject obj, bool added)
        {
            if (added)
            {
                if (obj.TryGetComponent<Crash>(out var crash))
                {
                    crash.AttackLastTarget(); 
                }
                else if (obj.TryGetComponent<SinkingGroundChunk>(out var chunk))
                {
                    Destroy(chunk);

                    var collider = obj.AddComponent<BoxCollider>();
                    collider.size = new Vector3(0.736f, 0.51f, 0.564f);
                    collider.center = new Vector3(0.076f, 0.224f, 0.012f);

                    obj.transform.Find("models").localPosition = Vector3.zero;
                }
				else if (obj.GetComponent<Pickupable>()?.GetTechType() == TechType.JeweledDiskPiece)
				{
					var rb = obj.GetComponent<Rigidbody>();
					rb.mass = 1f;
					rb.useGravity = false;

					obj.EnsureComponent<WorldForces>();
				}

            }
        }
    }
}
