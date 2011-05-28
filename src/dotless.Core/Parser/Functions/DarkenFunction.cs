namespace dotless.Core.Parser.Functions 
{
    using Infrastructure.Nodes;
    using Tree;

    public class DarkenFunction : LightnessFunction 
    {
        protected override Node EditColor(Color color, Number number) 
        {
            return base.EditColor(color, number.Inverse);
        }
    }
}
