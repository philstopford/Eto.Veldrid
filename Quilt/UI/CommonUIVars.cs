using Eto.Forms;
using Eto.Veldrid;
using VeldridEto;

namespace Quilt;

public partial class MainForm
{
    private const int viewportSize = 752;

    private ListBox listBox_entries;
    private TextBox text_patternElement;
    private Panel settings;
    private TableLayout main_tl; // the non-viewport layout container.
    private TableLayout left_tl; // left-hand side table layout container within main_tl
    private TableLayout right_tl; // right-hand side table layout container within main_tl

    
    private Label lbl_viewportZoom, lbl_viewportPos;
    private NumericStepper num_viewportZoom, num_viewportX, num_viewportY;
    private VeldridSurface vSurface;
    private VeldridDriver viewPort;
    
    private CheckBox checkBox_suspendBuild, checkBox_showInput;
    
    private TabControl tabControl;
    private TabPage quiltPage;

    private Panel quiltUISplitter;
    
    private bool openGLErrorReported;

    private TableLayout settings_tl;
}