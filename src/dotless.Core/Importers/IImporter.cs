namespace dotless.Core.Importers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Parser;
    using Parser.Tree;

    public interface IImporter
    {
        /// <summary>
        ///  a list of the imported paths in order resolve urls to be relative to the base imported file
        /// </summary>
        List<string> Paths { get; }

        /// <summary>
        ///  The current directory of the base file in order to map imported files
        /// </summary>
        string CurrentDirectory { get; }

        /// <summary>
        ///  Imports an import and return true if successful
        /// </summary>
        bool Import(Import import);

        /// <summary>
        ///  A list of imports
        /// </summary>
        List<string> Imports { get; }

        /// <summary>
        ///  A method set by the parser implementation in order to get a new parser for use in importing
        /// </summary>
        Func<Parser> Parser { get; set; }
    }
}
