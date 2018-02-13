using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Tests {

	// ?? System.Reflection.Emit.SignatureHelper

	internal class SignatureHelper {
		private readonly SignatureMode _signatureMode;

		private bool IgnoreParameterName;
		private bool IgnoreReturnType;

		public static SignatureHelper ForCompare = new SignatureHelper(SignatureMode.Compare);
		public static SignatureHelper ForCompareIgnoreReturnType = new SignatureHelper(SignatureMode.CompareIgnoreReturnType);
		public static SignatureHelper ForCode = new SignatureHelper(SignatureMode.Code);

		private SignatureHelper(SignatureMode signatureMode) { _signatureMode = signatureMode; }

		public string Sig(MethodInfo arg) {
			var sb = new StringBuilder();

			if (arg.IsPublic) sb.Append("public ");
			else if (arg.IsFamilyAndAssembly) sb.Append("protected internal ");
			else if (arg.IsAssembly) sb.Append("internal ");
			else if (arg.IsFamily) sb.Append("protected ");

			if (arg.IsStatic) sb.Append("static ");
			if (_signatureMode == SignatureMode.CompareIgnoreReturnType) { /*skip*/ }
			else sb.Append(Sig(arg.ReturnType) + " ");
			sb.Append(arg.Name);
			sb.Append("(");
			sb.Append(Sig(arg.GetParameters()));
			sb.Append(")");
			return sb.ToString();
		}

		public string Sig(ParameterInfo[] parameterInfos) {
			if (parameterInfos.Length == 0) return string.Empty;
			var sb = new StringBuilder();
			foreach (var pi in parameterInfos) sb.Append(", " + Sig(pi));
			return sb.ToString(2, sb.Length                   - 2);
		}

		public string Sig(ParameterInfo parameterInfo) {
			var sb = new StringBuilder();
			//Attributes?

			switch (_signatureMode) {
				case SignatureMode.Compare:
				case SignatureMode.CompareIgnoreReturnType:
					sb.Append(Sig(parameterInfo.ParameterType));
					break;
				case SignatureMode.Code:
					sb.Append(Sig(parameterInfo.ParameterType));
					sb.Append(" " + parameterInfo.Name);
					break;
			}
			
			return sb.ToString();
		}

		public string Sig(Type type) {

			if (type.IsGenericType) {
				var sb   = new StringBuilder();
				var gt   = type.GetGenericTypeDefinition();
				var gtfn = gt.FullName.Substring(0, gt.FullName.IndexOf("`"));
				sb.Append(gtfn);
				sb.Append("<");
				sb.Append(Sig(type.GetGenericArguments()));
				sb.Append(">");
				return sb.ToString();
			}

			var fn = type.FullName;
			switch (fn) {
				case "System.Void":    return "void";
				case "System.UInt16":  return "ushort";
				case "System.UInt32":  return "uint";
				case "System.UInt64":  return "ulong";
				case "System.Int16":   return "short";
				case "System.Int32":   return "int";
				case "System.Int64":   return "long";
				case "System.Char":    return "char";
				case "System.String":  return "string";
				case "System.Boolean": return "bool";
				case "System.Byte":    return "byte";
				case "System.SByte":   return "sbyte";
				case "System.Double":  return "double";
				case "System.Single":  return "float";
				case "System.Decimal": return "decimal";
			}
//			if (fn.StartsWith("System.")) return fn.Substring(7);
			return fn;
		}

		public string Sig(Type[] genericArguments) {
			if (genericArguments.Length == 0) return string.Empty;
			var sb = new StringBuilder();
			foreach (var ga in genericArguments) sb.Append(", " + Sig(ga));
			return sb.ToString(2, sb.Length                     - 2);
		}
	}

	internal enum SignatureMode {
		Compare,
		Code,
		CompareIgnoreReturnType
	}

}
