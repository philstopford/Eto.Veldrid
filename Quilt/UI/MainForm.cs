using Eto.Drawing;
using Eto.Forms;
using VeldridEto;
using Eto.Veldrid;
using Veldrid;

namespace Quilt;

/// <summary>
/// Your application's main form
/// </summary>
public partial class MainForm : Form
{
    private QuiltContext quiltContext;
    
    private OVPSettings ovpSettings;

    private object drawingLock;

    private CommonVars commonVars;
    
    public MainForm(QuiltContext _quiltContext)
    {
        pMainForm(_quiltContext);
    }

    private void pMainForm(QuiltContext _quiltContext)
    {
        pUI(_quiltContext);
    }

    private bool _veldridReady;

    private bool VeldridReady
    {
        get => _veldridReady;
        set
        {
            _veldridReady = value;

            pSetUpVeldrid();
        }
    }

    private bool _formReady;

    private bool FormReady
    {
        get => _formReady;
        set
        {
            _formReady = value;

            pSetUpVeldrid();
        }
    }

    private void pSetUpVeldrid()
    {
        if (!(FormReady && VeldridReady))
        {
            return;
        }

        viewPort.Clock.Start();
    }
    private void pUI(QuiltContext _quiltContext)
    {
        quiltContext = _quiltContext ?? new QuiltContext("", VeldridSurface.PreferredBackend);

        Shown += (_, _) => FormReady = true;

        commonVars = new CommonVars(ref quiltContext);
        
        ovpSettings = new OVPSettings();
        ovpSettings.drawDrawn(true);
        ovpSettings.drawFilled(quiltContext.filledPolygons);
        ovpSettings.drawPoints(quiltContext.drawPoints);
        ovpSettings.setZoomStep(quiltContext.openGLZoomFactor);
        ovpSettings.aA(quiltContext.AA);

        Title = commonVars.titleText;

        // Compensate for menu.
        MinimumSize = new Size(750, 450);
        Size = new Size(1250,760);

        Panel vp = new();

        TableLayout vp_tl = new();

        vp.Content = vp_tl;

        vp_tl.Rows.Add(new TableRow());

        pPatternSettingsUI();
        Scrollable settings_sc = new() {Content = settings};

        quiltUISplitter = new Panel
        {
            Content = new Splitter
            {
                Orientation = Orientation.Horizontal,
                FixedPanel = SplitterFixedPanel.Panel1,
                Panel1 = settings_sc,
                Panel2 = vp
                // Panel1MinimumSize = settings.Size.Width,
            }
        };

        tabControl = new TabControl();
        quiltPage = new TabPage {Text = "Quilt"};
        tabControl.Pages.Add(quiltPage);
        quiltPage.Content = quiltUISplitter;

        Content = tabControl;
        
        GraphicsDeviceOptions options = new(
            false,
            Veldrid.PixelFormat.R32_Float,
            false,
            ResourceBindingModel.Improved);

        vSurface = new VeldridSurface(quiltContext.backend, options);
        
        viewPort = new VeldridDriver(ref ovpSettings, ref vSurface)
        {
            Surface = vSurface,
        };

        vSurface.VeldridInitialized += (_, _) =>
        {
            viewPort.SetUpVeldrid();
            
            VeldridReady = true;
        };

        vSurface.Size = new Size(viewportSize, viewportSize);
        viewPort.updateHostFunc = pViewportUpdateHost;
        viewPort.updateHostSelectionFunc = pViewportSelectionFunc;

        const string viewportToolTipText = "(w/a/s/d) to navigate\r\n(r) to reset\r\n(n/m) to zoom\r\n(f) to freeze/thaw\r\n(x) to zoom extents\r\n(z) to zoom selected";
        vp.ToolTip = viewportToolTipText;

        commonVars.titleText += " " + vSurface.Backend;

        Title = commonVars.titleText;

        vp_tl.Rows[0].Cells.Add(new TableCell { Control = vSurface });

    }
    
