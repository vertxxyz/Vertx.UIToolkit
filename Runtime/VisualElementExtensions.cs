using UnityEngine.UIElements;

namespace Vertx.UIToolkit
{
	public static class VisualElementExtensions
	{
		/// <summary>
		/// Overrides the inline <see cref="IStyle.display"/> based on the <see cref="visible"/> parameter.
		/// </summary>
		/// <param name="element">The element to apply the display style on.</param>
		/// <param name="visible">If visible <see cref="DisplayStyle.Flex"/> is applied. Otherwise <see cref="DisplayStyle.None"/>.</param>
		public static void SetDisplay(this VisualElement element, bool visible) => element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
	}
}