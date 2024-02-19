using System.Numerics;
using Veldrid;

namespace TestEtoVeldrid2;

public partial class VeldridDriver
{
	public void Draw()
	{
		if (!Ready)
		{
			return;
		}

		CommandList.Begin();

		CurrentTime = DateTime.Now;
		if (Animate)
		{
			double radians = Convert.ToDouble((CurrentTime - PreviousTime).TotalMilliseconds / 10.0);
			float degrees = Convert.ToSingle(radians * (System.Math.PI / 180.0));
			degrees *= Speed;

			ModelMatrix *= Matrix4x4.CreateFromAxisAngle(
				new Vector3(0, 0, _direction),
				degrees);
		}

		PreviousTime = CurrentTime;
		CommandList.UpdateBuffer(ModelBuffer, 0, ModelMatrix);

		CommandList.SetFramebuffer(Surface.Swapchain.Framebuffer);

		// These commands differ from the stock Veldrid "Getting Started"
		// tutorial in two ways. First, the viewport is cleared to pink
		// instead of black so as to more easily distinguish between errors
		// in creating a graphics context and errors drawing vertices within
		// said context. Second, this project creates its swapchain with a
		// depth buffer, and that buffer needs to be reset at the start of
		// each frame.
		CommandList.ClearColorTarget(0, RgbaFloat.Pink);
		CommandList.ClearDepthStencil(1.0f);

		CommandList.SetVertexBuffer(0, VertexBuffer);
		CommandList.SetIndexBuffer(IndexBuffer, IndexFormat.UInt16);
		CommandList.SetPipeline(Pipeline);
		CommandList.SetGraphicsResourceSet(0, ModelMatrixSet);

		CommandList.DrawIndexed(
			indexCount: 4,
			instanceCount: 1,
			indexStart: 0,
			vertexOffset: 0,
			instanceStart: 0);

		CommandList.End();

		Surface.GraphicsDevice.SubmitCommands(CommandList);
		Surface.GraphicsDevice.SwapBuffers(Surface.Swapchain);
	}
}
