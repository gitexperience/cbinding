using ClangSharp;

namespace CBinding.Parser
{
	public class Macro : Symbol
	{
		public Macro (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}
}
