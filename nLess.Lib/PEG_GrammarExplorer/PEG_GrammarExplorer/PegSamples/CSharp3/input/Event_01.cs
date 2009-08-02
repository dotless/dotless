class Control: Component
{
	// Unique keys for events
	static readonly object mouseDownEventKey = new object();
	static readonly object mouseUpEventKey = new object();
	// Return event handler associated with key
	protected Delegate GetEventHandler(object key) { }
	// Add event handler associated with key
	protected void AddEventHandler(object key, Delegate handler) { }
	// Remove event handler associated with key
	protected void RemoveEventHandler(object key, Delegate handler) { }
	// MouseDown event
	public event MouseEventHandler MouseDown {
		add { AddEventHandler(mouseDownEventKey, value); }
		remove { RemoveEventHandler(mouseDownEventKey, value); }
	}
	// MouseUp event
	public event MouseEventHandler MouseUp {
		add { AddEventHandler(mouseUpEventKey, value); }
		remove { RemoveEventHandler(mouseUpEventKey, value); }
	}
	// Invoke the MouseUp event
	protected void OnMouseUp(MouseEventArgs args) {
		MouseEventHandler handler; 
		handler = (MouseEventHandler)GetEventHandler(mouseUpEventKey);
		if (handler != null)
			handler(this, args);
	}
}