    private void pPatternSettingsUI()
    {
        settings_tl = new TableLayout();
        Panel p = new() {Content = settings_tl};

        settings = new Panel {Size = new Size(540, 270), Content = p};
            
        main_tl = new TableLayout();
        Panel pMain = new() {Content = main_tl};

        TableRow main_tr = new();
        main_tl.Rows.Add(main_tr);
        TableCell main_tc0 = new();
        main_tr.Cells.Add(main_tc0);

        left_tl = new TableLayout();
        main_tc0.Control = left_tl;

        right_tl = new TableLayout();
        Scrollable right_s = new() {Content = right_tl};

        TableCell main_tc1 = new() {Control = right_s};
        main_tr.Cells.Add(main_tc1);

        settings_tl.Rows.Add(new TableRow { ScaleHeight = true });
        settings_tl.Rows[^1].Cells.Add(new TableCell { Control = pMain });

        pPatternSettingsUI_2();
    }

    private void pPatternSettingsUI_2()
    {
        TableRow left_layout_tr = new();
        left_tl.Rows.Add(left_layout_tr);

        Panel layout_pnl = new();
        TableLayout layout_tl = new();
        layout_pnl.Content = TableLayout.AutoSized(layout_tl);
        left_layout_tr.Cells.Add(new TableCell { Control = layout_pnl });

        layout_tl.Rows.Add(new TableRow());
        
        TableRow left_tr0 = new() {ScaleHeight = true};
        left_tl.Rows.Add(left_tr0);

        listBox_entries = new ListBox();

        TableCell left_tr0_0 = new();
        left_tr0.Cells.Add(left_tr0_0);

        left_tr0_0.Control = null;

        TableLayout settings_table = new();
        Panel lowerLeftContainer = new() {Content = settings_table};

        TableRow left_tr1 = new();
        left_tl.Rows.Add(left_tr1);
        TableCell left_tr1_0 = new();
        left_tr1.Cells.Add(left_tr1_0);
        left_tr1_0.Control = lowerLeftContainer;

        settings_table.Rows.Add(new TableRow());

        
        settings_table.Rows.Add(new TableRow());
        TableLayout row2tl = new();
        Panel row2panel = new() {Content = row2tl};
        settings_table.Rows[^1].Cells.Add(new TableCell { Control = row2panel });

        row2tl.Rows.Add(new TableRow());

        lbl_viewportZoom = new Label {Text = "Zoom"};

        int nW = 55;
        num_viewportZoom = new NumericStepper {Value = 1.0f, DecimalPlaces = 2, MinValue = 1E-2};
        num_viewportZoom.LostFocus += pZoomChanged;

        TableLayout row2leftTL = new();
        Panel row2left = new() {Content = row2leftTL};
        row2tl.Rows[^1].Cells.Add(new TableCell { Control = row2left });

        row2leftTL.Rows.Add(new TableRow());
        row2leftTL.Rows[^1].Cells.Add(new TableCell { Control = lbl_viewportZoom });

        row2leftTL.Rows[^1].Cells.Add(new TableCell { Control = num_viewportZoom });

        row2leftTL.Rows[^1].Cells.Add(new TableCell { Control = null, ScaleWidth = true });
        
        row2tl.Rows[^1].Cells.Add(new TableCell { Control = null });

        settings_table.Rows.Add(new TableRow());
        TableLayout row3tl = new();
        Panel row3panel = new() {Content = row3tl};
        settings_table.Rows[^1].Cells.Add(new TableCell { Control = row3panel });

        row3tl.Rows.Add(new TableRow());

        lbl_viewportPos = new Label {Text = "Position"};

        row3tl.Rows[^1].Cells.Add(new TableCell { Control = lbl_viewportPos });

        num_viewportX = new NumericStepper
        {
            Value = 0.0f, DecimalPlaces = 2, ToolTip = "X coordinate at center of viewport."
        };
        num_viewportX.LostFocus += pPosChanged;

        row3tl.Rows[^1].Cells.Add(new TableCell { Control = num_viewportX });

        num_viewportY = new NumericStepper
        {
            Value = 0.0f, DecimalPlaces = 2, ToolTip = "Y coordinate at center of viewport."
        };
        num_viewportY.LostFocus += pPosChanged;

        row3tl.Rows[^1].Cells.Add(new TableCell { Control = num_viewportY });

        row3tl.Rows[^1].Cells.Add(new TableCell { Control = null });

        settings_table.Rows.Add(new TableRow());
    }
}