using UnityEngine;
using Platformer.Utils;

namespace Platformer {
    public class Projectile : PoolObject {

        [Header("Components")]
        [SerializeField] Rigidbody2D Body = null;
        [SerializeField] Collider2D Collider = null;

        [Header("Mechanics")]
        [SerializeField] float TimeTillDespawn = 1.0f;

        float Timer = 0;

        public void SetColliderIgnored(Collider2D collider) {
            Physics2D.IgnoreCollision(Collider, collider);
        }

        void OnCollisionEnter2D(Collision2D collision) {
            ObjectsPool.Instance.Despawn(this);
        }

        public void Shoot(float velocity) {
            Body.velocity = Vector3.right * velocity;
        }

        void FixedUpdate() {
            Timer += Time.fixedDeltaTime;
            if (Timer >= TimeTillDespawn) {
                ObjectsPool.Instance.Despawn(this);
            }
        }
    }
}
