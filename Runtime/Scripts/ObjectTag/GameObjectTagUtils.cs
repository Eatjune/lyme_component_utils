namespace LymeGame.Utils.Component {
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Component = UnityEngine.Component;

	public static class GameObjectTagUtils {
		/// <summary>
		/// 获取该游戏对象下第一个包含指定tag组件的对象
		/// </summary>
		/// <param name="obj">游戏对象</param>
		/// <param name="tag">指定标签文本</param>
		/// <param name="includeInactive"></param>
		public static GameObject GetObjectInChildrenByTag(this Component obj, string tag, bool includeInactive = false) {
			return GetObjectInChildrenByTag(obj.gameObject, tag, includeInactive);
		}

		/// <summary>
		/// 获取该游戏对象下第一个包含指定tag组件的对象
		/// </summary>
		/// <param name="obj">游戏对象</param>
		/// <param name="tag">指定标签文本</param>
		/// <param name="includeInactive"></param>
		public static GameObject GetObjectInChildrenByTag(this GameObject obj, string tag, bool includeInactive = false) {
			foreach (var componentsInChild in obj.GetComponentsInChildren<GameObjectCom_Tag>(includeInactive)) {
				if (componentsInChild.Tags.Contains(tag)) {
					return componentsInChild.gameObject;
				}
			}

			return null;
		}

		/// <summary>
		/// 获取该游戏对象下所有包含指定tag组件的对象
		/// </summary>
		/// <param name="obj">游戏对象</param>
		/// <param name="tag">指定标签文本</param>
		/// <param name="includeInactive"></param>
		public static GameObject[] GetObjectsInChildrenByTag(this Component obj, string tag, bool includeInactive = false) {
			return GetObjectsInChildrenByTag(obj.gameObject, tag, includeInactive);
		}

		/// <summary>
		/// 获取该游戏对象下所有包含指定tag组件的对象
		/// </summary>
		/// <param name="obj">游戏对象</param>
		/// <param name="tag">指定标签文本</param>
		/// <param name="includeInactive"></param>
		public static GameObject[] GetObjectsInChildrenByTag(this GameObject obj, string tag, bool includeInactive = false) {
			var list = new List<GameObject>();
			foreach (var componentsInChild in obj.GetComponentsInChildren<GameObjectCom_Tag>(includeInactive)) {
				if (componentsInChild.Tags.Contains(tag)) {
					list.Add(componentsInChild.gameObject);
				}
			}

			return list.ToArray();
		}
	}
}