using System;
using Platformer.Utils;
using UnityEngine;

namespace Platformer {

    enum PlayerState {
        OnGround,
        InAir,
        Dead
    }

    enum PlayerAction {
        StartJump,
        EndJump,
        Shoot,
        Land,
        Die
    }

    public class Player : MonoBehaviour {

        [Header("Components")]
        [SerializeField] GameObject Camera = null;
        [SerializeField] Weapon Weapon = null;
        [SerializeField] Collider2D Collider = null;

        [Header("Mechanics")]
        [SerializeField] float JumpForce = 1.0f;
        [SerializeField] AnimationCurve ForceCurve = null;
        [SerializeField] float MaxJumpForceTime = 1.0f;
        [SerializeField] float Velocity = 1.0f;

        Rigidbody2D Body;
        Vector3 InitialPosition;
        Vector3 OffsetFromCamera;

        PlayerState State;

        bool JumpForceActive;
        float JumpForceTime;

        void Awake() {
            Body = GetComponent<Rigidbody2D>();
            InitialPosition = transform.position;
            OffsetFromCamera = Camera.transform.position - transform.position;

            Weapon.SetColliderIgnored(Collider);

            Reset();
        }

        void Reset() {
            Log.Debug("Reset()");
            Body.velocity = Vector2.zero;
            Body.bodyType = RigidbodyType2D.Dynamic;
            transform.position = InitialPosition;
            SetState(PlayerState.OnGround);
        }

        void FixedUpdate() {
            if (State != PlayerState.Dead) {
                if (JumpForceActive) {
                    float forceStep = Mathf.InverseLerp(0.0f, MaxJumpForceTime, JumpForceTime);
                    float forceFactor = ForceCurve.Evaluate(forceStep);
                    float force = JumpForce * forceFactor;
                    Body.AddForce(Vector2.up * force);

                    JumpForceTime += Time.fixedDeltaTime;
                    if (JumpForceTime >= MaxJumpForceTime) {
                        TriggerAction(PlayerAction.EndJump);
                    }
                }

                Body.velocity = new Vector2(Velocity, Body.velocity.y);
            }
        }

        void Update() {
            Camera.transform.position = transform.position + OffsetFromCamera;

            if (Input.GetKeyDown(KeyCode.R)) {
                Reset();
            } else {
                if (Input.GetKey(KeyCode.Space)) {
                    TriggerAction(PlayerAction.StartJump);
                } else if (Input.GetKeyUp(KeyCode.Space)) {
                    TriggerAction(PlayerAction.EndJump);
                }
                if (Input.GetKeyDown(KeyCode.LeftControl)) {
                    TriggerAction(PlayerAction.Shoot);
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.tag == Constants.TagKiller) {
                TriggerAction(PlayerAction.Die);
            } else {
                TriggerAction(PlayerAction.Land);
            }
        }

        void TriggerAction(PlayerAction action) {
            Log.Debug("TriggerAction({0})", action);
            switch (action) {
                case PlayerAction.StartJump:
                    if (State == PlayerState.OnGround) {
                        SetState(PlayerState.InAir);
                        JumpForceActive = true;
                        JumpForceTime = 0.0f;
                    }
                    break;
                case PlayerAction.EndJump:
                    JumpForceActive = false;
                    break;
                case PlayerAction.Land:
                    if (State == PlayerState.InAir) {
                        SetState(PlayerState.OnGround);
                    }
                    break;
                case PlayerAction.Shoot:
                    if (State != PlayerState.Dead) {
                        Weapon.Shoot();
                    }
                    break;
                case PlayerAction.Die:
                    SetState(PlayerState.Dead);
                    GameOver();
                    break;
                default:
                    throw new Exception(string.Format("PlayerAction not handled: {0}", action));
            }
        }

        void SetState(PlayerState state) {
            Log.Debug("SetState({0})", state);
            State = state;
        }

        void GameOver() {
            Log.Debug("GameOver()");
            Body.bodyType = RigidbodyType2D.Static;
            JumpForceActive = false;
        }
    }
}
