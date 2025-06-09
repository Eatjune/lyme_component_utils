namespace LymeGame.Utils.Component {
	using UnityEngine;

	/// <summary>
	/// 自动根据设定的最小／最大尺寸限制，缩放指定的 SpriteRenderer。
	/// </summary>
	[AddComponentMenu("LymeGame/SpriteRenderer/SizeClamp")]
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteCom_SizeClamp : MonoBehaviour {
		[Header("尺寸范围（世界单位）")]
		public Vector2 MinSize = new Vector2(1f, 1f);

		public Vector2 MaxSize = new Vector2(3f, 3f);

		public bool ResizeWhenEnable = true;

		private SpriteRenderer m_spriteRenderer;

		protected void Awake() {
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		protected void OnEnable() {
			if (ResizeWhenEnable) {
				ClampSize();
			}
		}

		/// <summary>
		/// 根据当前 bounds 大小，调整 transform.localScale 使其在指定范围内。
		/// </summary>
		public void ClampSize() {
			if (!m_spriteRenderer) m_spriteRenderer = GetComponent<SpriteRenderer>();
			Vector2 currentSize = m_spriteRenderer.bounds.size;

			// 计算放大／缩小比例
			var scaleUpX = MinSize.x / currentSize.x;
			var scaleUpY = MinSize.y / currentSize.y;
			var scaleDownX = MaxSize.x / currentSize.x;
			var scaleDownY = MaxSize.y / currentSize.y;

			var finalScale = 1f;

			// 当前小于最小值，需要放大
			if (currentSize.x < MinSize.x || currentSize.y < MinSize.y) {
				finalScale = Mathf.Max(scaleUpX, scaleUpY);
			}
			// 当前大于最大值，需要缩小
			else if (currentSize.x > MaxSize.x || currentSize.y > MaxSize.y) {
				finalScale = Mathf.Min(scaleDownX, scaleDownY);
			}

			// 应用缩放
			transform.localScale = transform.localScale * finalScale;
		}
	}
}