using System;

namespace KJFramework.ServiceModel.Exceptions
{
    public class GeneralException
    {
        public static System.Exception InvaildReturnValueException = new System.Exception("非法的返回值。");
        public static string InvaildReturnValueExceptionFullName = InvaildReturnValueException.InnerException.GetType().FullName;
        public static string InvaildReturnValueExceptionAssemblyFullName = InvaildReturnValueException.InnerException.GetType().Assembly.FullName;
        public static string GeneralErrorMessage = String.Format("{0}, {1}",
                                                          InvaildReturnValueExceptionFullName,
                                                          InvaildReturnValueExceptionAssemblyFullName);
    }
}