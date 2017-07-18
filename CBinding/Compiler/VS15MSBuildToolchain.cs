//
// VS15MSBuildToolchain.cs: Provides functionality to compile using VS15 MSBuild Toolchain.
//
// Authors:
//   Anubhav Singh <mailtoanubhav02@gmail.com>
// Copyright (C) 2017 Anubhav Singh
//
//
// This source code is licenced under The MIT License:
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Mono.Addins;

namespace CBinding
{
	[Extension ("/CBinding/Toolchains")]
	public class VS15MSBuildToolchain : VisualStudioToolchain
	{

		/// <summary>
		/// The name of this Toolchain.
		/// </summary>
		/// <value>The name.</value>
		public override string ToolchainName {
			get {
				return "VS15 MSBuild Toolchain";
			}
		}

		public override string GeneratorID {
			get {
				return "Visual Studio 15 2017";
			}
		}
	}
}