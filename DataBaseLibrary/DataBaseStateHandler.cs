using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    public delegate void DataBaseStateHandler(object sender, DataBaseEventArgs e);

    public class DataBaseEventArgs //internal по умолчанию
    {
        public string Message { get; }

        internal DataBaseEventArgs(string message)
        {
            Message = message;
        }
    }
}