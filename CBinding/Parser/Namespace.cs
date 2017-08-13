using ClangSharp;

namespace CBinding.Parser
{
	public class Namespace : Symbol
	{
		public Namespace (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}
}
