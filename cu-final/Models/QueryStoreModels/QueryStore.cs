using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.QueryStoreModels
{
    public class QueryStore
    {
        public Double avg_physical_io_reads { get; set; }

        public Double avg_duration { get; set; }

        public String query_sql_text { get; set; }

        public Int64 query_id { get; set; }

        public Int64 query_text_id { get; set; }

        public Int64 plan_id { get; set; }
       
        public Int64 runtime_stats_id { get; set; }

        public Double avg_rowcount { get; set; }

        public Int64 count_executions { get; set; }
    }
}
