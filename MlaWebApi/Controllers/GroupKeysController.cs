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
    public class GroupKeysController : ApiController
    {// GET api/GroupKeys


        public string cfmgr = ConfigurationManager.ConnectionStrings["MlaDatabase"].ConnectionString;
        SqlConnection cnn = null;


        //get all group keysby userid, groupid, keyversion
        public IEnumerable<group_keys> GetKey(int userId, int groupId, int groupKeyVersion)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from group_keys where userId=" + userId + " AND groupId="+groupId+" AND groupKeyVersion="+ groupKeyVersion, cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("group_keys");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new group_keys
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupKeyVersion = Int32.Parse(Convert.ToString(row["groupKeyVersion"])),
                    encryptedGroupKey = Convert.ToString(row["encryptedGroupKey"]),
                    status = Convert.ToBoolean(row["status"])
                };
            }

        }

        //get latest group key for user by looking if status true
        public IEnumerable<group_keys> GetLatestKey(int userId, int groupId)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from group_keys where userId=" + userId + " AND groupId=" + groupId + " AND status='true'", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("group_keys");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new group_keys
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupKeyVersion = Int32.Parse(Convert.ToString(row["groupKeyVersion"])),
                    encryptedGroupKey = Convert.ToString(row["encryptedGroupKey"]),
                    status = Convert.ToBoolean(row["status"])
                };
            }

        }

        //insert new group key
        public HttpResponseMessage InsertNewGroupKey(int userId, int groupId, String encryptedGroupKey, int groupKeyVersion)
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
                        SqlCommand command1 = new SqlCommand("Insert into group_keys(userId, groupId, encryptedGroupKey, groupKeyVersion, status) values('"
                    + userId
                    + "','" + groupId
                    + "','" + encryptedGroupKey
                    + "','" + groupKeyVersion
                    + "','true')", connection1);

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

        //update key status
        //set status = false for all groups keys with a specific version number
        public HttpResponseMessage UpdateStatus(int groupId, int groupKeyVersion, Boolean status)
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
                        SqlCommand command1 = new SqlCommand("Update group_keys SET status='" + status + "' where groupId=" + groupId+" AND groupKeyVersion="+groupKeyVersion, connection1);

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