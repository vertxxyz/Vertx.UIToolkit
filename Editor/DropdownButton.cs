using UnityEngine.UIElements;

namespace UnityEditor.UIElements
{
	/// <summary>
	/// A drop-down menu.
	/// </summary>
	public class DropdownButton : TextElement, IToolbarMenuElement
	{
		private readonly TextElement _textElement;

		/// <summary>
		/// The menu that data can be appended to using <see cref="DropdownMenu.AppendAction"/>
		/// </summary>
		public DropdownMenu menu { get; }

		public override string text
		{
			get => base.text;
			set
			{
				_textElement.text = value;
				base.text = value;
			}
		}
		
		public DropdownButton()
		{
			generateVisualContent = null;
			this.AddManipulator(new Clickable(this.ShowMenu));
			menu = new DropdownMenu();

			AddToClassList(BasePopupField<string, string>.ussClassName);
			AddToClassList(BasePopupField<string, string>.inputUssClassName);
			
			_textElement = new TextElement();
			_textElement.AddToClassList(BasePopupField<string, string>.textUssClassName);
			_textElement.pickingMode = PickingMode.Ignore;
			Add(_textElement);
			
			var arrowElement = new VisualElement();
			arrowElement.AddToClassList(BasePopupField<string, string>.arrowUssClassName);
			arrowElement.pickingMode = PickingMode.Ignore;
			Add(arrowElement);
		}

		/// <summary>
		/// Instantiates a <see cref="DropdownButton"/> using the data read from a UXML file.
		/// </summary>
		public new class UxmlFactory : UxmlFactory<DropdownButton, UxmlTraits> { }

		/// <summary>
		/// Defines UxmlTraits for the ToolbarMenu.
		/// </summary>
		public new class UxmlTraits : TextElement.UxmlTraits { }
	}
}