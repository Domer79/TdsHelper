using System;
using System.Collections.Generic;
using System.Text;

namespace TdsHelper
{
    class Queries
    {
        public const string MsTableFullSchemaQuery = @"select
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

        public const string MsTableSchema = @"select 
	                                            table_catalog tablecatalog, 
	                                            table_schema tableschema, 
	                                            table_name tablename
                                            from 
	                                            information_schema.tables 
                                            where 
	                                            table_name = @tablename";

        public const string PgCheckForeignServer = @"select cast(1 as boolean) from information_schema.foreign_servers where foreign_server_name = :serverName union all select false limit 1";
    }
}
