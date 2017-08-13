using ClangSharp;

namespace CBinding.Parser
{
	public class Class : Symbol
	{
		public Class (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}
}
