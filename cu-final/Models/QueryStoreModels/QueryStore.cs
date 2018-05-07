using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.QueryStoreModels
{
    public class QueryStore
    {
        public Int64 query_text_id { get; set; }

        public String query_sql_text { get; set; }

        public String statement_sql_handle { get; set; }
    }
}
