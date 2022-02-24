# Vertx.UIToolkit
Improvements/additions made to UIToolkit fields.

#### `BetterFieldMouseDragger`
`Apply()`  
Provides dragging on a visual element to change a value field.  
This dragger uses the distance travelled, not the current mouse delta to modify the values.  
This means it can be used in combination with a field that supports rounding.  
See [this](https://forum.unity.com/threads/fieldmousedragger-improvement.1225911/) forum thread for updates.  
To use `Apply` on composite fields `T` has to be the manipulated type. Eg. for `Vector3Field` that is `float`.  
```csharp
BetterFieldMouseDragger<float>.Apply<Vector3Field, Vector3, FloatField>(vector3Field);
```

#### `VisualElementExtensions`
`VisualElement.SetDisplay`  
Sets the `display` inline style. Avoid repeatedly doing `style.display = value ? DisplayStyle.Flex : DisplayStyle.None;`.

#### `FocusControllerExtensions`
`FocusController.IsInputFieldFocused`  
Checks whether a field that can receive input is focused.  


## Installation

<details>
<summary>Add from OpenUPM <em>| via scoped registry, recommended</em></summary>

This package is available on OpenUPM: https://openupm.com/packages/com.vertx.uitoolkit

To add it the package to your project:

- open `Edit/Project Settings/Package Manager`
- add a new Scoped Registry:
  ```
  Name: OpenUPM
  URL:  https://package.openupm.com/
  Scope(s): com.vertx
  ```
- click <kbd>Save</kbd>
- open Package Manager
- click <kbd>+</kbd>
- select <kbd>Add from Git URL</kbd>
- paste `com.vertx.uitoolkit`
- click <kbd>Add</kbd>

</details>

<details>
<summary>Add from GitHub | <em>not recommended, no updates through UPM</em></summary>

You can also add it directly from GitHub on Unity 2019.4+. Note that you won't be able to receive updates through Package Manager this way, you'll have to update manually.

- open Package Manager
- click <kbd>+</kbd>
- select <kbd>Add from Git URL</kbd>
- paste `https://github.com/vertxxyz/Vertx.UIToolit.git`
- click <kbd>Add</kbd>  
  **or**
- Edit your `manifest.json` file to contain `"com.vertx.uitoolkit": "https://github.com/vertxxyz/Vertx.UIToolkit.git"`,

To update the package with new changes, remove the lock from the `packages-lock.json` file.
</details>