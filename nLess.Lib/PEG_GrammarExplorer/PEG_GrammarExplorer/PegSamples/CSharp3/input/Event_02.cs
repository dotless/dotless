public delegate void EventHandler(object sender, EventArgs e);
public class Button: Control
{
	public event EventHandler Click;
}
public class LoginDialog: Form
{
	Button OkButton;
	Button CancelButton;
	public LoginDialog() {
		OkButton = new Button( );
		OkButton.Click += new EventHandler(OkButtonClick);
		CancelButton = new Button( );
		CancelButton.Click += new EventHandler(CancelButtonClick);
	}
	void OkButtonClick(object sender, EventArgs e) {
		// Handle OkButton.Click event
	}
	void CancelButtonClick(object sender, EventArgs e) {
		// Handle CancelButton.Click event
	}
}
