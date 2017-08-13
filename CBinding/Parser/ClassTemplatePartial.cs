using ClangSharp;

namespace CBinding.Parser
{
	public class ClassTemplatePartial : ClassTemplate
	{
		public ClassTemplatePartial (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}
	}
}
