using Veldrid;

namespace Quilt;

public class QuiltContext
{
    // This is intended to hold application context settings to allow for cleaner handling of
    // commandline options related to headless mode, XML file loading from the command line, viewport switches

    public int openGLZoomFactor;
    public bool AA;
    public bool filledPolygons;
    public bool drawPoints;
    public GraphicsBackend backend;

    public string licenceName;

    public QuiltContext(string xmlFileArg_, GraphicsBackend _backend)
    {
        makeContext(xmlFileArg_, _backend);
    }

    private void makeContext(string xmlFileArg_, GraphicsBackend _backend)
    {
        openGLZoomFactor = 1;
        filledPolygons = false;
        drawPoints = false;
        AA = true;
        backend = _backend;
        licenceName = "GPLv3";
    }
}