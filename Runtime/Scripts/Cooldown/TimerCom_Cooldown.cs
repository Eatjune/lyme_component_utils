using Unity.Collections;
using UnityEngine;

namespace LymeGame.Utils.Component {
	/// <summary>
	/// 冷却组件
	/// </summary>
	[AddComponentMenu("LymeGame/Timer/Cooldown")]
	public class TimerCom_Cooldown : MonoBehaviour {
		public enum CooldownStates {
			Idle,
			Consuming,
			PauseOnEmpty,
			Refilling
		}

		public enum TimeTypeEnum {
			Unscaled,
			Scaled,
		}

		/// <summary>
		/// 是否激活
		/// </summary>
		public bool Active = false;

		/// <summary>
		/// 冷却时间前延迟时间
		/// </summary>
		public float DelayDuration = 0f;

		/// <summary>
		/// 延迟后暂停时间
		/// </summary>
		public float PauseOnEmptyDuration = 1f;

		/// <summary>
		/// 冷却时间
		/// </summary>
		public float RefillDuration = 1f;

		/// <summary>
		/// 冷却是否可以被打断重置
		/// </summary>
		public bool CanInterruptRefill = true;

		public TimeTypeEnum TimeType = TimeTypeEnum.Scaled;

		[ReadOnly]
		public CooldownStates CooldownState = CooldownStates.Idle;

		[ReadOnly]
		///进入下个状态的当前剩余时间
		public float CurrentDurationLeft;

		protected float _emptyReachedTimestamp = 0f;

		/// <summary>
		/// 初始化
		/// </summary>
		public virtual void Initialization() {
			CurrentDurationLeft = DelayDuration;
			CooldownState = CooldownStates.Idle;
			_emptyReachedTimestamp = 0f;
		}

		/// <summary>
		/// 是否准备完毕
		/// 无冷却，或冷却可以打断状态
		/// </summary>
		public virtual bool IsReady() {
			if (!Active) {
				return true;
			}

			if (CooldownState == CooldownStates.Idle) {
				return true;
			}

			if ((CooldownState == CooldownStates.Refilling) && (CanInterruptRefill)) {
				return true;
			}

			return false;
		}

		/// <summary>
		/// 进入冷却(包含冷却前消耗时间【例如攻击动画时间】）
		/// </summary>
		public virtual void Enter() {
			if (IsReady()) {
				CooldownState = CooldownStates.Consuming;
			}
		}

		/// <summary>
		/// 停止冷却
		/// </summary>
		public virtual void Stop() {
			if (CooldownState == CooldownStates.Consuming) {
				CooldownState = CooldownStates.PauseOnEmpty;
			}
		}

		/// <summary>
		/// 当前冷却进程
		/// </summary>
		public float Progress {
			get {
				if (!Active) {
					return 1f;
				}

				if (CooldownState == CooldownStates.Consuming || CooldownState == CooldownStates.PauseOnEmpty) {
					return 0f;
				}

				if (CooldownState == CooldownStates.Refilling) return /*Mathf.Clamp01(*/CurrentDurationLeft / RefillDuration /*)*/;

				return 1f; // refilled
			}
		}

		protected virtual void Update() {
			if (!Active) {
				return;
			}

			var time = 0f;
			var deltaTime = 0f;
			if (TimeType == TimeTypeEnum.Scaled) {
				time = Time.time;
				deltaTime = Time.deltaTime;
			} else {
				time = Time.unscaledTime;
				deltaTime = Time.unscaledDeltaTime;
			}

			switch (CooldownState) {
				case CooldownStates.Idle: break;

				case CooldownStates.Consuming:
					CurrentDurationLeft = CurrentDurationLeft - deltaTime;
					if (CurrentDurationLeft <= 0f) {
						CurrentDurationLeft = 0f;
						_emptyReachedTimestamp = time;
						CooldownState = CooldownStates.PauseOnEmpty;
					}

					break;

				case CooldownStates.PauseOnEmpty:
					if (time - _emptyReachedTimestamp >= PauseOnEmptyDuration) {
						CooldownState = CooldownStates.Refilling;
						CurrentDurationLeft = 0;
					}

					break;

				case CooldownStates.Refilling:
					CurrentDurationLeft += deltaTime;
					if (CurrentDurationLeft >= RefillDuration) {
						CurrentDurationLeft = DelayDuration;
						CooldownState = CooldownStates.Idle;
					}

					break;
			}
		}
	}
}