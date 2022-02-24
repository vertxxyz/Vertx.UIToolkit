using UnityEngine.UIElements;

namespace Vertx.UIToolkit
{
	public static class FocusControllerExtensions
	{
		/// <summary>
		/// Checks whether a field that can receive input is focused.
		/// </summary>
		public static bool IsInputFieldFocused(this FocusController focusController) =>
			focusController.focusedElement is VisualElement visualElement && 
			visualElement.Query<TextElement>(null, "unity-text-element--inner-input-field-component").Focused().Build().First() != null;
	}
}