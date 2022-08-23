using System.Linq;
using DG.Tweening;
using Managers;
using Plugins.Renatus.Util.State_Machine;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Entities.Enemies.Dagon_Statue {
    public class DagonStatueAttackState : IDagonState {
        
        #region Fields, Properties, & Constructors 
        
        private const float PredictedPositionTimeAhead = 1.4f;
        private const float Speed = 12;

        public DagonStatue DagonStatue { get; set; }
        public Projectile Projectile { get; set; }

        private float _timer;
        
        public DagonStatueAttackState(DagonStatue dagonStatue) {
            DagonStatue = dagonStatue;
            DagonStatue.ProjectilesPool.Initialize();
            
            DagonStatue.ProjectilesPool.PoolObjects.ToList().ForEach(i => {
                var projectile = i.GetComponent<Projectile>();
                projectile.Initialize(OnProjectileHit, OnProjectileMissed);
                projectile.IgnoreCollision(DagonStatue.StatueCollider);
            });
        }
        
        #endregion

        #region Interface Functions

        public void Execute() {
            
            _timer += Time.deltaTime;
            
            if (_timer < 1 / DagonStatue.ProjectileFrequency) return;

            Shoot();
            _timer = 0;
        }

        public void Enter() {
            _timer = 1 / DagonStatue.ProjectileFrequency;
            
            DagonStatue.OnAttackFX.Play();
        }

        public void Exit() {
            DagonStatue.ProjectilesPool.Enqueue(DagonStatue.Projectile.gameObject);
        }

        #endregion

        #region Private Functions
        
        /// <summary> Dequeues a projectile from the Pool of available projectiles, and shoots it at a predicted player position. </summary>
        private void Shoot() {
            var prediction = DagonStatue.GameManager.Player.GetPredictedPosition(PredictedPositionTimeAhead);

            var projectile = DagonStatue.Projectile = DagonStatue.ProjectilesPool.Dequeue().GetComponent<Projectile>();
            projectile.transform.localScale = Vector3.zero;
            projectile.transform.DOScale(1, 0.65f).SetEase(Ease.InOutElastic).OnComplete(() => projectile.Shoot(prediction, Speed));
            
            DagonStatue.EnergyGatheringFX.Play();
        }
        
        /// <summary> When the projectile hits a target, we return the projectile to a queue and- TODO?? </summary>
        private void OnProjectileHit(Collision collision) {
            DagonStatue.ProjectilesPool.Enqueue(DagonStatue.Projectile.gameObject);

            if (collision.collider.CompareTag(GameManager.PlayerTag)) {
                Debug.Log("Player dies!");
            }
        }

        private void OnProjectileMissed() {
            DagonStatue.ProjectilesPool.Enqueue(DagonStatue.Projectile.gameObject);
        }
        
        #endregion
 }
}