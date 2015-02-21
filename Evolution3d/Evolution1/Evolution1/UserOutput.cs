using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StillDesign.PhysX;

namespace Evolution1
{
    /// <summary>
    /// PhysXから送られてくるメッセージを処理します。
    /// コードはみたまんまかと。
    /// </summary>
    public class UserOutput : UserOutputStream
    {
        public override void Print(string message)
        {
            Console.WriteLine("PhysX: " + message);
        }
        public override AssertResponse ReportAssertionViolation(string message, string file, int lineNumber)
        {
            Console.WriteLine("PhysX: " + message);

            return AssertResponse.Continue;
        }
        public override void ReportError(ErrorCode errorCode, string message, string file, int lineNumber)
        {
            Console.WriteLine("PhysX: " + message);
        }
    }
}
