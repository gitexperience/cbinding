using ClangSharp;

namespace CBinding.Parser
{
	public class Enumerator : Symbol
	{
		public Enumerator (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}
}
