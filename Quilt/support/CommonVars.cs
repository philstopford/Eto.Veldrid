namespace Quilt;

public class CommonVars
{
    private QuiltContext quiltContext;
    
    public string titleText = CentralProperties.productName + " " + CentralProperties.version;
    
    public CommonVars(ref QuiltContext context)
    {
        pInit(ref context);
    }

    private void pInit(ref QuiltContext context)
    {
        quiltContext = context;

        titleText += " (" + quiltContext.licenceName + ")";
    }
}