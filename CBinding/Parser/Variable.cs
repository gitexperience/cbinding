using ClangSharp;

namespace CBinding.Parser
{
	public class Variable : Symbol
	{
		public Variable (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}

	}

}
