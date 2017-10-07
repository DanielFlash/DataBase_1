using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    interface IMember //по умолчанию public (не может быть не public)
    {
        void Change(string key, string value); //методы и свойства интерфеса всегда публичные
    }
}
