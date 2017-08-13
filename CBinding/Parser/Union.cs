using ClangSharp;

namespace CBinding.Parser
{
	public class Union : Symbol
	{
		public Union (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	
	}

}
