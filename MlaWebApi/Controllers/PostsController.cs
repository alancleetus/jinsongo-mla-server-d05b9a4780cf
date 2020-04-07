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
    public class PostsController : ApiController
    {

        public string cfmgr = ConfigurationManager.ConnectionStrings["MlaDatabase"].ConnectionString;
        SqlConnection cnn = null;

        public IEnumerable<post> GetAllPosts()
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from posts", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("posts");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new post
                {
                    postId = Int32.Parse(Convert.ToString(row["postId"])),
                    message = Convert.ToString(row["message"]),
                    messagekey = Convert.ToString(row["messageKey"]),
                    digitalSignature = Convert.ToString(row["digitalSignature"]),
                    signer = Convert.ToInt64(row["signer"]),
                    timestamp = Convert.ToDateTime(row["timestamp"]),
                    keyVersion = Convert.ToInt32(row["keyVersion"]),
                    groupId = Convert.ToInt64(row["groupId"]),
                    originalPostId = Convert.ToInt32(row["originalPostId"]),
                    postType = Convert.ToString(row["postType"]),
                };
            }

        }

        public IEnumerable<post> GetPostsByGroup(String groupId)
        {

            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from posts where groupId='" + groupId + "'", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("posts");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new post
                {
                    postId = Int32.Parse(Convert.ToString(row["postId"])),
                    message = Convert.ToString(row["message"]),
                    messagekey = Convert.ToString(row["messageKey"]),
                    digitalSignature = Convert.ToString(row["digitalSignature"]),
                    signer = Convert.ToInt64(row["signer"]),
                    timestamp = Convert.ToDateTime(row["timestamp"]),
                    keyVersion = Convert.ToInt32(row["keyVersion"]),
                    groupId = Convert.ToInt64(row["groupId"]),
                    originalPostId = Convert.ToInt32(row["originalPostId"]),
                    postType = Convert.ToString(row["postType"]),
                };
            }

        }
         
        public HttpResponseMessage NewPost(String message,String messageKey, String digitalSignature, int signer, int keyVersion, int groupId, String postType, int originalPostId)
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
                        SqlCommand command1 = new SqlCommand("Insert into posts(message, messagekey, digitalSignature, signer, keyVersion, groupId, originalPostId, postType) values('"
                    + message
                    + "','" + messageKey
                    + "','" + digitalSignature
                    + "','" + signer
                    + "','" + keyVersion
                    + "','" + groupId
                    + "','" + originalPostId
                    + "','" + postType
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