using UnityEngine;

namespace Interfaces
{
    public interface IBuildable
    {
        public void StartBuild();
        public void Proceed();
        protected void CompleteBuild();
    }


    public interface IPlayerMovable
    {
        public Vector3 GetDestinationPos();
    }


    public interface IDamagable
    {
        /// <summary>
        /// 引数分のダメージを受ける。
        /// </summary>
        /// <param name="damageAmount"> 受けるダメージ量 </param>
        public void TakeDamage(int damageAmount);

        /// <summary>
        /// 移動速度が引数割合分遅くなる。
        /// </summary>
        /// <param name="slowRatio_percentage"></param>
        public void TakeSlow(float slowRatio_percentage);
    }
}