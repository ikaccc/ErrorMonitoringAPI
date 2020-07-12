using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ExceptionHandler.API.Common
{
    public static class ReflectionUtil
    {
        public static string SnapLocalVariables(Exception exception)
        {
            StackTrace stackTrace = new StackTrace(exception.InnerException, true);
            return ReflectionUtil.SnapLocalVariables(new StackTrace(true));
        }

        public static string SnapLocalVariables(StackTrace stackTrace)
        {
            StackFrame[] frames = stackTrace.GetFrames();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (StackFrame stackFrame in frames)
            {
                MethodInfo method = stackFrame.GetMethod() as MethodInfo;
                stringBuilder.AppendLine("----------------");
                stringBuilder.AppendLine("Method: " + method.Name);
                stringBuilder.AppendLine(" Parameters:");
                foreach (ParameterInfo parameter in method.GetParameters())
                    stringBuilder.AppendLine(" Name:" + parameter.Name + " Type:" + parameter.ParameterType.ToString());
                stringBuilder.AppendLine(" Local Variables:");
                MethodBody methodBody = method.GetMethodBody();
                if (methodBody != null)
                {
                    foreach (LocalVariableInfo localVariable in (IEnumerable<LocalVariableInfo>)methodBody.LocalVariables)
                        stringBuilder.AppendLine(string.Format(" Index:{0} Type:{1}", (object)localVariable.LocalIndex, (object)localVariable.LocalType.ToString()));
                }
            }
            stringBuilder.AppendLine("----------------");
            return stringBuilder.ToString();
        }
    }
}
