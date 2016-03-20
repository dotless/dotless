namespace dotless.Core.Parser.Functions
{
    using System;
    using System.IO;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class DataUriFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            var filename = GetDataUriFilename();
            string base64 = ConvertFileToBase64(filename);
            string mimeType = GetMimeType(filename);

            return new TextNode(string.Format("url(\"data:{0};base64,{1}\")", mimeType, base64));
        }

        private string GetDataUriFilename()
        {
            Guard.ExpectMinArguments(1, Arguments.Count, this, Location);

            // Thanks a lot LESS for putting the optional parameter first!
            var filenameNode = Arguments[0];
            if (Arguments.Count > 1)
                filenameNode = Arguments[1];

            Guard.ExpectNode<Quoted>(filenameNode, this, Location);
            var filename = ((Quoted)filenameNode).Value;

            Guard.Expect(!(filename.StartsWith("http://") || filename.StartsWith("https://")),
                string.Format("Invalid filename passed to data-uri '{0}'. Filename must be a local file", filename), Location);

            return filename;
        }

        private string ConvertFileToBase64(string filename)
        {
            string base64;
            try
            {
                base64 = Convert.ToBase64String(File.ReadAllBytes(filename));
            }
            catch (IOException e)
            {
                // this is more general than just a check to see whether the file exists
                // it could fail for other reasons like security permissions
                throw new ParsingException(String.Format("Data-uri function could not read file '{0}'", filename), e, Location);
            }
            return base64;
        }

        private string GetMimeType(string filename)
        {
            if (Arguments.Count > 1)
            {
                Guard.ExpectNode<Quoted>(Arguments[0], this, Location);
                var mimeType = ((Quoted) Arguments[0]).Value;

                if (mimeType.IndexOf(';') > -1)
                    mimeType = mimeType.Split(';')[0];

                return mimeType;
            }

            return new MimeTypeLookup().ByFilename(filename);
        }
    }
}
