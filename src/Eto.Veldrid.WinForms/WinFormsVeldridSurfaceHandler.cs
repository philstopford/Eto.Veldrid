﻿using Eto.Veldrid;
using Eto.Veldrid.WinForms;
using Eto.WinForms.Forms;
using System;
using System.Runtime.InteropServices;
using Veldrid;
using Veldrid.OpenGL;

[assembly: Eto.ExportHandler(typeof(VeldridSurface), typeof(WinFormsVeldridSurfaceHandler))]

namespace Eto.Veldrid.WinForms
{
	public class WinFormsVeldridSurfaceHandler : WindowsControl<WinFormsVeldridUserControl, VeldridSurface, VeldridSurface.ICallback>, VeldridSurface.IHandler
	{
		public int RenderWidth => Control.Width;
		public int RenderHeight => Control.Height;

		public WinFormsVeldridSurfaceHandler()
		{
			Control = new WinFormsVeldridUserControl();

			Control.HandleCreated += Control_HandleCreated;
		}

		public Swapchain CreateSwapchain()
		{
			Swapchain swapchain;

			if (Widget.Backend == GraphicsBackend.OpenGL)
			{
				swapchain = Widget.GraphicsDevice.MainSwapchain;
			}
			else
			{
				// To embed Veldrid in an Eto control, these platform-specific
				// versions of CreateSwapchain use the technique outlined here:
				//
				//   https://github.com/mellinoe/veldrid/issues/155
				//
				var source = SwapchainSource.CreateWin32(
					Control.Handle,
					Marshal.GetHINSTANCE(typeof(VeldridSurface).Module));

				swapchain = Widget.GraphicsDevice.ResourceFactory.CreateSwapchain(
					new SwapchainDescription(
						source,
						(uint)RenderWidth,
						(uint)RenderHeight,
						Widget.GraphicsDeviceOptions.SwapchainDepthFormat,
						Widget.GraphicsDeviceOptions.SyncToVerticalBlank,
						Widget.GraphicsDeviceOptions.SwapchainSrgbFormat));
			}

			return swapchain;
		}

		private void Control_HandleCreated(object sender, EventArgs e)
		{
			OpenGLPlatformInfo glInfo = null;

			if (Widget.Backend == GraphicsBackend.OpenGL)
			{
				Control.CreateOpenGLContext();

				glInfo = new OpenGLPlatformInfo(
					   VeldridGL.GetGLContextHandle(),
					   VeldridGL.GetProcAddress,
					   Control.MakeCurrent,
					   VeldridGL.GetCurrentContext,
					   VeldridGL.ClearCurrentContext,
					   VeldridGL.DeleteContext,
					   VeldridGL.SwapBuffers,
					   VeldridGL.SetVSync,
					   VeldridGL.SetSwapchainFramebuffer,
					   VeldridGL.ResizeSwapchain);
			}

			Callback.InitializeGraphicsBackend(Widget, glInfo);

			Control.HandleCreated -= Control_HandleCreated;
		}

		public override void AttachEvent(string id)
		{
			switch (id)
			{
				case VeldridSurface.DrawEvent:
					Control.Paint += (sender, e) => Callback.OnDraw(Widget, EventArgs.Empty);
					break;
				default:
					base.AttachEvent(id);
					break;
			}
		}
	}
}