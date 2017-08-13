using ClangSharp;

namespace CBinding.Parser
{
	public class Enumeration : Symbol
	{
		public Enumeration (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}
}