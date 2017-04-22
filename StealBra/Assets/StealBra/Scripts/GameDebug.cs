using UnityEngine;
using System.Collections;
using System.Reflection;
public class GameDebug {

	public static string GetInvokeClassAndMethodName(int index){
		index = index >= 1 ? index : 1;
		System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace ();
		MethodBase method = trace.GetFrame (index).GetMethod ();
		return "class name:" + method.ReflectedType.Name + ", method name:" + method.Name;
	}
}
