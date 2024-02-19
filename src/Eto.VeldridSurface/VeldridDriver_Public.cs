using Eto.Forms;
using Veldrid;

namespace TestEtoVeldrid2;

public partial class VeldridDriver
{
	public Shader VertexShader { get; private set; }
	public Shader FragmentShader { get; private set; }

	public bool Animate { get; set; } = true;

	public bool Clockwise
	{
		get { return _direction == 1 ? true : false; }
		set { _direction = value ? 1 : -1; }
	}

	public int Speed { get; set; } = 1;

	public UITimer Clock { get; } = new UITimer();

}
