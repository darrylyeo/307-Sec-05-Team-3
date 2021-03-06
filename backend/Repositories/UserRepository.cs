﻿using System;
using System.Collections.Generic;
using backend.Exceptions;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository
    {
        private string _sqlConnectionString;
        public UserRepository(bool isTest = false)
        {
            if (isTest)
            {
                _sqlConnectionString = Properties.Resources.testsqlconnection;
            }
            else
            {
                _sqlConnectionString = Properties.Resources.sqlconnection;
            }
        }
        public string PostNewUser(UserRecord newUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Users (UserName, Password, FirstName, LastName, JoinDate, IsAdmin) values
                        (@UserName, @Password, @FirstName, @LastName, @JoinDate, @IsAdmin);";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", newUser.UserName);
                        cmd.Parameters.AddWithValue("@Password", newUser.Password);
                        cmd.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", newUser.LastName);
                        cmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsAdmin", Convert.ToInt32(false));
                        cmd.ExecuteNonQuery();
                    }
                }
                return string.Format("Welcome {0}", newUser.FirstName);
            }
            catch (SqlException e)
            {
                throw new RepoException(e.Message);
            }
        }

        public UserRecord GetUserById (int userId)
        {
            try
            {
                UserRecord retrievedUser = new UserRecord();
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"select * from cn.Users where UserId = @userId";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                retrievedUser = new UserRecord()
                                {
                                    UserId = Int32.Parse(reader["UserId"].ToString()),
                                    UserName = reader["UserName"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    JoinDate = DateTime.Parse(reader["JoinDate"].ToString()),
                                    IsAdmin = bool.Parse(reader["isAdmin"].ToString())
                                };
                            }
                        }
                    }
                }
                return retrievedUser;
            }
            catch(SqlException e)
            {
                throw new RepoException("user not found");
            }
        }

        /*
         * The Methods below are only to be used for sql testing,
         * and will not work on the production database
         */

        public bool ResetUsers()
        { 
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string getEventQuery = @"delete from cn.Users;";
                        using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("You cannot delete all users from the production database");
                return false;
            }
        }

        public bool ResetAutoIncrement()
        {
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string resetIdentityQuery = @"DBCC CHECKIDENT('cn.Users', RESEED, 0)";
                        using (SqlCommand cmd = new SqlCommand(resetIdentityQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("You cannot reset an identity in the production database");
                return false;
            }
        }
    }
}