using ClangSharp;

namespace CBinding.Parser
{
	public class Typedef : Symbol
	{
		public Typedef (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}

	}

}
