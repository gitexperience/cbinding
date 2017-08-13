using ClangSharp;

namespace CBinding.Parser
{
	public class ClassTemplate : Class
	{
		public ClassTemplate (CMakeProject proj, CXCursor cursor ) : base (proj , cursor)
		{
		}
	}
}
