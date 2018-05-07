using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Data;
using ContosoUniversity.Models.QueryStoreModels;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ContosoUniversity.Controllers
{
    public class QueryStoreController : Controller
    {
        private readonly SchoolContext _context;


        public QueryStoreController(SchoolContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<ActionResult> Index()
        {
            List<QueryStore> groups = new List<QueryStore>();


            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT * FROM sys.query_store_query_text";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new QueryStore { query_text_id = reader.GetInt64(0) };
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
    }
}