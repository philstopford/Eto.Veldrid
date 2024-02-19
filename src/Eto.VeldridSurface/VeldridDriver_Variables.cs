using Eto.Veldrid;
using System.Numerics;
using Veldrid;

namespace TestEtoVeldrid2;

public partial class VeldridDriver
{
	private VeldridSurface _surface;
	public VeldridSurface Surface
	{
		get { return _surface; }
		set
		{
			_surface = value;

			Surface.MouseDown += Surface_MouseDown;
			Surface.KeyDown += Surface_KeyDown;
			Surface.MouseWheel += Surface_MouseWheel;

			Surface.Draw += (sender, e) => Draw();
		}
	}
	private CommandList CommandList { get; set; }
	private DeviceBuffer IndexBuffer { get; set; }
	private DeviceBuffer ModelBuffer { get; set; }
	private DeviceBuffer VertexBuffer { get; set; }
	private Pipeline Pipeline { get; set; }
	private Matrix4x4 ModelMatrix { get; set; } = Matrix4x4.Identity;
	private ResourceSet ModelMatrixSet { get; set; }

	private bool Ready = false;

	private DateTime CurrentTime;
	private DateTime PreviousTime = DateTime.Now;

	private int _direction = 1;
}
