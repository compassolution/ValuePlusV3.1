using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Log
{
    public interface ILog
    {
        void Error(object message, Exception e);
        void Error(object message);
        void Debug(object message, Exception e);
        void Debug(object message);
        void Warn(object message, Exception e);
        void Warn(object message);
        void Fatal(object message, Exception e);
        void Fatal(object message);
        void Info(object message, Exception e);
        void Info(object message);
    }
}
