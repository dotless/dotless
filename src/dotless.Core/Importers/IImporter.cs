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
        ///  Get a list of the current paths, used to pass back in to alter url's after evaluation
        /// </summary>
        /// <returns></returns>
        List<string> GetCurrentPathsClone();

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

        /// <summary>
        ///  Called for every Url and allows the importer to adjust relative url's to be relative to the
        ///  primary url
        /// </summary>
        string AlterUrl(string url, List<string> pathList);
    }
}
