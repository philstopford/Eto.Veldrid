using System;
using System.Runtime.InteropServices;

namespace Eto.Veldrid.Gtk
{
	internal static class WaylandInterop
	{
		const string linux_libgdk_x11_name = "libgdk-3.so.0";

		[DllImport(linux_libgdk_x11_name)]
		static public extern IntPtr gdk_wayland_window_get_wl_surface(IntPtr gdkWindow);
		[DllImport(linux_libgdk_x11_name)]
		static public extern IntPtr gdk_wayland_display_get_wl_display(IntPtr gdkDisplay);


	}
}
