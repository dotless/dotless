namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections.Generic;
    using Tree;

    public class Closure
    {
        public Ruleset Ruleset { get; set; }
        public List<Ruleset> Context { get; set; }
    }
}