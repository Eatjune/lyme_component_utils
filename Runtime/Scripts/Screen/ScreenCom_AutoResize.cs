using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LymeGame.Utils.Component {
	/// <summary>
	/// 屏幕自动设置分辨率组件
	/// <example>鼠标拖拉边框可以重新设置分辨率</example>
	/// </summary>
	[AddComponentMenu("LymeGame/SpriteRenderer/AutoResize")]
	public class ScreenCom_AutoResize : MonoBehaviour {
		public bool IsUse = true;
		public Vector2 ScreenRatio = new Vector2(16f, 9f);

		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left; //最左坐标
			public int Top; //最上坐标
			public int Right; //最右坐标
			public int Bottom; //最下坐标
		}

		[DllImport("user32.dll")]
		public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

		//***********************
		IntPtr myintptr;
		RECT rect;
		float w_h;

		int w;
		int h;
		int x;
		int y;

		private void Start() {
			myintptr = GetActiveWindow();
			w_h = ScreenRatio.x / ScreenRatio.y; //窗口横纵比例
			GetWindowRect(myintptr, ref rect);

			w = rect.Right - rect.Left; //窗口的宽度
			h = rect.Bottom - rect.Top; //窗口的高度
		}

		private void LateUpdate() {
			if (IsUse) SetWindow();
		}

		private void SetWindow() {
			GetWindowRect(myintptr, ref rect);
			w = rect.Right - rect.Left; //窗口的宽度
			h = rect.Bottom - rect.Top; //窗口的高度
			x = rect.Left;
			y = rect.Top;
			if (h == 0 || w == 0) return;
			float z = w / h;
			if (z > w_h + 0.01f || z < w_h - 0.01f) {
				h = (int) (w / w_h);
				MoveWindow(myintptr, x, y, w, h, true);
			}
		}
	}
}