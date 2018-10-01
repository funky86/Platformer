using UnityEngine;
using Platformer.Utils;

namespace Platformer {
    public class Weapon : MonoBehaviour {

        [Header("Components")]
        [SerializeField] Projectile Projectile = null;

        [Header("Mechanics")]
        [SerializeField] float ProjectileVelocity = 1.0f;

        Collider2D IgnoredCollider = null;

        public void SetColliderIgnored(Collider2D collider) {
            IgnoredCollider = collider;
        }

        public void Shoot() {
            Log.Debug("Shoot()");
            var projectile = ObjectsPool.Instance.Spawn(Projectile, transform, false);
            projectile.SetColliderIgnored(IgnoredCollider);
            projectile.Shoot(ProjectileVelocity);
        }
    }
}
