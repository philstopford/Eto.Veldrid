using Eto.Forms;
using System;
using System.Threading;

namespace Quilt;

public partial class MainForm
{
    private void pPosChanged(object sender, EventArgs e)
    {
        if (openGLErrorReported)
        {
            return;
        }

        pMoveCamera((float)num_viewportX.Value, (float)num_viewportY.Value);
    }

    private void pMoveCamera(float x, float y)
    {
        viewPort.ovpSettings.setCameraPos(x, y);
    }

    private void pZoomChanged(object sender, EventArgs e)
    {
        if (openGLErrorReported)
        {
            return;
        }

        double zoomValue = Convert.ToDouble(num_viewportZoom.Value);
        if (zoomValue < num_viewportZoom.MinValue)
        {
            zoomValue = num_viewportZoom.MinValue;
        }

        viewPort.ovpSettings.setZoomFactor((float)(1.0f / zoomValue));
    }

    private void pViewportUpdateHost()
    {
        double value = 1.0f / viewPort.ovpSettings.getZoomFactor();
        if (value < num_viewportZoom.MinValue)
        {
            value = num_viewportZoom.MinValue;
        }
        num_viewportZoom.Value = value;
        num_viewportX.Value = viewPort.ovpSettings.getCameraX();
        num_viewportY.Value = viewPort.ovpSettings.getCameraY();
        viewPort.ovpSettings.selectedIndex = listBox_entries.SelectedIndex;
    }

    private void pViewportSelectionFunc(int index)
    {
        Application.Instance.Invoke(() =>
        {
            listBox_entries.SelectedIndex = index;
        });
    }
}