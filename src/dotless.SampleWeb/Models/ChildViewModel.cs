using System.Collections.Generic;
using System.Linq;

namespace dotless.SampleWeb.Models
{
    public class ChildViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
    }

    public class ChildrenOverviewViewModel
    {
        public string ParentName { get; set; }
        private readonly List<ChildViewModel> _children;

        public ChildrenOverviewViewModel(string parentName, IEnumerable<ChildViewModel> children)
        {
            ParentName = parentName;
            _children = children.Take(4).ToList();
        }

        public IReadOnlyList<ChildViewModel> Children
        {
            get { return _children; }
        }
    }
}
