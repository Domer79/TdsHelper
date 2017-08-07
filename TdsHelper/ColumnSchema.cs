using System;
using System.Collections.Generic;
using System.Text;

namespace TdsHelper
{
    class ColumnSchema
    {
        public string TableCatalog { get; set; }
        public string TableSchame { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int OrdinalPosition { get; set; }
        public string ColumnDefault { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharacterMaximumLength { get; set; }
        public int CharacterOctetLength { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericPrecisionRadix { get; set; }
        public int NumericScale { get; set; }
        public int DatetimePrecision { get; set; }
        public object CharacterSetCatalog { get; set; }
        public object CharacterSetSchema { get; set; }
        public string CharacterSetName { get; set; }
    }
}
