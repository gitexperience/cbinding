using ClangSharp;
using CBinding;

namespace CBinding.Parser
{
	public class Field : Symbol
	{
		public Field (CMakeProject proj, CXCursor cursor) : base (proj, cursor)
		{
		}

	}

}