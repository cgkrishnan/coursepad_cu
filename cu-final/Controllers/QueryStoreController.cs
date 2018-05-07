using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Data;
using ContosoUniversity.Models.QueryStoreModels;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;

namespace ContosoUniversity.Controllers
{
    public class QueryStoreController : Controller
    {
        private readonly SchoolContext _context;

        public QueryStoreController(SchoolContext context)
        {
            _context = context;
        }

        /* Controller Method for populating Query Store List.
         * Retrives data based on the query and result
         * populated to the Model Class created */

        // GET: QuertStore
        public async Task<ActionResult> Index()
        {
            List<QueryStore> groups = new List<QueryStore>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT TOP 25 rs.avg_physical_io_reads, rs.avg_duration, qt.query_sql_text, q.query_id, qt.query_text_id, p.plan_id, rs.runtime_stats_id, rs.avg_rowcount, rs.count_executions " +
                                   "FROM sys.query_store_query_text AS qt "+
                                   "JOIN sys.query_store_query AS q "+
                                   "ON qt.query_text_id = q.query_text_id "+
                                   "JOIN sys.query_store_plan AS p "+
                                   "ON q.query_id = p.query_id "+
                                   "JOIN sys.query_store_runtime_stats AS rs "+
                                   "ON p.plan_id = rs.plan_id "+
                                   "ORDER BY rs.avg_duration DESC";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new QueryStore { avg_physical_io_reads = reader.GetDouble(0),
                                                       avg_duration = reader.GetDouble(1),
                                                       query_sql_text = reader.GetString(2),
                                                       query_id = reader.GetInt64(3),
                                                       query_text_id = reader.GetInt64(4),
                                                       plan_id = reader.GetInt64(5),
                                                       runtime_stats_id = reader.GetInt64(6),
                                                       avg_rowcount = reader.GetDouble(7),
                                                       count_executions = reader.GetInt64(8)};
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            return View(groups);
        }

        /* Controller Method for populating Query details.
         * Retrives data based on the query ID selected and result
         * populated to a DataTable created */

        // GET: Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queryDetails = new DataTable();
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT Txt.query_text_id, Txt.query_sql_text, Qry.* "+
                                   "FROM  sys.query_store_query AS Qry "+
                                   "JOIN sys.query_store_query_text AS Txt "+
                                   "ON Qry.query_text_id = Txt.query_text_id "+
                                   "where Qry.query_id ="+id;
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        queryDetails.Load(reader);
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            return View(queryDetails);
        }
    }
}