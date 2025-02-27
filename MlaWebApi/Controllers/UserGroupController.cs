﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data; 
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Providers.Entities;
using MlaWebApi.Models;


namespace MlaWebApi.Controllers
{
     
    public class UserGroupController : ApiController
    {
        public string cfmgr = ConfigurationManager.ConnectionStrings["MlaDatabase"].ConnectionString;
        SqlConnection cnn = null;

        public IEnumerable<user_groups> GetAllUserGroups()
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from user_groups", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_groups");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_groups
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupName = Convert.ToString(row["groupName"])
                };
            }

        }

        public IEnumerable<user_groups> GetAllGroups()
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open(); 

            SqlCommand comm = new SqlCommand("Select DISTINCT groupId, groupName from user_groups where groupName NOT LIKE '[_]%'", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_groups");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_groups
                {
                    userId = -1,
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupName = Convert.ToString(row["groupName"])
                };
            }

        }



        public IEnumerable<user_groups> GetGroupsByUserId(int userId)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from user_groups where userId="+userId + " ", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_groups");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_groups
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupName = Convert.ToString(row["groupName"])
                };
            }

        }

        //select g.groupId, u.groupName, u.userId from group_status as g inner join user_groups as u on u.groupId=g.groupId where g.status = 0 and u.userId= 74;
        public IEnumerable<user_groups> GetOutdatedGroups(int userId)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("select g.groupId, u.groupName, u.userId from group_status as g inner join user_groups as u on u.groupId=g.groupId where g.status = 0 and u.userId=" + userId + " ", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_groups");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_groups
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupName = Convert.ToString(row["groupName"])
                };
            }
        }


            public IEnumerable<GroupMembers> GetMembersInGroupByUserId(string userId)
        {
            /*select ug2.groupId, ug2.userId, ug2.groupName, r.userName from user_groups as ug
            inner join user_groups as ug2
            on ug.groupId = ug2.groupId
            inner join register as r on r.userId = ug2.userId
            where ug.userId = 74;*/

            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("select ug2.groupId, ug2.userId, ug2.groupName, r.userName from user_groups as ug inner join user_groups as ug2 on ug.groupId = ug2.groupId inner join register as r on r.userId = ug2.userId where ug.userId = '" + userId + "' ", cnn);

            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("GroupMembers");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new GroupMembers
                {
                    userId = Convert.ToString(row["userId"]),
                    groupId = Convert.ToString(row["groupId"]),
                    groupName = Convert.ToString(row["groupName"]),
                    userName = Convert.ToString(row["userName"])
                };
            }

        }
        public IEnumerable<user_groups> GetUsersByGroupId(int groupId)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from user_groups where groupId=" + groupId + " ", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_groups");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_groups
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupName = Convert.ToString(row["groupName"])
                };
            }

        }

        public IEnumerable<user_groups> GetGroupsByGroupName(String groupName)
        {
            cnn = new SqlConnection(cfmgr);
            cnn.Open();

            SqlCommand comm = new SqlCommand("Select * from user_groups where groupName='" + groupName + "' ", cnn);
            SqlDataAdapter Sqlda = new SqlDataAdapter(comm);
            DataSet dsDatast = new DataSet("user_groups");
            Sqlda.Fill(dsDatast);

            foreach (DataRow row in dsDatast.Tables[0].Rows)
            {
                yield return new user_groups
                {
                    userId = Int32.Parse(Convert.ToString(row["userId"])),
                    groupId = Int32.Parse(Convert.ToString(row["groupId"])),
                    groupName = Convert.ToString(row["groupName"])
                };
            }

        }



        public HttpResponseMessage CreateNewGroup(int userId, String groupName)
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
                        SqlCommand command1 = new SqlCommand("Insert into user_groups(groupId,userId,groupName) Select MAX(u.groupId)+1, '"
                    + userId
                    + "','" + groupName
                    + "' from user_groups as u", connection1);

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

        public HttpResponseMessage AddMemberToGroup(int userId, int groupId, String groupName)
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
                        SqlCommand command1 = new SqlCommand("Insert into user_groups(userId,groupId,groupName) values('"
                    + userId
                    + "','" + groupId
                    + "','" + groupName
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

        [HttpPost]
        public HttpResponseMessage RmvUser(int userId, int groupId)
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
                        SqlCommand command1 = new SqlCommand("DELETE FROM user_groups where userId = "+userId+" AND groupId="+groupId+"", connection1);
                        
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