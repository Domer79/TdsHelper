using System;
using System.Collections.Generic;
using System.Text;

namespace TdsHelper
{
    class Queries
    {
        public const string TableSchemaQuery = @"select
                            TABLE_SCHEMA, 
                            ORDINAL_POSITION, 
                            COLUMN_NAME, 
                            DATA_TYPE, 
                            case DATA_TYPE
                                when ''varchar'' then COLUMN_NAME +'' varchar('' + cast(CHARACTER_MAXIMUM_LENGTH as nvarchar(10)) + ''),''

                            when ''nvarchar'' then COLUMN_NAME +'' varchar('' + cast(CHARACTER_MAXIMUM_LENGTH as nvarchar(10)) + ''),''

                            when ''uniqueidentifier'' then COLUMN_NAME +'' uuid,''
                            else COLUMN_NAME + '' '' + DATA_TYPE + '',''

                            end dataType,
                                CHARACTER_MAXIMUM_LENGTH
                            from
                                INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @table_name order by ordinal_position";

    }
}
