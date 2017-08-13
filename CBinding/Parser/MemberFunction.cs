using ClangSharp;

namespace CBinding.Parser
{

	public class MemberFunction : Function
	{
		public MemberFunction (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}

}
