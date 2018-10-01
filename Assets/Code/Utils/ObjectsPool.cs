using UnityEngine;

namespace Platformer.Utils {
    public class ObjectsPool {

        static ObjectsPool Instance_;

        ObjectsPool() {
        }

        public static ObjectsPool Instance {
            get {
                if (Instance_ == null) {
                    Instance_ = new ObjectsPool();
                }
                return Instance_;
            }
        }

        public T Spawn<T>(T obj, Transform transform = null, bool worldPositionStays = true) where T : PoolObject {
            var instance = Object.Instantiate(obj, transform, worldPositionStays);
            return instance;
        }

        public void Despawn<T>(T instance) where T : PoolObject {
            Object.Destroy(instance.gameObject);
        }
    }
}
