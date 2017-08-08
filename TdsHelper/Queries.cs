using System;
using System.Collections.Generic;
using System.Text;

namespace TdsHelper
{
    class Queries
    {
        public const string TableSimpleSchemaQuery = @"select
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

        public const string TableFullSchemaQuery = @"select
	                                                    TABLE_CATALOG TABLECATALOG,                
	                                                    TABLE_SCHEMA TABLESCHEMA,
	                                                    TABLE_NAME TABLENAME,
	                                                    COLUMN_NAME COLUMNNAME,
	                                                    ORDINAL_POSITION ORDINALPOSITION,
	                                                    COLUMN_DEFAULT COLUMNDEFAULT,
	                                                    cast(case IS_NULLABLE when N'YES' then 1 when N'NO' then 0 end as bit) ISNULLABLE,
	                                                    DATA_TYPE DATATYPE,
	                                                    CHARACTER_MAXIMUM_LENGTH CHARACTERMAXIMUMLENGTH,
	                                                    CHARACTER_OCTET_LENGTH CHARACTEROCTETLENGTH,
	                                                    NUMERIC_PRECISION NUMERICPRECISION,
	                                                    NUMERIC_PRECISION_RADIX NUMERICPRECISIONRADIX,
	                                                    NUMERIC_SCALE NUMERICSCALE,
	                                                    DATETIME_PRECISION DATETIMEPRECISION,
	                                                    CHARACTER_SET_CATALOG CHARACTERSETCATALOG,
	                                                    CHARACTER_SET_SCHEMA CHARACTERSETSCHEMA,
	                                                    CHARACTER_SET_NAME CHARACTERSETNAME,
	                                                    COLLATION_CATALOG COLLATIONCATALOG,
	                                                    COLLATION_SCHEMA COLLATIONSCHEMA,
	                                                    COLLATION_NAME COLLATIONNAME,
	                                                    DOMAIN_CATALOG DOMAINCATALOG,
	                                                    DOMAIN_SCHEMA DOMAINSCHEMA,
	                                                    DOMAIN_NAME DOMAINNAME
                                                    from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename";

        public const string TableSchema = @"select 
	                                            table_catalog tablecatalog, 
	                                            table_schema tableschema, 
	                                            table_name tablename
                                            from 
	                                            information_schema.tables 
                                            where 
	                                            table_name = @tablename";
    }
}
