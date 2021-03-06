// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under MIT X11 license (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using dnSpy.Contracts.Decompiler;
using dnSpy.Contracts.Text;

namespace ICSharpCode.NRefactory.VB.Ast {
	public abstract class Expression : AstNode
	{
		#region Null
		public new static readonly Expression Null = new NullExpression ();
		
		sealed class NullExpression : Expression
		{
			public override bool IsNull {
				get {
					return true;
				}
			}
			
			public override S AcceptVisitor<T, S> (IAstVisitor<T, S> visitor, T data)
			{
				return default (S);
			}
			
			protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
			{
				return other == null || other.IsNull;
			}
		}
		#endregion
		
		#region Builder methods
		/// <summary>
		/// Builds an member reference expression using this expression as target.
		/// </summary>
		public MemberAccessExpression Member(object annotation, string memberName)
		{
			return new MemberAccessExpression { Target = this, MemberName = Identifier.Create(annotation, memberName) };
		}
		
		/// <summary>
		/// Builds an invocation expression using this expression as target.
		/// </summary>
		public InvocationExpression Invoke2(object annotations, string methodName, IEnumerable<Expression> arguments)
		{
			return Invoke(annotations, methodName, null, arguments);
		}
		
		/// <summary>
		/// Builds an invocation expression using this expression as target.
		/// </summary>
		public InvocationExpression Invoke2(object annotations, string methodName, params Expression[] arguments)
		{
			return Invoke(annotations, methodName, null, arguments);
		}
		
		/// <summary>
		/// Builds an invocation expression using this expression as target.
		/// </summary>
		public InvocationExpression Invoke(object annotation, string methodName, IEnumerable<AstType> typeArguments, IEnumerable<Expression> arguments)
		{
			InvocationExpression ie = new InvocationExpression();
			MemberAccessExpression mre = new MemberAccessExpression();
			mre.Target = this;
			mre.MemberName = Identifier.Create(BoxedTextColor.InstanceMethod, methodName);
			mre.TypeArguments.AddRange(typeArguments);
			ie.Target = mre;
			ie.Arguments.AddRange(arguments);
			return ie;
		}
		
		/// <summary>
		/// Builds an invocation expression using this expression as target.
		/// </summary>
		public InvocationExpression Invoke(IEnumerable<Expression> arguments)
		{
			InvocationExpression ie = new InvocationExpression();
			ie.Target = this;
			ie.Arguments.AddRange(arguments);
			return ie;
		}
		
		/// <summary>
		/// Builds an invocation expression using this expression as target.
		/// </summary>
		public InvocationExpression Invoke(params Expression[] arguments)
		{
			InvocationExpression ie = new InvocationExpression();
			ie.Target = this;
			ie.Arguments.AddRange(arguments);
			return ie;
		}
		
		public CastExpression CastTo(AstType type)
		{
			return new CastExpression { CastType = CastType.CType, Type = type,  Expression = this };
		}
		#endregion
	}
}
