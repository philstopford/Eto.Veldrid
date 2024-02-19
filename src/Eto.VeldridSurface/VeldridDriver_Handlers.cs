using Eto.Forms;

namespace TestEtoVeldrid2;

public partial class VeldridDriver
{
	private void Surface_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Keys.C)
		{
			Clockwise = !Clockwise;
			e.Handled = true;
		}
	}

	private void Surface_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Buttons == MouseButtons.Primary)
		{
			Animate = !Animate;
			e.Handled = true;
		}
	}

	private void Surface_MouseWheel(object sender, MouseEventArgs e)
	{
		Speed += (int)e.Delta.Height;
	}

	private void Clock_Elapsed(object sender, EventArgs e) => Surface.Invalidate();
}
