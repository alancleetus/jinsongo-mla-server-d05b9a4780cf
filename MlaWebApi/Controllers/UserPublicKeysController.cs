using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using MlaWebApi.Models;

namespace MlaWebApi.Controllers
{
    public class UserPublicKeysController : ApiController
    {
        public string cfmgr = ConfigurationManager.ConnectionStrings["MlaDatabase"].ConnectionString;
        SqlConnection cnn = null;

        public IEnumerable<user_public_keys> GetAll()
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from user_public_keys", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_public_keys");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_public_keys
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    publicKey = Convert.ToString(row["publicKey"]),
                };
            }

        }

        public IEnumerable<user_public_keys> GetById(int userId)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from user_public_keys where userId="+userId, cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_public_keys");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_public_keys
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    publicKey = Convert.ToString(row["publicKey"]),
                };
            }

        }

        public HttpResponseMessage PostPublicKey(int userId, String publicKey)
        {
            HttpResponseMessage response;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection1 = new SqlConnection(cfmgr))
                    {
                        connection1.Open();
                        SqlCommand command1 = new SqlCommand("Insert into user_public_keys(userId,publicKey) values('"
                    + userId
                    + "','" + publicKey
                    + "')", connection1);

                        int returnValue = command1.ExecuteNonQuery();
                        writer.WriteLine("Rows to be affected by command1: {0}", returnValue);

                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }

                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return response;
        }
    }
}