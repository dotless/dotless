namespace dotless.Core.Parser.Functions 
{
    using Tree;

    public class DarkenFunction : LightnessFunction 
    {
        protected override Infrastructure.Nodes.Node EditColor(Color color, Number number) 
        {
            return base.EditColor(color, number.Inverse);
        }
    }
}
