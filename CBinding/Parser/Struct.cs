using ClangSharp;

namespace CBinding.Parser
{
	public class Struct: Symbol
	{
		public Struct (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	
	}

}
